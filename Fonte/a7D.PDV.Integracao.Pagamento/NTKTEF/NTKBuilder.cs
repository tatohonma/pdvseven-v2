using a7D.PDV.Integracao.Pagamento;
using System;
using System.Reflection;

namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    public class NTKBuilder
    {
        private const string Certificacao = "5828"; // Fornecido por Diego em 05/10/2018

        private NTKValores Valores;

        #region Builder

        public NTKBuilder(NTKEtapa etapa, NTKComandos comando, long identificacao)
        {
            Valores = new NTKValores(etapa, identificacao);
            Valores[NTKCampos.Comando] = comando.ToString();
        }

        public NTKBuilder Com(NTKCampos campo, string valor)
        {
            Valores[campo] = valor;
            return this;
        }

        public NTKBuilder Com(NTKCampos campo, long valor)
        {
            Valores[campo] = valor.ToString();
            return this;
        }

        public NTKBuilder Com(NTKCampos campo, int valor)
        {
            Valores[campo] = valor.ToString();
            return this;
        }

        public NTKBuilder Com(NTKCampos campo, decimal valor)
        {
            Valores[campo] = ((int)(valor * 100)).ToString();
            return this;
        }

        public NTKBuilder ComClienteCPF(long documento)
        {
            Valores[NTKCampos.EntidadeCliente] = "F";
            Valores[NTKCampos.IdentificadorCliente] = documento.ToString();
            return this;
        }

        public NTKBuilder ComClienteCNPJ(long documento)
        {
            Valores[NTKCampos.EntidadeCliente] = "J";
            Valores[NTKCampos.IdentificadorCliente] = documento.ToString();
            return this;
        }

        #endregion

        public static NTKBuilder CancelarTransacaoPendente()
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(NTKEtapa.ADM, NTKComandos.NCN, 1)
                .Com(NTKCampos.CapacidadesAutomacao, "255")
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao);
        }

        public static NTKBuilder Administar()
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(NTKEtapa.ADM, NTKComandos.ADM, 1)
                .Com(NTKCampos.CapacidadesAutomacao, "255")
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao);
        }
        
        public static NTKBuilder ConfirmarAdministracao(NTKValores valores)
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(NTKEtapa.ADM, NTKComandos.CNF, valores.Identificacao)
                .Com(NTKCampos.CapacidadesAutomacao, "255")
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao)
                .Com(NTKCampos.RedeAdquirente, valores[NTKCampos.RedeAdquirente])
                .Com(NTKCampos.CodigoControle, valores[NTKCampos.CodigoControle])
                ;
        }

        public static NTKBuilder AutorizacaoVenda(long identificacao,
            string estabelecimento, string terminal, DateTime data,
            long documento, decimal valorTotal, decimal valorServico)
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(NTKEtapa.VendaEnviada, NTKComandos.CRT, identificacao)
                .Com(NTKCampos.DocumentoFiscal, documento)
                .Com(NTKCampos.ValorTotal, valorTotal)
                .Com(NTKCampos.Moeda, 0)
                .Com(NTKCampos.TaxaServico, valorServico)
                .Com(NTKCampos.CodigoEstabelecimento, estabelecimento)
                .Com(NTKCampos.NumeroLogicoTerminal, terminal)
                .Com(NTKCampos.CapacidadesAutomacao, "255")
                //.Com(NTKCampos.DataHoraFiscal, data.ToString("yyMMddhhmmss"))
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao);
        }

        public static NTKBuilder ConfirmaVenda(NTKValores valores)
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(
                valores.Etapa = NTKEtapa.ConfirmacaoEnviada, NTKComandos.CNF, valores.Identificacao)
                .Com(NTKCampos.DocumentoFiscal, valores[NTKCampos.DocumentoFiscal])
                .Com(NTKCampos.RedeAdquirente, valores[NTKCampos.RedeAdquirente])
                .Com(NTKCampos.CodigoControle, valores[NTKCampos.CodigoControle])
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao);
        }

        public static NTKBuilder EstornaVenda(NTKValores valores)
        {
            string versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];

            return new NTKBuilder(
                valores.Etapa = NTKEtapa.ConfirmacaoEnviada, NTKComandos.CNC, valores.Identificacao)
                .Com(NTKCampos.DocumentoFiscal, valores[NTKCampos.DocumentoFiscal])
                .Com(NTKCampos.RedeAdquirente, valores[NTKCampos.RedeAdquirente])
                .Com(NTKCampos.CodigoControle, valores[NTKCampos.CodigoControle])
                .Com(NTKCampos.EmpresaAutomacao, AutomacaoConstantes.Empresa)
                .Com(NTKCampos.NomeAutomacao, AutomacaoConstantes.Nome)
                .Com(NTKCampos.VersaoAutomacao, versao)
                .Com(NTKCampos.RegistroCertificacao, Certificacao);
        }

        public static void Commit(NTKValores valores)
        {
            var req = new NTKWriter();
            foreach (var item in valores.Lista)
                req.Write(item.Key, item.Value);

            req.Commit();
        }

        public ITEF IniciaTransacao()
        {
            Commit(Valores);
            // Busca a resposta para a proxima etapa
            return new NTKPinpadPayGo(Valores, true);
        }

        public ITEF CriaTEF()
        {
            return new NTKPinpadPayGo(Valores, false);
        }
        
        public string VerRequisicao()
        {
            return Valores.ToString();
        }
    }
}