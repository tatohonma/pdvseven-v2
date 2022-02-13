using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.EF.Enum;
using System.Text;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmTransferirProdutos : FormTouch
    {
        public frmTransferirProdutos()
        {
            InitializeComponent();
        }

        private PedidoInformation Pedido1;
        private ETipoPedido tipoPedido_selecionado;
        private string GUIDIdentificacao_selecionado;
        private ETipoPedido tipoPedido_destino;
        private string GUIDIdentificacao_destino;
        private string hashOriginal;
        private List<object> itens1;
        UsuarioInformation usuario;

        public frmTransferirProdutos(ETipoPedido idTipoPedido_selecionado, String guidIdentificacao_selecionado) : this()
        {
            tipoPedido_selecionado = idTipoPedido_selecionado;
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;

            Pedido1 = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
            hashOriginal = Pedido.GetHash(Pedido1, out itens1);
        }

        private void frmTransferirProdutos_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            MesaInformation mesa;
            ComandaInformation comanda;

            if (tipoPedido_selecionado == ETipoPedido.Mesa)
            {
                mesa = Mesa.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "MESA " + mesa.Numero;
            }
            else if (tipoPedido_selecionado == ETipoPedido.Comanda)
            {
                comanda = Comanda.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "COMANDA " + comanda.Numero;
            }

            var listaTipoPedido = new List<TipoPedidoInformation>
            {
                new TipoPedidoInformation { IDTipoPedido = 10, Nome = "Mesa" },
                new TipoPedidoInformation { IDTipoPedido = 20, Nome = "Comanda" }
            };

            cbbTipoPedido.DataSource = listaTipoPedido;
            CarregarGrid();

            if (Pedido1.ListaPagamento.Count() > 0)
            {
                MessageBox.Show("Já existem pagamentos parciais\r\nNão é possível realizar transferências parciais", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rdbParcial.Enabled = false;
                rdbParcial.Checked = false;
                rdbTotal.Checked = true;
                rdbTotal_CheckedChanged(null, null);
            }
        }

        private void CarregarGrid()
        {
            Object[] row;
            String descricao;
            String modificacoes;
            foreach (var item in Pedido1.ListaProduto.Where(l => l.Produto.TipoProduto.IDTipoProduto == 10 && l.Cancelado != true).ToList())
            {
                descricao = item.Produto.Nome;
                if (item.Notas != null && item.Notas.Length > 0)
                    descricao += " (Notas: " + item.Notas + ")";

                if (item.ListaModificacao != null && item.ListaModificacao.Count > 0)
                {
                    modificacoes = "";
                    foreach (var modificacao in item.ListaModificacao)
                        modificacoes += ", " + modificacao.Produto.Nome;

                    descricao += System.Environment.NewLine + modificacoes.Substring(2);
                }

                row = new Object[]
                {
                    descricao,
                    item.Quantidade.Value.ToString("#,##0.00"),
                    "-",
                    "0",
                    "+",
                    item.IDPedidoProduto
                };

                dgvItens.Rows.Add(row);
            }

            dgvItens.Update();
        }

        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ((DataGridView)sender).Columns[e.ColumnIndex].GetType() == typeof(DataGridViewButtonColumn))
            {
                if (rdbTotal.Checked == true)
                {
                    if (rdbParcial.Enabled)
                        MessageBox.Show("Para mudar a quantidade de produtos que deseja transferir, selecione a opção \"Parcial\"!");
                    return;
                }
                var qtd = Convert.ToDecimal(dgvItens.SelectedRows[0].Cells["qtd"].Value);
                var quantidadeInteira = qtd % 1 == 0;
                switch ((((DataGridView)sender).Columns[e.ColumnIndex]).Name)
                {
                    case "menos":
                        if (quantidadeInteira)
                        {
                            if (Convert.ToInt32(dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value) > 0)
                                dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value) - 1;
                        }
                        else
                            dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value = 0;

                        break;

                    case "mais":
                        if (quantidadeInteira)
                        {
                            if (Convert.ToDecimal(dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value) < Convert.ToDecimal(dgvItens.SelectedRows[0].Cells["qtd"].Value))
                                dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value) + 1;
                        }
                        else
                            dgvItens.SelectedRows[0].Cells["qtdTransferir"].Value = qtd.ToString("#,##0.00");

                        break;
                }
                dgvItens.Update();
            }
        }

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            if (!SelecionarDestino())
                return;

            // Ao clicar tem que recarregar tudo para ver se nada mudou
            Pedido1 = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
            var exDiff = Pedido.Compare(hashOriginal, itens1, Pedido1);
            if (exDiff != null)
                Logs.ErroBox(CodigoErro.A900, exDiff);

            if (Pedido1.StatusPedido == null || Pedido1.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                MessageBox.Show("O pedido não está mais aberto para realizar transferências");

            else if (Pedido1.ListaPagamento.Count() > 0 && this.rdbParcial.Checked)
                MessageBox.Show("Foi adicionado pagamentos ao pedido, não é possivel fazer transferência parciais");

            else
            {
                if (ConfiguracoesSistema.Valores.SolicitarSenhaTransferencia)
                {
                    if (NormalOuTouch.Autenticacao(false, true, false, false, out usuario) != DialogResult.OK)
                        return;
                }
                else
                    usuario = AC.Usuario;

                EfetuaTransferencia();
            }

            this.Close();
        }

        private void EfetuaTransferencia()
        {
            var log = new StringBuilder();
            try
            {
                var pedidoDestino = Pedido.CarregarUltimoPedido(GUIDIdentificacao_destino);

                if (pedidoDestino == null || pedidoDestino.IDPedido == null)
                {
                    if (tipoPedido_destino == ETipoPedido.Mesa)
                        pedidoDestino = Pedido.NovoPedidoMesa(GUIDIdentificacao_destino);
                    else
                        pedidoDestino = Pedido.NovoPedidoComanda(GUIDIdentificacao_destino, null, null);

                    log.AppendLine($"Novo IDPedido: {pedidoDestino.IDPedido}");
                }
                else
                    log.AppendLine($"IDPedido: {pedidoDestino.IDPedido}");

                foreach (DataGridViewRow item in dgvItens.Rows)
                {
                    var idPedidoProduto = Convert.ToInt32(item.Cells[5].Value);
                    var quantidade = Convert.ToDecimal(item.Cells[3].Value);

                    if (quantidade > 0)
                        log.AppendLine(PedidoProduto.TransferirProduto(idPedidoProduto, quantidade, pedidoDestino));
                }

                log.AppendLine();
                foreach (var pagamento in Pedido1.ListaPagamento)
                    log.AppendLine(PedidoPagamento.TransferirPagamento(pagamento, pedidoDestino, usuario.IDUsuario.Value));

                Logs.Info(CodigoInfo.I001, log.ToString(), $"TRANSFERÊNCIA: {lblIdentificacao.Text} => {tipoPedido_destino.ToString()} {txtDestino.Text} USUARIO: {usuario.IDUsuario}-{usuario.Nome}");

                if (!rdbTotal.Checked)
                    return;

                if (Pedido1.TipoPedido?.TipoPedido == ETipoPedido.Comanda
                 && ConfiguracoesSistema.Valores.ComandaComCheckin)
                    return;

                var resp = MessageBox.Show($"Fechar {Pedido1.TipoPedido?.Nome.ToLower()} de origem?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    Pedido1.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(Pedido1.IDPedido.Value);
                    if (Pedido.TentaFecharPorTransferencia(Pedido1, frmPrincipal.Caixa1.IDCaixa.Value))
                        FecharVenda.LiberaMesaComanda(Pedido1.TipoPedido.TipoPedido, GUIDIdentificacao_selecionado);
                    else
                        MessageBox.Show("Há valores pendentes, e não é possiver fazer o fechamento");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("log", log);
                Logs.ErroBox(CodigoErro.E804, ex);
            }
        }

        private void rdbTotal_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgvItens.Rows)
                item.Cells[3].Value = item.Cells[1].Value;
        }

        private void rdbParcial_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgvItens.Rows)
                item.Cells[3].Value = "0";
        }

        private void txtDestino_Leave(object sender, EventArgs e)
        {
            SelecionarDestino();
        }

        private bool SelecionarDestino()
        {
            GUIDIdentificacao_destino = null;

            if (txtDestino.Text == "" || !Int32.TryParse(txtDestino.Text, out int r))
            {
                MessageBox.Show("Informe o número de uma mesa/comanda para qual deseja transferir os produtos");
                return false;
            }

            tipoPedido_destino = (ETipoPedido)Convert.ToInt32(cbbTipoPedido.SelectedValue);

            if (tipoPedido_destino == ETipoPedido.Mesa)
            {
                var mesa = Mesa.CarregarPorNumero(Convert.ToInt32(txtDestino.Text));

                if (mesa.IDMesa == null)
                    MessageBox.Show("Verifique o número da mesa informada!");
                else
                {
                    switch ((EStatusMesa)mesa.StatusMesa.IDStatusMesa)
                    {
                        case EStatusMesa.ContaSolicitada:
                            MessageBox.Show("CONTA SOLICITADA! \nNão é mais possível transferir produtos.");
                            break;
                        default:
                            GUIDIdentificacao_destino = mesa.GUIDIdentificacao;
                            break;
                    }
                }
            }
            else if (tipoPedido_destino == ETipoPedido.Comanda)
            {
                var comanda = ComandaUtil.CarregarPorNumeroOuCodigo(txtDestino.Text);

                if (comanda.IDComanda == null)
                    MessageBox.Show("Verifique o número da comanda informada!");
                else
                {
                    switch (comanda.ValorStatusComanda)
                    {
                        case EStatusComanda.Liberada:
                            if (ConfiguracoesSistema.Valores.ComandaComCheckin)
                                MessageBox.Show("Comanda fechada! \nFavor informar o gerente.");
                            else
                                GUIDIdentificacao_destino = comanda.GUIDIdentificacao;
                            break;
                        case EStatusComanda.Cancelada:
                            MessageBox.Show("COMANDA CANCELADA! \nFavor informar o gerente.");
                            break;
                        case EStatusComanda.Perdida:
                            MessageBox.Show("COMANDA PERDIDA! \nFavor informar o gerente.");
                            break;
                        default:
                            GUIDIdentificacao_destino = comanda.GUIDIdentificacao;
                            break;
                    }
                }
            }
            return GUIDIdentificacao_destino != null;
        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }
    }
}