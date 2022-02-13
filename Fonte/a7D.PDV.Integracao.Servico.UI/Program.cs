using a7D.PDV.BLL;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.UI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Obtém o nome do processo atual (esta aplicação)
            if (Componentes.Processo.EmExecucao(args))
                return;

            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory); 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (s, e) => Logs.ErroBox(CodigoErro.E030, e.Exception);

            Application.Run(new frmPrincipal());

            Environment.Exit(Environment.ExitCode); // Garante o termino completo da aplicação matando qualquer thead
        }
    }
}
