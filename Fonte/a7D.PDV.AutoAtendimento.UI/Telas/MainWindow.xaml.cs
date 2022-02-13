using a7D.PDV.AutoAtendimento.UI.Paginas;
using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Threading;

namespace a7D.PDV.AutoAtendimento.UI
{
    public partial class MainWindow : Window
    {
        public int TotalTime;
        private DispatcherTimer timer;
        int timeOut;
        bool clickSeguro;
        bool fimPage;

        public MainWindow()
        {
            InitializeComponent();
            timeOut = 0;
            timer = new DispatcherTimer();
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        public string StatusBar
        {
            set
            {
                if (value == null)
                    loading.Visibility = Visibility.Collapsed;
                else
                {
                    loading.Text = value;
                    loading.Visibility = Visibility.Visible;
                }
            }
        }

        // <Frame x:Name="mainFrame" NavigationUIVisibility="Hidden" Navigating="MainFrame_OnNavigating"></Frame>
        //private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        //{
        //    var page = e.Content as Page;
        //    if (!(page is VendaModificacaoProdutoPage))
        //        return;

        //    var ta = new ThicknessAnimation
        //    {
        //        Duration = TimeSpan.FromSeconds(0.3),
        //        DecelerationRatio = 0.7,
        //        To = new Thickness(0, 0, 0, 0)
        //    };
        //    if (e.NavigationMode == NavigationMode.New)
        //    {
        //        ta.From = new Thickness(500, 0, 0, 0);
        //    }
        //    else if (e.NavigationMode == NavigationMode.Back)
        //    {
        //        ta.From = new Thickness(0, 0, 500, 0);
        //    }
        //    page.BeginAnimation(MarginProperty, ta);
        //}

        public void TimeoutFim()
        {
            timeOut = PdvServices.TimeoutInativo - 15;
            fimPage = true;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                TotalTime++;

                if (!App.Timeout)
                {
                    if (TotalTime > PdvServices.VerificarDisponibilidade * 60 && App.CurrentPage is InicialPage)
                        App.Navigate<InicialPage>();

                    return;
                }
                else if (timeOut > PdvServices.TimeoutAlerta)
                {
                    if (timeOut > PdvServices.TimeoutInativo)
                        App.Navigate<InicialPage>();
                    else if (!fimPage)
                        StatusBar = $"Terminal inativo, a sessão será encerrada em {(PdvServices.TimeoutInativo - timeOut)}";
                }

                timeOut++;
            }
            catch (Exception)
            {
            }
        }

        private void loading_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (clickSeguro)
            {
                if (App.CurrentPage is InicialPage)
                    App.Navigate<SairPage>();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickSeguro = !App.Timeout && TotalTime < 5;
            timeOut = 0;
        }
    }
}