using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTipoMovimentacaoEditar : Form
    {
        private TipoMovimentacaoInformation TipoMovimentacao1 { get; set; }

        public frmTipoMovimentacaoEditar()
        {
            TipoMovimentacao1 = new TipoMovimentacaoInformation();
            InitializeComponent();
        }

        public frmTipoMovimentacaoEditar(int idTipoMovimentacao)
        {
            TipoMovimentacao1 = TipoMovimentacao.Carregar(idTipoMovimentacao);
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    TipoMovimentacao1.Nome = txtNome.Text;
                    TipoMovimentacao1.Descricao = txtDescricao.Text;
                    TipoMovimentacao1.Tipo = rbEntrada.Checked ? "+" : "-";
                    TipoMovimentacao.Salvar(TipoMovimentacao1);

                    Close();
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private bool Validar()
        {
            var msg = new StringBuilder();

            if (!rbEntrada.Checked && !rbSaida.Checked)
                msg.AppendLine("Selecione o tipo de movimentação");

            if (string.IsNullOrWhiteSpace(txtNome.Text))
                msg.AppendLine("Nome não pode ser vazio!");

            return string.IsNullOrWhiteSpace(msg.ToString());
        }

        private void frmTipoMovimentacaoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            if (TipoMovimentacao1.IDTipoMovimentacao.HasValue)
            {
                txtNome.Text = TipoMovimentacao1.Nome;
                txtDescricao.Text = TipoMovimentacao1.Descricao;
                if (TipoMovimentacao1.Tipo == "+")
                    rbEntrada.Checked = true;
                else
                    rbSaida.Checked = true;
            }
            else
                LimparFormulario();
        }

        private void LimparFormulario()
        {
            txtNome.Text = string.Empty;
            txtDescricao.Text = string.Empty;

            rbEntrada.Checked = false;
            rbSaida.Checked = false;
        }
    }
}
