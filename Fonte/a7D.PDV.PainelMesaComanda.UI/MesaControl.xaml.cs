using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a7D.PDV.PainelMesaComanda.UI
{
    public partial class MesaControl : UserControl
    {
        readonly static SolidColorBrush red = new SolidColorBrush(Colors.Red);
        readonly static SolidColorBrush green = new SolidColorBrush(Colors.Green);
        readonly static SolidColorBrush redBack = new SolidColorBrush(Colors.LightPink);
        readonly static SolidColorBrush greenBack = new SolidColorBrush(Colors.LightGreen);

        public String Mesa
        {
            set { MesaText.Content = value; }
        }

        public String Comanda
        {
            set { ComandaText.Content = value; }
        }

        public bool Ocupada
        {
            get
            {
                return GridBox.Background ==redBack;
            }
            set
            {
                GridBox.Background = value ? redBack : greenBack;
                (Parent as Button).BorderBrush = value ? red : green;
            }
        }

        public MesaControl()
        {
            InitializeComponent();
        }

        internal static Button CreateButton(int x, int y, int width, int height, int mesa, RoutedEventHandler clickHandler, int digitos)
        {
            int margin = 4;
            var content = new MesaControl
            {
                Mesa = "Mesa " + mesa.ToString(new string('0', digitos)),
                Comanda = "",
                Width = width - margin * 2,
                Height = height - margin * 2
            };

            var btn = new Button()
            {
                Tag = mesa,
                Name = "mesa" + mesa,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(margin),
                BorderBrush = Brushes.Transparent,
                Padding = new Thickness(0),
                Margin = new Thickness(margin / 2),
                Content = content,
            };

            Grid.SetRow(btn, y);
            Grid.SetColumn(btn, x);

            btn.Click += clickHandler;

            return btn;
        }
    }
}