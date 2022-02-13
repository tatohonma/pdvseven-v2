using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestConfiguracao
    {
        [TestMethod]
        public void Configuracao_Leitura_Correta()
        {
            var config1 = ConfiguracaoBD.ConfiguracaoOuPadrao("GerarTicketPrePago");
            Assert.IsNull(config1);

            var config2 = ConfiguracaoBD.ConfiguracaoOuPadrao("GerarTicketPrePago", null, (int)ETipoPDV.AUTOATENDIMENTO);
            Assert.IsNotNull(config2);

            var config3 = ConfiguracaoBD.ConfiguracaoOuPadrao("GerarTicketPrePago", 58, (int)ETipoPDV.AUTOATENDIMENTO);
            Assert.IsNotNull(config3);
        }
    }
}