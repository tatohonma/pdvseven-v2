using a7D.PDV.BLL;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.EasyChopp.Model;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.EasyChopp
{
    public partial class IntegraEasyChopp : IntegracaoTask
    {
        public override string Nome => "EasyChopp";

        private ConfiguracoesEasyChopp config;

        private int idPDV;

        const int TipoIntegracaoEasyChopp = (int)ETipoIntegracao.EasyChopp;

        public override void Executar()
        {
            Configurado = false;
            Disponivel = BLL.PDV.PossuiEasyChopp();
            if (!Disponivel)
            {
                AddLog("Sem Licenças para EasyChopp");
                return;
            }

            var pdv = BLL.PDV.Listar().First(p => p.TipoPDV.Tipo == ETipoPDV.EASYCHOPP && p.Ativo == true);
            idPDV = pdv.IDPDV.Value;

            config = new ConfiguracoesEasyChopp();
            if (!config.IntegracaoEasyChopp)
            {
                AddLog("Integração EasyChopp desligada");
                return;
            }

            if (string.IsNullOrEmpty(config.EmailUsuario)
             || string.IsNullOrEmpty(config.ChaveSeguranca)
             || config.CategoriaCredito <= 0)
            {
                AddLog("Falta configurar o EasyChopp API para subir compras de créditos");
                return;
            }

            EasyChoppServices.ConfigEasyChoppServices(config.URLBase, config.ChaveSeguranca, config.EmailUsuario);

            Configurado = true;
            Iniciar(() => Loop());
        }

        private void Loop()
        {
            try
            {
                AddLog("Aguardando pedidos!");
                while (Executando)
                {
                    Sleep(10);

                    if (!IntegraClientes())
                        continue;

                    IntegraPedidos();
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EE31, ex);
            }
        }

        private void IntegraPedidos()
        {
            string sql = $@"SELECT pp.IDPedidoProduto, p.IDCliente, pp.Quantidade * pp.ValorUnitario Valor
FROM tbPedidoProduto pp
INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido
INNER JOIN tbProdutoCategoriaProduto pc ON pp.IDProduto=pc.IDProduto
WHERE pc.IDCategoriaProduto={config.CategoriaCredito} AND NOT p.IDCliente IS NULL
AND NOT pp.IDPedidoProduto IN (SELECT IDInterno 
                               FROM tbIntegracao
                               WHERE Tabela='tbPedidoProduto' AND IDTipo={TipoIntegracaoEasyChopp})";

            var novos = Repositorio.Query<ItensCredito>(sql);
            //AddLog("Novos registros a integrar: " + novos.Length);
            if (novos.Length == 0)
                return;

            foreach (var novo in novos)
            {
                var clientePDV = Repositorio.Carregar<tbCliente>(c => c.IDCliente == novo.IDCliente);
                if (clientePDV == null)
                {
                    continue;
                }
                else if (string.IsNullOrEmpty(clientePDV.Documento1))
                {
                    AddLog("Cliente sem Documento: " + clientePDV.NomeCompleto);
                    continue;
                }

                int idCliente = clientePDV.IDCliente;
                var comanda = Repositorio.Carregar<tbComanda>(c => c.IDCliente == idCliente);
                if (comanda == null)
                {
                    AddLog($"Cliente '{clientePDV.Documento1}' sem comanda");
                    continue;
                }
                else if (comanda.Codigo == null)
                {
                    AddLog($"Cliente '{clientePDV.Documento1}' na comanda '{comanda.Numero}' sem TAG");
                    continue;
                }
                string tagHEX = comanda.Codigo.Value.ToString("X");

                try
                {
                    AddLog($"Buscando cliente '{clientePDV.Documento1}' no EasyChoop");
                    var clienteEasyChopp = EasyChoppServices.GetClienteDocumento(clientePDV.Documento1);
                    //var clinteEasyChopp = EasyChoppServices.GetClienteTAG(tag);
                    if (!clienteEasyChopp.stIntegracao)
                    {
                        AddLog($"Adicionando cliente '{clientePDV.Documento1}' no EasyChoop");
                        var retCliente = EasyChoppServices.AddClienteDocumento(clientePDV.Documento1, clientePDV.NomeCompleto, clientePDV.Email, clientePDV.Sexo, clientePDV.DataNascimento);
                        //var retCliente = EasyChoppServices.AddClienteTAG(tag, cliente.NomeCompleto, cliente.Email, cliente.Sexo, cliente.DataNascimento);
                        // no proximo inicio de loop o sistema irá integrar o novo cliente adicionado
                        if (!retCliente.stIntegracao)
                        {
                            AddLog($"Erro '{retCliente}' ao inserir cliente '{clientePDV.NomeCompleto}': {retCliente.dsError}");
                            continue;
                        }
                    }

                    if (clienteEasyChopp.idCliente > 0)
                    {
                        // Garante o histórico na integração
                        var clienteIntegrado = Repositorio.Carregar<tbIntegracao>(
                            c => c.IDTipo == TipoIntegracaoEasyChopp
                              && c.IDExterno == clienteEasyChopp.idCliente.ToString()
                              && c.Tabela == "tbCliente");

                        if (clienteIntegrado == null)
                        {
                            clienteIntegrado = new tbIntegracao()
                            {
                                IDTipo = TipoIntegracaoEasyChopp,
                                dtMovimento = DateTime.Now,
                                Tabela = "tbCliente",
                                IDInterno = clientePDV.IDCliente,
                                Operacao = "I",
                                IDExterno = clienteEasyChopp.idCliente.ToString(),
                            };
                            Repositorio.Inserir(clienteIntegrado);
                        }
                    }

                    AddLog($"Inserindo R$ {novo.Valor.ToString("N2")} ao cliente '{clientePDV.Documento1}' no EasyChoop");
                    var retorno = EasyChoppServices.AddCreditoDocumento(clientePDV.Documento1, FormaPagamento.OnLine, novo.Valor, "integracao", "");
                    //var retorno = EasyChoppServices.AddCreditoTAG(tag, FormaPagamento.OnLine, novo.Valor, "integracao", "");
                    if (!retorno.stIntegracao)
                    {
                        AddLog($"Erro ao adicionar o crédito '{novo.IDPedidoProduto}' cliente '{clientePDV.NomeCompleto}': {retorno.dsError}");
                        continue;
                    }

                    AddLog($"Concluído! Credito: {clientePDV.NomeCompleto}  Item: {novo.IDPedidoProduto}  +R$ {novo.Valor.ToString("N2")} adicionado ao EasyChopp!");
                    var integracao = new tbIntegracao()
                    {
                        IDTipo = TipoIntegracaoEasyChopp,
                        dtMovimento = DateTime.Now,
                        Tabela = "tbPedidoProduto",
                        IDInterno = novo.IDPedidoProduto,
                        Operacao = "I",
                        IDExterno = "x",
                    };
                    Repositorio.Inserir(integracao);

                    if (clienteEasyChopp.idTAG == null || clienteEasyChopp.idTAG != tagHEX)
                    {
                        retorno = EasyChoppServices.DefineClienteTAG(clientePDV.Documento1, tagHEX);
                        if (!retorno.stIntegracao)
                        {
                            AddLog($"Erro ao relelacionar a TAG '{tagHEX}' ao cliente '{clientePDV.Documento1}': {retorno.dsError}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddLog(ex);
                }
            }
        }

        private bool IntegraClientes()
        {
            var dtMax = Repositorio.ExecuteScalar<DateTime?>("SELECT MAX(dtMovimento) FROM tbIntegracao WHERE Tabela='tbCliente'");
            if (dtMax == null)
                dtMax = new DateTime(1950, 1, 1);

            var clientes = EasyChoppServices.GetClientes(dtMax.Value, out string dsErro);
            if (dsErro != null)
            {
                AddLog(dsErro);
                return false;
            }

            if (clientes == null)
                return true;

            foreach (var clienteEasyChopp in clientes)
            {
                if (clienteEasyChopp.dsTipoDocumento != "CPF")
                {
                    AddLog("Tipo de Documento Invalido: " + clienteEasyChopp.dsTipoDocumento + " - " + clienteEasyChopp);
                    continue;
                }

                var documento = clienteEasyChopp.nrDocumento.SoNumeros();
                if (string.IsNullOrEmpty(documento) || documento.Length < 11)
                {
                    AddLog("Número de documento inválido: " + documento + " - " + clienteEasyChopp);
                    continue;
                }

                var clienteIntegrado = Repositorio.Carregar<tbIntegracao>(c => c.IDTipo == TipoIntegracaoEasyChopp && c.IDExterno == clienteEasyChopp.idCliente.ToString() && c.Tabela == "tbCliente");
                if (clienteIntegrado == null)
                {
                    var novo = new tbCliente()
                    {
                        NomeCompleto = clienteEasyChopp.dsNomeCliente,
                        Email = clienteEasyChopp.dsEmail,
                        Sexo = clienteEasyChopp.dsSexo,
                        Documento1 = documento
                    };

                    var tel = clienteEasyChopp.nrTelefone.SoNumeros();
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Length > 11) // igora outros digitos como ramal, etc...
                        {
                            novo.Observacao = clienteEasyChopp.nrTelefone;
                            tel = tel.Substring(0, 11);
                        }

                        if (tel.Length >= 10) //2+8 ou 2+9
                        {
                            novo.Telefone1DDD = int.Parse(tel.Substring(0, 2));
                            novo.Telefone1Numero = int.Parse(tel.Substring(2));
                        }
                        else
                        {
                            novo.Telefone1DDD = ConfiguracoesSistema.Valores.DDDPadrao;
                            novo.Telefone1Numero = int.Parse(tel.Substring(2));
                        }
                    }

                    var clientePDV = BLL.Cliente.BuscarCliente(BLL.Cliente.TipoCliente.CPFCNPJ, documento, novo);
                    clienteIntegrado = new tbIntegracao()
                    {
                        IDTipo = TipoIntegracaoEasyChopp,
                        dtMovimento = DateTime.Now,
                        Tabela = "tbCliente",
                        IDInterno = clientePDV.IDCliente,
                        Operacao = "I",
                        IDExterno = clienteEasyChopp.idCliente.ToString(),
                    };

                    Repositorio.Inserir(clienteIntegrado);
                }

                if (clienteEasyChopp.idTAG != null)
                {
                    try
                    {
                        var idTag = Comanda.HexaOuDecimalOuZero(clienteEasyChopp.idTAG, true);
                        if (idTag == 0)
                        {
                            AddLog("TAG inválida " + clienteEasyChopp.idTAG.ToUpper());
                            continue;
                        }

                        var comanda = Comanda.CarregarPorNumeroOuCodigo(idTag);
                        if (comanda == null || comanda.IDComanda == null)
                        {
                            AddLog("Criando comanda para a TAG " + clienteEasyChopp.idTAG.ToUpper());
                            int maxComanda = Repositorio.ExecuteScalar<int>("SELECT MAX(Numero) FROM tbComanda");
                            var novaComanda = new tbComanda()
                            {
                                Numero = maxComanda + 1,
                                Codigo = idTag,
                                IDStatusComanda = (int)EStatusComanda.Liberada,
                                GUIDIdentificacao = Guid.NewGuid().ToString(),
                                IDCliente = clienteIntegrado.IDInterno
                            };
                            Repositorio.Inserir(novaComanda);
                            comanda = Comanda.Carregar(novaComanda.IDComanda);
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog(ex);
                    }
                }
            }

            return true;
        }
    }
}
