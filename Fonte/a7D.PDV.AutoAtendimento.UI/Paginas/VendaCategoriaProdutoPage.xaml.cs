using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class VendaCategoriaProdutoPage : Page, INavPageClick
    {
        private static bool RequestToBottom = false;
        public VendaCategoriaProdutoPage()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Redireciona diretamente para a tela de leitura de comanda
            App.Navigate<ComandaLeitoraPage>();

            try
            {
                App.Pedido.Bind(TotalPedido, null, null);
                App.Pedido.Bind(Confirmar);
                App.Produtos.FillProdutos(produtosLista, Adicionar_Click);
                if (RequestToBottom)
                {
                    RequestToBottom = false;
                    this.Pedido.ToBottom();
                }
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }

        public void ComandaNumero_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaLeitoraPage>();
        }

        public void Buttom_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Tag == null)
                return;

            var ids = ((string)btn.Tag).Split(',');
            LayoutServices.CategoriaSelecionadas = ids.Select(c => int.Parse(c)).ToArray();
            App.Navigate<VendaCategoriaProdutoPage>();
        }

        public void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            var produto = (sender as Button).Tag as Produto;
            if (App.Pedido.Adicionar(produto))
            {
                if (produto.PaineisDeModificacao.Count > 0)
                {
                    RequestToBottom = true;
                    App.Navigate<VendaModificacaoProdutoPage>();
                }
                else
                    this.Pedido.ToBottom();
            }
        }

        public void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.TotalProdutos > 0 &&
                ModalSimNaoWindow.Show("Deseja cancelar todo pedido?", true) != MessageBoxResult.Yes)
                return;

            App.Pedido.Cancelar();
            App.Navigate<InicialPage>();
        }

        public void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<VendaResumoPage>();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            btnSubir.Visibility = ProdutoScroll.VerticalOffset > 10 ? Visibility.Visible : Visibility.Collapsed;
            btnDescer.Visibility = ProdutoScroll.ExtentHeight - ProdutoScroll.VerticalOffset - 10 > ProdutoScroll.ActualHeight ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnSubir_Click(object sender, RoutedEventArgs e)
        {
            ScrollAnimarTo(ProdutoScroll.VerticalOffset - ProdutoScroll.ActualHeight / 2);
        }

        private void btnDescer_Click(object sender, RoutedEventArgs e)
        {
            ScrollAnimarTo(ProdutoScroll.VerticalOffset + ProdutoScroll.ActualHeight / 2);
        }

        #region Animação do Scroll

        public static DependencyProperty VerticalOffsetProperty =
        DependencyProperty.RegisterAttached("VerticalOffset",
                                            typeof(double),
                                            typeof(VendaCategoriaProdutoPage),
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

            verticalAnimation.From = ProdutoScroll.VerticalOffset;
            verticalAnimation.To = toPosition;
            verticalAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, ProdutoScroll);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(VerticalOffsetProperty)); // Attached dependency property
            storyboard.Begin();

        }

        #endregion
    }
}
