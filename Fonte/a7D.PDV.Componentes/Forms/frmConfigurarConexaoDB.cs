using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.Integracao.API2.Client;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmConfigurarConexaoDB : Form
    {
        private bool DetectIP = false;

        public static string NovoIP = null;

        public static bool Verifica(bool detectIP = false, bool validaVersao = true)
        {
            if (ConfigurationManager.ConnectionStrings["connectionString"] == null
            || string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                using (var frm = new frmConfigurarConexaoDB())
                {
                    frm.DetectIP = detectIP;
                    frm.ShowDialog();
                    Application.Exit();
                    return false;
                }
            }
            else
            {
                Configuracao.Init();
                if (validaVersao && !ValidacaoSistema.VerificarConexaoEVersaoDB())
                {
                    Application.Exit();
                    return false;
                }
                else
                    return true;
            }
        }

        public frmConfigurarConexaoDB()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string servidor = txtServidor.Text;
            var comum = servidor.Comum();
            string name = "connectionString";
            string providerName = "System.Data.SqlClient";
            string bancoDeDados = string.IsNullOrWhiteSpace(txtBD.Text) ? "PDV7" : txtBD.Text;

            string connectionString = "Data Source=" + servidor;

            if (comum)
                connectionString += "\\PDV7;Initial Catalog=" + bancoDeDados + ";Persist Security Info=false;User ID=pdv7;Password=pdv@77";
            else
                connectionString += ";Initial Catalog=" + bancoDeDados + ";Persist Security Info=false;User ID=pdv7;Password=pdv@77";

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                conn.Close();

                Configuracao.AlterarConnectionStrings(name, connectionString, providerName);

                MessageBox.Show("Atualizado com sucesso! O programa será finalizado. Favor iniciá-lo novamente.");

                NovoIP = servidor;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível conectar no Servidor. \nVerifique o IP e se o servidor está ligado!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmConfigurarConexaoDB_Load(object sender, EventArgs e)
        {
            if (DetectIP)
            {
                var ips = NetworUtil.GetAllIP(out string log);
                if (ips.Length == 1)
                    txtServidor.Text = ips[0];
                else if (ips.Length > 1)
                {
                    string mensagem = string.Join("\r\n", ips);
                    mensagem = "Foram encontrados mais de um IP válidos\r\nUse o IP correto que será distribuido via UDP\r\n\r\n" + mensagem;
                    MessageBox.Show(mensagem, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    txtServidor.Text = ips.FirstOrDefault(ip => ip.Contains(".77.")) ?? "";
                }
                else
                    MessageBox.Show("Não há IPs validos para acesso externo", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var server = a7D.PDV.Integracao.API2.Client.UDPClient.Send("SQL");
                if (server != null)
                    txtServidor.Text = server;
            }
        }
    }

    static class LugaresComunsExtension
    {
        public static bool Comum(this string s)
        {
            var comum = false;
            switch (s.Trim().ToLowerInvariant())
            {
                case "localhost":
                    comum = true;
                    break;
                case ".":
                    comum = true;
                    break;
                default:
                    break;
            }

            if (Regex.IsMatch(s, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                comum = true;
            return comum;
        }
    }
}
