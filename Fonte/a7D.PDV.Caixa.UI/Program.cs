using a7D.PDV.BLL;
using a7D.PDV.Caixa.UI.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            if (Componentes.Processo.EmExecucao(args))
                return 0;

            //Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (s,e) => Logs.ErroBox(CodigoErro.E000, e.Exception);

            try
            {
                var sqlclrTypesInstalado = Settings.Default.SQLSysClrTypesInstalado;
                if (!sqlclrTypesInstalado)
                {
                    var cmdArguments = $"/i \"{Path.Combine(Environment.CurrentDirectory, "redist", "SQLSysClrTypes_x86.msi")}\" /passive /norestart";
                    var startInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        FileName = "msiexec",
                        Arguments = cmdArguments,
                        WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "redist")
                    };
                    using (var p = Process.Start(startInfo))
                    {
                        p.WaitForExit();
                        if (p.ExitCode == 0)
                        {
                            Settings.Default.SQLSysClrTypesInstalado = true;
                            Settings.Default.Save();
                        }
                        else if (p.ExitCode == 3010)
                        {
                            MessageBox.Show("É necessário reiniciar o computador para completar a instalação", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return 3010;
                        }
                        else
                        {
                            var erro = p.StandardError?.ReadToEnd();
                            MessageBox.Show(erro ?? $"Ocorreu um erro na instalação do VCRedist 2013 - Código {p.ExitCode}.\nPor favor reinicie o computador", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return p.ExitCode;
                        }
                    }
                }
            }
            catch { }
            Application.Run(new frmPrincipal());
            //BLL.EventLog.Info("Finalizando Caixa", "PDV7-Exit");

            Environment.Exit(Environment.ExitCode); // Garante o termino completo da aplicação matando qualquer thead
            return Environment.ExitCode;
        }
    }
}