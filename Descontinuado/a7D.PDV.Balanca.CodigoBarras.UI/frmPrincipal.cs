using a7D.PDV.Balanca.CodigoBarras.UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Balanca.CodigoBarras.UI
{
    public partial class frmPrincipal : Form
    {

        private string Tipo { get; set; } = "_peso";
        private string Modelo { get; set; } = "ET901";

        private string Etiqueta
        {
            get
            {
                return $"{Modelo}{Tipo}";
            }
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void cbbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Modelo = cbbTipo.SelectedItem as string;
            CarregarImagem();
        }

        private void CarregarImagem()
        {
            pbCodigoBarras.Image?.Dispose();
            pbCodigoBarras.Image = Resources.ResourceManager.GetObject(Etiqueta) as Image;
            pbCodigoBarras.Location = new Point((pbCodigoBarras.Parent.ClientSize.Width / 2) - (pbCodigoBarras.Image.Width / 2), pbCodigoBarras.Location.Y);
            pbCodigoBarras.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbbTipo.SelectedIndex = 0;
        }

        private void rbPeso_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeso.Checked)
            {
                Tipo = "_peso";
                CarregarImagem();
            }
        }

        private void rbQuantidade_CheckedChanged(object sender, EventArgs e)
        {
            if (rbQuantidade.Checked)
            {
                Tipo = "_unidade";
                CarregarImagem();
            }

        }
    }
}
