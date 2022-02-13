using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmForcarFechamento : FormTouch
    {
        private List<CaixaInformation> CaixasAbertos { get; set; }

        public frmForcarFechamento()
        {
            InitializeComponent();
        }

        public frmForcarFechamento(List<CaixaInformation> caixasAbertos) : this()
        {
            CaixasAbertos = caixasAbertos;
        }

        private void frmForcarFechamento_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            ListarOuFechar();
        }

        private void ListarOuFechar()
        {
            if (CaixasAbertos.Count == 0)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            dgvPrincipal.DataSource = CaixasAbertos.Select(c => new
            {
                c.IDCaixa,
                c.PDV.Nome,
                Abertura = c.DtAbertura?.ToString("dd/MM/yyyy HH:mm:ss"),
                Usuario = c.Usuario?.Nome
            }).ToArray();
        }

        private void VerificarNovamente()
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            Refresh();
            CaixasAbertos = BLL.Caixa.ListarAbertos().ToList();
            ListarOuFechar();
            Enabled = true;
            Cursor = Cursors.Default;
            Refresh();
        }

        private void btnVerificarNovamente_Click(object sender, EventArgs e)
        {
            VerificarNovamente();
        }

        private void btnForcarFechamento_Click(object sender, EventArgs e)
        {
            foreach (var caixa in CaixasAbertos)
            {
                BLL.Caixa.ForcarFechamento(caixa);
                frmPrincipal.Impressora1.RelatorioGerencial(Relatorio.FechamentoCaixa(caixa.IDCaixa.Value, false), 7);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmForcarFechamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
                DialogResult = DialogResult.Cancel;
        }
    }
}
