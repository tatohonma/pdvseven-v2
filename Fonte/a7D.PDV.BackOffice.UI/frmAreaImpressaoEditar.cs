using a7D.PDV.BLL;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmAreaImpressaoEditar : Form
    {
        private AreaImpressaoInformation AreaImpressao1;

        public frmAreaImpressaoEditar()
        {
            InitializeComponent();
            AreaImpressao1 = new AreaImpressaoInformation();
        }

        public frmAreaImpressaoEditar(Int32 idAreaImpressao)
        {
            InitializeComponent();
            AreaImpressao1 = AreaImpressao.Carregar(idAreaImpressao);
        }

        private void frmAreaImpressaoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarListas();

            if (AreaImpressao1.IDAreaImpressao != null)
                CarregarCampos();
            else
                rbImpressora.Checked = true;

            rbImpressoraOuCaixa_CheckedChanged(null, null);
        }

        private void CarregarCampos()
        {
            txtNome.Text = AreaImpressao1.Nome;
            if (AreaImpressao1.PDV?.IDPDV == null)
            {
                rbImpressora.Checked = true;
                cbbImpressora.SelectedItem = AreaImpressao1.NomeImpressora;
            }
            else
            {
                rbCaixa.Checked = true;
                foreach (PDVNomeID item in cbbCaixa.Items)
                {
                    if (item.id == AreaImpressao1.PDV.IDPDV)
                    {
                        cbbCaixa.SelectedItem = item;
                        break;
                    }
                }
            }

            switch (AreaImpressao1.TipoAreaImpressao.IDTipoAreaImpressao.Value)
            {
                case 0:
                    rbConta.Checked = true;
                    break;
                case 1:
                    rbContaPadrao.Checked = true;
                    break;
                case 2:
                    rbProducao.Checked = true;
                    break;
                case 3:
                    rbSAT.Checked = true;
                    break;
            }

        }

        private void CarregarListas()
        {
            cbbImpressora.Items.Add("");
            foreach (string impressora in PrinterSettings.InstalledPrinters)
                cbbImpressora.Items.Add(impressora);

            var caixas = BLL.PDV.Listar().Where(p => p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            foreach (var caixa in caixas)
            {
                var pdv = new PDVNomeID(caixa);
                string tipo = ConfiguracaoBD.ValorOuPadrao(EConfig.TipoGerenciadorImpressao, ETipoPDV.CAIXA, caixa.IDPDV.Value);
                if (int.TryParse(tipo, out int idTipo))
                {
                    var tpGP = (ETipoGerenciadorImpressao)idTipo;
                    if (tpGP == ETipoGerenciadorImpressao.ACBr)
                        pdv.nome += " (ACBR)";
                    else if (tpGP == ETipoGerenciadorImpressao.SemImpressora)
                        pdv.nome += " (Sem Impressora)";
                    else
                    {
                        string nome = ConfiguracaoBD.ValorOuPadrao(EConfig.ModeloImpressora, ETipoPDV.CAIXA, caixa.IDPDV.Value);
                        if (string.IsNullOrEmpty(nome))
                            pdv.nome += " (Impressora não definida)";
                        else
                        {
                            pdv.nome += " - " + nome;
                            pdv.ativo = true;
                        }
                    }
                }
                else
                    pdv.nome += " (tipo desconhecido)";

                cbbCaixa.Items.Add(pdv);
            }
        }

        private void cbbCaixa_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                var pdv = cbbCaixa.Items[e.Index] as PDVNomeID;
                var rc = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                e.Graphics.DrawString(pdv.nome, this.Font, pdv.ativo ? Brushes.Black : Brushes.Gray, rc);
            }
            e.DrawFocusRectangle();
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            AreaImpressao1.Nome = txtNome.Text;
            AreaImpressao1.TipoAreaImpressao = new TipoAreaImpressaoInformation();

            if (rbImpressora.Checked)
            {
                AreaImpressao1.PDV = null;
                AreaImpressao1.NomeImpressora = cbbImpressora.SelectedItem.ToString();
            }
            else if (rbCaixa.Checked)
            {
                AreaImpressao1.PDV = new PDVInformation() { IDPDV = ((PDVNomeID)cbbCaixa.SelectedItem).id };
                AreaImpressao1.NomeImpressora = cbbCaixa.SelectedItem.ToString();
            }

            if (rbConta.Checked)
                AreaImpressao1.TipoAreaImpressao.TipoAreaImpressao = ETipoAreaImpressao.Conta;
            else if (rbContaPadrao.Checked)
                AreaImpressao1.TipoAreaImpressao.TipoAreaImpressao = ETipoAreaImpressao.ContaPadrao;
            else if (rbProducao.Checked)
                AreaImpressao1.TipoAreaImpressao.TipoAreaImpressao = ETipoAreaImpressao.Producao;
            else if (rbSAT.Checked)
                AreaImpressao1.TipoAreaImpressao.TipoAreaImpressao = ETipoAreaImpressao.SAT;

            AreaImpressao.Salvar(AreaImpressao1);

            this.Close();
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            //var qtdAreaSAT = AreaImpressao.QuantidadeAreaSAT(AreaImpressao1?.IDAreaImpressao);

            if (rbImpressora.Checked && cbbImpressora.SelectedIndex == -1)
                msg += "Selecione uma impressora \n";

            if (rbCaixa.Checked && cbbCaixa.SelectedIndex == -1)
                msg += "Selecione um Caixa \n";

            //if (qtdAreaSAT == 1 && rbSAT.Checked)
            //    msg += "Já existe uma área de impressão do SAT\nSomente uma área de impressão do SAT é permitida.";

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbSAT_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSAT.Checked)
            {
                txtNome.Text = "S@T";
                txtNome.Enabled = false;
            }
            else
            {
                txtNome.Text = string.Empty;
                txtNome.Enabled = true;
            }
        }

        private void rbImpressoraOuCaixa_CheckedChanged(object sender, EventArgs e)
        {
            cbbCaixa.Visible = rbCaixa.Checked;
            cbbImpressora.Visible = rbImpressora.Checked;
        }

        private void cbbCaixa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbCaixa.SelectedIndex < 0)
                return;
            var pdv = cbbCaixa.Items[cbbCaixa.SelectedIndex] as PDVNomeID;
            if (!pdv.ativo)
                cbbCaixa.SelectedIndex = -1;
        }
    }
}
