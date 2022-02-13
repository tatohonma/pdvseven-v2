using a7D.PDV.AutoAtendimento.UI.Paginas;
using a7D.PDV.AutoAtendimento.UI.Properties;
using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.API2.Client;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace a7D.PDV.AutoAtendimento.UI
{
    public partial class App : Application
    {
#if TESTE
        internal const int waitStep = 20;
#else
        internal const int waitStep = 200;
#endif

        static PagamentoServices tef;
        static PdvServices pdv;
        static MainWindow mainWindow;
        static ClienteWS ws;

        internal static string StatusBar { set => mainWindow.StatusBar = value; }
        internal static bool Timeout;
        internal static bool NTKCertTEST;

        internal static PedidoServices Pedido { get; private set; }
        internal static ProdutoServices Produtos { get; private set; }
        internal static ImpressoraServices Impressora { get; private set; }

        protected async override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (EmExecucao())
                {
                    Shutdown();
                    return;
                }

                base.OnStartup(e);

                EventLogServices.VerificarExistenciaFonte();

                if (e?.Args != null && e.Args.Length == 1 && e.Args[0].Equals("adm", StringComparison.OrdinalIgnoreCase))
                {
                    var ntk = Integracao.Pagamento.NTKTEF.NTKBuilder.Administar();
                    var tef = ntk.CriaTEF();
                    while (tef.Processando()) // Usa o timeout padrão...
                        System.Threading.Thread.Sleep(1000);

                    // Environment.Exit(0);
                }

                Mouse.OverrideCursor = Cursors.Wait;

                mainWindow = new MainWindow();
                mainWindow.ConfigScreen();
                mainWindow.Show();
                MainWindow.WindowState = WindowState.Maximized;

                await Task.Delay(waitStep);

                var urlWS2 = Settings.Default.EnderecoAPI2;
                if (urlWS2 == ".")
                {
#if TESTE
                    StatusBar = "Ambiente de Teste...";
                    urlWS2 = "http://localhost:7777";
#else
                    StatusBar = "Identificando Servidor...";
                    var server = UDPClient.Send("WS2");
                    if (!string.IsNullOrEmpty(server))
                        urlWS2 = "http://" + server;
                    else
                        urlWS2 = "http://localhost:7777";
#endif
                }

                StatusBar = "Autenticando PDV...";
                await Task.Delay(waitStep);

                ws = new ClienteWS(urlWS2);
                pdv = new PdvServices(ws);
                pdv.ValidarPDV();

                StatusBar = "Lendo configurações...";
                await Task.Delay(waitStep);
                pdv.LerConfiguracoes();

                StatusBar = "Inicializando Impressora...";
                await Task.Delay(waitStep);
                Impressora = new ImpressoraServices(PdvServices.ImpressaoLocal);
#if !TESTE
                Impressora.ImprimirTexto($"Autoatendimento {PdvServices.PDVID}: {PdvServices.PDVNome}\r\n{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
#endif
                try
                {
                    StatusBar = "Verificando Certificado...";
                    await Task.Delay(waitStep);
                    var cert = new X509Certificate();
                    cert.Import("certificado.crt");
                    string resultsTrue = cert.ToString(true);
                    NTKCertTEST = resultsTrue.Contains("CN=SETIS CA TEST");
                    if (NTKCertTEST)
                        ModalSimNaoWindow.Show("Certificado de TESTE NTK Detectado!");
                }
                catch (Exception)
                {
                }

                StatusBar = "Lendo tema...";
                await Task.Delay(waitStep);
                LayoutServices.Load(ws, Settings.Default.Layout, Settings.Default.Idioma);

                StatusBar = "Configurando Serviços...";
                await Task.Delay(waitStep);
                Produtos = new ProdutoServices(ws);
                tef = new PagamentoServices();

                StatusBar = "OK";
                await Task.Delay(waitStep);
                Mouse.OverrideCursor = PdvServices.ExibirMouse ? Cursors.Hand : Cursors.None;
                EventLogServices.SuccessAudit("Iniciando Autoatendimento");

                Navigate<InicialPage>();
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                Application.Current.Shutdown();
            }
        }

        public static bool EmExecucao()
        {
            string meuProcesso = Process.GetCurrentProcess().ProcessName;

            // Procura o processo atual na lista de processos que estão a sendo executados neste momento
            var processos = Process.GetProcessesByName(meuProcesso);
            foreach (Process processo in processos)
            {
                if (processo.Id != Process.GetCurrentProcess().Id)
                    return true; // Se mata
            }
            return false;
        }

        internal static void Finaliza()
        {
            Application.Current.Shutdown();
        }

        public static void Navigate<TNavPage>() where TNavPage : Page, new()
        {
            try
            {
                Timeout = true; // Valor padrão: timeout habilitado para todas as páginas
                StatusBar = ""; // Valor padrão: Sem informações
                var page = new TNavPage();
                LayoutServices.Bind(page);
                mainWindow.TotalTime = 0;
                mainWindow.mainFrame.Navigate(page);
                if (page.GetType() == typeof(FimPage))
                    mainWindow.TimeoutFim();
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }

        public static void Voltar()
        {
            Timeout = true; // Valor padrão: timeout habilitado para todas as páginas
            StatusBar = ""; // Valor padrão: Sem informações
            mainWindow.TotalTime = 0;
            mainWindow.mainFrame.GoBack();
        }

        public static Page CurrentPage
            => (Page)mainWindow.mainFrame.Content;

        public static void Refresh()
            => mainWindow.mainFrame.Refresh();

        internal static void NovoPedido()
            => Pedido = new PedidoServices(ws, tef);
    }
}