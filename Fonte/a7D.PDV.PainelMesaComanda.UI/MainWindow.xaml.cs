using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.PainelMesaComanda.UI.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace a7D.PDV.PainelMesaComanda.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        MesaComandasTotal[] mesas;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Status.Text = "Iniciando...";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += AtualizaMesas;
            timer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (timer != null)
                timer.Stop();
        }

        private void AtualizaMesas(object sender, EventArgs e)
        {
            try
            {
                Status.Text = "Carregando Mesas...";
                mesas = App.LerMesas();

                if (grdMesas.Children.Count != mesas.Length)
                {
                    int maxMesas = mesas.Length;

                    double aspect = 1.3; // grdMesas.ActualWidth / grdMesas.ActualHeight;
                    int maxCols = (int)Math.Round(aspect * Math.Sqrt(maxMesas), 0);
                    int maxRows = (int)Math.Round((double)maxMesas / (double)maxCols, 0);

                    if (maxCols * maxRows < maxMesas)
                        maxCols++;

                    Fill(grdMesas, mesas, Mesa_Click, maxCols, maxRows);

                    timer.Interval = TimeSpan.FromSeconds(Settings.Default.Intervalo);
                }

                foreach (Button btn in grdMesas.Children)
                {
                    int mesaNumero = (int)btn.Tag;
                    var mesa = mesas.FirstOrDefault(m => m.Mesa == mesaNumero);
                    if (mesa == null)
                        continue;

                    var mc = btn.Content as MesaControl;
                    if (mc.Ocupada = mesa.Comandas > 0)
                        mc.Comanda = $"{mesa.Comandas} Comandas\r\nR$ {mesa.Total.Value.ToString("N2")}";
                    else
                        mc.Comanda = "";
                }

                Status.Text = $"Atualizado em {DateTime.Now.ToString("HH:mm:ss")}";
            }
            catch (Exception ex)
            {
                Status.Text = "ERRO: ";

                while (ex!=null)
                {
                    Status.Text += ex.Message + "\r\n";
                    ex = ex.InnerException;
                }
            }
        }

        public void Mesa_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var mc = btn.Content as MesaControl;
            if (mc.Ocupada)
            {
                var modal = new ModalMesa((int)btn.Tag);
                modal.ShowDialog();
            }
        }

        private void Fill(Grid grid, MesaComandasTotal[] mesas, RoutedEventHandler clickHandler, int cols, int rows)
        {
            grid.Children.Clear();
            //grid.ShowGridLines = true;

            int width = Convert.ToInt32(grid.ActualWidth / cols);
            int height = Convert.ToInt32(grid.ActualHeight / rows);

            for (int x = 0; x < cols; x++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width) });

            for (int y = 0; y < rows; y++)
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height) });

            int mesa = 0;
            int digitos = mesas.Length.ToString().Length;
            for (int y = 0; y < rows && mesa < mesas.Length; y++)
            {
                for (int x = 0; x < cols && mesa < mesas.Length; x++)
                {
                    var btn = MesaControl.CreateButton(x, y, width, height, mesas[mesa].Mesa, clickHandler, digitos);
                    grid.Children.Add(btn);
                    mesa++;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
