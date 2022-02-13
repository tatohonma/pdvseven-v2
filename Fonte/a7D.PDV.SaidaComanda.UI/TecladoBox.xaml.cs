using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace a7D.PDV.SaidaComanda.UI
{
    public partial class TecladoBox : UserControl
    {
        public string Text { get; private set; }

        public event TextChangedEventHandler onChange;

        public void Clear()
        {
            Text = texto.Text = "";
        }

        public TecladoBox()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            texto.Text = "";
            teclado.onClick += TecladoKeyPress;
        }

        public void TecladoKeyPress(char key)
        {
            try
            {
                if (key == 13)
                {
                    Text = texto.Text;
                    onChange?.Invoke(this, null);
                }
                else if (texto.Text.Length < 9)
                {
                    texto.Text += key;
                    texto.SelectionStart = texto.Text.Length;
                }
            }
            catch (Exception)
            {
                texto.Text = "";
            }
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (texto.Text.Length > 0)
            {
                texto.Text = texto.Text.Substring(0, texto.Text.Length - 1);
                texto.SelectionStart = texto.Text.Length;
            }
        }

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Clear();
            onChange?.Invoke(this, null);
        }

        internal void SetFocus()
        {
            texto.Focus();
        }

        private void texto_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            e.Handled= regex.IsMatch(e.Text);
        }
    }
}
