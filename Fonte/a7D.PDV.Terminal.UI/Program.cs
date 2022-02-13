using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (Componentes.Processo.EmExecucao(args))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (s, e) => Logs.ErroBox(CodigoErro.E020, e.Exception);

            if (!frmConfigurarConexaoDB.Verifica())
                return;

            using (var pdvServico = new Licencas())
            {
                try
                {
                    var serial = ValidacaoSistema.RetornarSerialHD();
                    pdvServico.Carregar(ETipoPDV.TERMINAL_WIN, serial);
                    new ConfiguracoesTerminalWindows(AC.PDV.IDPDV.Value);
                }
                catch (Exception ex)
                {
                    if (ex is ExceptionPDV exPDV && exPDV.CodigoErro.ToString().StartsWith("F"))
                    {
                        var frmValidacaoOffline = new frmValidacaoOffline();
                        frmValidacaoOffline.ShowDialog();
                    }
                    else
                        Logs.ErroBox(CodigoErro.E021, ex);

                    return;
                }
            }
            Application.Run(new frmPrincipal());

            Environment.Exit(Environment.ExitCode); // Garante o termino completo da aplicação matando qualquer thead
        }
    }
}