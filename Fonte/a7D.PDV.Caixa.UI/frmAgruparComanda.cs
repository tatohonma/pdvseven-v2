using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using System.Text;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAgruparComanda : FormTouch
    {
        frmPrincipal FormPrincipal;
        List<PedidoInformation> ListaPedido;

        public frmAgruparComanda(frmPrincipal formPrincipal)
        {
            FormPrincipal = formPrincipal;

            InitializeComponent();
        }

        private void frmAgruparComanda_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            ListaPedido = new List<PedidoInformation>();
            ListarComandas();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarComanda();
        }

        private void AdicionarComanda()
        {
            var comanda = ComandaUtil.CarregarPorNumeroOuCodigo(txtCodigoComanda.Text);

            if (comanda.IDComanda == null)
                MessageBox.Show("Comanda não cadastrada! Verifique o número da comanda.");
            else if (comanda.ValorStatusComanda == EStatusComanda.Cancelada || comanda.ValorStatusComanda == EStatusComanda.Perdida)
                MessageBox.Show("Comanda perdida/cancelada! Não é possível agrupá-la.");
            else if (comanda.ValorStatusComanda == EStatusComanda.Liberada)
                MessageBox.Show("Comanda fechada!");
            else
            {
                PedidoInformation pedido = Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                if (pedido.ListaPagamento.Count() > 0)
                {
                    MessageBox.Show("Já existem pagamentos parciais\r\nNão é possível agrupar com pagamentos parciais", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ListaPedido.Any(p => p.IDPedido == pedido.IDPedido))
                {
                    ListaPedido.Add(pedido);
                    ListarComandas();
                }

                txtCodigoComanda.Text = "";
                txtCodigoComanda.Focus();
            }
        }

        private void ListarComandas()
        {
            ComandaInformation comanda;
            dgvComandas.Rows.Clear();
            Object[] row;

            foreach (var item in ListaPedido)
            {
                comanda = BLL.Comanda.CarregarPorGUID(item.GUIDIdentificacao);
                row = new Object[]
                    {
                        null,
                        comanda.Numero,
                        item.Cliente?.NomeCompleto,
                        item.QtdItens,
                        item.ValorTotalTemp,
                        item.GUIDIdentificacao,
                        item.TipoEntrada.ValorConsumacaoMinima
                    };

                dgvComandas.Rows.Add(row);
            }
            var valorProdutos = ListaPedido.Sum(l => l.ValorTotalProdutos);
            var consumacao = ListaPedido.Sum(l => l.TipoEntrada.ValorConsumacaoMinima.Value);

            lblProdutos.Text = valorProdutos.ToString("R$ #,##0.00");
            lblConsumacao.Text = consumacao.ToString("R$ #,##0.00");

            lblValorTotal.Text = Math.Max(valorProdutos, consumacao).ToString("R$ #,##0.00");
        }

        private void btnPagamento_Click(object sender, EventArgs e)
        {
            //Zerar entradas
            //Adicionar valores de entrada como produto na comanda master
            //Transferir produtos de todas comandas para a comanda master
            //Zerar consumacao mínima
            //Alterar consumacao mínima na comanda master com a consumacao mínima de todas as comandas
            //Fecha comandas agrupadas

            if (ListaPedido.Count < 2)
            {
                MessageBox.Show("Você deve informar pelo menos 2 comandas para poder agrupar!");
            }
            else
            {
                PedidoInformation pedidoMaster = ListaPedido[0];
                List<PedidoProdutoInformation> listaEntrada = new List<PedidoProdutoInformation>();
                List<PedidoProdutoInformation> listaTotalProduto = new List<PedidoProdutoInformation>();
                Decimal consumacaoMinima = pedidoMaster.TipoEntrada.ValorConsumacaoMinima.Value;
                String guidAgrupamentoPedido = Guid.NewGuid().ToString();

                for (int i = 0; i < ListaPedido.Count; i++)
                {
                    var h1 = Pedido.GetHash(ListaPedido[i], out List<object> i1);
                    var pedido2 = Pedido.CarregarCompleto(ListaPedido[i].IDPedido.Value);
                    var ex = Pedido.Compare(h1, i1, pedido2);
                    if (ex != null)
                    {
                        var comanda = BLL.Comanda.CarregarPorGUID(pedido2.GUIDIdentificacao);
                        MessageBox.Show($"A comanda {comanda} foi alterada\ne foi excluida deste agrupamento");
                        ListaPedido.RemoveAt(i);
                        ListarComandas();
                        return;
                    }
                }

                var log = new StringBuilder();
                try
                {
                    log.AppendLine($"AGRUPANDO {ListaPedido.Count} PEDIDOS");

                    log.AppendLine($"PEDIDO {pedidoMaster.IDPedido} PRINCIPAL");

                    log.AppendLine();

                    for (int i = 1; i < ListaPedido.Count; i++)
                    {
                        log.AppendLine($"PEDIDO {ListaPedido[i].IDPedido}");

                        consumacaoMinima += ListaPedido[i].TipoEntrada.ValorConsumacaoMinima.Value;

                        foreach (var produto1 in ListaPedido[i].ListaProduto.Where(l => l.Produto.IDProduto != ProdutoInformation.IDProdutoEntrada))
                        {
                            log.AppendLine($"\t{produto1.IDPedidoProduto}: {produto1.Produto.Nome}");
                            produto1.Pedido = pedidoMaster;
                            PedidoProduto.Salvar(produto1);
                            foreach (var modificacao in produto1.ListaModificacao)
                            {
                                log.AppendLine($"\t\t{modificacao.IDPedidoProduto}: {modificacao.PedidoProdutoPai.IDPedidoProduto} - {modificacao.Produto.Nome} ");
                                modificacao.Pedido = pedidoMaster;
                                PedidoProduto.Salvar(modificacao);
                            }
                        }

                        foreach (var produto1 in ListaPedido[i].ListaProduto.Where(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoEntrada))
                        {
                            log.AppendLine($"\t{produto1.IDPedidoProduto}: {produto1.Produto.Nome}");
                            produto1.Pedido = pedidoMaster;
                            produto1.Notas += " / " + ListaPedido[i].Cliente?.NomeCompleto ?? "";
                            PedidoProduto.Salvar(produto1);
                        }

                        ListaPedido[i].DtPedidoFechamento = DateTime.Now;
                        ListaPedido[i].ValorServico = 0;
                        ListaPedido[i].ValorDesconto = 0;
                        ListaPedido[i].StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Finalizado };
                        ListaPedido[i].GUIDAgrupamentoPedido = guidAgrupamentoPedido;
                        ListaPedido[i].Caixa = frmPrincipal.Caixa1;

                        Pedido.Salvar(ListaPedido[i]);

                        var comanda = BLL.Comanda.CarregarPorGUID(ListaPedido[i].GUIDIdentificacao);
                        comanda.StatusComanda = EStatusComanda.Liberada.ToObjInfo();

                        log.AppendLine($"COMANDA {comanda.Numero} FECHADA");
                        BLL.Comanda.Salvar(comanda);

                        log.AppendLine();
                    }

                    pedidoMaster.GUIDAgrupamentoPedido = guidAgrupamentoPedido;
                    pedidoMaster.ValorConsumacaoMinima = consumacaoMinima;

                    Pedido.Salvar(pedidoMaster);

                    var result = NormalOuTouch.FechaPagamento(pedidoMaster.IDPedido.Value);
                    log.AppendLine("Resultado do pagamento: " + result.ToString());

                    if (result != DialogResult.OK)
                        MessageBox.Show("As comandas foram agrupadas\nMas o pagamento ainda não foi realizado", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Logs.Info(CodigoInfo.I009, log.ToString());
                }
                catch (Exception ex)
                {
                    ex.Data.Add("log", log.ToString());
                    Logs.ErroBox(CodigoErro.E143, ex);
                }
                this.Close();
            }
        }

        private void dgvComandas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Abrir form frmDetalhesPedidoProduto
            if (dgvComandas.SelectedRows.Count > 0)
            {
                var gUIDIdentificacao = dgvComandas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();
                new frmDetalhesPedidoProduto(FormPrincipal, gUIDIdentificacao).ShowDialog();
            }
        }

        private void txtCodigoComanda_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (BLL.Comanda.KeyPressValid(e))
                e.Handled = true;
        }

        private void txtCodigoComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtCodigoComanda.Text != "")
                AdicionarComanda();
        }

        private void dgvComandas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var gUIDIdentificacao = dgvComandas.Rows[e.RowIndex].Cells["GUIDIdentificacao"].Value.ToString();
                var pedido = Pedido.CarregarUltimoPedido(gUIDIdentificacao);
                ListaPedido = ListaPedido.Where(p => p.IDPedido != pedido.IDPedido).ToList();
                ListarComandas();
            }
        }
    }
}
