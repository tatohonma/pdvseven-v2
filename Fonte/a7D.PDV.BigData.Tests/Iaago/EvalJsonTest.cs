using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace a7D.PDV.BigData.WebAPI.Tests.Iaago
{
    [TestClass]
    public class EvalJsonTest
    {
        public class JsonIaago
        {
            public string intencao;
            public string mensagem;
            public IaagoResposta[] acaoResposta;

            public override string ToString() => intencao;
        }

        public class IaagoResposta
        {
            public string tipoResposta;
            public string[] api;
            public IaagoRetorno[] retorno;

            public override string ToString() => tipoResposta;
        }

        public class IaagoRetorno
        {
            public string[] condicao;
            public string mensagem;
            public string ignorarMensagemIntencao;
            public string intencao;

            public override string ToString() => intencao;
        }

        [TestMethod, TestCategory("Iaago")]
        public void Eval_Json_Test()
        {
            var fi = new FileInfo(@"Iaago\iniciarConexao2.json");
            if (!fi.Exists)
                Assert.Inconclusive("Arquivo não existe: " + fi.FullName);

            var sw = new Stopwatch();
            sw.Start();

            var json = File.ReadAllText(fi.FullName);
            Console.WriteLine($"Load: {sw.ElapsedMilliseconds}ms");
            sw.Restart();

            var iaago = JsonConvert.DeserializeObject<JsonIaago[]>(json);
            Console.WriteLine($"Parse: {sw.ElapsedMilliseconds}ms");
            sw.Restart();

            foreach (var a in iaago)
            {
                Console.WriteLine(a);
            }
        }
    }
}
