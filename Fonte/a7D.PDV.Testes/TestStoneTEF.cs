using System;
using a7D.PDV.Integracao.Pagamento.StoneTEF;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestStoneTEF
    {
        [TestInitialize]
        public void Stone_Start()
        {
            // string StoneCode = "185346049"; // TESTE
            //string StoneCode = "407709482"; // TESTE
            string StoneCode = "112634281"; // PRODUÇÃO PDVSeven

            AddLog("Ativando Pinpad");
            if (!AuthorizationCore.TryActivate(StoneCode, "PDVSeven Teste"))
                Assert.Fail("Stone Códe inválido");
        }

        [TestMethod, TestCategory("TEF")]
        public void Stone_Pagar_TEF()
        {
            AddLog("Iniciando Transação");
            var result = AuthorizationCore.Authorize(DateTime.Now.Second, 0.03m, false, "ferreira@pdvseven.com.br", s => AddLog(s));
            if (result == null)
                Assert.Fail("Erro no pagamento");

            Console.WriteLine(result);
            AddLog("Desligando Pinpad");
            AuthorizationCore.ClosePinpad();
            AddLog("Fim");
        }

        [TestMethod, TestCategory("TEF")]
        public void Stone_Cancelar_TEF()
        {
            AddLog("Iniciando Cancelamento");
            if (!AuthorizationCore.Cancel("25393624468103", 5m, out string result))
                Assert.Fail("Erro no pagamento: " + result);

            AddLog("Desligando Pinpad");
            AuthorizationCore.ClosePinpad();
            AddLog("Fim");
        }

        [TestMethod, TestCategory("TEF")]
        public void Stone_Linha_Comprovante()
        {
            Console.WriteLine(AuthorizationCore.ParValor("123", "6789"));
            Console.WriteLine(AuthorizationCore.ParValor("Empresa com um nome muito longo que será cortado", "23.562.488/0001-46"));

        }

        private void AddLog(string info)
        {
            info = $"{DateTime.Now.ToString("HH:mm:ss")} {info}";
            Console.WriteLine(info);
        }
    }
}
