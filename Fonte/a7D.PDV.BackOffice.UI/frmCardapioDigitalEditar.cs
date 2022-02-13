using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmCardapioDigitalEditar : Form
    {
        private TemaCardapioPDVInformation TemaCardapioPDV1;

        public frmCardapioDigitalEditar()
        {
            InitializeComponent();

            TemaCardapioPDV1 = new TemaCardapioPDVInformation();
            TemaCardapioPDV1.DtUltimaAlteracao = DateTime.Now;
        }

        public frmCardapioDigitalEditar(Int32 idTemaCardapioPDV)
        {
            InitializeComponent();

            TemaCardapioPDV1 = TemaCardapioPDV.Carregar(idTemaCardapioPDV);
        }

        private void frmCardapioDigitalEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            List<PDVInformation> listaPDV = BLL.PDV.Listar()
                .Where(p => p.TipoPDV.Tipo == EF.Enum.ETipoPDV.CARDAPIO_DIGITAL
                         || p.TipoPDV.Tipo == EF.Enum.ETipoPDV.AUTOATENDIMENTO
                         || p.TipoPDV.Tipo == EF.Enum.ETipoPDV.TORNEIRA)
                         .ToList();

            listaPDV.Insert(0, new PDVInformation { Nome = "Padrão" });
            cbbPDV.DataSource = listaPDV;

            List<TemaCardapioInformation> listaTemaCardapio = TemaCardapio.Listar().OrderBy(l => l.Nome).ToList();
            listaTemaCardapio.Insert(0, new TemaCardapioInformation());
            cbbTemaCardapio.DataSource = listaTemaCardapio;

            if (TemaCardapioPDV1.PDV != null)
                cbbPDV.SelectedValue = TemaCardapioPDV1.PDV.IDPDV;

            if (TemaCardapioPDV1.TemaCardapio != null)
                cbbTemaCardapio.SelectedValue = TemaCardapioPDV1.TemaCardapio.IDTemaCardapio;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                TemaCardapioPDV1.Ativo = true;

                if (cbbPDV.SelectedIndex > 0)
                {
                    TemaCardapioPDV1.PDV = new PDVInformation();
                    TemaCardapioPDV1.PDV.IDPDV = Convert.ToInt32(cbbPDV.SelectedValue);
                }
                else
                    TemaCardapioPDV1.PDV = null;

                if (cbbTemaCardapio.SelectedIndex > 0)
                {
                    TemaCardapioPDV1.TemaCardapio = new TemaCardapioInformation();
                    TemaCardapioPDV1.TemaCardapio.IDTemaCardapio = Convert.ToInt32(cbbTemaCardapio.SelectedValue);
                }
                else
                    TemaCardapioPDV1.TemaCardapio = null;

                TemaCardapioPDV.Salvar(TemaCardapioPDV1);

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (cbbTemaCardapio.SelectedIndex < 1)
                msg += "Campo \"Tema\" é obrigatório. \n";

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
    }
}