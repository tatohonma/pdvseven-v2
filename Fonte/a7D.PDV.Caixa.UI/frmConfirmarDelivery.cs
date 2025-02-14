using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmConfirmarDelivery : FormTouch
    {
        volatile int alertas = 0;

        public PedidoInformation Pedido { get; internal set; }

        public frmConfirmarDelivery()
        {
            InitializeComponent();
        }

        private void frmConfirmarDelivery_Load(object sender, EventArgs e)
        {
            // SystemSounds.Beep.Play();
            if (ConfiguracoesCaixa.Valores.NotificarDelivery == "AUDIO")
                ((frmPedidos)Owner).Player.Play();

            TagInformation tagDisplayId;
            switch (Pedido.OrigemPedido.IDOrigemPedido)
            {
                case 2:
                    tagDisplayId = BLL.Tag.Carregar(Pedido.GUIDIdentificacao, "ifood-displayId");
                    Text = $"Pedido {Pedido.IDPedido} iFood {tagDisplayId.Valor}";
                    break;
                case 3:
                    tagDisplayId = BLL.Tag.Carregar(Pedido.GUIDIdentificacao, "DeliveryOnline-order_id");
                    Text = $"Pedido {Pedido.IDPedido} DeliveryOnline {tagDisplayId.Valor}";
                    break;
                case 4:
                    tagDisplayId = BLL.Tag.Carregar(Pedido.GUIDIdentificacao, "anotaai-shortReference");
                    Text = $"Pedido {Pedido.IDPedido} Anota-Ai {tagDisplayId.Valor}";

                    TagInformation tagStatus = BLL.Tag.Carregar(Pedido.GUIDIdentificacao, "anotaai-status");
                    if(tagStatus.Valor == "em-producao")
                        ConfirmarPedido();

                    ConfiguracoesAnotaAi configAnotaAi = new ConfiguracoesAnotaAi();
                    if (configAnotaAi.AceitarAutomatico)
                        ConfirmarPedido();

                    break;
            }

            txtPedido.Text = Pedido.Observacoes;
            lblResumo.Text =
                Pedido.Cliente.NomeCompleto +
                "\r\nEndereço: " + Pedido.Cliente.EnderecoCompleto +
                "\r\nValor Total: R$ " + Pedido.ValorTotal.Value.ToString("N2");

            if (Pedido.StatusPedido.StatusPedido == EF.Enum.EStatusPedido.EmCancelamento)
            {
                Text = "CANCELAMENTO " + Text;
                btnConfirmar.Visible = false;
                btnRejeirar.Text = "CONFIRMAR CANCELAMENTO";

                int w = (Width - 36) / 2;
                btnRejeirar.Left = 12;
                btnRejeirar.Width = w;

                btnVer.Left = w + 24;
                btnVer.Width = w;
                
            }
            else
                Text = "NOVO " + Text;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            ConfirmarPedido();
        }

        private void ConfirmarPedido()
        {
            tmrWait.Stop();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnRejeirar_Click(object sender, EventArgs e)
        {
            tmrWait.Stop();
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnVer_CheckedChanged(object sender, EventArgs e)
        {
            ((frmPedidos)Owner).Player?.Stop();
            txtPedido.Visible = true;
            lblResumo.Visible = false;
            btnVer.Visible = false;
        }

        private void frmConfirmarDelivery_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((frmPedidos)Owner).Player?.Stop();
        }

        private void tmrWait_Tick(object sender, EventArgs e)
        {
            if (alertas++ > 5)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}