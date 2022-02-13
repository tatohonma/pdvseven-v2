using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class ListaPedidoComanda : UserControl
    {
        public delegate void PedidoSelecionadoEventHandler(object sender, System.EventArgs e);
        public event PedidoSelecionadoEventHandler PedidoSelecionado;

        public String GUIDIdentificacao_selecionado;
        private DataTable ListaComandas;
        private DateTime dtLastKey;

        public Action<string> AdicionarCreditos = null;

        public ListaPedidoComanda()
        {
            InitializeComponent();
        }

        private void btnAbrirComanda_Click(object sender, EventArgs e)
        {
            var frm = new frmAbrirComanda();
            var result = frm.ShowDialog();

            AtualizarLista(true);

            if (result == DialogResult.OK
             && ConfiguracoesSistema.Valores.ComandaComCredito)
            {
                var saldo = Saldo.ClienteSaldoLiquido(frm.Pedido.Cliente.IDCliente.Value);
                if (saldo <= 0)
                    AdicionarCreditos?.Invoke(frm.Pedido.GUIDIdentificacao);
            }
        }

        public void AtualizarLista(bool clear = false)
        {
            if (clear)
                dgvComandas.Rows.Clear();

            ListaComandas = ComandaUtil.ListarAbertas(txtPesquisarComanda.Text, txtCodigoComanda.Text);
            ExibirLista();
        }

        private void ExibirLista()
        {
            //txtCodigoComanda_Leave(null, null);

            Object[] row;
            SuspendLayout();
            var list = from l in ListaComandas.AsEnumerable()
                       select new
                       {
                           GUIDIdentificacao = l.Field<String>("GUIDIdentificacao"),
                           IDStatusComanda = l.Field<Int32>("IDStatusComanda"),
                           Numero = l.Field<Int32>("Numero"),
                           NomeCompleto = l.Field<String>("NomeCompleto"),
                           Bloqueado = l.Field<Boolean>("Bloqueado")
                       };

            for (int i = dgvComandas.Rows.Count - 1; i >= 0; i--)
            {
                if (list.Any(l => l.GUIDIdentificacao == dgvComandas.Rows[i].Cells["GUIDIdentificacao"].Value.ToString()) == false)
                    dgvComandas.Rows.RemoveAt(i);
            }

            Boolean adicionar;
            foreach (var item in list)
            {
                row = new Object[]
                {
                    item.Numero,
                    item.NomeCompleto,
                    item.NomeCompleto,
                    item.GUIDIdentificacao,
                    item.IDStatusComanda,
                    item.Bloqueado
                };

                adicionar = true;
                for (int i = dgvComandas.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvComandas.Rows[i].Cells["GUIDIdentificacao"].Value.ToString() == item.GUIDIdentificacao)
                    {
                        adicionar = false;
                        break;
                    }
                }

                if (adicionar == true)
                    dgvComandas.Rows.Add(row);
            }
            ResumeLayout();
            AtualizarStatusComandas();
        }

        private void AtualizarStatusComandas()
        {
            var comandas = EF.Repositorio.Listar<tbComanda>();
            foreach (DataGridViewRow item in dgvComandas.Rows)
            {
                var comanda = comandas.FirstOrDefault(c => c.GUIDIdentificacao == item.Cells["guidIdentificacao"].Value.ToString());
                if (Convert.ToBoolean(item.Cells["Bloqueado"].Value) == true)
                {
                    item.Cells["Numero"].Style.ForeColor = Color.Black;
                    item.Cells["Numero"].Style.BackColor = Color.Red;
                    item.Cells["Numero"].Style.SelectionForeColor = Color.DarkRed;
                    item.Cells["NomeExibicao"].Style.ForeColor = Color.Black;
                    item.Cells["NomeExibicao"].Style.BackColor = Color.Red;
                    item.Cells["NomeExibicao"].Style.SelectionForeColor = Color.DarkRed;
                    item.Cells["NomeExibicao"].Value = "(Bloqueado:" + StatusComanda.CarregarNome((EStatusComanda)comanda.IDStatusComanda) + ") " + item.Cells["NomeCompleto"].Value;
                }
                else if (Convert.ToInt32(item.Cells["IDStatusComanda"].Value) != (int)EStatusComanda.EmUso)
                {
                    item.Cells["Numero"].Style.ForeColor = Color.Black;
                    item.Cells["Numero"].Style.BackColor = Color.Yellow;
                    item.Cells["Numero"].Style.SelectionForeColor = Color.Yellow;
                    item.Cells["NomeExibicao"].Style.ForeColor = Color.Black;
                    item.Cells["NomeExibicao"].Style.BackColor = Color.Yellow;
                    item.Cells["NomeExibicao"].Style.SelectionForeColor = Color.Yellow;
                    item.Cells["NomeExibicao"].Value = "(" + StatusComanda.CarregarNome((EStatusComanda)comanda.IDStatusComanda) + ") " + item.Cells["NomeCompleto"].Value;
                }
            }
        }

        private void dgvComandas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                GUIDIdentificacao_selecionado = dgvComandas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();
                PedidoSelecionado(sender, e);
            }
        }

        private void txtPesquisarComanda_TextChanged(object sender, EventArgs e)
        {
            dtLastKey = DateTime.Now;
            if (txtPesquisarComanda.Text.Length > 1)
                PesquisarComanda(sender, e);
        }

        private void PesquisarComanda(object sender, EventArgs e)
        {
            AtualizarLista();

            if (dgvComandas.Rows.Count == 1)
            {
                GUIDIdentificacao_selecionado = dgvComandas.Rows[0].Cells["GUIDIdentificacao"].Value.ToString();
                AtualizarStatusComandas();
            }
            else
            {
                GUIDIdentificacao_selecionado = null;
            }

            PedidoSelecionado(sender, e);
        }

        private void dgvComandas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                GUIDIdentificacao_selecionado = dgvComandas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();

                PedidoInformation pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
                if (pedido.IDPedido != null)
                {
                    frmAdicionarCliente form = new frmAdicionarCliente(pedido.Cliente.IDCliente.Value);
                    form.ShowDialog();
                }
                else
                    AtualizarLista();
            }
            else
            {
                GUIDIdentificacao_selecionado = null;
            }

            ExibirLista();
            AtualizarStatusComandas();

            PedidoSelecionado(sender, e);
        }

        private void txtCodigoComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PesquisarComanda(sender, e);
        }

        private void txtCodigoComanda_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (Comanda.KeyPressValid(e))
                e.Handled = true;
            else
                dtLastKey = DateTime.Now;

        }

        private async void txtCodigoComanda_Leave(object sender, EventArgs e)
        {
            await Task.Delay(1000);

            if (txtPesquisarComanda.Focused)
                return;

            txtCodigoComanda.Focus();
        }
    }
}
