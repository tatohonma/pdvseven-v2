using a7D.PDV.AutoAtendimento.UI.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    /// <summary>
    /// Interaction logic for TecladoAlfanumerico.xaml
    /// </summary>
    public partial class TecladoAlfanumerico : UserControl
    {
        private string NumericFormat = "(##) #####-####"; // Apenas telefone
        private TextBox text;
        private bool isNumeric;

        public ImageBrush BackgroundAlfabetico { get; set; }
        public ImageBrush BackgroundNumerico { get; set; }

        public TecladoAlfanumerico()
        {
            InitializeComponent();
        }

        public void SetTextEdit(TextBox txt)
        {
            txt.GotFocus += Text_GotFocus;
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            text = sender as TextBox;
            LayoutNumeric(text.Name == "Telefone");
        }

        private void LayoutNumeric(bool lNumeric)
        {
            if (teclas.Children.Count < 10)
                return;
            else if (isNumeric == lNumeric && Background != null) // Já está no formato correto
                return;

            isNumeric = lNumeric;

            if (isNumeric)
                Background = BackgroundNumerico;
            else
                Background = BackgroundAlfabetico;

            for (char caracter = 'A'; caracter <= 'Z'; caracter++)
            {
                string letra = new string(caracter, 1);
                var btn = Find(letra);
                btn.Visibility = isNumeric ? Visibility.Collapsed : Visibility.Visible;
            }
            Space.Visibility = isNumeric ? Visibility.Collapsed : Visibility.Visible;
            Backspace.Visibility = isNumeric ? Visibility.Collapsed : Visibility.Visible;

            for (char caracter = '0'; caracter <= '9'; caracter++)
            {
                string letra = "N" + new string(caracter, 1);
                var btn = Find(letra);
                btn.Visibility = isNumeric ? Visibility.Visible : Visibility.Collapsed;
            }
            NBackspace.Visibility = isNumeric ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Alfabetico
            for (char caracter = 'A'; caracter <= 'Z'; caracter++)
            {
                string letra = new string(caracter, 1);
                var btn = new Button()
                {
                    Name = letra,
                    Content = letra
                };
                btn.Click += Btn_Click;
                teclas.Children.Add(btn);
            }
            Space.Content = " ";

            // Numerico
            for (char caracter = '0'; caracter <= '9'; caracter++)
            {
                string letra = new string(caracter, 1);
                var btn = new Button()
                {
                    Name = "N" + letra,
                    Content = letra
                };
                btn.Click += Btn_Click;
                teclas.Children.Add(btn);
            }

            LayoutServices.Bind(this);

            if (text != null)
                LayoutNumeric(text.InputScope.Names.Count > 0 && text.InputScope.Names[0].ToString() == "TelephoneNumber");
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (text is null)
                return;

            var btn = sender as Button;
            if ((btn.Name.Length == 1 || btn.Name == Space.Name) && text.Text.Length < text.MaxLength)
            {
                int cursor = text.CaretIndex;
                if (cursor == text.Text.Length)
                {
                    text.Text += btn.Content.ToString();
                }
                else
                {
                    text.Text = text.Text.Substring(0, cursor) + btn.Content.ToString() + text.Text.Substring(cursor);
                }
                text.CaretIndex = cursor + 1;
            }
            else if (btn.Name == Backspace.Name && text.Text.Length > 0)
            {
                int cursor = text.CaretIndex;
                int size = text.SelectionLength;
                if (cursor == 0)
                    return;
                else if (cursor == text.Text.Length)
                {
                    text.Text = text.Text.Substring(0, text.Text.Length - 1);
                    text.CaretIndex = text.Text.Length;
                }
                else if (size > 0)
                {
                    text.Text = text.Text.Substring(0, cursor) + text.Text.Substring(cursor + size);
                    text.CaretIndex = cursor ;
                }
                else
                {
                    text.Text = text.Text.Substring(0, cursor - 1) + text.Text.Substring(cursor + size);
                    text.CaretIndex = cursor - 1;
                }
            }
            else if (btn.Name.Length == 2 && isNumeric && text.Text.Length < NumericFormat.Length)
            {
                string numeros = text.Text.SoNumeros();
                numeros += btn.Content.ToString();
                string formatado = "";
                int i = 0;
                foreach (var c in NumericFormat)
                {
                    if (c == '#')
                    {
                        formatado += numeros[i];
                        if (++i >= numeros.Length)
                            break;
                    }
                    else
                        formatado += c;
                }
                text.Text = formatado;
                text.CaretIndex = text.Text.Length;
            }
            else if (btn.Name == NBackspace.Name && isNumeric)
            {
                while (text.Text.Length > 0)
                {
                    char c = text.Text[text.Text.Length - 1];
                    text.Text = text.Text.Substring(0, text.Text.Length - 1);
                    if (c >= '0' && c <= '9')
                        break;
                }
                if (text.Text == NumericFormat.Substring(0, 1))
                    text.Text = "";

                text.CaretIndex = text.Text.Length;
            }

            text.Focus();
        }

        internal Button Find(string name)
        {
            foreach (var tecla in teclas.Children)
            {
                if (tecla is Button btn)
                {
                    if (btn.Name == name)
                        return btn;
                }
            }
            return null;
        }
    }
}
