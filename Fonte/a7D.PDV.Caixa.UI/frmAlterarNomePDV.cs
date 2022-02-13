using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAlterarNomePDV : FormTouch
    {
        public frmAlterarNomePDV()
        {
            InitializeComponent();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Int32 idPDV = BLL.AC.PDV.IDPDV.Value;
            String nome = txtNome.Text;

            if (String.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Favor informar o nome do PDV!");
            }
            else
            {
                BLL.PDV.AlterarNome(idPDV, nome);
                BLL.AC.PDV.Nome = nome;

                MessageBox.Show("Nome alterado com sucesso!");

                this.Close();
            }
        }

        private void frmAlterarNomePDV_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = BLL.AC.PDV.Nome;
        }
    }
}
