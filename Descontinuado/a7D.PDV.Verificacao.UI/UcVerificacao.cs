using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Verificacao.UI
{
    public partial class UcVerificacao : UserControl
    {
        private IVerificacao Verificacao { get; }

        public UcVerificacao()
        {
            InitializeComponent();
        }
        public UcVerificacao(IVerificacao verificacao) : this()
        {
            Verificacao = verificacao;
        }

        private void UcVerificacao_Load(object sender, EventArgs e)
        {
            lblNome.Text = Verificacao.Nome;
            if (!Verificacao.Invalido)
            {
                pbIcon.BackgroundImage = iconList.Images[0];
                lblMensagem.Text = "OK";
            }
            else
            {
                lblMensagem.Text = Verificacao.Mensagem;
                pbIcon.BackgroundImage = iconList.Images[(int)Verificacao.Nivel];
                blinkTimer.Enabled = Verificacao.Nivel == Nivel.ERRO;
            }
        }

        private void blinkTimer_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                pbIcon.Visible = !pbIcon.Visible;
            }));
        }
    }
}
