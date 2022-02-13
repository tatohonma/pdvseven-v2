using a7D.PDV.Balanca;
using a7D.PDV.Balanca.Interfaces;
using System;
using System.Windows.Forms;

namespace a7D.PDV.LocalizadorBalanca.UI
{
    public partial class frmLocalizarBalanca : Form
    {
        public frmLocalizarBalanca()
        {
            InitializeComponent();
        }

        private async void btnLocalizar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            UseWaitCursor = true;
            btnLocalizar.Enabled = false;
            cbbProtocolo.Enabled = false;
            lblStatus.Text = "...";
            Refresh();
            decimal? peso = null;
            var ports = System.IO.Ports.SerialPort.GetPortNames();
            var portaBalanca = "Nenhuma";
            foreach (var port in ports)
            {
                lblStatus.Text = $"Verificando {port}...";
                Refresh();
                try
                {
                    var balanca = ObterBalanca(port);
                    int i = 0;
                    Dados resultado;
                    do
                    {
                        resultado = await balanca.LerPesoAsync();
                        if (resultado.Status == Status.OK)
                        {
                            portaBalanca = port;
                            peso = resultado.Peso;
                        }
                    } while (resultado.Status != Status.OK && i++ < 3);
                }
                catch
                {
                    continue;
                }
            }
            btnLocalizar.Enabled = true;
            cbbProtocolo.Enabled = true;
            UseWaitCursor = false;
            Cursor = Cursors.Default;
            lblStatus.Text = portaBalanca;
            if (peso.HasValue)
                lblStatus.Text += $"\r\n{peso} Kg";
        }


        private IBalanca ObterBalanca(string port)
        {
            switch (cbbProtocolo.SelectedIndex)
            {
                case 0:
                    return BalancaFactory.ObterBalanca(ETipoBalanca.TOLEDO, port);
                case 1:
                    return BalancaFactory.ObterBalanca(ETipoBalanca.FILIZOLA, port);
                default:
                    return null;
            }
        }

        private void frmLocalizarBalanca_Load(object sender, EventArgs e)
        {
            cbbProtocolo.SelectedIndex = 0;
        }
    }
}
