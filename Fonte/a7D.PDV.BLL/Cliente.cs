using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Utils;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.DAL;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace a7D.PDV.BLL
{
    public static class Cliente
    {
        public static List<ClienteInformation> Listar()
        {
            ClienteInformation objFiltro = new ClienteInformation();

            List<object> listaObj = CRUD.Listar(objFiltro);
            List<ClienteInformation> lista = listaObj.ConvertAll(new Converter<object, ClienteInformation>(ClienteInformation.ConverterObjeto));

            return lista;
        }

        public static List<ClienteInformation> ListarResumido(string nomeCompleto = null, int? telefone1Numero = null, string documento1 = null, string email = null)
        {
            return ClienteDAL.ListarResumido(nomeCompleto, telefone1Numero, documento1, email);
        }

        public static ClienteInformation Carregar(int idCliente)
        {
            ClienteInformation obj = new ClienteInformation { IDCliente = idCliente };
            obj = (ClienteInformation)CRUD.Carregar(obj);

            return obj;
        }

        //public static void Salvar(ClienteInformation obj)
        //{
        //    if (obj.IDCliente == null)
        //        obj.DtInclusao = DateTime.Now;
        //    CRUD.Salvar(obj);
        //}

        public static List<ClienteInformation> BuscarCliente(string nome, string email, string documento, string telefone, int pagina, int qtd)
        {
            return DAL.ClienteDAL.BuscarCliente(nome, email, documento, telefone, pagina, qtd);
        }

        public static tbCliente BuscarCliente(TipoCliente tipo, string valor, tbCliente novo = null, string ifoodClienteID = null)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return novo;

            valor = valor.Trim().ToLowerInvariant();
            tbCliente cliente = null;

            if (!string.IsNullOrEmpty(ifoodClienteID))
            {
                var nota = $"IFood #{ifoodClienteID} "; // Igual ao criado
                cliente = EF.Repositorio.Listar<tbCliente>(c => c.Observacao.Contains(nota)).FirstOrDefault();
            }

            if (cliente == null && valor.Length > 6) // comprimento minimo para qualquer busca
            {
                switch (tipo)
                {
                    case TipoCliente.NENHUM:
                        throw new Exception("Nenhum tipo de Cliente selecionado");
                    case TipoCliente.CPFCNPJ:
                        cliente = EF.Repositorio.Listar<tbCliente>(c => c.Documento1 == valor).FirstOrDefault();
                        break;
                    case TipoCliente.EMAIL:
                        cliente = EF.Repositorio.Listar<tbCliente>(c => c.Email != null && c.Email.ToLower() == valor).FirstOrDefault();
                        break;
                    case TipoCliente.TELEFONE:
                        cliente = BuscarClientePorTelefone(valor + "%");
                        break;
                }
            }

            if (cliente != null)
                return cliente;

            if (tipo != TipoCliente.CPFCNPJ // Apenas para não buscar duas vezes!
             && !string.IsNullOrEmpty(novo?.Documento1))
                // Sempre valida a existencia de outro CPF/CNPJ
                cliente = EF.Repositorio.Listar<tbCliente>(c => c.Documento1 == novo.Documento1).FirstOrDefault();

            if (cliente != null)
                return cliente;

            cliente = novo ?? new tbCliente
            {
                NomeCompleto = "",
                Email = "",
                Documento1 = "",
            };

            switch (tipo)
            {
                case TipoCliente.CPFCNPJ:
                    if (string.IsNullOrEmpty(cliente.NomeCompleto))
                        cliente.NomeCompleto = "CPF: " + valor;

                    cliente.Documento1 = valor;
                    break;
                case TipoCliente.EMAIL:
                    if (string.IsNullOrEmpty(cliente.NomeCompleto))
                        cliente.NomeCompleto = "e-mail: " + valor;

                    cliente.Email = valor;
                    break;
                case TipoCliente.TELEFONE:
                    if (string.IsNullOrEmpty(cliente.NomeCompleto))
                        cliente.NomeCompleto = "Telefone: " + valor;

                    try
                    {
                        if (valor.Length < 10 || valor.Length > 12)
                            cliente.Telefone1Numero = Convert.ToInt32(valor);
                        else
                        {
                            cliente.Telefone1DDD = Convert.ToInt32(valor.Substring(0, 2));
                            cliente.Telefone1Numero = Convert.ToInt32(valor.Substring(2));
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("valor", valor);
                        throw new Exception("O telefone precisa ser numérico", ex);
                    }
            }

            if (cliente.IDCliente == 0)
            {
                cliente.DtInclusao = DateTime.Now;
                cliente.Bloqueado = false;

                if (cliente.Telefone1DDD == null) cliente.Telefone1DDD = ConfiguracoesSistema.Valores.DDDPadrao;
                if (cliente.Telefone1Numero == null) cliente.Telefone1Numero = 0;

                EF.Repositorio.Inserir(cliente);
            }
            else
                EF.Repositorio.Atualizar(cliente);

            return cliente;
        }

        private static tbCliente BuscarClientePorTelefone(string valor)
        {
            string sql = @"SELECT * 
FROM tbCliente
WHERE 
	((NOT Telefone1DDD IS NULL) AND 
	 (NOT Telefone1Numero IS NULL) AND
	 CONCAT(Telefone1DDD, Telefone1Numero) LIKE @valor)
	OR
	((NOT Telefone2DDD IS NULL) AND 
     (NOT Telefone2Numero IS NULL) AND
	 CONCAT(Telefone2DDD, Telefone2Numero) LIKE @valor)";
            return EF.Repositorio.Query<tbCliente>(sql, new SqlParameter("@valor", valor)).FirstOrDefault();
        }

        /// <summary>
        /// Validação dos campos obrigatórios de clientes
        /// </summary>
        /// <remarks>
        /// Código de Teste: T21
        /// </remarks>
        public static string ValidarCliente(int idCliente, string nome, string endereco, string complemento, string bairro, string cidade, string cpfcnpj, string rg, string nascimento, bool delivery)
        {
            string msg = "";

            // Nome
            if (string.IsNullOrEmpty(nome))
                msg += "Campo \"Nome\" é obrigatório\n";
            else if (nome.Trim().Length < 2)
                msg += "Campo \"Nome\" deve ter ao menos 2 letras\n";
            else if (nome.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido no nome do cliente, verifique os acentos, simbolos e espaços em branco\n";

            // Validação de caracteres especiais no endereço
            if (endereco != null && endereco.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido no endereço\n";
            if (complemento != null && complemento.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido no complemento\n";
            if (bairro != null && bairro.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido no bairro\n";
            if (cidade != null && cidade.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido na cidade\n";

            // CPF
            if (!delivery && ConfiguracoesSistema.Valores.ClienteCPFObrigatorio && string.IsNullOrEmpty(cpfcnpj))
                msg += "Campo \"CPF/CNPJ\" obrigatório\n";
            else if (!string.IsNullOrEmpty(cpfcnpj) && !ValidacaoDocumento.IsCpf(cpfcnpj) && !ValidacaoDocumento.IsCnpj(cpfcnpj))
                msg += "Campo \"CPF/CNPJ\" inválido\n";
            else if (!string.IsNullOrEmpty(cpfcnpj))
            {
                var outros = EF.Repositorio.Contar<tbCliente>(c => c.Documento1 == cpfcnpj && c.IDCliente != idCliente);
                if (outros > 0)
                {
                    var plural = outros == 1 ? "" : "s";
                    msg += $"Há {outros} cliente{plural} cadastrado{plural} com esse CPF\n";
                }
            }

            // RG
            if (!delivery && ConfiguracoesSistema.Valores.ClienteRGObrigatorio && string.IsNullOrEmpty(rg))
                msg += "Campo \"RG\" obrigatório\n";

            // Data de Nascimento
            DateTime dt = DateTime.MinValue;
            if (!delivery && ConfiguracoesSistema.Valores.ClienteDataNascimentoObrigatorio && string.IsNullOrEmpty(nascimento))
                msg += "Campo \"Data de Nascimento\" obrigatória\n";
            else if (!string.IsNullOrEmpty(nascimento) && !DateTime.TryParse(nascimento, out dt))
                msg += "Campo \"Data de nascimento\" inválido\n";
            else if (dt != DateTime.MinValue && (dt.Year <= 1900 || dt.Year > dt.Year))
                msg += "Campo \"Data de nascimento\" com valor inválido\n";

            return msg;
        }

        public enum TipoCliente
        {
            NENHUM = -1,
            CPFCNPJ = 0,
            EMAIL = 1,
            TELEFONE = 2
        }

        internal static void Fill(PedidoInformation pedido, ImpressaoHelper h)
        {
            if (pedido.Cliente != null && pedido.Cliente.IDCliente.HasValue)
            {
                if (string.IsNullOrEmpty(pedido.Cliente.NomeCompleto))
                    pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                h.ClienteNome = pedido.Cliente.NomeCompleto;
                h.plain.AppendLine("Cliente: " + h.ClienteNome);
                h.EnderecoCompleto = $"{pedido.Cliente.Endereco}, {pedido.Cliente.EnderecoNumero} {pedido.Cliente.Complemento}";
                h.Bairro = pedido.Cliente.Bairro;
                h.Cidade = pedido.Cliente.Cidade;
                h.CEP = pedido.Cliente.CEP?.ToString();
                h.Telefone = pedido.Cliente.Telefone1;
                h.Observacoes = pedido.Cliente.Observacao;
                h.ObservacaoCupom = pedido.ObservacaoCupom;
                h.EnderecoReferencia = pedido.Cliente.EnderecoReferencia;
            }
        }

        internal static int ClienteDraw(this Graphics g, ImpressaoHelper dados, int Y, Font fNormal, int totalWidth)
        {
            if (string.IsNullOrWhiteSpace(dados.ClienteNome) == false || string.IsNullOrWhiteSpace(dados.Telefone) == false)
            {
                Y += Constantes.Espacamento;
                Y += g.DrawText(dados.ClienteNome ?? "", string.IsNullOrEmpty(dados.Telefone) ? null : $"Tel: {dados.Telefone}", fNormal, Y, totalWidth);
            }

            if (string.IsNullOrWhiteSpace(dados.EnderecoCompleto) == false)
                Y += g.DrawText(dados.EnderecoCompleto, null, fNormal, Y, totalWidth);

            string bairroCidadeCep = dados.Bairro ?? "";
            if (string.IsNullOrWhiteSpace(dados.Cidade) == false)
            {
                if (string.IsNullOrWhiteSpace(bairroCidadeCep))
                    bairroCidadeCep = dados.Cidade;
                else
                    bairroCidadeCep += " - " + dados.Cidade;
            }

            if (string.IsNullOrWhiteSpace(dados.CEP) == false)
                bairroCidadeCep += " CEP: " + dados.CEP;

            Y += g.DrawText(bairroCidadeCep, null, fNormal, Y, totalWidth);

            if (string.IsNullOrWhiteSpace(dados.EnderecoReferencia) == false)
                Y += g.DrawText(dados.EnderecoReferencia, null, fNormal, Y, totalWidth);

            return Y;
        }
    }
}