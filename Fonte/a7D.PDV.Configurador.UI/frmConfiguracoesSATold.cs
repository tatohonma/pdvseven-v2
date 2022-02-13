using a7D.PDV.BLL;
using a7D.PDV.Componentes.Controles;
using a7D.PDV.Model;
using a7D.PDV.SATComunicacao;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmConfiguracoesSATold : Form
    {
        private ConfiguracoesSAT configuracoesSat;

        private List<ConfiguracaoBDInformation> ConfiguracoesSAT { get; set; }

        public frmConfiguracoesSATold()
        {
            InitializeComponent();

            List<AtualizarDLLs> lista = new List<AtualizarDLLs> {
                new Elgin(),
                new Dimep(),
                new TancaSDK()
            };

            cbbMarcaSAT.DataSource = lista;
            cbbMarcaSAT.DisplayMember = "Nome";
        }

        private void frmConfiguracoesSAT_Load(object sender, EventArgs e)
        {
            configuracoesSat = new ConfiguracoesSAT();
            ConfiguracoesSAT = ConfiguracaoBD.ListarConfiguracoes()
                .Where(c => c.Chave.StartsWith("infCFe_", StringComparison.InvariantCultureIgnoreCase))
                .Where(c => c.Chave != "infCFe_ide_signAC")
                .Where(c => !c.Chave.EndsWith("_bkp"))
                .ToList();

            tableSat.SuspendLayout();
            tableSat.Controls.Clear();
            tableSat.RowCount = 1;

            tableSat.RowStyles[0] = new RowStyle(SizeType.Absolute, 1f);

            foreach (var config in ConfiguracoesSAT)
            {
                tableSat.RowCount = tableSat.RowCount + 1;
                tableSat.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var ucc = new UCConfiguracao(config);
                tableSat.Controls.Add(ucc, 0, tableSat.RowCount - 1);
                ucc.Dock = DockStyle.Fill;
            }

            tableSat.ResumeLayout();

            txtinfCFe_ide_signAC.Text = configuracoesSat.InfCFe_ide_signAC;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            var error = string.Empty;

            if (txtinfCFe_ide_signAC.Text.Length != 344)
            {
                if (txtinfCFe_ide_signAC.Text == "SGR-SAT SISTEMA DE GESTAO E RETAGUARDA DO SAT")
                    MessageBox.Show("Assinatura inserida é a de desenvolvimento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    error += "Assinatura inválida\n";
            }



            if (string.IsNullOrWhiteSpace(error) == false)
            {
                MessageBox.Show(error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConfiguracaoBD.AlterarConfiguracaoSistema("infCFe_ide_signAC", txtinfCFe_ide_signAC.Text);
            configuracoesSat = new ConfiguracoesSAT();
            MessageBox.Show("Assinatura salva com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnConsultarSAT_Click(object sender, EventArgs e)
        {
            btnAssociar.Enabled = false;
            cbbMarcaSAT.Enabled = false;
            string[] resposta = new string[1];
            try
            {
                int numeroSessao = new Random().Next(1, 999999);
                resposta = ComunicacaoSat.ConsultarSat(numeroSessao).Split('|');
                var codigo = resposta[1];
                MessageBox.Show(resposta[2], codigo, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (codigo == "08000")
                {
                    HabilitarBotoesSat();
                }
                else
                {
                    HabilitarBotoesSat(false);
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(resposta[0], "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAssociar_Click(object sender, EventArgs e)
        {
            var sat = (AtualizarDLLs)cbbMarcaSAT.SelectedItem;

            try
            {
                if (sat != null)
                {
                    var fbd = new FolderBrowserDialog();
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                    var frm = new frmCaixaOuServer();
                    frm.ShowDialog();
                    var instalacao = frm.Instalacao;
                    switch (instalacao)
                    {
                        case -1:
                            return;
                        case 0:
                            fbd.Description = "Selecione o diretório de instalação do Caixa";
                            break;
                        case 1:
                            fbd.Description = "Selecione o diretório de instalação do www_sat";
                            break;

                    }

                    var resp = fbd.ShowDialog();

                    if (resp == DialogResult.OK)
                    {
                        var caminho = fbd.SelectedPath;
                        switch (instalacao)
                        {
                            case 0:
                                if (File.Exists(Path.Combine(caminho, "a7D.PDV.Caixa.UI.exe")) == false)
                                {
                                    MessageBox.Show("Arquivo executável do Caixa não encontrado no caminho especificado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                if (sat.Atualizar(caminho, true))
                                    HabilitarBotoesSat();
                                break;
                            case 1:
                                var caminhoBin = Path.Combine(caminho, "bin", "System.Web.Http.dll");
                                var www_sat = File.Exists(caminhoBin);
                                if (!www_sat && !File.Exists(Path.Combine(caminho, "System.Web.Http.dll")))
                                {
                                    MessageBox.Show("Diretório do www_sat inválido");
                                    return;
                                }
                                if (www_sat)
                                    caminho = caminhoBin;

                                sat.Atualizar(caminho);
                                break;
                        }
                    }
                    else
                        sat.Atualizar();
                    cbbMarcaSAT.Enabled = false;
                    btnAssociar.Enabled = false;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("As DLLs estão sendo utilizadas por outro processo.\nReinicie o computador e tente novamente", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTesteFimAFim_Click(object sender, EventArgs e)
        {
            btnAssociar.Enabled = false;
            cbbMarcaSAT.Enabled = false;
            int numeroSessao = new Random().Next(1, 999999);
            //var resposta = ComunicacaoSat.TesteFimAFimSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao).Split('|');
            var resposta = SATServer.TesteFimAFim();
            //string msg = "";
            //int count = 0;

            //foreach (var item in resposta)
            //{
            //    if (count == 3)
            //        break;
            //    msg += item + Environment.NewLine;
            //    count++;
            //}
            var msg = $"Resposta: {resposta.Codigo} {Environment.NewLine} {resposta.Mensagem}";

            MessageBox.Show( msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAtivarSAT_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("O programa tentará ativar o SAT com os dados informados na aba de \"Configurações do Programa\"\nProsseguir?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                try
                {
                    btnAssociar.Enabled = false;
                    DefinirComoEspera();
                    HabilitarBotoesSat(false);
                    cbbMarcaSAT.Enabled = false;
                    int numeroSessao = new Random().Next(1, 999999);
                    var resposta = ComunicacaoSat.AtivarSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao, configuracoesSat.InfCFe_emit_CNPJ, Convert.ToInt32(cbbMarcaSAT.SelectedValue)).Split('|');
                    MessageBox.Show(resposta[2], resposta[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    RemoverEspera();
                    HabilitarBotoesSat();
                }
            }
        }

        private void btnAssociarAssinatura_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("O programa tentará ativar o SAT com os dados informados na aba de \"Configurações do Programa\"\nProsseguir?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                btnAssociar.Enabled = false;
                int numeroSessao = new Random().Next(1, 999999);
                var resposta = ComunicacaoSat.AssociarAssinaturaSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao, string.Format("{0}{1}", configuracoesSat.InfCFe_ide_CNPJ, configuracoesSat.InfCFe_emit_CNPJ), txtinfCFe_ide_signAC.Text).Split('|');
                MessageBox.Show(resposta[2], resposta[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExtrairLogs_Click(object sender, EventArgs e)
        {
            btnAssociar.Enabled = false;
            cbbMarcaSAT.Enabled = false;
            int numeroSessao = new Random().Next(1, 999999);
            var resposta = ComunicacaoSat.ExtrairLogsSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao).Split('|');
            var codigo = resposta[1];
            if (codigo == "15000")
            {
                var bytea = Convert.FromBase64String(resposta[5]);
                var log = Encoding.UTF8.GetString(bytea);
                new frmLogSAT(log).ShowDialog();
            }
            else
            {
                MessageBox.Show(resposta[2], resposta[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DefinirComoEspera()
        {
            tabControl1.Enabled = false;
            cbbMarcaSAT.Enabled = false;
            btnAssociar.Enabled = false;
            UseWaitCursor = true;

            btnConfigurar.Enabled = false;
        }

        public void RemoverEspera()
        {
            tabControl1.Enabled = true;
            cbbMarcaSAT.Enabled = true;
            btnAssociar.Enabled = true;
            UseWaitCursor = false;

            btnConfigurar.Enabled = true;
        }

        public void HabilitarBotoesSat(bool ativado = true)
        {
            btnConsultarSAT.Enabled =
                btnAtivarSAT.Enabled =
                btnAssociarAssinatura.Enabled =
                btnTesteFimAFim.Enabled =
                btnExtrairLogs.Enabled =
                btnDesbloquear.Enabled =
                btnConsultarStatus.Enabled =
                ativado;

            btnConsultarSAT.UseWaitCursor =
                btnAtivarSAT.UseWaitCursor =
                btnAssociarAssinatura.UseWaitCursor =
                btnTesteFimAFim.UseWaitCursor =
                btnExtrairLogs.UseWaitCursor =
                btnDesbloquear.UseWaitCursor =
                btnConsultarStatus.UseWaitCursor =
                !ativado;
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("O programa tentará ativar o SAT com os dados informados na aba de \"Configurações do Programa\"\nProsseguir?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                btnAssociar.Enabled = false;
                cbbMarcaSAT.Enabled = false;
                int numeroSessao = new Random().Next(1, 999999);
                var resposta = ComunicacaoSat.DesbloquearSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao).Split('|');
                MessageBox.Show(resposta[2], resposta[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnConsultarStatus_Click(object sender, EventArgs e)
        {
            btnAssociar.Enabled = false;
            cbbMarcaSAT.Enabled = false;
            int numeroSessao = new Random().Next(1, 999999);
            var resposta = ComunicacaoSat.ConsultarStatusOperacionalSat(numeroSessao, configuracoesSat.InfCFe_codigoAtivacao).Split('|');

            var codigo = resposta[1];
            if (codigo == "10000")
            {
                var respostaLista = resposta.ToList();
                respostaLista.RemoveRange(0, 5);
                resposta = respostaLista.ToArray();
                new frmConsultarStatusOperacional(resposta).ShowDialog();
            }
            else
                MessageBox.Show(resposta[2], resposta[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabConfiguracoesSAT)
            {
                configuracoesSat = new ConfiguracoesSAT();
            }
        }
    }

    abstract class AtualizarDLLs
    {
        internal Dictionary<string, byte[]> DLLs;

        public string Nome { get; private set; }

        public AtualizarDLLs(string nome)
        {
            Nome = nome;
        }

        internal bool Atualizar(string caminho = null, bool testar = false)
        {
            foreach (var dll in DLLs)
            {
                if (string.IsNullOrWhiteSpace(caminho) == false)
                    File.WriteAllBytes(Path.Combine(caminho, dll.Key), dll.Value);
                File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), dll.Key), dll.Value);
            }

            if (testar)
            {
                int numeroSessao = new Random().Next(1, 999999);
                var resposta = ComunicacaoSat.ConsultarSat(numeroSessao).Split('|');
                var codigo = resposta[1];
                MessageBox.Show(resposta[2], codigo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return codigo == "08000";
            }

            return true;
        }
    }

    class Elgin : AtualizarDLLs
    {
        internal Elgin() : base("Elgin")
        {
            DLLs = new Dictionary<string, byte[]>
            {
                { "sat.dll", SATDLL.elginsat },
                { "zlib.dll", SATDLL.elginzlib },
                { "zlib1.dll", SATDLL.elginzlib1 }
            };
        }
    }

    class Dimep : AtualizarDLLs
    {
        internal Dimep() : base("Dimep")
        {
            DLLs = new Dictionary<string, byte[]>
            {
                { "sat.dll", SATDLL.dimepsat },
                { "zlib.dll", SATDLL.dimepzlib },
                { "zlib1.dll", SATDLL.dimepzlib1 }
            };
        }
    }

    class TancaSDK : AtualizarDLLs
    {
        internal TancaSDK() : base("Tanca SDK")
        {
            DLLs = new Dictionary<string, byte[]>
            {
                { "sat.dll", SATDLL.tancasdk }
            };
        }
    }
}
