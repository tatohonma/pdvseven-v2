using a7D.PDV.BLL;
using a7D.PDV.Componentes.Properties;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmPedidoProdutosModificacoesAssistente : Form
    {
        Stack<StackAssistente> stackVariavelList;
        StackAssistente variaveis;

        PedidoProdutoInformation originalPedidoProduto;
        List<PedidoProdutoInformation> originalListaModificacao;

        public frmPedidoProdutosModificacoesAssistente(PedidoProdutoInformation pedidoProduto, int? startId)
        {
            InitializeComponent();
            StackAssistente.minId = startId;

            stackVariavelList = new Stack<StackAssistente>();
            originalPedidoProduto = pedidoProduto;
            originalListaModificacao = pedidoProduto.ListaModificacao?.ToList();
            RealocaModificacoes(pedidoProduto);
            variaveis = new StackAssistente(pedidoProduto);
        }

        private void RealocaModificacoes(PedidoProdutoInformation pedidoProduto)
        {
            if (pedidoProduto.ListaModificacao == null)
            {
                pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
                return;
            }

            var novaLista = originalListaModificacao.ToList();
            int i = 0;
            while (i < novaLista.Count)
            {
                if (novaLista[i].PedidoProdutoPai.IDPedidoProduto == pedidoProduto.IDPedidoProduto)
                {
                    RealocaModificacoes(novaLista[i]);
                    i++;
                }
                else
                    novaLista.RemoveAt(i); // Remove modificações não relacionadas ao produto atual
            }
            pedidoProduto.ListaModificacao = novaLista;
        }

        private void frmPedidoProdutosModificacoes_Load(object sender, EventArgs e)
        {
            if (Settings.Default.frmPedidoProdutosModificacoesAssistenteLocation != Point.Empty)
            {
                StartPosition = FormStartPosition.Manual;
                Location = Settings.Default.frmPedidoProdutosModificacoesAssistenteLocation;
            }
            if (Settings.Default.frmPedidoProdutosModificacoesAssistenteSize != Size.Empty)
            {
                Size = Settings.Default.frmPedidoProdutosModificacoesAssistenteSize;
            }
            ExibirPainelAtual();
        }

        private void frmPedidoProdutos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.frmPedidoProdutosModificacoesAssistenteSize = Size;
                Settings.Default.frmPedidoProdutosModificacoesAssistenteLocation = Location;
            }
            else
            {
                Settings.Default.frmPedidoProdutosModificacoesAssistenteSize = RestoreBounds.Size;
                Settings.Default.frmPedidoProdutosModificacoesAssistenteLocation = RestoreBounds.Location;
            }
            if (DialogResult != DialogResult.OK)
                originalPedidoProduto.ListaModificacao = originalListaModificacao;
        }

        void ExibirPainelAtual()
        {
            variaveis.FillExibirPainelAtual(lblProduto, lblMinMax, lvItens, btnVoltar, btnConfirmar, txtNotas);
            tmr.Start(); // depois que tudo feito, e eventuais clicks são descartados libera a tela em 300ms
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            btnConfirmar.Enabled = lvItens.Enabled = true;
            btnVoltar.Enabled = variaveis.PodeVoltar || stackVariavelList.Count > 0;
            tmr.Stop();
        }

        // https://msdn.microsoft.com/pt-br/library/system.windows.forms.listview.drawitem(v=vs.110).aspx
        private void lvItens_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.Tag is ProdutoInformation prod)
            {
                bool selecionado = variaveis.AchaProduto(prod) != -1;
                e.Graphics.FillRectangle(selecionado ? new SolidBrush(SystemColors.Highlight) : new SolidBrush(Color.FromArgb(224, 231, 234)), e.Bounds);

                var fntBrush = new SolidBrush(SystemColors.HighlightText);
                var fnt = new Font("Arial", 10.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                var rcNome = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 20);
                e.Graphics.DrawString(prod.Nome, fnt, selecionado ? fntBrush : Brushes.Black, rcNome);
                e.Graphics.DrawString(PrecoAssistente.ProdutoValor(prod, variaveis.PainelPreco()).Value.ToString("C"), fnt, selecionado ? fntBrush : Brushes.Black, e.Bounds, new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far });
            }
        }

        private void lvItens_Resize(object sender, EventArgs e)
        {
            if (ClientSize.Width < 10)
                return;

            int qtdColunas = lvItens.ClientSize.Width / 190;
            if (qtdColunas < 1)
                qtdColunas = 1;

            int colPixel = (lvItens.ClientSize.Width) / qtdColunas;
            lvItens.TileSize = new Size(colPixel, 80);
            lvItens.Refresh();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!variaveis.PainelValidar(txtNotas, false))
                return;

            while (true)
            {
                if (!variaveis.Voltar())
                {
                    if (stackVariavelList.Count > 0)
                    {
                        variaveis = stackVariavelList.Pop();
                        continue;
                    }
                }
                break;
            }
            ExibirPainelAtual();
        }

        private void btnAvancar_Click(object sender, EventArgs e)
        {
            if (!variaveis.PainelValidar(txtNotas, true))
                return;
            if (!variaveis.Avancar())
            {
                if (stackVariavelList.Count > 0)
                {
                    variaveis = stackVariavelList.Pop();
                }
                else
                {
                    // caso de confirmação final
                    DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            ExibirPainelAtual();

        }

        private void lvItens_Click(object sender, EventArgs e)
        {
            if (lvItens.SelectedItems.Count == 0)
                return;

            foreach (var itemObj in lvItens.SelectedItems)
            {
                if (itemObj is ListViewItem item && item.Tag is ProdutoInformation prod)
                {
                    variaveis.SelecionaProduto(prod, txtNotas.Text);

                    if (item.Selected == true && prod.ListaPainelModificacao?.Count() > 0)
                        EntrarNaModificacao();

                    break;
                }
            }
            lvItens.Refresh();
        }

        private void EntrarNaModificacao()
        {
            if (variaveis.Up())
            {
                stackVariavelList.Push(variaveis);
                variaveis = new StackAssistente(variaveis.Open);
            }
            ExibirPainelAtual();
        }

        public void PainelRetornaProdutoItens(PedidoProdutoInformation pedidoProduto)
        {
            // para cada subitem já calculou e refez!
            PrecoAssistente.FinalizaNivel(pedidoProduto, false);

            // planifica as referencias
            var listaFinal = new List<PedidoProdutoInformation>();
            FinalizaModificacoes(pedidoProduto, listaFinal);
            pedidoProduto.ListaModificacao = listaFinal;
        }

        private void FinalizaModificacoes(PedidoProdutoInformation pedidoProduto, List<PedidoProdutoInformation> listaFinal)
        {
            if (pedidoProduto.ListaModificacao == null)
                return;

            foreach (var pedprod in pedidoProduto.ListaModificacao)
            {
                PrecoAssistente.FinalizaNivel(pedprod, true);
                FinalizaModificacoes(pedprod, listaFinal);
                listaFinal.Add(pedprod);
            }
            pedidoProduto.ListaModificacao = null;
        }
    }
}