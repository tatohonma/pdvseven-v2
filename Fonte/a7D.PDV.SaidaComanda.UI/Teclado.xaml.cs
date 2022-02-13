using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace a7D.PDV.SaidaComanda.UI
{
    public delegate void TecladoKeyPressEventHandler(char key);

    public partial class Teclado : UserControl
    {
        public TecladoKeyPressEventHandler onClick;

        public Teclado()
        {
            InitializeComponent();
        }

        private void btnKey_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string text = btn.Content.ToString();
            if (text.Length == 1)
                onClick?.Invoke(text[0]);
            else
                onClick?.Invoke((char)13);
        }
    }
}