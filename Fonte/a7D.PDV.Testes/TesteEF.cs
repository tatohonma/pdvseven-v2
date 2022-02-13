using a7D.PDV.EF.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TesteEF
    {
        // Analise:
        //   O tempo médio da primeira conexão ao banco é de 4.5 segundos
        //   Usar o contexto diretamente é com certeza mais rápido do que fazer a busca em repositório
        //   Mas estamos falando de alguns milisegundos, onde em 100 vezes em 2 treads, os tempos foram sempre bem proximos
        //   Usar controles de transação deixou mais lento de forma geral
        // Conclusão:
        //   Vale a pena continuar usando da forma que está pela praticidade, performance e padronização
        //   .AsNoTracking() responde um pouco mais rápido e foi implementado para listas rápidas
        // Veja mais em:
        //   https://pt.stackoverflow.com/questions/113698/busca-de-dados-com-entity-framework
        [TestMethod, TestCategory("EF")]
        public void Consulta_Performance()
        {
            // Apenas para inicializar todos os contrutores internos e conexão
            using (var pdv = new pdv7Context())
                pdv.TipoPDVs.ToArray();

            var st = new Stopwatch();
            st.Start();

            var t1 = Task.Run(() =>
            {
                //using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    //using (var pdv = new pdv7Context())
                    {
                        var s1 = new Stopwatch();
                        for (int n = 0; n < 100; n++)
                        {
                            s1.Restart();
                            var produtos = EF.Repositorio.Contar<tbProduto>();
                            //var produtos = EF.Repositorio.ListarFast<tbProduto>();
                            //var produtos = pdv.tbProdutoes.AsNoTracking().ToArray();
                            Console.WriteLine($"A: {n} - {s1.ElapsedMilliseconds.ToString("N0")}");
                        }
                    }
                }
            });

            var t2 = Task.Run(() =>
            {
                //using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    //using (var pdv = new pdv7Context())
                    {
                        var s2 = new Stopwatch();
                        for (int n = 0; n < 100; n++)
                        {
                            s2.Restart();
                            var produtos = EF.Repositorio.Contar<tbProduto>();
                            //var produtos = pdv.tbProdutoes.AsNoTracking().ToArray();
                            //var produtos = pdv.tbProdutoes.ToArray();
                            Console.WriteLine($"B: {n} - {s2.ElapsedMilliseconds.ToString("N0")}");
                        }
                    }
                }
            });

            Task.WaitAll(t1, t2);
            //Task.WaitAll(t1 );

            Console.WriteLine("Total: " + st.ElapsedMilliseconds.ToString("N0"));
        }
    }
}
