using a7D.PDV.BLL;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class PedidoPagamentoProdutos : UserControl
    {
        public PedidoPagamentoProdutos()
        {
            InitializeComponent();
        }

        public void Fill(List<PedidoProdutoInformation> listaProduto)
        {
            dgvItens.Rows.Clear();
            var itens = listaProduto.Where(p => p.Cancelado == false).OrderBy(l => l.Produto.Nome);
            dgvItens.Columns[2].Visible = itens.Any(p => p.Viagem == true);
            object[] row;
            foreach (var item in itens)
            {
                if (item.ValorUnitario > 0 || ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                {
                    row = new object[] {
                        $"{item.Quantidade.Value.ToString("0.000")}×{item.Produto.Nome}",
                        item.ValorUnitarioString,
                        item.Viagem==true ? BLL.Utils.ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                    };
                    dgvItens.Rows.Add(row);
                    if (item.ListaModificacao != null)
                    {
                        foreach (var modificacao in item.ListaModificacao)
                        {
                            if (modificacao.ValorUnitario > 0 || ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                            {
                                row = new object[] {
                                    " - " + modificacao.Produto.Nome,
                                    modificacao.ValorUnitarioString,
                                    item.Viagem==true ? BLL.Utils.ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                                dgvItens.Rows.Add(row);
                            }
                        }
                    }
                }
                else if (item.ListaModificacao != null)
                {
                    bool exibiuPai = false;
                    foreach (var modificacao in item.ListaModificacao.OrderByDescending(mod => valorUnitario))
                    {
                        if (modificacao.ValorUnitario > 0 || !ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                        {
                            if (exibiuPai)
                            {
                                row = new object[] {
                                    " - " + modificacao.Produto.Nome,
                                    modificacao.ValorUnitario.Value.ToString("#,##0.00"),
                                    item.Viagem==true ? BLL.Utils.ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                            }
                            else
                            {
                                row = new object[] {
                                    $"{item.Quantidade.Value.ToString("0.000")}×{item.Produto.Nome}\r\n - {modificacao.Produto.Nome}",
                                    modificacao.ValorUnitarioString,
                                    item.Viagem==true ? BLL.Utils.ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                                exibiuPai = true;
                            }
                            dgvItens.Rows.Add(row);
                        }
                    }
                }
            }
            dgvItens.ClearSelection();
        }
    }
}
