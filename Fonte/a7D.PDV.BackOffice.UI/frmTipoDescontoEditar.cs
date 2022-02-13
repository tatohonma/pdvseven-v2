using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTipoDescontoEditar : Form
    {
        private TipoDescontoInformation TipoDesconto1;

        public frmTipoDescontoEditar(int idTipoDesconto)
        {
            TipoDesconto1 = TipoDesconto.Carregar(idTipoDesconto);
            InitializeComponent();
        }

        public frmTipoDescontoEditar()
        {
            TipoDesconto1 = new TipoDescontoInformation { Ativo = true };
            InitializeComponent();
        }

        private void frmTipoDescontoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = TipoDesconto1.Nome;
            txtDescricao.Text = TipoDesconto1.Descricao;
            ckbAtivo.Checked = (TipoDesconto1.Ativo == true);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNome.Text))
                mensagem += "Por favor preencha o nome";

            if (string.IsNullOrWhiteSpace(mensagem) == false)
            {
                MessageBox.Show(mensagem, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TipoDesconto1.Nome = txtNome.Text;
            TipoDesconto1.Descricao = txtDescricao.Text;
            TipoDesconto1.Ativo = ckbAtivo.Checked;

            TipoDesconto.Salvar(TipoDesconto1);
            Close();
        }
    }
}
