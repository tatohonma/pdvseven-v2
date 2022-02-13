using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    public delegate void TecladoKeyPressEventHandler(char key);

    public partial class Teclado : UserControl
    {
        public const int Confirm = 13;
        public const int Cancel = 27;
        public const int Back = 8;

        public TecladoKeyPressEventHandler onClick;

        public int LayoutStyle
        {
            set
            {
                if (value == 2)
                {
                    btnEnter.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Visible;
                    btnBackspace.Visibility = Visibility.Visible;
                    Grid.SetColumn(btn0, 1);
                } 
                else
                {
                    btnEnter.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnBackspace.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btn0, 0);
                }
            }
        }

        public Teclado()
        {
            InitializeComponent();
        }

        private void btnKey_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string text = btn.Content.ToString();
            if (btn.Name == btnEnter.Name)
                onClick?.Invoke((char)Confirm);
            else if (btn.Name == btnCancel.Name)
                onClick?.Invoke((char)Cancel);
            else if (btn.Name == btnBackspace.Name)
                onClick?.Invoke((char)Back);
            else if (text.Length == 1)
                onClick?.Invoke(text[0]);
        }
    }
}
