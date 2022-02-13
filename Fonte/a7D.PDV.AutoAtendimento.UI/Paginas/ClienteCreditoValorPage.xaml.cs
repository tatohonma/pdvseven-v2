using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ClienteCreditoValorPage : Page
    {
        Produto credito;

        public ClienteCreditoValorPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.TipoFluxo == Services.EFluxo.AdicionarCredito)
                Titulo.Text = "Digite o valor do crédito";
            else
                Titulo.Text = $"Entrada: R$ {App.Pedido.TotalPedido.ToString("N2")} + crédito";

            Numero.Text = App.Pedido.ComandaNumero.ToString();
            Nome.Text = App.Pedido.ClienteNome;
            App.Pedido.Bind(Pagar);
            App.Pedido.Bind(Total, null, Saldo);

            LayoutServices.Bind(Teclado);
            Teclado.DisableCancelEnter();
            Teclado.TextFormat = "R$ 9999";
            Teclado.MaxLength = 4;

            var creditos = await App.Produtos.PacoteCreditos();
            credito = creditos.FirstOrDefault(p => p.ValorUnitario == 0);
            if (credito == null)
                throw new Exception("Não há crédito personalizado cadastrado");
            else
            {
                credito = new Produto
                {
                    IDProduto = credito.IDProduto,
                    IDTipoProduto = credito.IDTipoProduto,
                    Nome = credito.Nome,
                    ValorUnitario = 0,
                    Ativo = true,
                    Excluido = false
                };
            }
        }

        private void ValorNumero_Change(object sender, TextChangedEventArgs e)
        {
            Int32.TryParse(Teclado.Text, out int valor);
            credito.ValorUnitario = valor;
            App.Pedido.DefinirCredito(credito);
            Pagar.Visibility = App.Pedido.TotalAPagar > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void Pagar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.Pagar()) // Se transacionar abre a comanda, adicona creditos, adiciona o pagamento, mas não fecha a comanda
            {
                if (App.Pedido.TipoFluxo == Services.EFluxo.AdicionarCredito)
                    FimPage.MensagemFinal = "Créditos adicionados\nSaldo Atual: R$ " + App.Pedido.ClienteSaldo.ToString("N2");
                else
                    FimPage.MensagemFinal = "Comanda aberta e créditos adicionados\nSaldo Atual: R$ " + App.Pedido.ClienteSaldo.ToString("N2");

                App.Navigate<FimPage>();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ClienteCreditoPage>();
        }
    }
}
