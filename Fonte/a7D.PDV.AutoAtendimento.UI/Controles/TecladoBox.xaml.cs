using System;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    /// <summary>
    /// Interaction logic for TecladoBox.xaml
    /// </summary>
    public partial class TecladoBox : UserControl
    {
        public string Text { get; private set; }

        public int MaxLength { get; set; }

        public string TextFormat { get; set; }

        public bool MoneyFormat { get; set; } = false;

        public bool Cancelado { get; private set; }

        public event TextChangedEventHandler onChange;

        public int LayoutStyle
        {
            set
            {
                teclado.LayoutStyle = value;
                if (value == 2)
                {
                    btnBackspace.Visibility = System.Windows.Visibility.Collapsed;
                    btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                    btnEnter.Visibility = System.Windows.Visibility.Visible;
                    Grid.SetColumnSpan(Texto, 2);
                }
                else
                {
                    btnBackspace.Visibility = System.Windows.Visibility.Visible;
                    btnCancel.Visibility = System.Windows.Visibility.Visible;
                    btnEnter.Visibility = System.Windows.Visibility.Collapsed;
                    Grid.SetColumnSpan(Texto, 1);
                }
            }
        }

        internal void ValorInicial(string valor)
        {
            Text = valor;
            AtualizaTexto();
        }

        public void Clear()
        {
            Cancelado = false;
            Text = "";
            AtualizaTexto();
        }

        internal void UpdateGrid()
        {
            if (Texto.Height > grd.RowDefinitions[0].Height.Value)
                grd.RowDefinitions[0].Height = new System.Windows.GridLength(Texto.Height);

            if (btnEnter.Visibility == System.Windows.Visibility.Visible)
            {
                if (btnEnter.Height > grd.RowDefinitions[2].Height.Value)
                    grd.RowDefinitions[2].Height = new System.Windows.GridLength(btnEnter.Height);
            }
            else
            {
                if (btnCancel.Height > grd.RowDefinitions[2].Height.Value)
                    grd.RowDefinitions[2].Height = new System.Windows.GridLength(btnCancel.Height);
            }
        }

        public TecladoBox()
        {
            MaxLength = 6;
            Cancelado = false;
            InitializeComponent();
            Text = Texto.Text = "";
            teclado.onClick += TecladoKeyPress;
        }

        public void TecladoKeyPress(char key)
        {
            try
            {
                if (key == Teclado.Confirm)
                    Enter_Click(this, null);

                else if (key == Teclado.Cancel)
                    Cancel_Click(this, null);

                else if (key == Teclado.Back)
                    Back_Click(this, null);

                else if (Text.Length < MaxLength)
                {
                    Text += key;
                    AtualizaTexto();
                }
            }
            catch (Exception)
            {
                Clear();
            }
        }

        internal void DisableCancelEnter()
        {
            btnCancel.Visibility = System.Windows.Visibility.Collapsed;
            teclado.btnEnter.Visibility = System.Windows.Visibility.Collapsed;
            Grid.SetColumn(teclado.btn0, 1);
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Text.Length > 0)
                Text = Text.Substring(0, Text.Length - 1);

            AtualizaTexto();
        }

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Cancelado = true;
            Clear();
            onChange?.Invoke(this, null);
        }

        private void Enter_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            onChange?.Invoke(this, null);
        }

        private void AtualizaTexto()
        {
            try
            {
                if (Text.Length == 0)
                {
                    Text = string.Empty;
                    return;
                }

                if (MoneyFormat)
                {
                    decimal val = decimal.Parse(Text);
                    Texto.Text = val.ToString("#,##0.00").Replace(",", "#").Replace(".", ",").Replace("#", ".");
                    return;
                }

                if (string.IsNullOrEmpty(TextFormat))
                {
                    Texto.Text = Text;
                    return;
                }

                string exibir = "";
                int t = 0;
                for (int i = 0; i < TextFormat.Length && t < Text.Length; i++)
                {
                    if (TextFormat[i] == '9')
                    {
                        exibir += Text[t];
                        t++;
                    }
                    else
                        exibir += TextFormat[i];
                }
                Texto.Text = exibir;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (teclado.btnEnter.Visibility == System.Windows.Visibility.Collapsed
                 && btnEnter.Visibility == System.Windows.Visibility.Collapsed)
                    // Quando não tem o botão de ENTER, qualquer alteração é enviado
                    onChange?.Invoke(this, null);
            }
        }
    }
}
