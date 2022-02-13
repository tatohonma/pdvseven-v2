using a7D.PDV.BackOffice.UI.Sintegra;
using a7D.PDV.Fiscal.NFCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFe.Utils.NFe;
using System;
using System.IO;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestNFCe
    {
        // http://www.sped.fazenda.mg.gov.br/spedmg/nfce/
        // https://www.sefaz.rs.gov.br/nfe/nfe-val.aspx  <= RS !
        // http://validadornfe.tecnospeed.com.br/
        // http://www.semx.com.br/
        // http://nfce.fazenda.mg.gov.br/portalnfce/sistema/consultaarg.xhtml
        // NFe31190432040741000192650000000165181000165189
        // ---12345678901234567890123456789012345678901234

        MyTesteNFCe nfce;

        [TestInitialize]
        public void Start()
        {
            NFeFacade.ConfigPathXSD(new FileInfo(GetType().Assembly.Location).Directory.FullName);
            nfce = new MyTesteNFCe();
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Gerar_Assina_Valida_Envia()
        {
            var nfe = nfce.ObterNfeValidada(9);
            var result = nfce.ObterNfeValidadaEnvia(nfe);
            Console.WriteLine("chNFe: " + result.protNFe.infProt.chNFe);
            Console.WriteLine("xMotivo:" + result.protNFe.infProt.xMotivo);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Ler_Assina_Valida_Envia()
        {
            var xml = nfce.LerNfeValidadaEnvia(@"..\..\Venda-NFCe.xml", 3);
            //var xml = nfce.LerNfeValidadaEnvia(@"..\..\VendaPDV-NFCe.xml", 4);
            Console.WriteLine(xml);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Consulta_Protocolo()
        {
            var result = NFeFacade.Consulta("31190532040741000192650020000000191000000193");
            Assert.IsNotNull(result.protNFe);
            Console.WriteLine(result.protNFe.infProt.xMotivo);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Consulta_Status()
        {
            var result = NFeFacade.ConsultarStatusServico();
            Console.WriteLine($"{result.cUF} - {result.tpAmb} {result.versao} - {result.xMotivo}");
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Consulta_Cadastro()
        {
            var result = NFeFacade.ConsultaCadastro();
            Console.WriteLine(result.RetornoCompletoStr);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Consulta_Cancela()
        {
            //var chave = "31190532040741000192650020000000191000000193";
            //var result = NFeFacade.Consulta(chave);
            //Assert.IsNotNull(result.protNFe);

            var nfe = nfce.ObterNfeValidada(9);
            var result = nfce.ObterNfeValidadaEnvia(nfe);
            Console.WriteLine(result.NFe.infNFe.Id);

            var cancelamento = NFeFacade.Cancelar(result.protNFe, "teste homologação");

            Console.WriteLine(cancelamento.ProcEventosNFe[0].retEvento.infEvento.Id);
            Console.WriteLine(cancelamento.ProcEventosNFe[0].retEvento.infEvento.xMotivo);
            Console.WriteLine(cancelamento.RetornoCompletoStr);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Gerar_OffLine()
        {
            var nfe = nfce.ObterNfeValidada(21);
            var xml = NFeFacade.EmitirOffline(nfe, "teste de emissão offline em ambiente de homologação");
            NFeFacade.Imprimir(xml, @"..\..\Venda-Cupom-offline.png");

            var retorno = NFeFacade.Enviar(nfe);
            //Console.WriteLine("chNFe: " + result.protNFe.infProt.chNFe);
            //Console.WriteLine("xMotivo:" + result.protNFe.infProt.xMotivo);
        }

        [TestMethod, TestCategory("NFCe")]
        public void NFCe_Sintegra_Gerar()
        {
            string txt = SintegraServices.Gerar(DateTime.Now);
            File.WriteAllText(@"..\..\sintegra.txt", txt);
            Console.WriteLine(txt);
        }
    }
}