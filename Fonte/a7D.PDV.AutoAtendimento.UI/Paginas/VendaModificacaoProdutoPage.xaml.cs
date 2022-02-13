using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public class SelecaoModificacao
    {
        public string Nome { get; set; }
        public decimal? Valor { get; set; }
        public Visibility Painel { get; set; }
        public Visibility Modificacao { get; set; }
        public int Margin { get; set; }
        public SolidColorBrush Background { get; set; }
        public bool Selecionado { get; set; }
        public int ProdutoID { get; set; }

        public override string ToString()
        {
            return $"{Selecionado} {ProdutoID} {Nome}";
        }
    }

    public partial class VendaModificacaoProdutoPage : Page
    {
        private static SolidColorBrush backcolor = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0));

        decimal ValorUnitario;

        public List<SelecaoModificacao> itens;

        public VendaModificacaoProdutoPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var vi = App.Pedido.ItemEdit();

            itens = new List<SelecaoModificacao>();

            Produto.Text = vi.Produto.Nome;
            Descricao.Text = vi.Produto.Descricao;
            ValorUnitario = vi.Produto.ValorUnitario ?? 0m;

            if (vi.Produto.ValorUnitario == null)
                Preco.Text = "";
            else
                Preco.Text = "R$ " + ValorUnitario.ToString("N2");

            if (vi.Produto.urlImagem != null)
                Foto.Source = new BitmapImage(new Uri(vi.Produto.urlImagem));

            foreach (var pm in vi.Produto.PaineisDeModificacao)
            {
                itens.Add(new SelecaoModificacao
                {
                    Nome = pm.Titulo,
                    Painel = Visibility.Visible,
                    Modificacao = Visibility.Collapsed,
                    Background = default,
                    Selecionado = false
                });

                foreach (var mod in pm.Produtos.OrderBy(m => m.Ordem))
                {
                    if (itens.Any(i => i.ProdutoID == mod.IDProduto.Value))
                        continue;

                    var prod = App.Produtos.LoadModificacao(mod.IDProduto.Value);
                    if (prod == null)
                        continue;

                    itens.Add(new SelecaoModificacao
                    {
                        Nome = prod.Nome,
                        ProdutoID = mod.IDProduto.Value,
                        Valor = mod.Valor,
                        Painel = Visibility.Collapsed,
                        Modificacao = Visibility.Visible,
                        Margin = 5,
                        Background = backcolor,
                        Selecionado = vi.Modificacoes.Any(p => p.IDProduto == mod.IDProduto.Value)
                    });
                }
            }

            ModificacoesLista.ItemsSource = itens;

            CalculaValor();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            var vi = App.Pedido.ItemEdit();

            foreach (var item in itens)
            {
                if (item.Selecionado)
                {
                    if (!vi.Modificacoes.Any(p => p.IDProduto == item.ProdutoID))
                        vi.Modificacoes.Add(new Integracao.API2.Model.Item
                        {
                            IDProduto = item.ProdutoID,
                            Qtd = 1,
                            Preco = item.Valor
                        });
                }
                else if (item.ProdutoID > 0)
                {
                    var n = vi.Modificacoes.FindIndex(p => p.IDProduto == item.ProdutoID);
                    if (n >= 0)
                        vi.Modificacoes.RemoveAt(n);
                }
            }

            string erros = ValidaModificacoes(vi);

            if (string.IsNullOrEmpty(erros))
                App.Navigate<VendaCategoriaProdutoPage>();
            else
                ModalSimNaoWindow.Show(erros);
        }

        private string ValidaModificacoes(VendaItem vi)
        {
            var modificacoesSelecionadas = vi.Modificacoes.Select(m => m.IDProduto.Value).ToList();
            string erros = "";

            foreach (var painel in vi.Produto.PaineisDeModificacao)
            {
                // Mesma logica do caixa
                var num = modificacoesSelecionadas.Where(ms => painel.Produtos.Select(p => p.IDProduto.Value).Any(p => p == ms)).Count();

                var respeitaMin = painel.Min == null || num >= painel.Min;
                var respeitaMax = painel.Max == null || num <= painel.Max;

                if (!respeitaMin)
                    erros += $"Selecione ao menos {painel.Min} em {painel.Nome}\r\n";
                else if (!respeitaMax)
                    erros += $"Selecione no maximo {painel.Max} em {painel.Nome}\r\n";
            }
            return erros;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Pedido.ItemCancelar();

            App.Navigate<VendaCategoriaProdutoPage>();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CalculaValor();
        }

        private void CalculaValor()
        {
            decimal vTotal = ValorUnitario;

            foreach (var item in itens)
            {
                if (item.Selecionado)
                {
                    vTotal += item.Valor ?? 0;
                }
            }

            TotalProduto.Text = "R$ " + vTotal.ToString("N2");
        }
    }
}
