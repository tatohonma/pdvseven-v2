using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Web.UI;

namespace a7D.PDV.BigData.WebAPI.Tests.Iaago
{
    public class TesteInfo
    {
        public string nome { get; set; }
        public int idade { get; set; }
        public bool ativo { get; set; }

        public Func<int, object> calc;
    }

    [TestClass]
    public class EvalTest
    {
        readonly TesteInfo obj = new TesteInfo()
        {
            nome = "Fabio",
            idade = 41,
            ativo = false
        };

        [TestMethod, TestCategory("Iaago")]
        public void DataBinder_Eval_Test()
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/index
            // https://github.com/davideicardi/DynamicExpresso/

            Console.WriteLine(DataBinder.Eval(obj, "nome"));
            Console.WriteLine(DataBinder.Eval(obj, "idade>20"));
        }

        [TestMethod, TestCategory("Iaago")]
        public void CSharpScript_Expression_Test()
        {
            // https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples
            // https://github.com/dotnet/roslyn/wiki/Roslyn-Overview#compiler-pipeline-functional-areas
            // https://www.strathweb.com/2018/01/easy-way-to-create-a-c-lambda-expression-from-a-string-with-roslyn/
            var sw = new Stopwatch();

            Console.Write(CSharpScript.EvaluateAsync("1 + 2").Result);
            Console.WriteLine($" ({sw.ElapsedMilliseconds}ms)\r\n");
            sw.Restart();

            var script = CSharpScript.
                Create<int>("int x = 2;").
                ContinueWith("int y = 3;").
                ContinueWith("x + y");

            Console.Write(script.RunAsync().Result.ReturnValue);
            Console.WriteLine($" ({sw.ElapsedMilliseconds}ms)\r\n");
            sw.Restart();

            var state = CSharpScript.RunAsync<int>("int answer = 42;int eu = 21;").Result;
            foreach (var variable in state.Variables)
                Console.WriteLine($"{variable.Name} = {variable.Value} of type {variable.Type}");

            Console.WriteLine($" ({sw.ElapsedMilliseconds}ms)\r\n");
            sw.Restart();

            // var scriptObj = CSharpScript.Create("nome + \"  tem \" + idade.ToString() +\" anos\"", null, typeof(TesteInfo));

            string expressao = "$'{nome} tem {idade} anos, e calc({idade})={calc(idade)}'".Replace("'","\"");
            var scriptObj = CSharpScript.Create(expressao, null, typeof(TesteInfo));

            scriptObj.Compile();
            Console.WriteLine($"Compile ({sw.ElapsedMilliseconds}ms)\r\n");

            for (int i = 10; i < 20; i++)
            {
                sw.Restart();
                var valor = new TesteInfo
                {
                    nome = "ferreira",
                    idade = i,
                    calc = v => v * 10
                };

                Console.Write(scriptObj.RunAsync(valor).Result.ReturnValue);
                Console.WriteLine($" ({sw.ElapsedMilliseconds}ms)");
            }
        }
    }
}
