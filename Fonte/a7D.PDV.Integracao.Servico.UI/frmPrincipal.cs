using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.BLL.Services;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Models;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Integracao.DeliveryOnline;
using a7D.PDV.Integracao.iFood;
using a7D.PDV.Integracao.NTKServer;
using a7D.PDV.Integracao.Server;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Integracao.Servico.Core.Impressao;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.UI
{
    public partial class frmPrincipal : Form
    {
        List<IntegracaoItem> integracoes = null;
        int count = 0;
        int servico = 0;
        bool startLoop = true;
        bool sqlServerRun = false;

        private void InicializarIntegracoes()
        {
            integracoes = new List<IntegracaoItem>()
            {
                IntegracaoItem.From<IntegraServer>(txtStatusServer, pbServer, AddLogException),
                IntegracaoItem.From<IntegraOrdemProducao>(txtStatusImpressao, pbImpressao, AddLogException),
                IntegracaoItem.From<IntegraNTK>(txtStatusPOS, pbPagamentoIntegrado, AddLogException),
                IntegracaoItem.From<IntegraIFood>(txtIFood, pbIFood, AddLogException),
                IntegracaoItem.From<IntegraDeliveryOnline>(txtStatusDeliveryOnline, pbDeliveryOnline, AddLogException),
                //IntegracaoItem.From<IntegraERPCake>(txtStatusERP, pbERP, AddLogException),
                //IntegracaoItem.From<IntegraEasyChopp>(txtStatusEasyChopp, pbEasyChopp, AddLogException)
            };
        }

        public static string AddLogException(Exception exOriginal, bool saveLog)
        {
            ExceptionPDV exPDV;
            if (exOriginal is ExceptionPDV ex2)
                exPDV = ex2;
            else
                exPDV = new ExceptionPDV(CodigoErro.EEE1, exOriginal);

            if (exPDV.CodigoErro.ToString().StartsWith("A"))
                return exPDV.Message + "!";

            if (saveLog)
                Logs.Erro(exOriginal);

            string info = "ERRO";
            Exception ex = exOriginal;
            while (ex != null)
            {
                info += "\r\n\t" + ex.Message;

                foreach (var key in ex.Data.Keys)
                    info += $"\r\n\t{key}: {ex.Data[key]}";

                ex = ex.InnerException;
            }

            info += "\r\n" + exOriginal.StackTrace;
            return info;
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal2_Load(object sender, EventArgs e)
        {
            lvStatus.Clear();

            //tabServicos.TabPages.Remove(tabEasyChopp);
            //txtStatusServer.Text = "Inicializando...";

            //Tem que estar pelo menos configurado!
            if (!frmConfigurarConexaoDB.Verifica(true, false))
            {
                if (!string.IsNullOrEmpty(frmConfigurarConexaoDB.NovoIP))
                {
                    Configuracao.AlterarAppSettings("SQLServer", frmConfigurarConexaoDB.NovoIP);
                    Configuracao.AlterarAppSettings("WS2Server", frmConfigurarConexaoDB.NovoIP == "." ? "." : frmConfigurarConexaoDB.NovoIP + ":7777");
                }
                return;
            }

            tmr.Start();
        }

        private void frmPrincipal2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (integracoes != null)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ntf_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
                Hide();
            else
                Show();
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                stCount.Text = $"{count}:{servico}";
                if (servico == 0)
                {
                    if (VerificaIntegrador())
                    {
                        stStatus.Text = "OK";
                        //Ciclo de prioridade
                        if (startLoop)
                            count = 1;
                        else if (++count > 10 || count < 0)
                            count = 0;
                    }
                    else
                        count = -1; // Desativa incremento no finally
                }
                else if (integracoes != null)
                {
                    var integracao = integracoes[servico - 1]; // Base Zero!
                    if (!integracao.Executando)
                    {
                        if (integracao.ValidarOuExecutar(count, startLoop))
                            stStatus.Text = integracao.Nome + " iniciado";
                    }
                    else
                        stStatus.Text = integracao.Nome + " em execução";

                    // Verifica se qualquer instancia enviou umpedido de finalização 
                    for (int s = 0; s < integracoes.Count; s++)
                        if (integracoes[s].RequestClose)
                            Finalizar();

                    try
                    {
                        if (integracao.Instance is IntegraServer)
                            IntegraServer.ProcessaMensagens();
                        else if (integracao.Instance is IntegraOrdemProducao)
                            IntegraOrdemProducao.Update(lvStatus);
                    }
                    catch (Exception ex)
                    {
                        integracao.Instance.AddLog(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                servico = 0;
                stStatus.Text = ex.Message;
            }
            finally
            {
                if (count >= 0)
                {
                    // vai para o proximo serviço, se ainda existir
                    if (integracoes != null && ++servico > integracoes.Count) // N+1
                    {
                        startLoop = false;
                        servico = 0; // Revalidar conexão do banco
                    }
                }
            }
        }

        private bool VerificaIntegrador()
        {
            if (!ValidacaoSistema.VerificarConexao())
            {
                startLoop = true;
                stStatus.Text = "Sem conexão com o banco de dados";
                tmr.Interval = 20000; // altera para 20 segundos
                if (integracoes == null)
                {
                    txtStatusServer.Text += stStatus.Text + "\r\n";
                    //txtStatusImpressao.Text += stStatus.Text + "\r\n";

                    if (!sqlServerRun) // Se o SQL ainda nunca rodou....
                    {
                        sqlServerRun = true; //Vou iniciar o serviço!
                        try
                        {
                            txtStatusServer.Text += "Tentando ligar o serviço do SQL Server...\r\n";
                            var service = new ServiceController("MSSQL$PDV7");
                            txtStatusServer.Text += "SQL Server iniciado\r\n";
                            service.Start();
                        }
                        catch (Exception ex)
                        {
                            txtStatusServer.Text += ex.Message + "\r\n";
                        }
                    }
                    return false;
                }

                // Quando o banco cai, para as integrações, se já não tiver parado
                foreach (var integracao in integracoes)
                {
                    if (integracao.Executando)
                        integracao.Parar();
                    else
                        integracao.AtualizarTextBox("Sem conexão com banco de dados");
                }
                return false;
            }
            else if (!ntf.Visible) // Só na primeira vez
            {
                sqlServerRun = true; // SQL tá rodando!

                tmr.Stop(); // PAUSE GERAL!!! por causa da tela de licença ou qualquer outra de erro que pode ocorrer
                stStatus.Text = "Verificando versão do banco de dados";
                tmr.Interval = 20000; // altera para 20 segundos
                Application.DoEvents();
                try
                {
                    var log = pdv7Context.VerificaVersao();
                    if (!string.IsNullOrEmpty(log))
                    {
                        txtStatusServer.Text += log;
                        Application.DoEvents();
                        Logs.Info(CodigoInfo.I003, log);
                    }
                }
                catch (Exception ex)
                {
                    txtStatusServer.Text += "ERRO: " + ex.Message + "\r\n" + ex.StackTrace;
                    Logs.Erro(ex);
                    return false;
                }

                stStatus.Text = "Verificando licenças";
                Application.DoEvents();

                using (var pdvServico = new Licencas())
                {
                    if (frmBaixarLicenca.ConfiguraChaveAtivacao(pdvServico))
                    {
                        Close();
                        return false;
                    }

                    try
                    {
                        pdvServico.Validar(TipoApp.SERVER);
                    }
                    catch (ExceptionPDV exPDV)
                    {
                        txtStatusServer.Text += "\r\nERRO: " + exPDV.Message;
                        throw exPDV;
                    }

                    string[] assemble = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',');
                    string exeDll = assemble[0];

                    AC.RegitraLicenca(pdvServico.ChaveAtivacao);
                    AC.RegitraPDV(exeDll, assemble[1].Split('=')[1]);
                    this.Text = "PDVSeven Integrador: " + AC.Versao;
                }

                ntf.Visible = true;
                ntf.ShowBalloonTip(5000, this.Text, "Em execução", ToolTipIcon.Info);

                var nomeServico = EF.Repositorio.Carregar<tbProduto>(p => p.IDProduto == ProdutoInformation.IDProdutoServico)?.Nome;
                if (nomeServico != null && nomeServico != " * Serviço")
                {
                    CupomSATService.NomeTaxaServico = ContaServices.NomeTaxaServico = nomeServico;
                    txtStatusImpressao.Text += $"Nome da taxa de serviço: {CupomSATService.NomeTaxaServico}\r\n";
                }

                InicializarIntegracoes();

                tmr.Start(); // Continua!!!

            }
            GA.Post(this);

            if (tmr.Interval != 2000)
                tmr.Interval = 2000;

            return true;
        }

        private void mnuConfig_Click(object sender, EventArgs e)
        {
            using (var autenticacao = new frmAutenticacao(true, true, false, false, false))
            {
                if (autenticacao.ShowDialog() == DialogResult.OK)
                {
                    using (var config = new Configurador.UI.frmPrincipal())
                        config.ShowDialog();

                    count = 0;
                    servico = 0;
                }
            }
        }

        private void mnuSair_Click(object sender, EventArgs e)
        {
            using (var autenticacao = new frmAutenticacao(true, true, false, false, false))
            {
                if (autenticacao.ShowDialog() == DialogResult.OK)
                {
                    if (autenticacao.Usuario1.IDUsuario != 1)
                    {
                        MessageBox.Show("O Integrador deve ficar em execução sempre, somente o administrador principal pode encerrar o programa", "Ação não permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Finalizar();
                }
            }
        }

        private void Finalizar()
        {
            tmr.Stop();
            ntf.ShowBalloonTip(1000, this.Text, "Finalizando serviços...", ToolTipIcon.Warning);
            Application.DoEvents();

            integracoes.ForEach(i => i.Parar());
            ntf.Visible = false;
            Application.DoEvents();

            integracoes.Clear();
            integracoes = null;

            Atualizacao.ExecOnExit();

            this.Close();
        }

        private void Status_Click(object sender, EventArgs e)
        {
            startLoop = true;
            count = 0;
            servico = 0;
        }
    }
}