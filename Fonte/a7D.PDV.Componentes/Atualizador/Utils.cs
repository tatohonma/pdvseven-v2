using a7D.PDV.Integracao.API2.Client;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace a7D.PDV.Componentes
{
    public delegate void UpdateTextDelegate(string text);

    public class Utils
    {
        public const string ConnectionName = "connectionString";
        public const string ProviderName = "System.Data.SqlClient";

        public static void ClearAllPDV7process()
        {
            var meuProcesso = Process.GetCurrentProcess().Id;

            var processos = Process.GetProcesses();
            foreach (Process processo in processos)
            {
                if (processo.ProcessName.StartsWith("a7D.PDV.", StringComparison.InvariantCultureIgnoreCase)
                 || processo.ProcessName.StartsWith("w3wp", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (meuProcesso != processo.Id)
                    {
                        try
                        {
                            if (!processo.HasExited)
                                processo.Kill();
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("ProcessName", processo.ProcessName);
                            throw ex;
                        }
                    }
                }
            }
        }

        public static void CopyTo(string source, string dest, bool lOverride)
        {
            var origem = new DirectoryInfo(source);
            var destino = new DirectoryInfo(dest);

            // Só os diretórios e este mesmo recursivamente
            foreach (var subDest in origem.GetDirectories())
            {
                var subNew = new DirectoryInfo(Path.Combine(destino.FullName, subDest.Name));
                if (subNew.Exists)
                {
                    if (!lOverride)
                        throw new Exception("Diretório já existe");
                }
                else
                    subNew.Create();

                CopyTo(subDest.FullName, subNew.FullName, lOverride);
            }

            // Só os arquivos
            foreach (var fi in origem.GetFiles())
                fi.CopyTo(Path.Combine(dest, fi.Name), lOverride);
        }

        public static void Deltree(string source)
        {
            var origem = new DirectoryInfo(source);

            // Só os diretórios e este mesmo recursivamente
            foreach (var subDest in origem.GetDirectories())
                Deltree(subDest.FullName);

            // Só os arquivos
            foreach (var fi in origem.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception)
                {
                }
            }

            try
            {
                origem.Delete();
            }
            catch (Exception)
            {
            }
        }

        public static void CreateZipFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, Predicate<string> filter)
        {
            var fi = new FileInfo(destinationArchiveFileName);
            if (fi.Exists) fi.Delete();
            if (!fi.Directory.Exists) fi.Directory.Create();

            int n = sourceDirectoryName.Length + 1;
            var filesToAdd = Directory.GetFiles(sourceDirectoryName, "*", SearchOption.AllDirectories);
            var entryNames = filesToAdd.Select(f => f.Substring(n)).ToArray();
            using (var zipFileStream = new FileStream(destinationArchiveFileName, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                {
                    for (int i = 0; i < filesToAdd.Length; i++)
                    {
                        if (!filter(entryNames[i]))
                            continue;

                        archive.CreateEntryFromFile(filesToAdd[i], entryNames[i]);
                    }
                }
            }
        }

        private static void ConfigExec(string exeName)
        {
            var exe = new FileInfo(exeName);
            if (!exe.Exists)
            {
                return;
            }

            var config = ConfigurationManager.OpenExeConfiguration(exe.FullName);

            var conn = config.ConnectionStrings.ConnectionStrings[ConnectionName];
            if (conn == null)
            {
                var sql = UDPClient.Send("SQLDB");
                if (!string.IsNullOrEmpty(sql))
                {
                    var cnString = MakeConnection(sql);
                    if (!string.IsNullOrEmpty(cnString))
                    {
                        var newCN = new ConnectionStringSettings(ConnectionName, cnString, ProviderName);
                        config.ConnectionStrings.ConnectionStrings.Add(newCN);
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                }
            }
        }

        private static string MakeConnection(string servidorDBcatalog)
        {
            var prms = servidorDBcatalog.Split('|');

            string servidor = @".";
            string instancia = @"PDV7";
            string catalog = "PDV7";

            if (prms.Length >= 1)
                servidor = prms[0];

            if (prms.Length >= 2)
                instancia = prms[1];

            if (prms.Length >= 3)
                catalog = prms[2];

            var ips = NetworUtil.GetAllIP(out string log);
            if (ips.Contains(servidor))
                servidor = ".";

            if (!string.IsNullOrEmpty(instancia))
                instancia = @"\" + instancia;

            string connectionString = $@"Data Source={servidor}{instancia};Initial Catalog={catalog};Persist Security Info=false;User ID=pdv7;Password=pdv@77";

            try
            {
                var conn = new SqlConnection(connectionString);

                conn.Open();
                conn.Close();

                return connectionString;
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
