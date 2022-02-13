using a7D.PDV.Integracao.API2.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ClienteCreditoPage : Page
    {
        List<Produto> creditos;

        public ClienteCreditoPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.Pedido.TipoFluxo == Services.EFluxo.AdicionarCredito)
                    Titulo.Text = "Credito";
                else
                    Titulo.Text = $"Entrada: R$ {App.Pedido.TotalPedido.ToString("N2")} + Crédito";

                Numero.Text = App.Pedido.ComandaNumero.ToString();
                Nome.Text = App.Pedido.ClienteNome;
                App.Pedido.Bind(Pagar);
                App.Pedido.Bind(Total, null, Saldo);

                creditos = await App.Produtos.PacoteCreditos();
                creditos = creditos.OrderBy(p => p.ValorUnitario).ToList();
                if (creditos.Count == 0)
                    throw new Exception("Não há creditos diponível para compra");

                if (creditos[0].ValorUnitario == 0)
                {
                    if (creditos.Count == 1)
                    {
                        App.Navigate<ClienteCreditoValorPage>();
                        return;
                    }
                    else
                    {
                        creditos.Add(creditos[0]);
                        creditos.RemoveAt(0);
                    }
                }

                App.Produtos.FillCredito(creditos, CreditoItens, SelecionaCredito_Click, null);
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                App.Navigate<InicialPage>();
            }
        }

        private void SelecionaCredito_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var prod = btn.Tag as Produto;
            if (prod.ValorUnitario == 0)
                App.Navigate<ClienteCreditoValorPage>();
            else
            {
                App.Pedido.DefinirCredito(prod);
                App.Produtos.FillCredito(creditos, CreditoItens, SelecionaCredito_Click, prod);
            }
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
            App.Navigate<InicialPage>();
        }

        private void Consulta_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaExtratoPage>();
        }
    }
}