using System.Diagnostics;
using System.Threading;

namespace a7D.PDV.Componentes
{
    public static class Processo
    {
        public static bool EmExecucao(string[] args)
        {
            if (args.Length == 1 && args[0] == "wait")
                Thread.Sleep(2000); // Apenas para garantir que o processo que chamou se encerrou

            if (args.Length == 2 && args[0] == "wait" && int.TryParse(args[1], out int pID))
            {
                try
                {
                    Process processToWait = null;
                    try
                    {
                        processToWait = Process.GetProcessById(pID);
                    }
                    catch (System.Exception)
                    {
                    }

                    if (processToWait != null)
                    {
                        if (!processToWait.WaitForExit(10000))
                        {
                            processToWait.Kill();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    BLL.Logs.Erro(BLL.CodigoErro.EAA0, ex);
                    return true; // Se mata
                }
            }

            if (Atualizacao.EmAtualizacao())
                return true;

            string meuProcesso = Process.GetCurrentProcess().ProcessName;

            // Procura o processo atual na lista de processos que estão a sendo executados neste momento
            var processos = Process.GetProcessesByName(meuProcesso);
            foreach (Process processo in processos)
            {
                if (processo.Id != Process.GetCurrentProcess().Id)
                    return true; // Se mata
            }

            return false; // continua
        }
    }
}
