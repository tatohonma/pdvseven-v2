using a7D.PDV.BLL;
using a7D.PDV.BLL.Ativacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestErro
    {
        [TestMethod, TestCategory("Ativador")]
        public void Erro_ExceptionPDV_Envio()
        {
            var ex = new ExceptionPDV(CodigoErro.EAA1,"sem origem");
            Task.WaitAll(ex.SendAsync()); 

            AC.RegitraLicenca("001-06079-14"); // Ferreira
            AC.RegitraPDV("TDD", "2.20.0.0", new Model.PDVInformation() { IDPDV = 1, Nome = "teste" });
            AC.RegitraUsuario(new Model.UsuarioInformation() { IDUsuario = 1, Nome = "teste" });

            var sw = new Stopwatch();
            sw.Start();

            var ex2 = new Exception("final EX 2");
            ex2.Data.Add("dado extra EX2", "aaa");

            var ex1 = new Exception("inner EX 1", ex2);
            ex1.Data.Add("extra EX1", "bbbbb");
            ex1.Data.Add("dado extra EX1", "ccccccccc OK");
            ex1.Data.Add("yyy", "zzz");

            var exPDV = new ExceptionPDV(CodigoErro.EE01, ex1, "TDD Erro");
            exPDV.Data.Add("yyy", "aaa");

            //Logs.Erro(exPDV);
            Task.WaitAll(exPDV.SendAsync());
            sw.Stop();

            Console.WriteLine("Tempo: " + sw.ElapsedMilliseconds + "ms");
            foreach (string key in exPDV.Data.Keys)
                Console.WriteLine($"{key}: {exPDV.Data[key]}");

            string result = exPDV.Data["SendAsync"]?.ToString();
            if (result != "Notificado" && result != "Registrado")
                Assert.Fail(result);
        }

        [TestMethod, TestCategory("Ativador")]
        public void Erro_ExceptionPDV_EnvioDireto()
        {
            AC.RegitraLicenca("001-06079-14"); // Ferreira
            AC.RegitraPDV("TDD", "2.20.0.0", new Model.PDVInformation() { IDPDV = 1, Nome = "teste" });
            AC.RegitraUsuario(new Model.UsuarioInformation() { IDUsuario = 1, Nome = "teste" });

            var ws = new wsUtil();
            string result = ws.Erro(AC.Chave, AC.Aplicacao, AC.Versao, AC.idPDV, CodigoErro.EE01.ToString(), "teste", "stack", "dados");
            Console.WriteLine(result);
        }
    }
}