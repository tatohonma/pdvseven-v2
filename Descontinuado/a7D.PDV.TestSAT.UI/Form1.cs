using a7D.PDV.SATComunicacao;
using System;
using System.Net.Http;
using System.Windows.Forms;

namespace a7D.PDV.TestSAT.UI
{
    public partial class Form1 : Form
    {
        private string server = "http://localhost:8888/";
        private string ativacao = "123456789";
        private int cod = new Random().Next(1, 999999);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkLocal.Checked)
                {
                    txtOut.Text = "ConsultaSAT DLL direto!\r\n";
                    Application.DoEvents();

                    txtOut.Text += ComunicacaoSat.ConsultarSat(cod);
                }
                else
                {
                    txtOut.Text = $"ConsultaSAT via IIS: {server}!\r\n";
                    Application.DoEvents();

                    var httpClient = new HttpClient();
                    var url = new Uri(server);
                    var result = httpClient.GetAsync(new Uri(url, "/api/Sat")).Result;
                    var text = result.Content.ReadAsStringAsync().Result;
                    txtOut.Text += text;
                }
            }
            catch (Exception ex)
            {
                txtOut.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkLocal.Checked)
                {
                    txtOut.Text = "ConsultaSAT DLL direto!\r\n";
                    Application.DoEvents();

                    txtOut.Text += ComunicacaoSat.ConsultarSessao(ativacao, cod);
                }
                else
                {
                    txtOut.Text = $"ConsultaSAT via IIS: {server}!\r\n";
                    Application.DoEvents();

                    var sat = new SAT._007.SatApiClient(server);
                    var result = sat.ConsultaClient(ativacao, cod);
                    txtOut.Text += result;
                }
            }
            catch (Exception ex)
            {
                txtOut.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkLocal.Checked)
                {
                    txtOut.Text = "Venda DLL direto!\r\n";
                    Application.DoEvents();

                    var result = ComunicacaoSat.EnviarVenda(ativacao, txtXML.Text, cod);
                    txtOut.Text += result;
                }
                else
                {
                    txtOut.Text = $"Venda via IIS: {server}!\r\n";
                    Application.DoEvents();

                    var sat = new SAT._007.SatApiClient(server);
                    var result = sat.Venda(txtXML.Text, cod);
                    txtOut.Text += result;
                }
            }
            catch (Exception ex)
            {
                txtOut.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }
    }
}
