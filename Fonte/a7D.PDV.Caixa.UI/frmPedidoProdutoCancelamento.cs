using System;
using System.Collections.Generic;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using a7D.PDV.Model.DTO;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutoCancelamento : FormTouch
    {
        int IDPedidoProduto;
        int IDUsuario;
        public bool Cancelar { get; set; } = true;
        public InformacoesCancelamento? InformacoesCancelamento { get; set; }

        public frmPedidoProdutoCancelamento(int idPedidoProduto, int idUsuario)
        {
            IDPedidoProduto = idPedidoProduto;
            IDUsuario = idUsuario;

            InitializeComponent();
        }

        private void frmPedidoProdutoCancelamento_Load(object sender, EventArgs e)
        {
            GA.Post(this);

            var pedidoProduto = PedidoProduto.Carregar(IDPedidoProduto);
            gbRetornarAoEstoque.Visible = BLL.PDV.PossuiEstoque();

            var listaMotivo = MotivoCancelamento.Listar();
            listaMotivo.Insert(0, new MotivoCancelamentoInformation());
            cbbMotivoCancelamento.DataSource = listaMotivo;

            lblProduto.Text = pedidoProduto.Produto.Nome;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (cbbMotivoCancelamento.SelectedIndex <= 0)
            {
                MessageBox.Show("Selecione o motivo do cancelamento!");
            }
            else
            {
                var idMotivoCancelamento = ((MotivoCancelamentoInformation)cbbMotivoCancelamento.SelectedItem).IDMotivoCancelamento.Value;
                var observacoesCancelamento = txtDescricao.Text;

                InformacoesCancelamento = new InformacoesCancelamento
                {
                    IDUsuario = IDUsuario,
                    IDMotivoCancelamento = idMotivoCancelamento,
                    Observacoes = observacoesCancelamento,
                    RetornarAoEstoque = rbSim.Checked
                };

                if (Cancelar)
                {
                    Pedido.CancelarProduto(AC.PDV.IDPDV.Value, IDUsuario, IDPedidoProduto, idMotivoCancelamento, observacoesCancelamento, rbSim.Checked);

                    ImprimirComprovanteCancelamento(IDPedidoProduto);
                }
                DialogResult = DialogResult.OK;

                Close();
            }
        }

        public static void ImprimirComprovanteCancelamento(int idPedidoProduto)
        {
            var pedidoProduto = PedidoProduto.Carregar(idPedidoProduto);

            pedidoProduto.Usuario = Usuario.Carregar(pedidoProduto.Usuario.IDUsuario.Value);
            pedidoProduto.UsuarioCancelamento = Usuario.Carregar(pedidoProduto.UsuarioCancelamento.IDUsuario.Value);
            pedidoProduto.PDVCancelamento = BLL.PDV.Carregar(pedidoProduto.PDVCancelamento.IDPDV.Value);
            pedidoProduto.MotivoCancelamento = MotivoCancelamento.Carregar(pedidoProduto.MotivoCancelamento.IDMotivoCancelamento.Value);

            var relatorio = new List<string>();

            relatorio.Add("Data/hora: " + pedidoProduto.DtCancelamento.Value.ToString("dd/MM/yyyy HH:mm"));
            relatorio.Add("PDV: " + pedidoProduto.PDVCancelamento.Nome);
            relatorio.Add("Usuário cancelamento: " + pedidoProduto.UsuarioCancelamento.Nome);
            relatorio.Add("Usuário lançamento: " + pedidoProduto.Usuario.Nome + "\n");
            pedidoProduto.Pedido = Pedido.Carregar(pedidoProduto.Pedido.IDPedido.Value);
            switch (pedidoProduto.Pedido.TipoPedido.IDTipoPedido.Value)
            {
                case 10:
                    var mesa = Mesa.CarregarPorGUID(pedidoProduto.Pedido.GUIDIdentificacao);
                    relatorio.Add($"Mesa: {mesa.Numero}");
                    break;
                case 20:
                    var comanda = Comanda.CarregarPorGUID(pedidoProduto.Pedido.GUIDIdentificacao);
                    relatorio.Add($"Comanda: {comanda.Numero}");
                    break;
            }

            relatorio.Add("Produto: " + pedidoProduto.Produto.Nome);
            relatorio.Add("Qtd.: " + pedidoProduto.Quantidade + "\n");

            relatorio.Add("Motivo cancelamento: " + pedidoProduto.MotivoCancelamento.Nome + "\n");
            relatorio.Add("Observações: " + pedidoProduto.ObservacoesCancelamento + "\n\n");

            relatorio.Add("Ass. responsável: _______________________________\n\n\n");

            frmPrincipal.Impressora1.RelatorioGerencial(relatorio, 8);
        }
    }
}
