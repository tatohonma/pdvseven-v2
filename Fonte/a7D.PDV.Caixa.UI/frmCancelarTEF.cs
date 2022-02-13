using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.DAO;
using a7D.PDV.EF.DTO;
using a7D.PDV.Integracao.Pagamento;
using a7D.PDV.Model;
using System;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmCancelarTEF : FormTouch
    {
        public frmCancelarTEF()
        {
            InitializeComponent();
        }

        private void frmCancelarTEF_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            dgvPagamentos.AutoGenerateColumns = false;
            dgvPagamentos.DataSource = Pagamentos.ListarTEFStone();
        }

        private void dgvPagamentos_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            btnCancelar.Enabled = e.RowIndex >= 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Exibir mensagem para confirmacao, informando que o pedido será reaberto;  Após o pedido reaberto, não pode dar baixa no estoque novamente do itens que já foram lançados

            if (dgvPagamentos.SelectedRows.Count > 0
             && dgvPagamentos.SelectedRows[0].DataBoundItem is PagamentoTEF pgTEF)
            {
                if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == System.Windows.Forms.DialogResult.OK)
                {
                    if(PinpadTEF.CancelarTransacao(TipoTEF.STONE, pgTEF.Autorizacao))
                    {
                        Pagamentos.Cancelar(pgTEF.IDPedidoPagamento);
                        this.Close();
                    }
                }
            }
        }
    }
}
