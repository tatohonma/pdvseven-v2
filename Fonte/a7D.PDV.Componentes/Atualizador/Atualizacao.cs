using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Shared.Atualizacao;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;

namespace a7D.PDV.Componentes
{
    public static class Atualizacao
    {
        private static FileInfo FileEXE;
        private static DirectoryInfo PathEXE;
        public static string VersaoAtual { get; private set; }
        public static bool Cancel { get; set; }

        static Atualizacao()
        {
            var asm = Assembly.GetEntryAssembly();
            FileEXE = new FileInfo(asm.Location);
            PathEXE = FileEXE.Directory;
            VersaoAtual = asm.FullName.Split(',')[1].Split('=')[1];
        }
        /* 
        A atualização requer que o integrador já esteja atualizado, juntamente com o WS2 que proverá o arquivo
        Fluxo do processo de atualização!
        Somente o integrador tem o fluxo A e B diferenciado, mas o restante é igual

        Resumo dos Fluxos

        _) Verifica se há atualizaçao: (O integrador permite versões mais nova do cliente, mas não inferior)
        1) Entra no Fluxo da Atualização: Backup, Download, Descompactação dos arquivos
        2) Executa nova versão continuando: Substitui arquivos
        3) Apaga a parta de Update, e continua a execução normal

        Fluxos Completo do processo de atualização:

        __: Detecta se há atualização 
            (Recebe a URL de atualização, OK, se estiver atualizado, ou alguma mensagem de erro)
        1A: Realiza o backup dos arquivos da versão atual
        1B: Faz Download do update.zip do WS2
        1B1: Verifica a existencia do "a7D.PDV.Integracao.WS2.dll" no WWW_SAT e tenta apagar a pasta antes
        1C: Descompacta o update em uma subpasta 'update' e apaga o arquivo update.zip baixado
        1D: Copia a connectionstring dos executáveis para os novos
        1E: Cria o arquivo .OK da executáel indicando que o fluxo 1 foi concluida com sucesso

        __: Executa a nova versão dentro da parta de atualização
        2A: Finaliza todos processos A7D.PDV.* e W3WP*
        2B: Substitui os arquivos atuais pelo conteudo da subpasta 'update'
        2C: Apaga o arquivo .OK da executável liberadndo a limpeza do fluxo 3

        __: Executa a nova versão no diretório padrão
        3A: Renomeia a pasta Update para UpdateOK.Versão
            Continua a execução normal
        */
        public static bool EmAtualizacao()
        {
            Cancel = false;
            if (PathEXE.Name.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                // Fluxo 2: entra dentro da nova versão e precisa substituir a versão atual
                SplashScreen.ShowRun((s) => SubstituiArquivos(s));
                return true;
            }
            else if (Directory.Exists(Path.Combine(PathEXE.FullName, "update")))
            {
                string exec = Path.Combine(PathEXE.FullName, @"update\" + FileEXE.Name);
                if (File.Exists(exec + ".OK"))
                    SplashScreen.ShowRun((s) =>
                    {
                        s.ShowInfo("Reiniciando aplicação...[2C]");
                        try
                        {
                            Utils.ClearAllPDV7process();
                            File.Delete(exec + ".OK");
                        }
                        catch (Exception ex)
                        {
                            if (!File.Exists(exec + ".KILL"))
                            {
                                File.WriteAllText(exec + ".KILL", "KILL");
                                Reboot();
                                ex.Data.Add("Reboot", "Request");
                            }
                            else
                                File.Delete(exec + ".OK");

                            throw ex;
                        }
                        ExecFile(exec);
                    });
                else
                    // Fluxo 3 (final): Apaga a pasta de atualização e continua a execução.
                    SplashScreen.ShowRun((s) => FinalizaAtualizacao(s));

                return false;
            }
            else if (FileEXE.Name.Equals("a7D.PDV.Integracao.Servico.UI.exe", StringComparison.InvariantCultureIgnoreCase))
            {
                // Fluxo 1: Somente integrador, que ocorre dentro do serviço do sistema de houver mensagens
                return false;
            }
            else
            {
                // Fluxo 1: Somente para Caixa, Temrinal Windows, e BackOffice
                bool retorno = false;
                SplashScreen.ShowRun((s) => retorno = IniciaAtualizacaoSeNecessario(s));
                return retorno;
            }
        }

        private static void Reboot()
        {
            try
            {
                Process.Start("shutdown", "/r /t 30 /c \"Reiniciando em 30 segundos para finalizar os processos travados\"");
            }
            catch (Exception)
            {
            }
        }

        private static void SubstituiArquivos(IShowInfo s)
        {
            try
            {
                s.ShowInfo("Encerrando Processos...[2A]");
                Utils.ClearAllPDV7process();

                if (Cancel) return;

                s.ShowInfo("Copiando Arquivos...[2B]");
                Utils.CopyTo(PathEXE.FullName, PathEXE.Parent.FullName, true);

                if (Cancel) return;

                string exec = Path.Combine(PathEXE.Parent.FullName, FileEXE.Name); // inicia 'fluxo 3' (final)
                ExecFile(exec);

            }
            catch (Exception ex)
            {
                if (!File.Exists(PathEXE.FullName + ".KILL"))
                {
                    File.WriteAllText(PathEXE.FullName + ".KILL", "KILL");
                    Reboot();
                    ex.Data.Add("Reboot", "Request");
                }

                BLL.Logs.Erro(BLL.CodigoErro.EAA2, ex);
                s.ShowInfo(ex.Message);
                throw ex;
            }
        }

        private static void FinalizaAtualizacao(IShowInfo s)
        {
            s.ShowInfo("Finalizando Atualização...[3A]");
            var di = new DirectoryInfo(Path.Combine(PathEXE.FullName, "update"));
            try
            {
                string dest = di.FullName + "OK." + VersaoAtual;
                if (!Directory.Exists(dest))
                    di.MoveTo(dest);
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(BLL.CodigoErro.EAA3, ex);
                s.ShowInfo(ex.Message);
            }
        }

        private static bool IniciaAtualizacaoSeNecessario(IShowInfo s)
        {
            // Fluxo A: (padrão) busca por nova versão
            s.ShowInfo("Verificando...");

            //var url = "http://localhost:7777/release/update.zip";
            var url = UDPClient.Send("UPDATE" + VersaoAtual);

            if (string.IsNullOrEmpty(url))
            {
                s.ShowInfo("Sem resposta do Integrador!");
                Thread.Sleep(3000);
            }
            else if (url.StartsWith("http"))
            {
                // Fluxo B: Inicia processo de atualização
                IniciaAtualizacao(s, url);
                return true;
            }
            else
            {
                s.ShowInfo(url);
                if (url != "OK")
                    Thread.Sleep(5000);
                else
                    Thread.Sleep(2000);
            }
            return false;
        }

        private static string exeOnExit;
        public static void IniciaAtualizacao(IShowInfo s, string url, bool execExit = false)
        {
            try
            {
                var ws = new ClienteWS(url);
                string zipFile = Path.Combine(PathEXE.FullName, "update.zip");

                s.ShowInfo("Realizando backup da versão atual...[1A]");
                BackupVersaoIfExist();

                if (Cancel) return;

                s.ShowInfo("Baixando atualização...[1B]");
                ws.Download("", zipFile, (t) => s.ShowInfo($"Carregando {(t / 1000).ToString("N0")} Kbytes"));

                if (Cancel) return;

                s.ShowInfo("Aguarde, extraindo arquivos...[1C]");
                ExtraiArquivos(zipFile);
                File.Delete(zipFile);

                if (Cancel) return;

                s.ShowInfo("Configurando instalação...[1D]");
                CopyCNstring();

                if (Cancel) return;

                string exec = Path.Combine(PathEXE.FullName, @"update\" + FileEXE.Name);
                s.ShowInfo("Reiniciando aplicação...[1E]");
                File.WriteAllText(exec + ".OK", "OK");

                if (!execExit)
                    ExecFile(exec);
                else
                    exeOnExit = exec;

            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(BLL.CodigoErro.EAA1, ex);
                s.ShowInfo(ex.Message);
                throw ex;
            }
        }

        public static void BackupVersaoIfExist()
        {
            var zipFile = Path.Combine(PathEXE.FullName, $@"backup\backup-{VersaoAtual}.zip");
            if (!File.Exists(zipFile))
                Utils.CreateZipFromDirectory(PathEXE.FullName, zipFile,
                    f => !f.StartsWith("backup", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("Update", StringComparison.InvariantCultureIgnoreCase)
                      && !f.Equals("debug.log", StringComparison.InvariantCultureIgnoreCase));
        }

        public static void ExtraiArquivos(string zip)
        {
            string dest = Path.Combine(PathEXE.FullName, "update");
            if (Directory.Exists(dest))
                Directory.Delete(dest, true);

            ApagaAntigoWWW_SAT();

            ZipFile.ExtractToDirectory(zip, dest);
        }

        public static void ApagaAntigoWWW_SAT()
        {
            var dest = Path.Combine(PathEXE.FullName, @"WebServices\WWW_SAT\bin\a7D.PDV.Integracao.WS2.dll");
            try
            {
                var fi = new FileInfo(dest);
                if (fi.Exists)
                    return;

                if (fi.Directory.Exists)
                    Utils.Deltree(fi.Directory.Parent.FullName);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover versão anterior do WWW_SAT!\r\n" + dest, ex);
            }
        }

        public static void CopyCNstring()
        {
            var atualCN = ConfigurationManager.ConnectionStrings[Utils.ConnectionName];
            if (atualCN == null)
                return;

            var di = new DirectoryInfo(Path.Combine(PathEXE.FullName, @"update"));
            foreach (var fi in di.GetFiles("*.UI.exe"))
            {
                var config = ConfigurationManager.OpenExeConfiguration(fi.FullName);
                var conn = config.ConnectionStrings.ConnectionStrings[Utils.ConnectionName];
                if (conn != null)
                    config.ConnectionStrings.ConnectionStrings.Remove(Utils.ConnectionName);

                var newCN = new ConnectionStringSettings(Utils.ConnectionName, atualCN.ConnectionString, Utils.ProviderName);
                config.ConnectionStrings.ConnectionStrings.Add(newCN);
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        private static void ExecFile(string exec)
        {
            try
            {
                Process.Start(exec, "wait " + Process.GetCurrentProcess().Id); // Aguarda até 10 segundo o processo atual ser encerrado
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar '{exec}': " + ex.Message, ex);
            }
        }

        public static void ExecOnExit()
        {
            if (!string.IsNullOrEmpty(exeOnExit))
                ExecFile(exeOnExit);
        }
    }
}
