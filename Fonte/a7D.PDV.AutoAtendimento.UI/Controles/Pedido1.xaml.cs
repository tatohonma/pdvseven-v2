using a7D.PDV.AutoAtendimento.UI.Paginas;
using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    /// <summary>
    /// Interaction logic for Pedido1.xaml
    /// </summary>
    public partial class Pedido1 : UserControl
    {
        #region Animação do Scroll

        public static DependencyProperty VerticalOffsetProperty =
        DependencyProperty.RegisterAttached("VerticalOffset",
                                            typeof(double),
                                            typeof(Pedido1),
                                            new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        private void ScrollAnimarTo(double toPosition)
        {
            DoubleAnimation verticalAnimation = new DoubleAnimation();

            verticalAnimation.From = ItensScroll.VerticalOffset;
            verticalAnimation.To = toPosition;
            verticalAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, ItensScroll);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(VerticalOffsetProperty)); // Attached dependency property
            storyboard.Begin();

        }

        public void ToBottom()
        {
            ScrollAnimarTo(ItensScroll.VerticalOffset + ItensScroll.ActualHeight);
        }

        #endregion

        private PedidoServices pedido;
        private static double LastScrollPosition;
        public Pedido1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedido = App.Pedido;
            pedido?.Bind(pedidoLista);
            ItensScroll.ScrollToVerticalOffset(LastScrollPosition);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            LastScrollPosition = ItensScroll.ScrollableHeight;
        }

        public void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID == 0)
                pedido.Alterar(sender, -1);
        }

        public void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID == 0)
                pedido.Alterar(sender, 1);
        }

        public void Apagar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID == 0)
                pedido.Remover(sender);
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID != 0)
                return;

            if (pedido.Edit(sender))
                App.Navigate<VendaModificacaoProdutoPage>();
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutServices.Bind((Grid)sender);
        }
    }
}