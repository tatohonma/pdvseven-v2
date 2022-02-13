using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    public partial class frmPedidoProdutoCancelamento : FormTouch
    {
        Int32 IDPedidoProduto;
        Int32 IDUsuario;

        public frmPedidoProdutoCancelamento(Int32 idPedidoProduto, Int32 idUsuario)
        {
            IDPedidoProduto = idPedidoProduto;
            IDUsuario = idUsuario;

            InitializeComponent();
        }

        private void frmPedidoProdutoCancelamento_Load(object sender, EventArgs e)
        {
            PedidoProdutoInformation pedidoProduto = PedidoProduto.Carregar(IDPedidoProduto);
            ckbRetornar.Visible = BLL.PDV.PossuiEstoque();

            List<MotivoCancelamentoInformation> listaMotivo = MotivoCancelamento.Listar();
            listaMotivo.Insert(0, new MotivoCancelamentoInformation());
            cbbMotivoCancelamento.DataSource = listaMotivo;

            lblProduto.Text = pedidoProduto.Produto.Nome;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Int32 idMotivoCancelamento;
            String observacoesCancelamento;

            if (cbbMotivoCancelamento.SelectedIndex <= 0)
            {
                MessageBox.Show("Selecione o motivo do cancelamento!");
            }
            else
            {
                idMotivoCancelamento = ((MotivoCancelamentoInformation)cbbMotivoCancelamento.SelectedItem).IDMotivoCancelamento.Value;
                observacoesCancelamento = txtDescricao.Text;

                Pedido.CancelarProduto(AC.PDV.IDPDV.Value, IDUsuario, IDPedidoProduto, idMotivoCancelamento, observacoesCancelamento, ckbRetornar.Checked);

                this.Close();
            }
        }
        
    }
}
