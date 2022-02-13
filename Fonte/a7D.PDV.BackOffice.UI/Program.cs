using a7D.PDV.BackOffice.UI.Properties;
using a7D.PDV.BLL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    static class Program
    {
        internal static bool Force = false;

        [STAThread]
        static int Main(params string[] args)
        {
            if (args?.Length > 0)
            {
                if (args.Contains("/force"))
                    Force = true;
            }

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (s, e) => Logs.ErroBox(CodigoErro.E010, e.Exception);

            // instalar vcredist_x86
            try
            {
                var redistInstalled = Settings.Default.VCRedistInstalado;
                if (!redistInstalled || Program.Force)
                {
                    var cmdArguments = "/install /passive /norestart";
                    var startInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        FileName = Path.Combine(Environment.CurrentDirectory, "redist", "vcredist_x86.exe"),
                        Arguments = cmdArguments,
                        WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "redist")
                    };
                    using (var p = Process.Start(startInfo))
                    {
                        p.WaitForExit();
                        if (p.ExitCode == 0)
                        {
                            Settings.Default.VCRedistInstalado = true;
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
                        Settings.Default.SQLSysClrTypesInstalado = true;
                        Settings.Default.Save();
                    }
                }
            }
            catch { }

            if (Componentes.Processo.EmExecucao(args))
                return 1;

            Application.Run(new frmPrincipal());

            Environment.Exit(Environment.ExitCode); // Garante o termino completo da aplicação matando qualquer thead
            return Environment.ExitCode;
        }
    }
}
