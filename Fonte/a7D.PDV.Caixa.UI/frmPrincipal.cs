using a7D.PDV.Balanca;
using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.BLL.Services;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.Componentes;
using a7D.PDV.Componentes.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Fiscal.NFCe;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using a7D.PDV.Integracao.Pagamento;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPrincipal : Form
    {
        public static bool ModoContingencia { get; set; }
        public static Impressora.Impressora Impressora1 { get; set; }

        public static string InfoTitle { get; private set; } = "";
        public static bool ContaCliente { get; internal set; }

        public string msgRetorno;
        public DialogResult dialogRetorno;
        public static CaixaInformation Caixa1;
        private ConfiguracoesGerenciadorImpressao cfgOI;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void MensagemCarregando(string msg)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    lblCarregando.Visible = true;
                    lblCarregando.Text = msg;
                    lblCarregando.Refresh();
                }));
            }
            catch (Exception)
            {
            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            ModoContingencia = false;
            lblModoContingencia.Text = "";
            try
            {
                MensagemCarregando("Carregando...");

                progressBarCarregando.MarqueeAnimationSpeed = 100;
                progressBarCarregando.Visible = true;

                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception ex)
            {

                BLL.Logs.ErroBox(CodigoErro.E002, ex);
                this.Close();
            }
        }

        private void IniciarImpressora()
        {
            try
            {
                ImpressoraWindows.Margem = ConfiguracoesCaixa.Valores.MargemImpressaoWindows;
                ImpressoraWindows.Largura = ConfiguracoesCaixa.Valores.LarguraImpressaoWindows;
                ImpressoraWindows.FontePadrao = new Font(ConfiguracoesCaixa.Valores.FonteNomeImpressaoWindows, ConfiguracoesCaixa.Valores.FonteTamanhoImpressaoWindows);
            }
            catch (Exception)
            {
            }

            while (Impressora1.ImpressoraAtiva == false)
            {
                Impressora1.Iniciar(
                    ConfiguracoesCaixa.Valores.TipoGerenciadorImpressao,
                    ConfiguracoesCaixa.Valores.ModoFiscal,
                    ConfiguracoesCaixa.Valores.ModeloImpressora,
                    ConfiguracoesCaixa.Valores.VelocidadeImpressora,
                    ConfiguracoesCaixa.Valores.PortaImpressora,
                    out msgRetorno);

                if (ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.SemImpressora)
                    break;

                else if (Impressora1.ImpressoraAtiva == false)
                {
                    dialogRetorno = MessageBox.Show(msgRetorno + "\nDeseja tentar novamente?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogRetorno == DialogResult.No)
                    {
                        dialogRetorno = MessageBox.Show("Deseja continuar sem a impressora?\nCaso continue, será necessário fazer o lançamento das Notas Fiscais manualmente.", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogRetorno == DialogResult.Yes)
                            break;

                        else if (dialogRetorno == DialogResult.No)
                        {
                            Application.Exit();
                            break;
                        }
                    }
                }
            }

            var msg = "";
            if (ConfiguracoesCaixa.Valores.ModoFiscal == false)
                msg += "SISTEMA GERENCIAL\n";

            else if (ConfiguracoesCaixa.Valores.ModoFiscal == true
                && Impressora1.ModoFiscalAtivo == false
                && ConfiguracoesCaixa.Valores.GerenciadorImpressao != ETipoGerenciadorImpressao.SAT)
            // Validação apenas para ECF
            {
                msg += "SISTEMA EM MODO DE CONTINGÊNCIA\n";
                Invoke((MethodInvoker)delegate
                {
                    lblModo.ForeColor = Color.Red;
                });
            }

            else if (Impressora1.ImpressoraAtiva == false)
            {
                msg += "IMPRESSORA DESCONECTADA\n";
                Invoke((MethodInvoker)delegate
                {
                    lblModo.ForeColor = Color.Red;
                });
            }

            try
            {
                Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        lblModo.Text = msg;
                    }
                    catch (Exception)
                    {
                    }
                });

                timerImpressora.Enabled = Impressora1.ImpressoraAtiva;
            }
            catch (Exception)
            {
            }

        }

        public void AtualizarStatus()
        {
            Caixa1 = BLL.Caixa.Status(AC.PDV.IDPDV.Value);

            if (Caixa1 == null)
            {
                lblMensagem.Text = "Caixa não iniciado";

                pnlCaixaAberto.Visible = false;
                pnlCaixaFechado.Visible = true;

                btnAbrirCaixa.Enabled = true;
                btnAbrirCaixa.BackColor = Color.FromArgb(140, 198, 63);

                Refresh();
            }
            else if (Caixa1.DtAbertura != null && Caixa1.DtFechamento == null)
            {
                //Caixa aberto
                CaixaAberto();
            }
            else
            {
                //Caixa fechado
                CaixaFechado();
            }

            if (Impressora1.ImpressoraAtiva == true && Impressora1.ModoFiscalAtivo == true && Impressora1.Estado == ACBrFramework.ECF.EstadoECF.Bloqueada)
            {
                ReducaoZEmitida();
            }
        }

        private void ReducaoZEmitida()
        {
            string msg = "Já houve Redução Z no dia!\nCaixa liberado apenas após 23:59";
            lblMensagem.Text = msg;
            lblMensagem.ForeColor = Color.Red;

            MessageBox.Show(msg, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            pnlCaixaAberto.Enabled = false;
            pnlCaixaFechado.Enabled = false;

            pnlCaixaAberto.BackColor = Color.FromArgb(190, 199, 202);
            pnlCaixaFechado.BackColor = Color.FromArgb(190, 199, 202);
        }

        private void CaixaFechado()
        {
            lblMensagem.Text = "Caixa fechado em " + Caixa1.DtFechamento.Value.ToString("dd/MM HH:mm");

            pnlCaixaAberto.Visible = false;
            pnlCaixaFechado.Visible = true;

            btnAbrirCaixa.Enabled = true;
            btnAbrirCaixa.BackColor = Color.FromArgb(140, 198, 63);
        }

        private void CaixaAberto()
        {
            Invoke(new MethodInvoker(delegate
            {
                lblMensagem.Text = "Caixa aberto em " + Caixa1.DtAbertura.Value.ToString("dd/MM HH:mm");
                pnlCaixaAberto.Visible = true;
                pnlCaixaFechado.Visible = false;
            }));
        }

        private void btnFecharCaixaTurno_Click(object sender, EventArgs e)
        {
            var qtd = Pedido.QtdDeliveryAberto();
            if (qtd > 0)
            {
                if (MessageBox.Show($@"Há {qtd} pedido{(qtd > 1 ? "s" : "")} delivery não finalizado{(qtd > 1 ? "s" : "")}!
Isso poderá gerar divergência no caixa atual ou no próximo.

Deseja realizar o fechamento do caixa mesmo assim?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            frmFecharCaixa form = new frmFecharCaixa(this);
            form.ShowDialog();

            AtualizarStatus();
        }

        private void btnAbrirCaixa_Click(object sender, EventArgs e)
        {
            frmAbrirCaixa form = new frmAbrirCaixa(this);
            form.ShowDialog();

            AtualizarStatus();
        }

        private void btnAjusteCaixa_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                frmAjusteCaixa form = new frmAjusteCaixa(this, usuario);
                form.ShowDialog();
            }
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmPedidos form = new frmPedidos(this);
            form.Show();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bloquearLiberarComandaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAlterarStatusComanda frm = new frmAlterarStatusComanda();
            frm.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autenticar();
        }

        private void Autenticar()
        {
            if (NormalOuTouch.Autenticacao(true, true, true, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                lblUsuarioAutenticado.Invoke((MethodInvoker)delegate
                {
                    lblUsuarioAutenticado.Text = "Usuário: " + AC.Usuario.Nome + "\r\n";
                    lblUsuarioAutenticado.Text += "Login às " + DateTime.Now;
                });
            }
            else
                Close();
        }

        private void btnFechamentoDia_Click(object sender, EventArgs e)
        {
            this.Enabled = false; this.Refresh();
            FechamentoDia();
            this.Enabled = true; this.Refresh();
        }

        private DialogResult CaixasAbertos()
        {
            var caixasAbertos = BLL.Caixa.ListarAbertos().ToList();
            if (caixasAbertos.Count == 0)
                return DialogResult.No; // Sem caixas abertos

            var autoClose = caixasAbertos.Where(c => c.PDV.AutoFechamento());
            if (autoClose.Any())
            {
                var relatorio = new List<string>();
                foreach (var caixa in autoClose)
                {
                    BLL.Caixa.ForcarFechamento(caixa);
                    relatorio.AddRange(Relatorio.FechamentoCaixa(caixa.IDCaixa.Value, true));
                }

                Impressora1.RelatorioGerencial(relatorio, 7);
                return DialogResult.OK;
            }

            using (var frmForcarFechamento = new frmForcarFechamento(caixasAbertos))
                return frmForcarFechamento.ShowDialog(); // OK ou Cancel
        }

        private void FechamentoDia()
        {
            if (BLL.Caixa.CaixasSemFechamento().Any() == false)
            {
                MessageBox.Show("Não há fechamento pendente!");
                return;
            }

            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
                return;

            while (true)
            {
                var result = CaixasAbertos();
                if (result == DialogResult.Cancel)
                    return; // Operação cancelado pelo usuário
                if (result == DialogResult.No)
                    break; // Sem caixas abertos
            }

            try
            {
                int idCaixaFechamento = 0;
                if (ConfiguracoesSistema.Valores.ComandaComCredito)
                {
                    if (ConfiguracoesSistema.Valores.FecharComandasCreditoDia)
                        Comanda.FecharComandasContaCliente(AC.PDV.IDPDV.Value, usuario.IDUsuario.Value, ref idCaixaFechamento);

                    // Apenas depois de fechar pedidos pode registrar no caixa
                    // Comanda.RegistrarVendasComCreditoNoCaixa(AC.PDV.IDPDV.Value, usuario.IDUsuario.Value, ref idCaixaFechamento);
                }

                Saldo.ExpirarCreditos();

                Pedido.CancelarBalcaoPendentes(AC.PDV.IDPDV.Value, usuario.IDUsuario.Value, ref idCaixaFechamento);
                Pedido.FechaCancelados(AC.PDV.IDPDV.Value, usuario.IDUsuario.Value, ref idCaixaFechamento);

                if (idCaixaFechamento > 0)
                {
                    var caixaFechamento = BLL.Caixa.Carregar(idCaixaFechamento);
                    caixaFechamento.DtFechamento = DateTime.Now;
                    BLL.Caixa.Salvar(caixaFechamento);
                }

                FazerFechamentoDoDia(usuario.IDUsuario.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            AtualizarStatus();
        }

        private static void FazerFechamentoDoDia(int idUsuario)
        {
            var fechamento = Fechamento.Fechar(idUsuario, AC.PDV.IDPDV.Value);
            var relatorio = Relatorio.Fechamento(fechamento.IDFechamento.Value, false);

            Impressora1.RelatorioGerencial(relatorio, 7);
        }

        private void reduçãoZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false; this.Refresh();

            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                if (MessageBox.Show("Você tem certeza que deseja emitir a Redução Z? \n(Caso isso seja feito, não poderá mais fazer lançamentos hoje)", "PDV", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Impressora1.ReducaoZ();
                }
            }

            AtualizarStatus();
            this.Enabled = true; this.Refresh();
        }

        private void leituraXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false; this.Refresh();

            Impressora1.LeituraX();

            this.Enabled = true; this.Refresh();
        }

        private void resumoDoDiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                frmResumoDia form = new frmResumoDia();
                form.ShowDialog();
            }
        }

        private void produtosCanceladosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> relatorio = new List<string>();

            relatorio.Add("PRODUTOS CANCELADOS");
            relatorio.AddRange(BLL.Relatorio.ProdutosCancelados(null));
            relatorio.Add("\n\n");

            Impressora1.RelatorioGerencial(relatorio, 7);
        }
        
        private void timerImpressora_Tick(object sender, EventArgs e)
        {
            bool ret;
            string msgRetorno;

            ret = Impressora1.VerificarStatus(out msgRetorno);
            if (ret == false)
            {
                MessageBox.Show("Ocorreu algum erro com a impressora e o sistema será finalizado!\nRetorno impressora: " + msgRetorno, "ERRO IMPRESSORA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void alterarDisponibilidadeDeProdutosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDisponibilidadeProduto form = new frmDisponibilidadeProduto();
            form.ShowDialog();
        }

        private void alterarNomePDVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, false, false, false, out UsuarioInformation usuario, true) == DialogResult.OK)
            {
                frmAlterarNomePDV form = new frmAlterarNomePDV();
                form.ShowDialog();

                lblPDV.Text = AC.PDV.Nome;
            }
        }

        private void reimprimirFechamentoDoDiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                var frm = new frmReimprimirFechamentoDiario(this);
                frm.ShowDialog();
            }
        }

        private void abrirGavetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAutenticacao(false, false, true, false, false))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Impressora.ImpressoraHelper.AbrirGaveta(ConfiguracoesCaixa.Valores.ModeloImpressora);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            string versao = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            Invoke(new MethodInvoker(delegate
            {
                lblVersao.Text = "Versão: " + versao;
                lblVersao.Refresh();
            }));

            MensagemCarregando("Iniciando...");

            #region Verificar DB

            MensagemCarregando("Verificando conexão com o banco de dados...");
            if (!frmConfigurarConexaoDB.Verifica())
                return;

            #endregion

            #region Licença

            MensagemCarregando("Validando Licenças...");

            var inicio = DateTime.Now;
            var serialHD = string.Empty;
            string chaveAtivacao = null;
            using (var pdvServico = new Licencas())
            {
                try
                {
                    if (string.IsNullOrEmpty(pdvServico.ChaveAtivacao))
                        throw new ExceptionPDV(CodigoErro.A101);

                    chaveAtivacao = pdvServico.ChaveAtivacao;

                    //MensagemCarregando("Validando Licença...");
                    //pdvServico.Validar(TipoApp.CAIXA);

                    MensagemCarregando("Validando Caixa...");
                    serialHD = ValidacaoSistema.RetornarSerialHD();
                    pdvServico.Carregar(ETipoPDV.CAIXA, serialHD);

                    GA.Post(this);
                }
                catch (Exception ex)
                {
                    if (ex is ExceptionPDV exPDV && exPDV.CodigoErro.ToString().StartsWith("F"))
                    {
                        var frmValidacaoOffline = new frmValidacaoOffline();
                        frmValidacaoOffline.ShowDialog();
                    }
                    else
                        BLL.Logs.ErroBox(CodigoErro.E001, ex);

                    Application.Exit();
                    return;
                }
            }

            Invoke(new MethodInvoker(delegate
            {
                lblPDV.Text = AC.PDV.Nome;
                lblVersao.Text += $" [{serialHD}] - {AC.PDV.IDPDV.Value}";
            }));

            #endregion

            new ConfiguracoesCaixa(AC.PDV.IDPDV.Value);

            // Configura ordem de produção
            OrdemProducaoServices.UsarOrdemProducao = ConfiguracoesCaixa.Valores.OrdemImpressao;
            OrdemProducaoServices.UsarAreasProducao = ConfiguracoesCaixa.Valores.UsarAreas;
            OrdemProducaoServices.AreasProducaoPadrao = ConfiguracoesCaixa.Valores.AreasPadrao;
            OrdemProducaoServices.ImprimirViaExpedicao = ConfiguracoesCaixa.Valores.ImprimirViaExpedicao;
            OrdemProducaoServices.IDAreaViaExpedicao = ConfiguracoesCaixa.Valores.IDAreaViaExpedicao;

            SelectIDValor.onSelect += (st, itens) => frmSelecao.Select("SELECIONE", st, itens);

            // Precisa ter no integrador e WS2 ou qualquer outro que imprime também!
            var nomeServico = EF.Repositorio.Carregar<tbProduto>(p => p.IDProduto == ProdutoInformation.IDProdutoServico)?.Nome;
            if (nomeServico != null && nomeServico != " * Serviço")
                CupomSATService.NomeTaxaServico = ContaServices.NomeTaxaServico = nomeServico;

            Componentes.Controles.BotaoGrid.Configure(ConfiguracoesCaixa.Valores.CaixaTouchParametros);

            BalancaServices.InicializarBalanca(ConfiguracoesCaixa.Valores.ProtocoloBalanca, ConfiguracoesCaixa.Valores.PortaBalanca);

            #region ModoConingencia   

            if (ConfiguracoesSistema.Valores.Fiscal == "SAT"
             && ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.SAT)
            {
                MensagemCarregando("Testando conexão com o SAT...");
                TestarSat();
            }
            else if (ConfiguracoesSistema.Valores.Fiscal == "NFCe")
            {
                NFeFacade.ConfigPathXSD(new FileInfo(GetType().Assembly.Location).Directory.FullName);
            }

            #endregion

            #region "Configurações e impressões"

            MensagemCarregando("Iniciando impressoras...");

            Impressora1 = new Impressora.Impressora();

            IniciarImpressora();

            ConfigurarTEF();

            #endregion

            int idContaCliente = (int)EGateway.ContaCliente;
            ContaCliente = EF.Repositorio.Carregar<tbTipoPagamento>(p => p.IDGateway == idContaCliente && p.Ativo) != null;
            if (!ContaCliente
              && ConfiguracoesSistema.Valores.ComandaComCredito)
            {
                // Incoerencia!
                throw new ExceptionPDV(CodigoErro.E808);
            }

            MensagemCarregando("Insira sua chave de acesso...");
        }

        private void TestarSat()
        {

            ModoContingencia = false;
            var mensagemContingencia = "";

            var respostaSat = SATService.TesteConexaoSat();
            if (!respostaSat.EmOperacao)
            {
                ModoContingencia = true;
                mensagemContingencia = respostaSat.mensagem ?? respostaSat.Message;
            }
            else
            {
                //var resposta = SATServer.TesteFimAFim();
                //if (!resposta.OK)
                //    ModoContingencia = true;
            }

            if (ModoContingencia)
            {

                var tentarNovamente = MessageBox.Show("Erro: " + mensagemContingencia +
                      "\nTentar enviar novamente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (tentarNovamente == DialogResult.Yes)
                {
                    TestarSat();
                }
                else
                {
                    var modocontingencia = MessageBox.Show(
                      "Deseja entrar em modo de contingência?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (modocontingencia == DialogResult.Yes)
                    {
                        var resultContingencia = MessageBox.Show(
                                          "SISTEMA EM MODO DE CONTINGÊNCIA.\n\n" +
                                               "A partir de agora os cupons fiscais ficarão pendentes." +
                                               " Para emissao dos cupons fiscais pendentes, acesse Backoffice (aba S@t).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {

                        Invoke(new MethodInvoker(delegate
                        {
                            Close();
                        }));
                    }


                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    Logs.ErroBox(CodigoErro.E002, e.Error);
                    this.Close();
                }
                else
                {
                    AtualizarStatus();
                    progressBarCarregando.Visible = false;
                    lblCarregando.Visible = false;

                    if ((ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.ImpressoraWindows
                      || ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.SAT)
                     && !string.IsNullOrEmpty(ConfiguracoesCaixa.Valores.ModeloImpressora))
                    {
                        cfgOI = new ConfiguracoesGerenciadorImpressao();
                    }

                    // Habilita mensagens e verificação de licença assincronamente
                    tmrGeral.Enabled = true;
                }

                Text += $": {AC.PDV?.Nome} - {AC.Usuario?.Nome}";
                menuStrip1.Enabled = true;
                Logs.Info(CodigoInfo.I000);

                fiscalToolStripMenuItem.Visible = ConfiguracoesCaixa.Valores.ModoFiscal && Impressora1 != null
                    && ConfiguracoesCaixa.Valores.GerenciadorImpressao != ETipoGerenciadorImpressao.SAT;

                Autenticar();
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E002, ex);
            }
        }

        DateTime dtNextLicensas = DateTime.MinValue;
        int step = 0;

        private void tmrGeral_Tick(object sender, EventArgs e)
        {
            if (bwGeral.IsBusy)
                return;

            bwGeral.RunWorkerAsync();

            lblModoContingencia.Text = InfoTitle;
        }

        private void bwGeral_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            step++;
            if (dtNextLicensas < DateTime.Now)
            {
                dtNextLicensas = DateTime.Now.AddMinutes(15);
                using (var pdvServico = new Licencas())
                {
                    pdvServico.Validar(TipoApp.CAIXA);
                    pdvServico.VerificaDataPedidos();
                }
            }
            else if (step == 1 && cfgOI != null)
            {
                string statusPrint = DateTime.Now.ToString("dd/MM HH:mm ");
                try
                {
                    var localServer = new LocalPrintServer();
                    var queue = localServer.GetPrintQueue(ConfiguracoesCaixa.Valores.ModeloImpressora);
                    if (queue.QueueStatus == PrintQueueStatus.None)
                    {
                        statusPrint += "OK";

                        var info = OrdemImpressaoServices.ImprimiPendente(AC.idPDV, ConfiguracoesCaixa.Valores.ModeloImpressora, AC.Usuario, AC.PDV, cfgOI);
                        if (info != null)
                            e.Result = info;
                        else
                            e.Result = "";
                    }
                    else
                    {
                        e.Result = $"Erro na impressora '{ConfiguracoesCaixa.Valores.ModeloImpressora}' {queue.QueueStatus}";
                        statusPrint += queue.QueueStatus.ToString();
                    }
                }
                catch (Exception exP)
                {
                    statusPrint += " ERRO: " + exP.Message;
                }
                ConfiguracaoBD.DefinirValorPadraoTipoPDV(EConfig._StatusImpressora, ETipoPDV.CAIXA, AC.idPDV, statusPrint);
            }
            else if (step == 2)
            {
                e.Result = MensagemServices.RetornaMensagens();
            }
            else
            {
                step = 0;
            }
        }

        private void bwGeral_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    throw e.Error;

                else if (e.Result is IEnumerable<tbMensagem> msgs)
                    MensagemServices.ExibeMensagens(msgs);

                else if (e.Result is string msg)
                {
                    if (ModoContingencia && string.IsNullOrEmpty(msg))
                        InfoTitle = "EM MODO DE CONTINGÊNCIA";
                    else
                        InfoTitle = msg;
                }
            }
            catch (ExceptionPDV exPDV)
            {
                if (exPDV.CodigoErro.ToString().StartsWith("F"))
                {
                    Logs.ErroBox(exPDV);
                    Application.Exit();
                }
                else
                    Logs.ErroBox(exPDV);
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.A310, ex);
            }
        }

        private void AdministrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string viaCliente;
            string viaEstabelecimento;
            string mensagemOperador = PinpadTEF.Administrar(TipoTEF.NTKPayGo, out viaCliente, out viaEstabelecimento);

            if (!String.IsNullOrWhiteSpace(viaCliente))
                ImprimeRecibo(viaCliente);

            if (!String.IsNullOrWhiteSpace(viaEstabelecimento))
                ImprimeRecibo(viaEstabelecimento);

            if (!String.IsNullOrWhiteSpace(mensagemOperador))
                MessageBox.Show(mensagemOperador);
        }

        private void ConfigurarTEF()
        {
            string viaCliente;
            string viaEstabelecimento;
            string mensagemOperador;

            var tefNTK = TipoPagamento.Listar().Where(p => p.Ativo == true && p.IDGateway > 0 && (p.Gateway == EGateway.NTKTEF || p.Gateway == EGateway.NTKPOS)).Count() > 0;
            var tefStone = TipoPagamento.Listar().Where(p => p.Ativo == true && p.IDGateway > 0 && (p.Gateway == EGateway.StoneTEF || p.Gateway == EGateway.StonePOS)).Count() > 0;
            var tefGranito = TipoPagamento.Listar().Where(p => p.Ativo == true && p.IDGateway > 0 && (p.Gateway == EGateway.GranitoTEF || p.Gateway == EGateway.GranitoPOS)).Count() > 0;

            if (tefNTK)
            {
                Invoke(new MethodInvoker(delegate
                {
                    MenuTEF.Visible = true;
                    MenuNTK.Visible = true;
                }));
                
                mensagemOperador = PinpadTEF.Iniciar(TipoTEF.NTKPayGo, out viaCliente, out viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(viaCliente))
                    ImprimeRecibo(viaCliente);

                if (!String.IsNullOrWhiteSpace(viaEstabelecimento))
                    ImprimeRecibo(viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(mensagemOperador))
                    MessageBox.Show(mensagemOperador);
            }

            if (tefStone)
            {
                Invoke(new MethodInvoker(delegate
                {
                    MenuTEF.Visible = true;
                    MenuStone.Visible = true;
                }));
                
                Integracao.Pagamento.StoneTEF.PinpadStoneTEF.StoneCode = ConfiguracoesSistema.Valores.StoneCode;

                mensagemOperador = PinpadTEF.Iniciar(TipoTEF.STONE, out viaCliente, out viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(viaCliente))
                    ImprimeRecibo(viaCliente);

                if (!String.IsNullOrWhiteSpace(viaEstabelecimento))
                    ImprimeRecibo(viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(mensagemOperador))
                    MessageBox.Show(mensagemOperador);
            }

            if (tefGranito)
            {
                Invoke(new MethodInvoker(delegate
                {
                    MenuTEF.Visible = true;
                    MenuNTK.Visible = true;
                }));

                Integracao.Pagamento.GranitoTEF.GranitoPinpad.GranitoCode = ConfiguracoesSistema.Valores.GranitoCode;
                Integracao.Pagamento.GranitoTEF.GranitoPinpad.GranitoCNPJ = ConfiguracoesSistema.Valores.GranitoCNPJ;
                Integracao.Pagamento.GranitoTEF.GranitoPinpad.GranitoIdPDV = ConfiguracaoBD.ValorOuPadrao(EConfig.GranitoIDPDV, ETipoPDV.CAIXA, AC.PDV.IDPDV.Value);

                mensagemOperador = PinpadTEF.Iniciar(TipoTEF.GRANITO, out viaCliente, out viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(viaCliente))
                    ImprimeRecibo(viaCliente);

                if (!String.IsNullOrWhiteSpace(viaEstabelecimento))
                    ImprimeRecibo(viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(mensagemOperador))
                    MessageBox.Show(mensagemOperador);
            }
        }
        
        private void ImprimeRecibo(string recibo)
        {
            if (string.IsNullOrEmpty(recibo))
                return;

            ImpressoraWindows.ImprimirTexto(ConfiguracoesCaixa.Valores.ModeloImpressora, true, recibo);
        }

        private void CancelarOperaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCancelarTEF())
                frm.ShowDialog();
        }
    }
}
