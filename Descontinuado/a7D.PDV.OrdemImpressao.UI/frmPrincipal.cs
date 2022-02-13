using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using System.Drawing.Printing;
using System.Configuration;
using a7D.PDV.BLL;
using a7D.PDV.SAT;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.EF.Models;

namespace a7D.PDV.OrdemImpressao.UI
{
    public partial class frmPrincipal : Form
    {
        private List<OrdemImpressaoInformation> ListaOrdemImpressao { get; set; }
        private string ConteudoImpressao { get; set; }
        private PrintDocument DocToPrint { get; } = new PrintDocument();
        private bool Fechar { get; set; } = false;
        private bool Conectado { get; set; } = false;
        private bool AvisarDesconexao { get; set; } = false;
        private bool AvisarConexao { get; set; } = false;
        private object Lock { get; set; } = new object();
        private int ManterPorDias { get; set; } = 0;

        private Icon ErrorIcon { get; set; } = Resource1.pdv7_error_icon;
        private Icon NormalIcon { get; set; } = Resource1.pdv7_print_icon;

        private ConfiguracoesGerenciadorImpressao Configuracoes { get; set; }

        private PDVInformation PDVPadrao
        {
            get
            {
                return BLL.PDV.Listar().FirstOrDefault(p => (p.TipoPDV.IDTipoPDV == 10 || p.TipoPDV.IDTipoPDV == 40));
            }
        }

        private UsuarioInformation UsuarioPadrao
        {
            get
            {
                return Usuario.Listar().FirstOrDefault(u => u.PermissaoAdm == true);
            }
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (ConteudoImpressao != null && ConteudoImpressao != "")
            {
                var g = e.Graphics;
                //var font = new Font("Lucida Console", 10);
                var font = new Font("Arial", 10);

                g.DrawString(ConteudoImpressao, font, Brushes.Black, 0, 0);

                ConteudoImpressao = null;
            }
        }

        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            var documento = ((PrintDocument)sender).DocumentName;
            var idOrdemImpressao = Convert.ToInt32(documento.Split('.')[0]);
            if (ManterPorDias == 0)
            {
                BLL.OrdemImpressao.Excluir(idOrdemImpressao);
            }
            else
            {
                BLL.OrdemImpressao.Impresso(idOrdemImpressao);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(Lock))
            {
                try
                {
                    Task.Run(new Action(async () => await IniciarImpressao(VerificarOrdemImpressao)));
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
        }

        private async Task Conectar()
        {
            AvisarDesconexao = true;
            AvisarConexao = false;
            while (!ValidacaoSistema.VerificarConexaoDB())
            {
                BeginInvoke(new Action(() =>
                {
                    lblAtivo.Visible = false;
                    lblSemConexao.Visible = true;
                    Icon = ErrorIcon;
                    notifyIcon1.Icon = ErrorIcon;
                    if (AvisarDesconexao)
                    {
                        AvisarSemConexao();
                        AvisarDesconexao = false;
                        AvisarConexao = true;
                    }
                }));
                await Task.Delay(1000);
            }
        }

        private async Task IniciarImpressao(Action acao = null)
        {
            BeginInvoke(new Action(() =>
            {
                timer1.Enabled = false;
            }));
            await Conectar();
            BeginInvoke(new Action(() =>
            {
                ManterPorDias = ConfiguracoesSistema.Valores.ManterImpressaoPorDias;

                timer1.Enabled = true;


                lblAtivo.Visible = true;
                lblSemConexao.Visible = false;

                if (AvisarConexao)
                {
                    AvisarConexaoReestabelecida();
                }

                Icon = NormalIcon;
                notifyIcon1.Icon = NormalIcon;
                acao?.Invoke();
            }));
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                #region Verificar DB
                if (ConfigurationManager.ConnectionStrings["connectionString"] == null || ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString == "")
                {
                    using (var formConfigurarConexaoDB = new Configurador.UI.frmConfigurarConexaoDB())
                    {
                        formConfigurarConexaoDB.ShowDialog();
                        Fechar = true;
                        Application.Exit();
                        return;
                    }
                }
                #endregion

                Configuracoes = new ConfiguracoesGerenciadorImpressao();

                lblSemConexao.Visible = true;
                lblAtivo.Visible = false;
                Icon = Resource1.pdv7_error_icon;
                notifyIcon1.Icon = Resource1.pdv7_error_icon;

                Task.Run(new Action(async () => await IniciarImpressao()));
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message);
                Fechar = true;
                Application.Exit();
            }
        }

