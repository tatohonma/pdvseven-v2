using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmCancelarFiscal : FormTouch
    {
        internal static readonly string _formatoData = "yyyyMMddHHmmss";
        internal static readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");
        private List<RetornoSATInformation> listaRetornoSat;
        private int idUsuario;
        private int idPDV;

        public frmCancelarFiscal(int idUsuario, int idPDV)
        {
            this.idUsuario = idUsuario;
            this.idPDV = idPDV;
            InitializeComponent();
        }

        private void frmCancelarSAT_Load(object sender, EventArgs e)
        {
            //teste atualizacao branch otimizacao_cancelamento_sat
            GA.Post(this);
            PopularDataGridView();
        }

        private void PopularDataGridView()
        {
            var finalizadosUltimaHora = Pedido.ListarFinalizadosUltimaHora();
            finalizadosUltimaHora = finalizadosUltimaHora.Where(f => f.RetornoSAT_venda != null).ToList();

            listaRetornoSat = RetornoSAT.Listar();
            listaRetornoSat = listaRetornoSat.Where(r => r.EEEEE == "06000").ToList();
            listaRetornoSat = listaRetornoSat.Where(r => r.RetornoSATCancelamento == null).ToList();
            var horaConsultaUTC3 = DateTime.UtcNow.AddMinutes(-25);
            listaRetornoSat = listaRetornoSat.Where(r => DateTime.ParseExact(r.timeStamp, _formatoData, _cultureInfo).ToUniversalTime() > horaConsultaUTC3).ToList();

            var lista = listaRetornoSat.Select(r =>
                new DadosGridViewCancelamento(r,
                finalizadosUltimaHora.FirstOrDefault(p => p.RetornoSAT_venda != null && p.RetornoSAT_venda.IDRetornoSAT == r.IDRetornoSAT))
            );

            dgvPrincipal.DataSource = lista.ToArray();
            dgvPrincipal.ClearSelection();
        }

        private void btnCancelarSat_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                var resp = MessageBox.Show("Deseja realmente cancelar o pedido selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resp == DialogResult.Yes)
                {
                    dgvPrincipal.UseWaitCursor = true;
                    btnCancelarSAT.Enabled = false;
                    try
                    {
                        var row = dgvPrincipal.SelectedRows[0];
                        var retornoSat = RetornoSAT.Carregar(Convert.ToInt32(row.Cells["IDRetornoSAT"].Value));
                        var dataSat = DateTime.ParseExact(retornoSat.timeStamp, _formatoData, _cultureInfo).ToUniversalTime();
                        if (dataSat < DateTime.UtcNow.AddMinutes(-25))
                        {
                            MessageBox.Show("Esse pedido não pode ser mais cancelado, finalizado a mais de 30 minutos", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            PopularDataGridView();
                            return;
                        }
                        var retornoCancSat = FiscalServices
                            .Cancelamento(retornoSat, idPDV, idUsuario)
                            .Enviar(out PedidoInformation pedido);

                        if (pedido != null)
                            Pedido.SalvarRetornoSATCancelamento(pedido.IDPedido.Value, retornoCancSat.IDRetornoSAT.Value);

                        if (retornoCancSat.EEEEE == "07000" || retornoCancSat.EEEEE == "07007")
                        {
                            if (pedido != null)
                            {
                                Pedido.AlterarStatus(pedido.IDPedido.Value, EStatusPedido.Cancelado);
                                PedidoPagamento.CancelarPorPedido(pedido.IDPedido.Value, idUsuario);
                                var listaPedidoProduto = PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                                // TODO: remover o false do cancelamento com SAT
                                foreach (var pedidoProduto in listaPedidoProduto.Where(pp => pp.Cancelado == false).ToList())
                                    Pedido.CancelarProduto(idPDV, idUsuario, pedidoProduto.IDPedidoProduto.Value, -1, null, false);
                            }
                            retornoSat.RetornoSATCancelamento = retornoCancSat;
                            RetornoSAT.Salvar(retornoSat);
                        }

                        MessageBox.Show(retornoCancSat.mensagem, retornoCancSat.EEEEE, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else
            {
                MessageBox.Show("Selecione o pedido a ser cancelado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }

    class DadosGridViewCancelamento
    {

        public DadosGridViewCancelamento(RetornoSATInformation retornoSat, PedidoInformation pedido)
        {
            if (pedido != null)
            {
                IDPedido = pedido.IDPedido.Value;
                Origem = OrigemPedido(pedido);
            }

            Selecione = false;
            IDRetornoSAT = retornoSat.IDRetornoSAT.Value;
            NumeroSessao = retornoSat.numeroSessao;
            DataSAT = pedido != null ? pedido.DtPedidoFechamento.Value.ToString("dd/MM/yyyy HH:mm:ss") : DateTime.ParseExact(retornoSat.timeStamp, frmCancelarFiscal._formatoData, frmCancelarFiscal._cultureInfo).ToString("dd/MM/yyyy HH:mm:ss");
            CPFCNPJ = string.IsNullOrWhiteSpace(retornoSat.CPFCNPJValue) ? "Não informado" : retornoSat.CPFCNPJValue;
            Valor = retornoSat.valorTotalCFe;
        }

        public string OrigemPedido(PedidoInformation pedido)
        {
            switch (pedido.TipoPedido.TipoPedido)
            {
                case ETipoPedido.Mesa:
                    return "Mesa " + Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.Value.ToString();
                case ETipoPedido.Comanda:
                    return "Comanda " + Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.Value.ToString();
                case ETipoPedido.Delivery:
                    return "Delivery";
                case ETipoPedido.Balcao:
                    return "Balcão";
            }
            return "";
        }

        public bool Selecione { get; set; }
        public int IDRetornoSAT { get; set; }
        public string NumeroSessao { get; set; }
        public int IDPedido { get; set; }
        public string Origem { get; set; }
        public string DataSAT { get; set; }
        public string CPFCNPJ { get; set; }
        public string Valor { get; set; }
    }
}
