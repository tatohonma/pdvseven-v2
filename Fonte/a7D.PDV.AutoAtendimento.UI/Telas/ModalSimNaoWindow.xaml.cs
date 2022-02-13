using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace a7D.PDV.AutoAtendimento.UI
{
    public partial class ModalSimNaoWindow : Window
    {
        private static bool isOpen = false;
        private DispatcherTimer timer;
        private bool IsDialog;
        private int TimeOut = 100;
        private bool YesNo = false;
        private MessageBoxResult resposta = MessageBoxResult.None;

        public ModalSimNaoWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutServices.Bind(this);
            }
            catch (Exception)
            {
            }
            Sim.Visibility = Nao.Visibility = YesNo ? Visibility.Visible : Visibility.Collapsed;
        }

        public void HideWait()
        {
            Topmost = false;
            borderMain.Visibility = Visibility.Collapsed;
        }

        public void ShowWait(string mensagem, bool dialog = false)
        {
            txtStatus.Text = mensagem;
            if (timer.IsEnabled)
                return;

            Topmost = true;
            borderMain.Visibility = Visibility.Visible;
            timer.Start();
            if (IsDialog = dialog)
                ShowDialog();
            else
                Show();
        }

        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            if (!IsActive)
                Activate();

            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;

            if (IsDialog && TimeOut-- < 0)
                Close();
        }

        public static MessageBoxResult Show(string mensagem, bool lYesNo = false)
        {
            if (isOpen)
                return MessageBoxResult.Cancel;

            isOpen = true;
            var modal = new ModalSimNaoWindow();
            modal.ConfigScreen();
            modal.YesNo = lYesNo;
            modal.ShowWait(mensagem, true);
            isOpen = false;

            return modal.resposta;
        }

        public static string Show(string mensagem, Func<string> action)
        {
            var modal = new ModalSimNaoWindow();
            modal.txtStatus.Text = mensagem;
            modal.ConfigScreen();
            modal.Show();
            modal.WindowState = WindowState.Maximized;

            string result;
            try
            {
                result = Task.Factory.StartNew(action).Result;
            }
            catch (Exception ex)
            {
                result = "ERRO: " + ex.Message;
            }

            modal.Close();

            return result;
        }

        public static async Task ShowAsync(string mensagem, Func<string> action)
        {
            var modal = new ModalSimNaoWindow();
            modal.txtStatus.Text = mensagem;
            modal.ConfigScreen();
            modal.Show();
            modal.WindowState = WindowState.Maximized;

            string result;

            try
            {
                result = await Task.Factory.StartNew(action);
            }
            catch (Exception ex)
            {
                result = "ERRO: " + ex.Message;
            }

            if (string.IsNullOrEmpty(result))
            {
                modal.Close();
                return;
            }

            modal.txtStatus.Text = result;
            modal.IsDialog = true;
            modal.timer.Start();

            while (modal.resposta == MessageBoxResult.None)
                await Task.Delay(100);
        }

        public static MessageBoxResult Show(Exception ex)
        {
            EventLogServices.Error(ex);
            string mensagem = "ERRO: ";
            while (ex != null)
            {
                mensagem += ex.Message + "\n";
                ex = ex.InnerException;
            }
            return Show(mensagem + "\n(mais informações no log do windows)");
        }

        internal static async Task<MessageBoxResult> ShowAsync(string mensagem, Func<string> action = null, int timeout = 5000)
        {
            var modal = new ModalSimNaoWindow();
            try
            {
                modal.ShowWait(mensagem);
                if (action != null)
                {
                    var result = await Task.Run(action);
                    if (result == null || result == "OK")
                        return MessageBoxResult.OK;
                    else if (result == "YES")
                        return MessageBoxResult.Yes;

                    modal.ShowWait(result);

                    if (result.EndsWith("?"))
                        modal.Sim.Visibility = modal.Nao.Visibility = Visibility.Visible;
                }

                int count = timeout / 100;
                while (modal.resposta == MessageBoxResult.None && count-- > 0)
                    await Task.Delay(100);

                return modal.resposta;
            }
            finally
            {
                modal.Close();
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            resposta = MessageBoxResult.Cancel;
            if (IsDialog)
                Close();
        }

        private void gridMain_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void YesNo_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Name == Sim.Name)
                resposta = MessageBoxResult.Yes;
            else if (btn.Name == Nao.Name)
                resposta = MessageBoxResult.No;

            Close();
        }
    }
}