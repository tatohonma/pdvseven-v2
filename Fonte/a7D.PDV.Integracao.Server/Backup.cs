using a7D.PDV.BLL;
using a7D.PDV.DAL;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace a7D.PDV.Integracao.Server
{
    internal class Backup
    {
        static DateTime dtBackup = DateTime.MinValue;

        internal static void LimpaAntigos(OnMensagemListener AddInfo)
        {
            var dtLimite = DateTime.Now.AddDays(-30);
            var diEXE = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            var diBackup = diEXE.GetDirectories("backup");
            if (diBackup.Length == 0)
                return;
            var di = diBackup[0];
            var files = di.GetFiles("*.bak").OrderByDescending(f => f.LastWriteTime);
            int n = 0;
            foreach (var fi in files)
            {
                if (fi.LastWriteTime < dtLimite && ++n > 10)
                {
                    try
                    {
                        fi.Delete();
                        AddInfo("Excluido backup antigo: " + fi.FullName);
                    }
                    catch (Exception ex)
                    {
                        AddInfo($"Erro ao excluir backup antigo '{fi.FullName}': {ex.Message}");
                    }
                }
            }
        }

        internal static void Verifica(OnMensagemListener AddInfo, bool lForce = false)
        {
            if (!lForce && DateTime.Now.Subtract(dtBackup).TotalDays <= 1)
                return;

            // A cada 1 dia faz um backup
            dtBackup = DateTime.Now;
            try
            {
                AddInfo("Realizando Backup do SQL" + (lForce ? "!!!" : ""));

                string path = Assembly.GetExecutingAssembly().Location;
                path = path.Substring(0, path.LastIndexOf(@"\"));

                var fileBK = new FileInfo($@"{path}\backup\pdv7_{dtBackup.ToString("yyyyMMdd_HHmm")}.bak");
                if (!fileBK.Directory.Exists)
                    fileBK.Directory.Create();

                string sql = $@"BACKUP DATABASE PDV7 TO DISK = '{fileBK.FullName}' WITH FORMAT";
                EF.Repositorio.Execute(sql);
                AddInfo("Backup concluido em: " + fileBK.FullName);
            }
            catch (Exception ex)
            {
                AddInfo("ERRO ao realizar o Backup: " + ex.Message);
            }

            try
            {
                var size = DB.Size();
                var info = size + " MB";
                if (size > 4000)
                {
                    AddInfo("ATENÇÃO: Banco de dados está com " + info + " informar suporte!!!");
                    Logs.Erro(new ExceptionPDV(CodigoErro.E031, info));
                }
                else
                    AddInfo("Tamanho do banco de dados: " + info);
            }
            catch (Exception ex)
            {
                AddInfo("ERRO ao aobter o tamanho do banco de dados: " + ex.Message);
            }
        }
    }
}
