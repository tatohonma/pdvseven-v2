using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmClassificacaoFiscal : Form
    {
        public List<ClassificacaoFiscalInformation> ListaClassificacaoFiscal { get; private set; }
        public List<ClassificacaoFiscalInformation> ListaClassificacaoFiscalFiltrada { get; private set; }

        public frmClassificacaoFiscal()
        {
            InitializeComponent();
        }

        private void frmClassificacaoFiscal_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
            AtualizarGrid();
            CarregarComboBox();
            txtNCM.Text = string.Empty;
            txtNomeFiltro.Text = string.Empty;
        }

        private void CarregarComboBox()
        {
            var listaTipoTributacao = BLL.TipoTributacao.Listar();
            listaTipoTributacao.Insert(0, new TipoTributacaoInformation { Nome = "Todos" });

            cbbTipoTributacao.DataSource = listaTipoTributacao;
            cbbTipoTributacao.ValueMember = "IDTipoTributacao";
            cbbTipoTributacao.DisplayMember = "Nome";
        }

        private void AtualizarGrid()
        {
            var lista = from l in ListaClassificacaoFiscalFiltrada
                        orderby l.NCM
                        select new { l.IDClassificacaoFiscal, TipoTributacao = l.TipoTributacao.Nome, l.Nome, l.NCM };
            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void CarregarLista()
        {
            ListaClassificacaoFiscal = ClassificacaoFiscal.ListarCompleto();
            ListaClassificacaoFiscalFiltrada = ListaClassificacaoFiscal;
        }

        private void FiltrarGrid()
        {
            ListaClassificacaoFiscalFiltrada = ListaClassificacaoFiscal;
            if (cbbTipoTributacao.SelectedIndex > 0)
            {
                ListaClassificacaoFiscalFiltrada = ListaClassificacaoFiscalFiltrada.Where(cf => cf.TipoTributacao.IDTipoTributacao == Convert.ToInt32(cbbTipoTributacao.SelectedValue)).ToList();
            }

            if (string.IsNullOrWhiteSpace(txtNomeFiltro.Text) == false)
            {
                ListaClassificacaoFiscalFiltrada = ListaClassificacaoFiscalFiltrada.Where(cf => cf.Nome.ToLowerInvariant().Contains(txtNomeFiltro.Text.ToLowerInvariant())).ToList();
            }

            if (string.IsNullOrEmpty(txtNCM.Text) == false)
            {
                ListaClassificacaoFiscalFiltrada = ListaClassificacaoFiscalFiltrada.Where(cf => cf.NCM.ToLowerInvariant().Contains(txtNCM.Text.ToLowerInvariant())).ToList();
            }
            AtualizarGrid();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                int idClassificacaoFiscal = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDClassificacaoFiscal"].Value);

                frmClassificacaoFiscalEditar frm = new frmClassificacaoFiscalEditar(idClassificacaoFiscal);
                frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
                FiltrarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void btInserir_Click(object sender, EventArgs e)
        {
            frmClassificacaoFiscalEditar frm = new frmClassificacaoFiscalEditar();
            frm.ShowDialog();

            CarregarLista();
            AtualizarGrid();
            FiltrarGrid();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                int idClassificacaoFiscal = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDClassificacaoFiscal"].Value);
                var classificacaoFiscal = ClassificacaoFiscal.Carregar(idClassificacaoFiscal);

                var resp = MessageBox.Show(string.Format("Deseja realmente excluir a Classificação Fiscal {0}?", classificacaoFiscal.Nome), "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resp == DialogResult.Yes)
                {
                    ClassificacaoFiscal.Excluir(idClassificacaoFiscal);
                    CarregarLista();
                    AtualizarGrid();
                    FiltrarGrid();
                }
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja excluir!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbbTipoTributacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarGrid();
        }

        private void txtNomeFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            FiltrarGrid();
        }

        private void txtNCM_KeyUp(object sender, KeyEventArgs e)
        {
            FiltrarGrid();
        }
    }
}
