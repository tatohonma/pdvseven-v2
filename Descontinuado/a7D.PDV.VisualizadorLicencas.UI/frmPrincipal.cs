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

namespace a7D.PDV.VisualizadorLicencas.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            var licencas = txtLicencas.Lines;
            var pdvs = new List<PDVInformation>();
            foreach (var lic in licencas)
            {
                if (string.IsNullOrWhiteSpace(lic))
                    continue;

                var licenca = lic.Split('\t');

                if (licenca.Length != 3)
                    continue;

                pdvs.Add(new PDVInformation
                {
                    IDPDV = Convert.ToInt32(licenca[0]),
                    Nome = licenca[1],
                    Dados = licenca[2]
                }); // CarregarDados()
            }

            new frmLicencas(pdvs).ShowDialog();
        }
    }
}
