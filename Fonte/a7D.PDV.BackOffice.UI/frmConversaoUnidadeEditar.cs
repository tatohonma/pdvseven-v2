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
using System.Globalization;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmConversaoUnidadeEditar : Form
    {
        private readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private List<UnidadeInformation> listaUnidadede;
        private List<UnidadeInformation> listaUnidadepara;

        private ConversaoUnidadeInformation ConversaoUnidade1 { get; set; }

        public frmConversaoUnidadeEditar()
        {
            ConversaoUnidade1 = new ConversaoUnidadeInformation();
            InitializeComponent();
        }

        public frmConversaoUnidadeEditar(int idConversaoUnidade)
        {
            ConversaoUnidade1 = ConversaoUnidade.Carregar(idConversaoUnidade);
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmConversaoUnidadeEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            listaUnidadede = Unidade.Listar().Where(u => u.Excluido == false).ToList();
            listaUnidadede.Insert(0, new UnidadeInformation { Nome = "Selecione...", IDUnidade = -1 });

            listaUnidadepara = Unidade.Listar().Where(u => u.Excluido == false).ToList();
            listaUnidadepara.Insert(0, new UnidadeInformation { Nome = "Selecione...", IDUnidade = -1 });

            cbbUnidadeDe.DisplayMember = "Nome";
            cbbUnidadePara.DisplayMember = "Nome";

            cbbUnidadeDe.ValueMember = "IDUnidade";
            cbbUnidadePara.ValueMember = "IDUnidade";

            cbbUnidadeDe.DataSource = listaUnidadede;
            cbbUnidadePara.DataSource = listaUnidadepara;

            if (ConversaoUnidade1.IDConversaoUnidade.HasValue)
            {
                cbbUnidadeDe.SelectedValue = ConversaoUnidade1.Unidade_de.IDUnidade;
                cbbUnidadePara.SelectedValue = ConversaoUnidade1.Unidade_para.IDUnidade;

                txtDivisao.Text = ConversaoUnidade1.Divisao.Value.ToString();
                txtMultiplicacao.Text = ConversaoUnidade1.Multiplicacao.Value.ToString();
            }
            else
                LimparCampos();
            txtDivisao.TextChanged += txtDivisao_TextChanged;
            txtMultiplicacao.TextChanged += txtMultiplicacao_TextChanged;
        }

        private void LimparCampos()
        {
            cbbUnidadeDe.SelectedIndex = 0;
            cbbUnidadeDe.SelectedIndex = 0;

            txtDivisao.Text = string.Empty;
            txtMultiplicacao.Text = string.Empty;
        }

        private void txtDivisao_TextChanged(object sender, EventArgs e)
        {
            var divisao = decimal.Parse((string.IsNullOrWhiteSpace(txtDivisao.Text) ? "0" : txtDivisao.Text), _cultureInfo);
            txtMultiplicacao.TextChanged -= txtMultiplicacao_TextChanged;
            if (divisao > 0)
                txtMultiplicacao.Text = Math.Round((1 / divisao), 6, MidpointRounding.ToEven).ToString();
            else
                txtMultiplicacao.Text = "0";
            txtMultiplicacao.TextChanged += txtMultiplicacao_TextChanged;
        }

        private void txtMultiplicacao_TextChanged(object sender, EventArgs e)
        {
            var multiplicacao = decimal.Parse((string.IsNullOrWhiteSpace(txtMultiplicacao.Text) ? "0" : txtMultiplicacao.Text), _cultureInfo);
            txtDivisao.TextChanged -= txtDivisao_TextChanged;
            if (multiplicacao > 0)
                txtDivisao.Text = Math.Round((1 / multiplicacao), 6, MidpointRounding.ToEven).ToString();
            else
                txtDivisao.Text = "0";
            txtDivisao.TextChanged += txtDivisao_TextChanged;
        }

        private void txtDivisao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    ConversaoUnidade1.Unidade_de = (UnidadeInformation)cbbUnidadeDe.SelectedItem;
                    ConversaoUnidade1.Unidade_para = (UnidadeInformation)cbbUnidadePara.SelectedItem;
                    ConversaoUnidade1.Divisao = decimal.Parse(txtDivisao.Text, _cultureInfo);
                    ConversaoUnidade1.Multiplicacao = decimal.Parse(txtMultiplicacao.Text, _cultureInfo);

                    ConversaoUnidade.Salvar(ConversaoUnidade1);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool Validar()
        {
            var msg = new StringBuilder();

            if (cbbUnidadeDe.SelectedIndex == 0)
                msg.AppendLine("Selecione a Unidade de origem");
            if (cbbUnidadePara.SelectedIndex == 0)
                msg.AppendLine("Selecione a Unidade de destino");

            decimal divisao, multiplicacao;

            if (decimal.TryParse(txtDivisao.Text, NumberStyles.Float, _cultureInfo, out divisao))
            {
                if (divisao <= 0)
                    msg.AppendLine("Fator de divisão deve ser positivo e diferente de 0");
            }
            else
                msg.AppendLine("Fator de divisão inválido");

            if (decimal.TryParse(txtDivisao.Text, NumberStyles.Float, _cultureInfo, out multiplicacao))
            {
                if (multiplicacao <= 0)
                    msg.AppendLine("Fator de multiplicação deve ser positivo e diferente de 0");
            }
            else
                msg.AppendLine("Fator de multiplicação inválido");

            return string.IsNullOrWhiteSpace(msg.ToString());
        }
    }
}
