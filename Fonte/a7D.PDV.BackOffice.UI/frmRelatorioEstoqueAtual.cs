using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatorioEstoqueAtual : Form
    {
        public static bool rodando = false;

        DataTable estoqueAtual;
        public frmRelatorioEstoqueAtual()
        {
            InitializeComponent();
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            estoqueAtual = EntradaSaida.EstoqueInventario(dtpData.Value, exibirProdutosInativos: ckbProdutosInativos.Checked);
            estoqueAtual.Columns.RemoveAt(0);
            estoqueAtual.Columns.RemoveAt(1);
            dgvPrincipal.DataSource = estoqueAtual;

        }

        private void frmRelatorioEstoqueAtual_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (estoqueAtual != null)
                estoqueAtual.Dispose();
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (estoqueAtual != null)
            {
                RelatorioInformation relatorio = new RelatorioInformation
                {
                    Nome = "Posicao estoque"
                };

                String nomeArquivo = BackofficeUtil.NomeRelatorio(relatorio.Nome.ToLower().Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                BLL.Relatorio.ExportarParaTxt(estoqueAtual, nomeArquivo);

                MessageBox.Show("Relatório gerado em " + nomeArquivo);
            }
        }

        private void frmRelatorioEstoqueAtual_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            //btnInconsistencia.Visible = false; // TODO: Reverificar a necessidade e logica, isso pode dar problema!
            // AC.Usuario.IDUsuario == 1 && !rodando;
        }

        private void btnInconsistencia_Click(object sender, EventArgs e)
        {
            rodando = true;
            int qtd = 0;
            var pedidos = Pedido.ListarPedidoSemEstoque();
            foreach (var pedido in pedidos)
            {
                pedido.ListaProduto = PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                if (EntradaSaida.Movimentar(pedido))
                    qtd++;
            }
            rodando = false;
            MessageBox.Show("Total de pedidos corrigidos: " + qtd);
        }
    }
}
