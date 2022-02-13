using a7D.PDV.Iaago.Dialogo;
using System;
using System.IO;
using System.Linq;

namespace a7D.PDV.Iaago.FluxoConsole
{
    class Program
    {
        static IaagoFluxo Fluxo;

        static IaagoVars userIaago;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Informe o nome do arquivo .json\r\nExemplo: IaagoFluxo iniciarConexao.json");
                return;
            }

            var fi = new FileInfo(args[0]);
            if (!fi.Exists)
            {
                Console.WriteLine($"Arquivo '{fi.FullName}' não existe");
                return;
            }

            userIaago = new IaagoVars();
            Fluxo = new IaagoFluxo();
            userIaago.Intencao = Fluxo.LoadFile(fi.FullName);

            userIaago.Add("ola", 123, false);
            userIaago.Add("usuario", "Ferreira", false);
            
            DialogoLoop();

            Console.WriteLine("FIM");
            Console.ReadKey();
        }

        public static void DialogoLoop()
        {
            IaagoRetorno ret;
            while (!string.IsNullOrEmpty(userIaago.Intencao))
            {
                var intencaoAlvo = Fluxo.ExecutaIntencao(userIaago);
                if (intencaoAlvo == null)
                {
                    Console.WriteLine($"Ops, não entendi a intenção '{userIaago.Intencao}', digite a intenção:");
                    userIaago.Intencao = Console.ReadLine();
                    continue;
                }

                Console.WriteLine(intencaoAlvo.ExibirMensagem(userIaago));

                if (intencaoAlvo.acaoResposta == null || intencaoAlvo.acaoResposta.Length == 0)
                {
                    Console.WriteLine($"Fim, qual sua intenção agora (ou vazio para sair):");
                    userIaago.Intencao = Console.ReadLine();
                    continue;
                }

                if(intencaoAlvo.acaoResposta?.Any(a=>a.tipoResposta!=null)==true)
                {
                    userIaago.Text = Console.ReadLine();
                }

                ret = intencaoAlvo.BuscaRetorno(userIaago);

                if (ret != null)
                {
                    Console.WriteLine(ret.ExibirMensagem(userIaago));
                }
            }
        }
    }
}