        private PrintDocument CriarPrintDocument(string impressora)
        {
            var pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = impressora;
            pd.EndPrint += printDocument1_EndPrint;
            return pd;
        }

        private void VerificarOrdemImpressao()
        {
            ListaOrdemImpressao = BLL.OrdemImpressao.ListarNaoEnviados().Take(5).ToList();

            var i = 0;
            foreach (var item in ListaOrdemImpressao)
            {
                i++;
                try
                {
                    printDocument1.PrinterSettings.PrinterName = item.AreaImpressao.NomeImpressora;

                    if (item.Conta != true && item.SAT != true)
                    {
                        ConteudoImpressao = item.ConteudoImpressao;
                        printDocument1.DocumentName = item.IDOrdemImpressao.ToString() + ".PDV";
                        printDocument1.Print();
                    }
                    else if (item.Conta == true)
                    {
                        PedidoInformation pedido;
                        UsuarioInformation usuario;
                        PDVInformation pdv;

                        var conteudo = item.ConteudoImpressao?.Split('|');

                        pedido = Pedido.CarregarCompleto(Convert.ToInt32(conteudo[0]));

                        if (conteudo.Length == 3)
                        {
                            usuario = Usuario.Carregar(Convert.ToInt32(conteudo[1]));
                            pdv = BLL.PDV.Carregar(Convert.ToInt32(conteudo[2]));
                        }
                        else
                        {
                            usuario = UsuarioPadrao;
                            pdv = PDVPadrao;
                        }

                        using (var pd = CriarPrintDocument(item.AreaImpressao.NomeImpressora))
                        {
                            pd.DocumentName = item.IDOrdemImpressao.ToString() + ".PDV";
                            Pedido.AdicionarProdutoServico(pedido, true, pdv, usuario);
                            Pedido.AdicionarProdutoConsumacaoMinima(pedido, pdv, usuario);

                            ImpressaoSat.ImprimirConta(pedido, pd);
                        }

                    }
                    else if (item.SAT == true)
                    {
                        var conteudo = item.ConteudoImpressao?.Split('|');
                        if (conteudo.Length == 2)
                        {
                            var ex = new Exception();
                            ImpressaoSat.ImprimirCupomVenda(conteudo[0], Convert.ToInt32(conteudo[1]), item.AreaImpressao.NomeImpressora, Configuracoes.TipoImpressaoSat, Configuracoes.CupomSat, out ex);
                        }
                    }


                    if (ManterPorDias == 0)
                    {
                        BLL.OrdemImpressao.Excluir(item.IDOrdemImpressao.Value);
                    }
                    else
                    {
                        BLL.OrdemImpressao.Impresso(item.IDOrdemImpressao.Value);
                    }

                    if (i > 5) { break; }
                }
                catch (Exception ex)
                {
                    notifyIcon1.ShowBalloonTip(5000, "ERRO", ex.Message, ToolTipIcon.Error);
                    //EventLog.Erro(ex.Message);
                    // throw;
                }
            }

            if (ManterPorDias >= 0)
            {
                BLL.OrdemImpressao.ExcluirAntigos(ManterPorDias);
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Fechar)
            {
                e.Cancel = true;
                MinimizarParaSystray();
            }
        }

        private void MinimizarParaSystray()
        {
            Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "Monitor de impressão";
            notifyIcon1.BalloonTipText = "O monitor de impressão ainda está em execução";
            notifyIcon1.ShowBalloonTip(500);
        }

        private void AvisarSemConexao()
        {
            var visible = notifyIcon1.Visible;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500, "Erro de conexão", "Não foi possível se conectar ao servidor. Estamos tentando novamente.", ToolTipIcon.Error);
            notifyIcon1.Visible = visible;
        }

        private void AvisarConexaoReestabelecida()
        {
            var visible = notifyIcon1.Visible;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500, "Conexão Reestabelecida", "A conexão com o servidor foi reestabelecida.", ToolTipIcon.Info);
            notifyIcon1.Visible = visible;
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fechar = true;
            Close();
        }

        private void frmPrincipal_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                MinimizarParaSystray();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button)
            {
                if (!Visible)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    Focus();
                }
            }
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            DocToPrint?.Dispose();
        }
    }
}
