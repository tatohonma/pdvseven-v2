using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BLL
{
    public class StackAssistente
    {
        public static int? minId = -1;

        public PedidoProdutoInformation Open { get; set; }

        public PainelModificacaoInformation Painel => listaPainelModificacoes[painelAtual].PainelModificacao;

        public int AchaProduto(ProdutoInformation prod) => pedidoProdutoBase.ListaModificacao.FindIndex(x => x.Produto.IDProduto.Value == prod.IDProduto.Value
             && x.PainelModificacao.IDPainelModificacao == listaPainelModificacoes[painelAtual].PainelModificacao.IDPainelModificacao);

        public bool PodeVoltar => painelAtual > 0;

        private List<ProdutoPainelModificacaoInformation> listaPainelModificacoes => pedidoProdutoBase.Produto.ListaPainelModificacao;
        private IEnumerable<PedidoProdutoInformation> produtosSelecionados => pedidoProdutoBase.ListaModificacao.Where(p => p.PainelModificacao.IDPainelModificacao == Painel.IDPainelModificacao);
        private PedidoProdutoInformation pedidoProdutoBase { get; set; }
        private int painelAtual { get; set; }

        public StackAssistente(PedidoProdutoInformation pedProduto)
        {
            painelAtual = 0;
            pedidoProdutoBase = pedProduto;

            if (pedidoProdutoBase.ListaModificacao == null)
                pedidoProdutoBase.ListaModificacao = new List<PedidoProdutoInformation>();
        }

        public PainelModificacaoInformation PainelPreco()
        {
            // entre todos os paineis
            for (int p = 0; p < listaPainelModificacoes.Count; p++)
            {
                // dos que tem paineis relacionados
                for (int pr = 0; pr < listaPainelModificacoes[p].PainelModificacao.PaineisRelacionados.Count; pr++)
                {
                    // se o painel atual é um dos que está relacionado
                    if (Painel.IDPainelModificacao.Value == listaPainelModificacoes[p].PainelModificacao.PaineisRelacionados[pr].PainelModificacao2.IDPainelModificacao)
                        return listaPainelModificacoes[p].PainelModificacao;
                }
            }
            return Painel;
        }


        public bool Up()
        {
            Open = null;
            var produtoSelecionado = produtosSelecionados.LastOrDefault();
            if(produtoSelecionado != null)
            {
                if (produtoSelecionado.Produto?.ListaPainelModificacao.Count > 0)
                {
                    Open = produtoSelecionado;
                    Open.PedidoProdutoPai = new PedidoProdutoInformation()
                    {
                        IDPedidoProduto = pedidoProdutoBase.IDPedidoProduto,
                        PedidoProdutoPai = pedidoProdutoBase
                    };
                    return true;
                }
            }
            return false;
        }

        public bool Avancar()
        {
            // E depois para frente
            if (painelAtual < listaPainelModificacoes.Count - 1)
            {
                painelAtual++;
                return true;
            }
            else
                return false;
        }

        public bool Voltar()
        {
            if (PodeVoltar)
            {
                painelAtual--;
                return true;
            }
            else
                return false;
        }

        public bool PainelValidar(TextBox txtNotas, bool avancar)
        {
            var produtos = produtosSelecionados;
            if (avancar && Painel.Maximo.HasValue && produtos.Count() > Painel.Maximo.Value)
                MessageBox.Show($"Selecione no máximo {Painel.Maximo} {(Painel.Maximo.Value > 1 ? " itens" : " item")}");

            else if (avancar && Painel.Minimo.HasValue && produtos.Count() < Painel.Minimo.Value)
                MessageBox.Show($"Selecione no mínimo {Painel.Minimo} {(Painel.Minimo.Value > 1 ? " itens" : " item")}");

            else if (produtos.Count() ==0 && !string.IsNullOrEmpty(txtNotas.Text))
                MessageBox.Show("Não é possível adicionar observações sem nenhuma modificação selecionada");

            else
            {
                // ao ir para um proximo painel salva a observação atual em todos paineis selecionados
                foreach (var p in produtos)
                    p.Notas = txtNotas.Text;

                return true;
            }
            return false;
        }

        public void FillExibirPainelAtual(Label lblProduto, Label lblMinMax, ListView lvItens, Button btnVoltar, Button btnConfirmar, TextBox txtNotas)
        {
            var prod = pedidoProdutoBase;
            try
            {
                btnConfirmar.Enabled = btnVoltar.Enabled = lvItens.Enabled = false; // prevenção de clicks

                btnVoltar.Enabled = painelAtual > 0;
                btnConfirmar.Text = (pedidoProdutoBase.PedidoProdutoPai != null || painelAtual < listaPainelModificacoes.Count - 1) ? "AVANÇAR" : "CONFIRMAR";

                // Cada painel será uma 'etapa'
                lblProduto.Text = "";
                while (prod != null && prod.Produto != null)
                {
                    lblProduto.Text += prod.Produto.Nome + "\r\n";
                    prod = prod.PedidoProdutoPai;
                }
                lblProduto.Text += string.IsNullOrEmpty(Painel.Titulo) ? Painel.Nome : Painel.Titulo;

                if (Painel.Minimo > 0 || Painel.Maximo > 0)
                    lblMinMax.Text = $"Mín: {Painel.Minimo ?? 0} / Máx: {Painel.Maximo ?? 0}";
                else
                    lblMinMax.Text = "Opcional";

                if (produtosSelecionados.Count() > 0)
                    txtNotas.Text = produtosSelecionados.ElementAt(0).Notas;
                else
                    txtNotas.Text = "";

                lvItens.Clear();

                if (Painel.IDTipoItem == 3) // produtos das categorias
                {
                    var idsCategorias = (from categorias in Painel.ListaCategoria select categorias.Categoria.IDCategoriaProduto.Value).ToArray();
                    foreach (var produto in Produto.ListarPorCategorias(idsCategorias, false))
                        lvItens.Items.Add(new ListViewItem() { Tag = produto });
                }
                else 
                {
                    foreach (var itemProduto in Painel.ListaProduto.OrderBy(c => c.Ordem.Value))
                        lvItens.Items.Add(new ListViewItem() { Tag = itemProduto.Produto });
                }
            }
            catch(Exception ex)
            {
                var exPDV = new ExceptionPDV(CodigoErro.E220, ex);
                exPDV.Data.Add("prod.IDProduto", prod?.Produto?.IDProduto);
                exPDV.Data.Add("prod.Nome", prod?.Produto?.Nome);
                exPDV.Data.Add("listaPainelModificacoes.Count", listaPainelModificacoes?.Count);
                
                throw exPDV;
            }
        }

        private void IncluiProduto(ProdutoInformation prod, PainelModificacaoInformation painel, string notas)
        {
            // para uma melhor performance só vai buscar os modificações relacionadas ao selecionar o produto
            prod.ListaPainelModificacao = ProdutoPainelModificacao.Listar(prod.IDProduto.Value);

            minId--;
            var novoPedidoProduto = new PedidoProdutoInformation()
            {
                IDPedidoProduto = minId, // ??? o que é ???
                IDPedidoProduto_Original = minId,
                PedidoProdutoPai = pedidoProdutoBase,
                Produto = prod,
                CodigoAliquota = null,
                ValorUnitario = prod.ValorUnitario,
                Quantidade = 1,
                PDV = pedidoProdutoBase.PDV,
                Usuario = pedidoProdutoBase.Usuario,
                Cancelado = false,
                Notas = notas,
                PainelModificacao = painel
            };
            pedidoProdutoBase.ListaModificacao.Add(novoPedidoProduto);
        }

        public void SelecionaProduto(ProdutoInformation prod, string notas)
        {
            int apagar = AchaProduto(prod);
            if (apagar != -1)
            {
                pedidoProdutoBase.ListaModificacao.RemoveAt(apagar);
            }
            else
            {
                if (Painel.Minimo==1 && Painel.Maximo == 1)
                {
                    pedidoProdutoBase.ListaModificacao.RemoveAll(p => p.PainelModificacao.IDPainelModificacao == Painel.IDPainelModificacao);
                }
                IncluiProduto(prod, Painel, notas);
            }
        }
    }
}