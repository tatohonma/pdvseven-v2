using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        frmTipoPagamento fTipoPagamento;
        frmMesa fMesa;
        frmComanda fComanda;
        frmUsuario fUsuario;
        frmAreaImpressao fAreaImpressao;
        frmMapearProdutosAreaProducao fMapearProdutosAreaProducao;
        frmCategoria fCategoria;
        frmProduto fProduto;
        frmPainelModificacao fPainelModificacao;
        frmPDV fPDV;
        frmTipoEntrada fTipoEntrada;
        frmRelatoriosFechamento fRelatoriosFechamento;
        frmRelatoriosPorData fRelatoriosData;
        frmRelatoriosGerais fRelatoriosGerais;
        frmTema fTemaCardapio;
        frmCardapioDigital fCardapioDigital;
        frmClassificacaoFiscal fClassificacaoFiscal;
        frmTipoTributacao fTipoTributacao;
        frmCFE fCFE;
        frmTipoDesconto fTipoDesconto;
        frmUnidade fUnidade;
        frmConversaoUnidade fConversaoUnidade;
        frmTipoMovimentacao fTipoMovimentacao;
        frmMovimentacao fMovimentacao;
        frmInventario fInventario;
        frmRelatorioEstoqueAtual fRelatorioEstoqueAtual;
        frmRelatorioMovimentacoes fRelatorioHistoricoMovimentacoes;
        frmRelatorioValorizacaoEstoque fRelatorioValorizacaoEstoque;
        frmDashboard fDashboard;
        frmMotivoCancelamento fMotivoCancelamento;
        frmTaxaEntrega fTaxaEntrega;
        frmEntregador fEntregador;

        private readonly IFormatProvider _provider = new CultureInfo("pt-BR");

        private void btnMesas_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fMesa = new frmMesa();
            fMesa.MdiParent = this;
            fMesa.Dock = DockStyle.Fill;

            fMesa.Show();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                #region DB

                if (!frmConfigurarConexaoDB.Verifica())
                    return;

                #endregion

                #region Verificar Ativação

                using (var pdvServico = new Licencas())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(pdvServico.ChaveAtivacao))
                        {
                            Logs.ErroBox(CodigoErro.A101);
                            Close();
                            return;
                        }

                        var serial = ValidacaoSistema.RetornarSerialHD();
                        pdvServico.Validar(TipoApp.BACKOFFICE);
                        pdvServico.Carregar(ETipoPDV.BACKOFFICE, serial);

                        this.Text += ": " + AC.PDV.Nome;
                    }
                    catch (Exception ex)
                    {
                        if (ex is ExceptionPDV exPDV && exPDV.CodigoErro.ToString().StartsWith("F"))
                        {
                            var frmValidacaoOffline = new frmValidacaoOffline();
                            frmValidacaoOffline.ShowDialog();
                        }
                        else
                            BLL.Logs.ErroBox(CodigoErro.E011, ex);

                        Application.Exit();
                        return;
                    }
                }

                #endregion

                #region Layout de acordo com as configurações

                if (ConfiguracoesSistema.Valores.Fiscal == "none")
                    tabControl1.TabPages.Remove(tabFiscal);
                else
                    tabFiscal.Text = ConfiguracoesSistema.Valores.Fiscal;

                try
                {
                    if (BLL.PDV.PossuiEstoque())
                    {
                        btnIngredientes.Visible = true;
                        groupCadastrosEstoque.Visible = true;
                    }
                    else
                    {
                        btnIngredientes.Visible = false;
                        groupCadastrosEstoque.Visible = false;
                        tabControl1.TabPages.Remove(tabEstoque);
                    }
                }
                catch
                {
                    groupCadastrosEstoque.Visible = false;
                    tabControl1.TabPages.Remove(tabEstoque);
                }

                if (!BLL.PDV.PossuiTema())
                    tabControl1.TabPages.Remove(tabCardapioDigital);

                #endregion

                GA.Post(this);

                var frm = new frmAutenticacao(true, true, true, false, false);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    this.Close();
                    return;
                }

                if (AC.TipoUsuario != ETipoUsuario.Administrador)
                {
                    tabControl1.TabPages.Remove(tabAnalise);
                    btnUsuarios.Enabled = false;
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    CefSharp.Cef.EnableHighDPISupport();
                    var exitcode = CefSharp.Cef.ExecuteProcess();

                    if (exitcode >= 0)
                    {
                        this.Close();
                        throw new Exception("Erro ao inicializar Dashboard");
                    }

                    var settings = new CefSharp.CefSettings();
                    //settings.RemoteDebuggingPort = 8081;
                    CefSharp.Cef.Initialize(settings);

                    btnDashboardGeral_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E012, ex);
                Application.Exit();
            }
        }

        private void btnComandas_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fComanda = new frmComanda();
            fComanda.MdiParent = this;
            fComanda.Dock = DockStyle.Fill;

            fComanda.Show();
        }

        public void FecharJanelas()
        {
            foreach (var item in this.MdiChildren)
            {
                if (item != null)
                    item.Close();
            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fUsuario = new frmUsuario();
            fUsuario.MdiParent = this;
            fUsuario.Dock = DockStyle.Fill;

            fUsuario.Show();
        }

        //private void btnClientes_Click(object sender, EventArgs e)
        //{
        //    FecharJanelas();

        //    fCliente = new frmCliente();
        //    fCliente.MdiParent = this;
        //    fCliente.Dock = DockStyle.Fill;

        //    fCliente.Show();
        //}

        private void btnAreaProducao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fAreaImpressao = new frmAreaImpressao();
            fAreaImpressao.MdiParent = this;
            fAreaImpressao.Dock = DockStyle.Fill;

            fAreaImpressao.Show();
        }

        private void btnMapearProduto_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fMapearProdutosAreaProducao = new frmMapearProdutosAreaProducao();
            fMapearProdutosAreaProducao.MdiParent = this;
            fMapearProdutosAreaProducao.Dock = DockStyle.Fill;

            fMapearProdutosAreaProducao.Show();
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fCategoria = new frmCategoria();
            fCategoria.MdiParent = this;
            fCategoria.Dock = DockStyle.Fill;

            fCategoria.Show();
        }



        private void btnTipoPagamento_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTipoPagamento = new frmTipoPagamento();
            fTipoPagamento.MdiParent = this;
            fTipoPagamento.Dock = DockStyle.Fill;

            fTipoPagamento.Show();
        }

        private void btnPainelModificacao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fPainelModificacao = new frmPainelModificacao();
            fPainelModificacao.MdiParent = this;
            fPainelModificacao.Dock = DockStyle.Fill;

            fPainelModificacao.Show();
        }

        private void btnPDV_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fPDV = new frmPDV();
            fPDV.MdiParent = this;
            fPDV.Dock = DockStyle.Fill;

            fPDV.Show();
        }

        private void btnTipoEntrada_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTipoEntrada = new frmTipoEntrada();
            fTipoEntrada.MdiParent = this;
            fTipoEntrada.Dock = DockStyle.Fill;

            fTipoEntrada.Show();
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fRelatoriosData = new frmRelatoriosPorData();
            fRelatoriosData.MdiParent = this;
            fRelatoriosData.Dock = DockStyle.Fill;

            fRelatoriosData.Show();
        }

        private void btnRelatoriosFechamento_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fRelatoriosFechamento = new frmRelatoriosFechamento();
            fRelatoriosFechamento.MdiParent = this;
            fRelatoriosFechamento.Dock = DockStyle.Fill;

            fRelatoriosFechamento.Show();
        }

        private void btnSobre_Click(object sender, EventArgs e)
        {
            string versao = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            MessageBox.Show($"PDV7 BackOffice \n\nVersão: {versao}\n[{ValidacaoSistema.RetornarSerialHD()}] - {AC.PDV.IDPDV.Value}");
        }

        private void btnRelatorios_Click_1(object sender, EventArgs e)
        {
            FecharJanelas();

            fRelatoriosGerais = new frmRelatoriosGerais();
            fRelatoriosGerais.MdiParent = this;
            fRelatoriosGerais.Dock = DockStyle.Fill;

            fRelatoriosGerais.Show();
        }

        private void btnTema_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTemaCardapio = new frmTema();
            fTemaCardapio.MdiParent = this;
            fTemaCardapio.Dock = DockStyle.Fill;

            fTemaCardapio.Show();
        }

        private void btnTemaCardapio_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fCardapioDigital = new frmCardapioDigital();
            fCardapioDigital.MdiParent = this;
            fCardapioDigital.Dock = DockStyle.Fill;

            fCardapioDigital.Show();
        }

        private void btnTipoTributacao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTipoTributacao = new frmTipoTributacao();
            fTipoTributacao.MdiParent = this;
            fTipoTributacao.Dock = DockStyle.Fill;

            fTipoTributacao.Show();
        }

        private void btClassificacaoFiscal_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fClassificacaoFiscal = new frmClassificacaoFiscal();
            fClassificacaoFiscal.MdiParent = this;
            fClassificacaoFiscal.Dock = DockStyle.Fill;

            fClassificacaoFiscal.Show();
        }

        private void btnCFE_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fCFE = new frmCFE();
            fCFE.MdiParent = this;
            fCFE.Dock = DockStyle.Fill;

            fCFE.Show();
        }

        private void btTipoDesconto_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTipoDesconto = new frmTipoDesconto();

            fTipoDesconto.MdiParent = this;
            fTipoDesconto.Dock = DockStyle.Fill;

            fTipoDesconto.Show();
        }


        private void tbUnidade_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fUnidade = new frmUnidade();

            fUnidade.MdiParent = this;
            fUnidade.Dock = DockStyle.Fill;

            fUnidade.Show();
        }

        private void tbUnidadeConversao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fConversaoUnidade = new frmConversaoUnidade();

            fConversaoUnidade.MdiParent = this;
            fConversaoUnidade.Dock = DockStyle.Fill;

            fConversaoUnidade.Show();
        }

        private void btTipoMovimentacao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fTipoMovimentacao = new frmTipoMovimentacao();

            fTipoMovimentacao.MdiParent = this;
            fTipoMovimentacao.Dock = DockStyle.Fill;

            fTipoMovimentacao.Show();
        }

        private void tbMovimentacao_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fMovimentacao = new frmMovimentacao();

            fMovimentacao.MdiParent = this;
            fMovimentacao.Dock = DockStyle.Fill;

            fMovimentacao.Show();
        }

        private void tbInventario_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            fInventario = new frmInventario();

            fInventario.MdiParent = this;
            fInventario.Dock = DockStyle.Fill;

            fInventario.Show();
        }

        private void btnRelEstoqueAtual_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                FecharJanelas();

                fRelatorioEstoqueAtual = new frmRelatorioEstoqueAtual();

                fRelatorioEstoqueAtual.MdiParent = this;
                fRelatorioEstoqueAtual.Dock = DockStyle.Fill;

                fRelatorioEstoqueAtual.Show();
            }
            else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnRelHistoricoMovimentacoes_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                FecharJanelas();

                fRelatorioHistoricoMovimentacoes = new frmRelatorioMovimentacoes();

                fRelatorioHistoricoMovimentacoes.MdiParent = this;
                fRelatorioHistoricoMovimentacoes.Dock = DockStyle.Fill;

                fRelatorioHistoricoMovimentacoes.Show();
            }
            else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnRelValorizacaoEstoque_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                FecharJanelas();

                fRelatorioValorizacaoEstoque = new frmRelatorioValorizacaoEstoque();

                fRelatorioValorizacaoEstoque.MdiParent = this;
                fRelatorioValorizacaoEstoque.Dock = DockStyle.Fill;

                fRelatorioValorizacaoEstoque.Show();
            }
            else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnTaxaEntrega_Click(object sender, EventArgs e)
        {
            FecharJanelas();
            fTaxaEntrega = new frmTaxaEntrega();
            fTaxaEntrega.MdiParent = this;
            fTaxaEntrega.Dock = DockStyle.Fill;

            fTaxaEntrega.Show();
        }

        private void btnEntregadores_Click(object sender, EventArgs e)
        {
            FecharJanelas();
            fEntregador = new frmEntregador();
            fEntregador.MdiParent = this;
            fEntregador.Dock = DockStyle.Fill;

            fEntregador.Show();
        }

        private void btnDashboardGeral_Click(object sender, EventArgs e)
        {
            if (!MdiChildren.Contains(fDashboard))
            {
                btnDashboardGeral.Enabled = false;
                FecharJanelas();
                fDashboard = new frmDashboard();

                fDashboard.AlterarAtivado = (ativado) =>
                {
                    if (InvokeRequired)
                        BeginInvoke(new Action(() =>
                        {
                            btnDashboardGeral.Enabled = ativado;
                        }));
                    else
                        btnDashboardGeral.Enabled = ativado;
                };
                fDashboard.MdiParent = this;
                fDashboard.Dock = DockStyle.Fill;

                fDashboard.Show();
            }
            else
            {
                fDashboard.IniciarDashboard();
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CefSharp.Cef.IsInitialized)
                CefSharp.Cef.Shutdown();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btnMotivoCancelamento_Click(object sender, EventArgs e)
        {
            FecharJanelas();
            fMotivoCancelamento = new frmMotivoCancelamento();

            fMotivoCancelamento.MdiParent = this;
            fMotivoCancelamento.Dock = DockStyle.Fill;
            fMotivoCancelamento.Show();
        }

        private void btnRelatorios_Click_2(object sender, EventArgs e)
        {
            FecharJanelas();

            fRelatoriosGerais = new frmRelatoriosGerais();
            fRelatoriosGerais.MdiParent = this;
            fRelatoriosGerais.Dock = DockStyle.Fill;

            fRelatoriosGerais.Show();
        }

        private void btnProduto_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            Button btn = (Button)sender;
            ETipoProduto tipo;
            if (btn.Name == btnItens.Name)
                tipo = ETipoProduto.Item;
            else if (btn.Name == btnModificacoes.Name)
                tipo = ETipoProduto.Modificacao;
            else if (btn.Name == btnIngredientes.Name)
                tipo = ETipoProduto.Ingrediente;
            else if (btn.Name == btnCreditos.Name)
                tipo = ETipoProduto.Credito;
            else
                throw new Exception("Botão desconhecido");

            fProduto = new frmProduto(tipo)
            {
                MdiParent = this,
                Dock = DockStyle.Fill
            };

            fProduto.Show();
        }
    }
}
