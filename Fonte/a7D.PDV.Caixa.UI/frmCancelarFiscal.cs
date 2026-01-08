using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Componentes;
using a7D.PDV.DAL;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmCancelarFiscal : FormTouch
    {
        internal static readonly string _formatoData = "yyyyMMddHHmmss";
        internal static readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");

        readonly int idUsuario;
        readonly int idPDV;

        public frmCancelarFiscal(int idUsuario, int idPDV)
        {
            this.idUsuario = idUsuario;
            this.idPDV = idPDV;
            InitializeComponent();
        }

        void frmCancelarSAT_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            PopularDataGridView();
        }

        void PopularDataGridView()
        {
            var pedidos = RetornoSATDAL.ListarPedidosParaCancelamento();
            dgvPrincipal.DataSource = pedidos;
            dgvPrincipal.ClearSelection();
        }

        void btnCancelarSat_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Selecione o pedido a ser cancelado", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            var resp = MessageBox.Show("Deseja realmente cancelar o pedido selecionado?",
                "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp != DialogResult.Yes)
                return;

            dgvPrincipal.UseWaitCursor = true;
            btnCancelarSAT.Enabled = false;

            try
            {
                var row = dgvPrincipal.SelectedRows[0];

                if (row.Cells["IDRetornoSAT"]?.Value == null)
                {
                    MessageBox.Show("Registro inválido: sem IDRetornoSAT.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var retornoSat = RetornoSAT.Carregar(Convert.ToInt32(row.Cells["IDRetornoSAT"].Value));
                if (retornoSat == null || !retornoSat.IDRetornoSAT.HasValue)
                {
                    MessageBox.Show("Não foi possível carregar o retorno fiscal da venda.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (retornoSat.RetornoSATCancelamento != null && retornoSat.RetornoSATCancelamento.IDRetornoSAT.HasValue)
                {
                    MessageBox.Show("Este pedido já possui cancelamento registrado.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopularDataGridView();
                    return;
                }

                DateTime dataSatUtc;
                try
                {
                    dataSatUtc = DateTime.ParseExact(retornoSat.timeStamp, _formatoData, _cultureInfo).ToUniversalTime();
                }
                catch
                {
                    MessageBox.Show("Timestamp fiscal inválido para validação do prazo.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PopularDataGridView();
                    return;
                }

                if (dataSatUtc < DateTime.UtcNow.AddMinutes(-27))
                {
                    MessageBox.Show("Esse pedido não pode mais ser cancelado (prazo expirado).", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PopularDataGridView();
                    return;
                }

                var retornoCancSat = FiscalServices
                    .Cancelamento(retornoSat, idPDV, idUsuario)
                    .Enviar(out PedidoInformation pedido);

                if (pedido != null && retornoCancSat?.IDRetornoSAT.HasValue == true)
                    Pedido.SalvarRetornoSATCancelamento(pedido.IDPedido.Value, retornoCancSat.IDRetornoSAT.Value);

                if (retornoCancSat != null && (retornoCancSat.EEEEE == "07000" || retornoCancSat.EEEEE == "07007"))
                {
                    if (pedido != null)
                    {
                        Pedido.AlterarStatus(pedido.IDPedido.Value, EStatusPedido.Cancelado);
                        PedidoPagamento.CancelarPorPedido(pedido.IDPedido.Value, idUsuario);

                        var listaPedidoProduto = PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                        foreach (var pedidoProduto in listaPedidoProduto.Where(pp => pp.Cancelado == false).ToList())
                            Pedido.CancelarProduto(idPDV, idUsuario, pedidoProduto.IDPedidoProduto.Value, -1, null, false);
                    }

                    retornoSat.RetornoSATCancelamento = retornoCancSat;
                    RetornoSAT.Salvar(retornoSat);
                }

                MessageBox.Show(retornoCancSat?.mensagem ?? "Operação concluída.",
                    retornoCancSat?.EEEEE ?? "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                PopularDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvPrincipal.UseWaitCursor = false;
                btnCancelarSAT.Enabled = true;
            }
        }
    }
}
