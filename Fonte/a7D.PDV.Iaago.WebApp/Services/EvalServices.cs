using System;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace a7D.PDV.Iaago.WebApp.Services
{
    public class EvalServices
    {
        public class TesteInfo
        {
            public string nome { get; set; }

            public int idade { get; set; }

            public bool ativo { get; set; }

            public Func<int, object> calc;
        }

        public static string Teste()
        {

            var sb = new StringBuilder();
            var sw = new Stopwatch();

            string expressao = "$'{nome} tem {idade} anos, e calc({idade})={calc(idade)}'".Replace("'", "\"");
            var scriptObj = CSharpScript.Create(expressao, null, globalsType: typeof(TesteInfo));

            scriptObj.Compile();
            Console.WriteLine($"Compile ({sw.ElapsedMilliseconds}ms)\r\n");

            for (int i = 10; i < 20; i++)
            {
                sw.Restart();
                var valor = new TesteInfo
                {
                    nome = "ferreira",
                    idade = i,
                    calc = v => v * 10,
                };

                sb.Append(scriptObj.RunAsync(valor).Result.ReturnValue);
                sb.AppendLine($" ({sw.ElapsedMilliseconds}ms)");
            }

            return sb.ToString();
        }
    }
}
