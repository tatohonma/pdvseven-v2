using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;
using Humanizer;
using System.Globalization;
using a7D.PDV.EF.Enum;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Componentes;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class ListaPedidoEntrega : UserControl
    {
        public delegate void PedidoSelecionadoEventHandler(object sender, System.EventArgs e);
        public event PedidoSelecionadoEventHandler PedidoSelecionado;

        public string GUIDIdentificacao_selecionado;
        DataTable ListaEntregas;
        private static readonly CultureInfo _culture = CultureInfo.GetCultureInfo("pt-BR");
        private readonly static Color darkYellow = Color.FromArgb(255, 232, 232, 192);

        public ListaPedidoEntrega()
        {
            InitializeComponent();
        }

        public void AtualizarLista()
        {
            ListaEntregas = Pedido.ListarDeliveryPendentes();
            ExibirLista();
        }

        private void ExibirLista()
        {
            string chave = txtPesquisarEntrega.Text.ToLower();
            Object[] row;

            var list = from l in ListaEntregas.AsEnumerable()
                       where
                           (
                                l.Field<int>("Telefone1Numero").ToString().Contains(chave) ||
                                l.Field<string>("NomeCompleto").ToLower().Contains(chave) ||
                                l.Field<string>("Endereco").ToLower().Contains(chave)
                           )
                       select new
                       {
                           GUIDIdentificacao = l.Field<string>("GUIDIdentificacao"),
                           IDStatusPedido = l.Field<int>("IDStatusPedido"),
                           StatusPedido = ObterStatus(l.Field<int>("IDStatusPedido"), l.Field<string>("StatusPedido"), l.Field<DateTime>("DtPedido"), l.Field<DateTime?>("DtEnvio")),
                           NomeCompleto = l.Field<string>("NomeCompleto"),
                           Telefone1Numero = l.Field<int>("Telefone1Numero"),
                           DataPedido = l.Field<DateTime>("DtPedido").ToUniversalTime().Humanize(utcDate: true, culture: _culture),
                           Endereco = l.Field<string>("Endereco"),
                           EnderecoNumero = l.Field<string>("EnderecoNumero"),
                           Observacao = l.Field<string>("Observacao"),
                           Bairro = l.Field<string>("Bairro"),
                           Cidade = l.Field<string>("Cidade")
                       };
            var saveRow = -1;
            if (dgvEntregas.Rows.Count > 0)
                saveRow = dgvEntregas.FirstDisplayedCell.RowIndex;
            dgvEntregas.Rows.Clear();
            foreach (var item in list)
            {
                row = new Object[]
                {
                    item.StatusPedido,
                    item.DataPedido,
                    item.NomeCompleto,
                    ObterEndereco(item.Endereco, item.EnderecoNumero, item.Bairro, item.Cidade, item.Observacao),
                    item.Telefone1Numero,
                    item.GUIDIdentificacao,
                    item.IDStatusPedido
                };

                dgvEntregas.Rows.Add(row);
            }

            AtualizarStatus();
            if (saveRow >= 0)
            {
                try
                {
                    dgvEntregas.FirstDisplayedScrollingRowIndex = saveRow;
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }
        }

        private static string ObterStatus(int idStatusPedido, string status, DateTime dataPedido, DateTime? dataEnvio)
        {
            string texto = status;
            if (idStatusPedido == (int)EStatusPedido.NaoConfirmado)
                texto = "Não Confirmado a " + dataPedido.ToUniversalTime().Humanize(utcDate: true, culture: _culture);
            else if (idStatusPedido == (int)EStatusPedido.Enviado)
                texto += " " + dataEnvio?.ToUniversalTime().Humanize(utcDate: true, culture: _culture);
            else
                texto += " " + dataPedido.ToUniversalTime().Humanize(utcDate: true, culture: _culture);

            return texto;
        }

        private static string ObterEndereco(string rua, string numero, string bairro, string cidade, string observacao)
        {
            var sb = new StringBuilder();

            sb.Append(rua);
            sb.Append(!string.IsNullOrWhiteSpace(numero) ? $", {numero}" : string.Empty);
            sb.Append(!string.IsNullOrWhiteSpace(bairro) ? $", {bairro}" : string.Empty);
            sb.Append(!string.IsNullOrWhiteSpace(cidade) ? $", {cidade}" : string.Empty);
            sb.Append(!string.IsNullOrWhiteSpace(observacao) ? $", {observacao}" : string.Empty);

            return sb.ToString();
        }

        private void AtualizarStatus()
        {
            //ComandaInformation comanda;
            dgvEntregas.ClearSelection();

            foreach (DataGridViewRow item in dgvEntregas.Rows)
            {
                if (item.Cells["GUIDIdentificacao"].Value.ToString() == GUIDIdentificacao_selecionado)
                    item.Selected = true;

                item.Cells[0].Style.Font = new Font(this.Font, FontStyle.Bold);

                DataGridViewImageCell icone = (DataGridViewImageCell)item.Cells[7];
                if (((string)item.Cells["GUIDIdentificacao"].Value).StartsWith("ifood#"))
                    icone.Value = Resources.ifood;
                else
                    icone.Value = Resources.semImagem;

                if (Convert.ToInt32(item.Cells["IDStatusPedido"].Value) == (int)EStatusPedido.NaoConfirmado)
                {
                    for (var i = 0; i < item.Cells.Count; i++)
                    {
                        item.Cells[i].Style.SelectionForeColor = Color.Black;
                        item.Cells[i].Style.BackColor = Color.LightPink;
                        item.Cells[i].Style.SelectionBackColor = Color.Red;
                    }
                }
                else if (Convert.ToInt32(item.Cells["IDStatusPedido"].Value) == (int)EStatusPedido.Enviado)
                {
                    for (var i = 0; i < item.Cells.Count; i++)
                    {
                        item.Cells[i].Style.SelectionForeColor = Color.Black;
                        item.Cells[i].Style.BackColor = Color.LightYellow;
                        item.Cells[i].Style.SelectionBackColor = darkYellow;
                    }
                }
            }
        }

        private void btnNovoPedidoEntrega_Click(object sender, EventArgs e)
        {
            using (var frm = frmNovoDelivery.NovoPedidoDelivery(false))
            {
                frm.ShowDialog();
            }

            AtualizarLista();
        }

        private void dgvEntregas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Refresh();
            if (dgvEntregas.SelectedRows.Count >= 0)
            {
                try
                {
                    GUIDIdentificacao_selecionado = dgvEntregas.SelectedRows[0].Cells["GUIDIdentificacao"].Value.ToString();
                    AtualizarStatus();
                    PedidoSelecionado(sender, e);
                }
                catch { }
            }

            Cursor = Cursors.Default;
            Refresh();
        }

        private void dgvEntregas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //clickWaiter.Stop();
            //if (e.RowIndex >= 0)
            //{
            //    Cursor = Cursors.WaitCursor;
            //    Refresh();
            //    GUIDIdentificacao_selecionado = dgvEntregas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();

            //    using (var frm = frmNovoDelivery.EditarPedidoDelivery(frmPrincipal.Caixa1, frmPrincipal.PDV1, GUIDIdentificacao_selecionado))
            //    {
            //        frm.ShowDialog();
            //    }
            //}
            //else
            //{
            //    GUIDIdentificacao_selecionado = null;
            //}

            //ExibirLista();
            //AtualizarStatus();

            //PedidoSelecionado(sender, e);

            //Cursor = Cursors.Default;
            //Refresh();
        }

        private void txtPesquisarEntrega_TextChanged(object sender, EventArgs e)
        {
            PesquisarPedido(sender, e);
        }

        private void PesquisarPedido(object sender, EventArgs e)
        {
            ExibirLista();

            if (dgvEntregas.Rows.Count == 1)
            {
                GUIDIdentificacao_selecionado = dgvEntregas.Rows[0].Cells["GUIDIdentificacao"].Value.ToString();
                AtualizarStatus();
            }
            else
            {
                GUIDIdentificacao_selecionado = null;
            }

            PedidoSelecionado(sender, e);
        }

        private void dgvEntregas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvEntregas.Columns["Endereco"].Index)
            {
                var cellEndereco = dgvEntregas.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (string.IsNullOrWhiteSpace(cellEndereco.Value as string) == false)
                {
                    cellEndereco.ToolTipText = cellEndereco.Value as string;
                }
            }
        }

        private void dgvEntregas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //clickWaiter.Stop();
            if (e.RowIndex >= 0)
            {
                GUIDIdentificacao_selecionado = dgvEntregas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();

                using (var frm = frmNovoDelivery.EditarPedidoDelivery(GUIDIdentificacao_selecionado))
                {
                    frm.ShowDialog();
                }
            }
            else
            {
                GUIDIdentificacao_selecionado = null;
            }

            ExibirLista();
            AtualizarStatus();

            PedidoSelecionado(sender, e);
        }

        private void clickTimer_tick(object sender, EventArgs e)
        {
            if (dgvEntregas.SelectedRows.Count >= 0)
            {
                Cursor = Cursors.WaitCursor;
                Refresh();
                GUIDIdentificacao_selecionado = dgvEntregas.SelectedRows[0].Cells["GUIDIdentificacao"].Value.ToString();
                AtualizarStatus();

                PedidoSelecionado(sender, e);

                Cursor = Cursors.Default;
                Refresh();
            }
        }

        private void toolstripMenuCancelarPedido_Click(object sender, EventArgs e)
        {
            if (dgvEntregas.SelectedRows.Count > 0)
            {
                if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
                {
                    using (var form = new frmCancelarPedido(usuario.IDUsuario.Value, GUIDIdentificacao_selecionado))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            GUIDIdentificacao_selecionado = null;
                        }
                    }
                    ExibirLista();
                    AtualizarStatus();

                    PedidoSelecionado(sender, e);
                }
            }
        }

        private void dgvEntregas_SelectionChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Refresh();
            if (dgvEntregas.SelectedRows.Count >= 0)
            {
                try
                {
                    GUIDIdentificacao_selecionado = dgvEntregas.SelectedRows[0].Cells["GUIDIdentificacao"].Value.ToString();
                    AtualizarStatus();
                    PedidoSelecionado(sender, e);
                }
                catch { }
            }

            Cursor = Cursors.Default;
            Refresh();
        }

        private void btnNovoPedidoRetirada_Click(object sender, EventArgs e)
        {
            using (var frm = frmNovoDelivery.NovoPedidoDelivery(true))
            {
                frm.ShowDialog();
            }

            AtualizarLista();
        }
    }
}
