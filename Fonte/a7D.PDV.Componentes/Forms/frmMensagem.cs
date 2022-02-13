using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.EF.Models;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmMensagem : FormTouch
    {
        public frmMensagem()
        {
            InitializeComponent();
        }

        private tbMensagem Mensagem;

        public static void Show(tbMensagem msg)
        {
            string title = "Mensagem (" + msg.IDMensagem + ")";
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == title)
                    return; // Mensagem já está em exibição
                else if (msg.IDRespostaProcesamento == -2 // ADIAR
                     && msg.DataVisualizada.HasValue
                     && msg.DataVisualizada > DateTime.Now.AddHours(-1))
                    return;
            }

            if (!msg.DataProcessamento.HasValue)
                msg.DataProcessamento = DateTime.Now;

            msg.ResultadoProcessamento = "exibido";
            EF.Repositorio.Atualizar(msg);

            var frm = new frmMensagem
            {
                Text = title,
                Mensagem = msg
            };

            bool simnao =
                   msg.Tipo == ETipoMensagem.Pergunta_SIMNAO
                || msg.Tipo == ETipoMensagem.Pergunta_Atualizar;

            frm.lblResumo.Text = msg.Texto;
            frm.btnSim.Visible = simnao;
            frm.btnNao.Visible = simnao;

            if (msg.Tipo == ETipoMensagem.Pergunta_Atualizar)
            {
                frm.btnOK.Text = "ADIAR";
                frm.btnOK.Visible = true;
            }
            else if (msg.Tipo == ETipoMensagem.Cobranca_Link)
            {
                frm.btnOK.Text = "PAGAR";
                frm.btnOK.Visible = true;
            }
            else if (msg.Tipo == ETipoMensagem.Abrir_Link)
            {
                frm.btnOK.Text = "ABRIR";
                frm.btnOK.Visible = true;
            }
            else
                frm.btnOK.Visible = !simnao;

            frm.Show();
        }

        private void btnSim_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnNao_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmMensagem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.None)
            {
                Mensagem.DataVisualizada = DateTime.Now;
                Mensagem.IDRespostaProcesamento = 0;
                if (DialogResult == DialogResult.OK)
                {
                    Mensagem.ResultadoProcessamento = btnOK.Text; // OK ou ADIAR
                    if (btnOK.Text == "ADIAR")
                        Mensagem.IDRespostaProcesamento = -2;
                    else if (btnOK.Text == "PAGAR" || btnOK.Text == "ABRIR")
                    {
                        try
                        {
                            var prm = (Mensagem.Parametros ?? "").Trim();
                            if (prm.StartsWith("http://") || prm.StartsWith("https://"))
                                System.Diagnostics.Process.Start(prm);
                            else if (prm.Contains("|"))
                            {
                                var prms = prm.Split('|');
                                System.Diagnostics.Process.Start(prms[0], prms[1]);
                            }
                        }
                        catch (Exception ex)
                        {
                            Mensagem.ResultadoProcessamento = ex.Message;
                        }
                    }
                }
                else if (DialogResult == DialogResult.Yes)
                    Mensagem.ResultadoProcessamento = "SIM";
                else if (DialogResult == DialogResult.No)
                    Mensagem.ResultadoProcessamento = "NÃO";
                else
                    Mensagem.ResultadoProcessamento = DialogResult.ToString();

                Mensagem.KeyDestino = BLL.AC.Usuario?.Login;

                EF.Repositorio.Atualizar(Mensagem);
                // O integrado irá enviar a mensagem de retorno ao destinatário correto
            }
        }

        private void tmrTimeOut_Tick(object sender, EventArgs e)
        {
            try
            {
                int id = Mensagem.IDMensagem;
                var msg = EF.Repositorio.Carregar<tbMensagem>(m => m.IDMensagem == id);
                if (msg.ResultadoProcessamento != "exibido")
                    Close(); // Fecha automaticamente se a mensagem for processada
            }
            catch (Exception)
            {
            }
        }
    }
}