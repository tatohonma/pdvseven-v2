using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.BLL.Utils;
using a7D.PDV.BLL.Extension;
using System.Threading.Tasks;
using a7D.PDV.Componentes;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmProdutoEditar : Form
    {
        private ProdutoInformation Produto1;
        private bool BuscaInicializada { get; set; }
        private List<ProdutoInformation> Produtos { get; set; }
        private List<ProdutoInformation> ProdutosRemovidos { get; set; }
        private UnidadeInformation unidadeAnterior;
        private ETipoProduto TipoProduto { get; }

        private List<CategoriaProdutoInformation> CategoriasProduto { get; set; } = new List<CategoriaProdutoInformation>();
        private List<CategoriaProdutoInformation> CategoriasSelecionadas { get; set; } = new List<CategoriaProdutoInformation>();

        private List<PainelModificacaoInformation> PaineisModificacao { get; set; } = new List<PainelModificacaoInformation>();
        private List<PainelModificacaoInformation> PaineisSelecionados { get; set; } = new List<PainelModificacaoInformation>();

        private List<AreaImpressaoInformation> AreasImpressao { get; set; } = new List<AreaImpressaoInformation>();
        private List<MapAreaImpressaoProdutoInformation> AreasMapeadas { get; set; } = new List<MapAreaImpressaoProdutoInformation>();

        private bool ReceitaCarregada { get; set; } = false;

        private string Titulo
        {
            get
            {
                var titulo = string.Empty;

                if (string.IsNullOrWhiteSpace(Produto1?.Nome))
                    titulo += "** NOVO ** - ";
                else
                    titulo += $"{Produto1.Nome} - ";

                switch (TipoProduto)
                {
                    case ETipoProduto.Item:
                        titulo += "ITEM";
                        break;
                    case ETipoProduto.Modificacao:
                        titulo += "MODIFICAÇÃO";
                        break;
                    case ETipoProduto.Servico:
                        titulo += "SERVIÇO";
                        break;
                    case ETipoProduto.Ingrediente:
                        titulo += "INGREDIENTE";
                        break;
                }

                return titulo;
            }
        }

        private bool UnidadeAlterada
        {
            get
            {
                return unidadeAnterior != cbbUnidade.SelectedItem as UnidadeInformation;
            }
        }

        private frmProdutoEditar()
        {
            InitializeComponent();
        }

        internal frmProdutoEditar(List<ProdutoInformation> produtos, ETipoProduto tipoProduto) : this()
        {
            TipoProduto = tipoProduto;
            Produtos = produtos;

            Produto1 = new ProdutoInformation();
            Produto1.ControlarEstoque = false;
            //Produto1.ProdutoImposto = new ProdutoImpostoInformation();
            Produto1.ListaPainelModificacao = new List<ProdutoPainelModificacaoInformation>();
            Produto1.ListaProdutoCategoria = new List<ProdutoCategoriaProdutoInformation>();
            Produto1.ListaProdutoTraducao = new List<ProdutoTraducaoInformation>();
            Produto1.ListaProdutoReceita = new List<ProdutoReceitaInformation>();
            Produto1.Unidade = new UnidadeInformation();
            unidadeAnterior = Produto1.Unidade;

            Produto1.DtUltimaAlteracao = DateTime.Now;
            Produto1.Disponibilidade = true;
            Produto1.DtAlteracaoDisponibilidade = DateTime.Now;

            lblValor2.Visible = lblValor3.Visible = txtValorUnitario2.Visible = txtValorUnitario3.Visible = ConfiguracoesSistema.Valores.PainelModificacaoAvancado;
            ckbAssistenteModificacoes.Visible = TipoProduto == ETipoProduto.Item && ConfiguracoesSistema.Valores.PainelModificacaoAvancado;

        }

        internal frmProdutoEditar(int idProduto, List<ProdutoInformation> produtos) : this()
        {
            Produtos = produtos;
            Produto1 = Produto.CarregarCompleto(idProduto);
            TipoProduto = (ETipoProduto)Produto1.TipoProduto.IDTipoProduto.Value;
            unidadeAnterior = Produto1.Unidade;

            lblValor2.Visible =
                lblValor3.Visible =
                txtValorUnitario2.Visible =
                txtValorUnitario3.Visible = ConfiguracoesSistema.Valores.PainelModificacaoAvancado;

            ckbAssistenteModificacoes.Visible = TipoProduto == ETipoProduto.Item && ConfiguracoesSistema.Valores.PainelModificacaoAvancado;

            if (idProduto == ProdutoInformation.IDProdutoServico)
            {
                txtCodigo.Enabled =
                    txtCEAN.Enabled =
                    txtNome.Enabled =
                    txtDescricao.Enabled =
                    txtValorUnitario.Enabled =
                    txtValorUnitario2.Enabled =
                    txtValorUnitario3.Enabled =
                    txtCodigoAliquota.Enabled =
                    cbbUnidade.Enabled =
                    ckbAtivo.Enabled =
                    ckbBalanca.Enabled =
                    ckbControlarEstoque.Enabled =
                    ckbCobrarTaxa.Enabled =
                    false;
            }
            else if (TipoProduto == ETipoProduto.Credito)
            {
                ckbControlarEstoque.Enabled =
                    ckbBalanca.Enabled =
                    false;
            }
        }

        private void CarregarListas()
        {
            List<CategoriaProdutoInformation> listaCategoriaProduto = CategoriaProduto.Listar().OrderBy(l => l.Nome).ToList();
            CategoriasProduto.AddRange(listaCategoriaProduto);
            cbbCategoriasProduto.ValueMember = "IDCategoriaProduto";
            cbbCategoriasProduto.DisplayMember = "Nome";
            AtualizarComboCategoria();

            List<PainelModificacaoInformation> listaPainelModificacao = PainelModificacao.Listar().OrderBy(l => l.Nome).ToList();
            PaineisModificacao.AddRange(listaPainelModificacao);
            AtualizarComboPainelModificacao();

            List<ClassificacaoFiscalInformation> listaClassificacaoFiscal = ClassificacaoFiscal.Listar().OrderBy(l => l.Nome).ToList();
            listaClassificacaoFiscal.Insert(0, new ClassificacaoFiscalInformation());
            cbbClassificacaoFiscal.DataSource = listaClassificacaoFiscal;
            cbbClassificacaoFiscal.DisplayMember = "Nome";
            cbbClassificacaoFiscal.ValueMember = "IDClassificacaoFiscal";

            List<IdiomaInformation> listaIdioma = Idioma.Listar();
            listaIdioma.Insert(0, new IdiomaInformation());
            cbbIdioma.DataSource = listaIdioma;
            cbbIdioma.DisplayMember = "Nome";
            cbbIdioma.ValueMember = "IDIdioma";

            var areas = AreaImpressao.ListarSomenteProducao();
            AreasImpressao.AddRange(areas);
            AreasMapeadas = MapAreaImpressaoProduto.ListarPorProduto(Produto1.IDProduto);
            AtualizarComboAreaImpressao();

            AtualizarComboTags();
        }

        private void AtualizarComboAreaImpressao()
        {
            var listaAreaImpressao = new List<AreaImpressaoInformation>();
            listaAreaImpressao.AddRange(AreasImpressao.Where(ai => !AreasMapeadas.Where(asl => asl.Status != StatusModel.Excluido).Any(asl => asl.AreaImpressao?.IDAreaImpressao == ai.IDAreaImpressao)).ToList());
            listaAreaImpressao = listaAreaImpressao.OrderBy(ai => ai.Nome).ToList();
            listaAreaImpressao.Insert(0, new AreaImpressaoInformation());
            cbbAreaProducao.DataSource = listaAreaImpressao;
        }

        private void AtualizarComboCategoria()
        {
            var listaCategoriaProduto = new List<CategoriaProdutoInformation>();
            listaCategoriaProduto.AddRange(CategoriasProduto);
            listaCategoriaProduto = listaCategoriaProduto.OrderBy(c => c.Nome).ToList();
            listaCategoriaProduto.Insert(0, new CategoriaProdutoInformation());
            cbbCategoriasProduto.DataSource = listaCategoriaProduto;
        }

        private void AtualizarComboPainelModificacao()
        {
            var listaPaineisModificacao = new List<PainelModificacaoInformation>();
            listaPaineisModificacao.AddRange(PaineisModificacao);
            listaPaineisModificacao = listaPaineisModificacao.OrderBy(pm => pm.Nome).ToList();
            listaPaineisModificacao.Insert(0, new PainelModificacaoInformation());
            cbbPainelModificacao.DataSource = listaPaineisModificacao;
        }

        private void AtualizarComboTags()
        {
            var listaTag = BLL.Tag.ListarChaves();

            TagInformation novaTag = new TagInformation();
            novaTag.Chave = "criar nova tag";

            listaTag.Insert(0, "");
            listaTag.Add("criar nova tag");

            cbbTagChave.DataSource = listaTag;            
        }

        private void frmProdutoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Text = Titulo;
            Refresh();

            CarregarListas();

            if (TipoProduto != ETipoProduto.Item)
            {
                if (TipoProduto != ETipoProduto.Credito)
                    tabControl1.TabPages.Remove(tabCategorias);

                tabControl1.TabPages.Remove(tabModificacoes);
                tabControl1.TabPages.Remove(tabImagem);

                if (TipoProduto != ETipoProduto.Modificacao)
                {
                    //tabControl1.TabPages.Remove(tabReceita);
                    tabControl1.TabPages.Remove(tabAreaProducao);
                }
            }

            if (ConfiguracoesSistema.Valores.Fiscal == "SAT")
            {
                lblCodigoAliquota.Visible = false;
                txtCodigoAliquota.Visible = false;
            }

            if (Produto1.ClassificacaoFiscal != null)
                cbbClassificacaoFiscal.SelectedValue = Produto1.ClassificacaoFiscal.IDClassificacaoFiscal.Value;

            lblValor.Text = ConfiguracoesSistema.Valores.PainelModificacaoAvancado ? "Valor 1 (Padrão)" : "Valor unitário";

            if (!string.IsNullOrWhiteSpace(Produto1.CodigoAliquota))
                txtCodigoAliquota.Text = Produto1.CodigoAliquota;

            if (!BLL.PDV.PossuiTema())
            {
                if (tabControl1.TabPages.Contains(tabImagem))
                    tabControl1.TabPages.Remove(tabImagem);
                tabControl1.TabPages.Remove(tabTraducao);
            }
            else
                AtualizarListaTraducoes();

            var controlarEstoque = Produto1.ControlarEstoque.Value;
            ckbControlarEstoque.Checked = controlarEstoque;

            if (BLL.PDV.PossuiEstoque())
            {
                var unidades = Unidade.ListarAtivos();
                cbbUnidade.DisplayMember = "Nome";
                cbbUnidade.ValueMember = "IDUnidade";
                cbbUnidade.DataSource = unidades;

                if (Produto1.Unidade != null && Produto1.Unidade.IDUnidade.HasValue)
                    cbbUnidade.SelectedValue = Produto1.Unidade.IDUnidade.Value;

                CarregarReceita();
            }
            else
            {
                tabControl1.TabPages.Remove(tabReceita);
                lblControleUnidadeEstoque.Enabled = false;
                ckbControlarEstoque.Enabled = false;

                cbbUnidade.Enabled = false;
            }

            if (Produto1.IDProduto != null)
            {
                Produto1.ListaProdutoCategoria = ProdutoCategoriaProduto.ListarPorProdutoCompleto(Produto1.IDProduto.Value).ToList();

                txtNome.Text = Produto1.Nome;
                txtDescricao.Text = Produto1.Descricao;
                txtCodigo.Text = Produto1.Codigo;
                //txtCodigoERP.Text = Produto1.CodigoERP;
                txtCEAN.Text = Produto1.cEAN;
                ckbBalanca.Checked = Produto1.UtilizarBalanca == true;
                ckbAtivo.Checked = Produto1.Ativo == true;
                ckbAssistenteModificacoes.Checked = Produto1.AssistenteModificacoes == true;

                if (Produto1.ValorUnitario != null)
                    txtValorUnitario.Text = Produto1.ValorUnitario.Value.ToString("#,##0.00");

                if (Produto1.ValorUnitario2 != null)
                    txtValorUnitario2.Text = Produto1.ValorUnitario2.Value.ToString("#,##0.00");

                if (Produto1.ValorUnitario3 != null)
                    txtValorUnitario3.Text = Produto1.ValorUnitario3.Value.ToString("#,##0.00");

                if (Produto1.EstoqueIdeal != null)
                    txtEstoqueIdeal.Text = Produto1.EstoqueIdeal.Value.ToString();

                if (Produto1.EstoqueMinimo != null)
                    txtEstoqueMinimo.Text = Produto1.EstoqueMinimo.Value.ToString();

                //Paineis Modificação
                AtualizarListaPainelModificacao();
                AtualizarListaCategoria();
                AtualizarListaAreaImpressao();
                AtualizarListaTags();

                //Imagem
                CarregarImagem();

                if (!ReceitaCarregada)
                {
                    pnlReceita.Visible = true;
                    lblMsgSalvarReceita.Visible = false;
                    CarregarReceita();
                }

                if (!String.IsNullOrEmpty(Produto1.GUIDIdentificacao))
                {
                    pnlTags.Visible = true;
                    lblMsgSalvarTags.Visible = false;
                }

                pnlReceita.Visible = true;
                lblMsgSalvarReceita.Visible = false;

                Task.Run(() => ObterEstoque());
            }

            txtNome.Focus();
        }

        private void AtualizarListaTags()
        {
            if (String.IsNullOrEmpty(Produto1.GUIDIdentificacao))
                return;

            List<TagInformation> listaTag = BLL.Tag.Listar(Produto1.GUIDIdentificacao);

            dgvTags.AutoGenerateColumns = false;
            dgvTags.SuspendLayout();
            dgvTags.DataSource = listaTag.ToArray();
            dgvTags.ResumeLayout();
            dgvTags.ClearSelection();
        }

        void ObterEstoque()
        {
            var qtd = EntradaSaida.EstoqueAtual(Produto1.IDProduto.Value);
            UpdateEstoqueText(qtd.ToString());
        }


        void UpdateEstoqueText(string estoque)
        {
            if (InvokeRequired)
                Invoke(new UpdateTextDelegate(UpdateEstoqueText), estoque);
            else
                txtEstoqueAtual.Text = estoque;
        }

        private void CarregarReceita()
        {
            var lista = Produto1
                .ListaProdutoReceita
                .Where(pr => pr.StatusModel != StatusModel.Excluido)
                .ToList();

            dgvReceita.SuspendLayout();
            foreach (var item in lista)
            {
                var linha = new object[] { null, item.IDProdutoReceita, item.ProdutoIngrediente.Nome, null, item.Quantidade };

                dgvReceita.Rows.Add(linha);
                var unidade = item.Unidade;
                var cb = dgvReceita.Rows[dgvReceita.Rows.Count - 1].Cells["colUnidade"] as DataGridViewComboBoxCell;

                if (cb != null && unidade != null)
                {
                    var unidades = ConversaoUnidade.ListarUnidadesConversaoPara(item.ProdutoIngrediente.Unidade);
                    unidades.Insert(0, Unidade.Carregar(item.ProdutoIngrediente.Unidade.IDUnidade.Value));
                    if (unidades.Any(u => u.IDUnidade == item.Unidade.IDUnidade.Value) == false)
                        unidades.Insert(1, Unidade.Carregar(item.Unidade.IDUnidade.Value));

                    cb.DisplayMember = "Nome";
                    cb.ValueMember = "IDUnidade";
                    cb.DataSource = unidades;
                    try
                    {
                        cb.Value = unidade.IDUnidade.Value;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            dgvReceita.ResumeLayout();
            dgvReceita.CellValueChanged += dgvReceita_CellValueChanged;
            ReceitaCarregada = true;
        }

        private void AtualizarListaTraducoes()
        {
            var lista = from l in Produto1.ListaProdutoTraducao
                        where l.StatusModel != StatusModel.Excluido
                        orderby l.Idioma.Nome
                        select new { l.IDProdutoTraducao.Value, Idioma = l.Idioma.Nome, l.Nome, l.Descricao };

            dgvTraducao.SuspendLayout();
            dgvTraducao.DataSource = lista.ToArray();
            dgvTraducao.ResumeLayout();

            dgvTraducao.ClearSelection();
        }

        private void AtualizarListaAreaImpressao()
        {
            var lista = from l in AreasMapeadas
                        where l.Status != StatusModel.Excluido
                        select new { l.IDMapAreaImpressaoProduto, l.AreaImpressao.IDAreaImpressao, l.AreaImpressao.Nome };

            dgvAreaProducao.SuspendLayout();
            dgvAreaProducao.DataSource = lista.ToArray();
            dgvAreaProducao.ResumeLayout();
            dgvAreaProducao.ClearSelection();
            AtualizarComboAreaImpressao();
        }

        private void AtualizarListaPainelModificacao()
        {
            PaineisSelecionados.Clear();
            PaineisSelecionados.AddRange(Produto1.ListaPainelModificacao.Where(pm => pm.StatusModel != StatusModel.Excluido).Select(pmp => pmp.PainelModificacao));
            PaineisModificacao = PaineisModificacao.Where(pm => !PaineisSelecionados.Any(ps => ps.IDPainelModificacao == pm.IDPainelModificacao)).ToList();
            var lista = from l in Produto1.ListaPainelModificacao
                        orderby l.Ordem
                        select new { l.IDProdutoPainelModificacao, IDPainelModificacao = l.PainelModificacao.IDPainelModificacao.Value, l.PainelModificacao.Nome };
            dgvPainelModificacao.SuspendLayout();
            dgvPainelModificacao.DataSource = lista.ToArray();
            dgvPainelModificacao.ResumeLayout();
            dgvPainelModificacao.ClearSelection();

            AtualizarComboPainelModificacao();
        }

        private void AtualizarListaCategoria()
        {
            CategoriasSelecionadas.Clear();
            CategoriasSelecionadas.AddRange(Produto1.ListaProdutoCategoria.Where(pcp => pcp.StatusModel != StatusModel.Excluido).Select(pcp => pcp.CategoriaProduto).DistinctBy(c => c.IDCategoriaProduto));
            CategoriasProduto = CategoriasProduto.Where(c => !CategoriasSelecionadas.Any(cs => cs.IDCategoriaProduto == c.IDCategoriaProduto)).ToList();

            var lista = Produto1.ListaProdutoCategoria
                .Where(l => l.StatusModel != StatusModel.Excluido)
                .Select(l => new { l.IDProdutoCategoriaProduto.Value, IDCategoriaProduto = l.CategoriaProduto.IDCategoriaProduto.Value, l.CategoriaProduto.Nome })
                .ToList();

            lista = lista.OrderBy(l => l.Value).ToList();

            dgvCategoriasProduto.SuspendLayout();
            dgvCategoriasProduto.DataSource = lista.ToArray();
            dgvCategoriasProduto.ResumeLayout();
            dgvCategoriasProduto.ClearSelection();
            AtualizarComboCategoria();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();

            Produto1 = Produto.CarregarCompleto(Produto1.IDProduto.Value);
        }

        private void Salvar(bool fechar = false)
        {
            if (Validar() == true)
            {
                Produto1.TipoProduto = new TipoProdutoInformation() { IDTipoProduto = (int)TipoProduto };

                if (BLL.PDV.PossuiEstoque())
                    Produto1.Unidade = cbbUnidade.SelectedItem as UnidadeInformation;
                else
                    Produto1.Unidade = Unidade.Carregar(1);

                Produto1.Nome = txtNome.Text;
                Produto1.Descricao = txtDescricao.Text;
                Produto1.Codigo = txtCodigo.Text;
                //Produto1.CodigoERP = txtCodigoERP.Text;
                Produto1.CodigoAliquota = txtCodigoAliquota.Text;
                Produto1.Ativo = ckbAtivo.Checked;
                Produto1.AssistenteModificacoes = ckbAssistenteModificacoes.Checked;
                Produto1.ControlarEstoque = ckbControlarEstoque.Checked;
                Produto1.UtilizarBalanca = ckbBalanca.Checked;
                var idClassificacaoFiscal = Convert.ToInt32(cbbClassificacaoFiscal.SelectedValue);

                if (idClassificacaoFiscal > 0)
                    Produto1.ClassificacaoFiscal = ClassificacaoFiscal.Carregar(idClassificacaoFiscal);
                else
                    Produto1.ClassificacaoFiscal = null;

                Produto1.cEAN = txtCEAN.Text;

                if (txtValorUnitario.Text != "")
                    Produto1.ValorUnitario = Convert.ToDecimal(txtValorUnitario.Text);
                else
                    Produto1.ValorUnitario = null;

                if (txtValorUnitario2.Text != "")
                    Produto1.ValorUnitario2 = Convert.ToDecimal(txtValorUnitario2.Text);
                else
                    Produto1.ValorUnitario2 = null;

                if (txtValorUnitario3.Text != "")
                    Produto1.ValorUnitario3 = Convert.ToDecimal(txtValorUnitario3.Text);
                else
                    Produto1.ValorUnitario3 = null;

                if (txtEstoqueMinimo.Text != "")
                    Produto1.EstoqueMinimo = Convert.ToDecimal(txtEstoqueMinimo.Text);
                else
                    Produto1.EstoqueMinimo = null;

                if (txtEstoqueIdeal.Text != "")
                    Produto1.EstoqueIdeal = Convert.ToDecimal(txtEstoqueIdeal.Text);
                else
                    Produto1.EstoqueIdeal = null;

                try
                {
                    Produto.SalvarCompleto(Produto1);

                    foreach (var map in AreasMapeadas)
                    {
                        switch (map.Status)
                        {
                            case StatusModel.Inalterado:
                                break;
                            case StatusModel.Novo:
                                MapAreaImpressaoProduto.Adicionar(map);
                                break;
                            case StatusModel.Alterado:
                                MapAreaImpressaoProduto.Salvar(map);
                                break;
                            case StatusModel.Excluido:
                                MapAreaImpressaoProduto.Excluir(map.AreaImpressao.IDAreaImpressao.Value, Produto1.IDProduto.Value);
                                break;
                            default:
                                break;
                        }
                    }

                    AreasMapeadas = MapAreaImpressaoProduto.ListarPorProduto(Produto1.IDProduto);
                    AtualizarListaAreaImpressao();

                    if (fechar)
                        Close();
                    else
                    {
                        if (!ReceitaCarregada)
                        {
                            pnlReceita.Visible = true;
                            lblMsgSalvarReceita.Visible = false;
                            CarregarReceita();
                        }

                        if(!String.IsNullOrEmpty(Produto1.GUIDIdentificacao))
                        {
                            pnlTags.Visible = true;
                            lblMsgSalvarTags.Visible = false;
                            AtualizarListaTags();
                        }

                        pnlReceita.Visible = true;
                        lblMsgSalvarReceita.Visible = false;

                        Text = Titulo;
                        Refresh();
                    }
                }
                catch (Exception ex)
                {
                    BLL.Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (Produto1.ExisteMovimentacao() && UnidadeAlterada)
                msg += "A \"Unidade\" não pode ser alterada porque o produto já possui movimentações.";

            if (ConfiguracoesSistema.Valores.Fiscal == "SAT" && cbbClassificacaoFiscal.SelectedIndex == 0)
                msg += "O campo Classificação Fiscal na aba Impostos (SAT) deve ser preenchido.\n";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            if (Decimal.TryParse(txtValorUnitario.Text, out decimal d1) == false || !Produto.ValorValido(d1))
                msg += "Campo \"Valor unitário\" deve ser numérico entre 0 e 1000000. \n";

            if (!string.IsNullOrEmpty(txtValorUnitario3.Text) && (!Decimal.TryParse(txtValorUnitario2.Text, out decimal d2) || !Produto.ValorValido(d2)))
                msg += "Campo \"Valor 2\" deve ser numérico entre 0 e 1000000. \n";

            if (!string.IsNullOrEmpty(txtValorUnitario3.Text) && (!Decimal.TryParse(txtValorUnitario3.Text, out decimal d3) || !Produto.ValorValido(d3)))
                msg += "Campo \"Valor 3\" deve ser numérico entre 0 1000000. \n";

            if (ConfiguracoesSistema.Valores.PainelModificacaoAvancado
             && ckbAssistenteModificacoes.Checked && Produto1.ListaPainelModificacao.Count == 0)
                msg += "Não há modificações para que seja habilitado o assistente de modificações\n";

            if (txtNome.Text.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido no nome do produto, verifique os acentos, simbolos e espaços em branco";

            if (txtDescricao.Text.TemCaracteresEspeciais())
                msg += "Há algum caracter não permitido na descrição do produto, verifique os acentos, simbolos e espaços em branco";

            if (!string.IsNullOrEmpty(txtEstoqueMinimo.Text) && (!Decimal.TryParse(txtEstoqueMinimo.Text, out decimal e1) || e1 < 0))
                msg += "Campo \"Estoque mínimo\" deve ser numérico.\n";

            if (!string.IsNullOrEmpty(txtEstoqueIdeal.Text) && (!Decimal.TryParse(txtEstoqueIdeal.Text, out decimal e2) || e2 < 0))
                msg += "Campo \"Estoque ideal\" deve ser numérico.\n";

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdicionarPainel_Click(object sender, EventArgs e)
        {
            if (cbbPainelModificacao.SelectedIndex > 0)
            {
                Int32 idProdutoPainelModificacao = -1;
                Int32 ordem = 0;

                if (Produto1.ListaPainelModificacao.Count > 0)
                {
                    idProdutoPainelModificacao = Produto1.ListaPainelModificacao.Min(l => l.IDProdutoPainelModificacao).Value - 1;
                    ordem = Produto1.ListaPainelModificacao.Max(l => l.Ordem).Value + 2;
                }

                Int32 idPainelModificacao = Convert.ToInt32(cbbPainelModificacao.SelectedValue);
                PainelModificacaoInformation painelModificacao = PainelModificacao.CarregarCompleto(idPainelModificacao);

                ProdutoPainelModificacaoInformation produtoPainelModificacao = new ProdutoPainelModificacaoInformation();
                produtoPainelModificacao.IDProdutoPainelModificacao = idProdutoPainelModificacao;
                produtoPainelModificacao.Ordem = ordem;
                produtoPainelModificacao.PainelModificacao = painelModificacao;

                Produto1.ListaPainelModificacao.Add(produtoPainelModificacao);
                AtualizarListaPainelModificacao();
            }
        }

        private void dgvPainelModificacao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 idProdutoPainelModificacao;
            ProdutoPainelModificacaoInformation produtoPainelModificacao;
            Int32 i = 0;

            if (e.RowIndex >= 0)
            {
                switch (dgvPainelModificacao.Columns[e.ColumnIndex].Name)
                {
                    case "Subir":
                        idProdutoPainelModificacao = Convert.ToInt32(dgvPainelModificacao["IDProdutoPainelModificacao", e.RowIndex].Value);
                        produtoPainelModificacao = Produto1.ListaPainelModificacao.First(l => l.IDProdutoPainelModificacao == idProdutoPainelModificacao);
                        produtoPainelModificacao.Ordem = produtoPainelModificacao.Ordem.Value - 3;

                        foreach (var item in Produto1.ListaPainelModificacao.OrderBy(l => l.Ordem).ToList())
                        {
                            item.Ordem = i;
                            i = i + 2;
                        }

                        AtualizarListaPainelModificacao();

                        break;

                    case "Descer":
                        idProdutoPainelModificacao = Convert.ToInt32(dgvPainelModificacao["IDProdutoPainelModificacao", e.RowIndex].Value);
                        produtoPainelModificacao = Produto1.ListaPainelModificacao.First(l => l.IDProdutoPainelModificacao == idProdutoPainelModificacao);
                        produtoPainelModificacao.Ordem = produtoPainelModificacao.Ordem.Value + 3;

                        foreach (var item in Produto1.ListaPainelModificacao.OrderBy(l => l.Ordem).ToList())
                        {
                            item.Ordem = i;
                            i = i + 2;
                        }

                        AtualizarListaPainelModificacao();

                        break;

                    case "Remover":
                        idProdutoPainelModificacao = Convert.ToInt32(dgvPainelModificacao["IDProdutoPainelModificacao", e.RowIndex].Value);
                        var idPainelModificacao = Convert.ToInt32(dgvPainelModificacao[nameof(colIDPainelModificacao), e.RowIndex].Value);
                        produtoPainelModificacao = Produto1.ListaPainelModificacao.First(l => l.IDProdutoPainelModificacao == idProdutoPainelModificacao);
                        Produto1.ListaPainelModificacao.Remove(produtoPainelModificacao);

                        PaineisModificacao.Add(PaineisSelecionados.First(ps => ps.IDPainelModificacao == idPainelModificacao));

                        AtualizarListaPainelModificacao();

                        break;
                }
            }
        }

        private void btnSelecionarImagem_Click(object sender, EventArgs e)
        {
            //string imagemBase;
            //String imagemOriginal;
            //String imagemAjustada;

            //picImagemProduto.;

            if (Produto1.IDProduto == null)
            {
                MessageBox.Show("Você deve salvar o produto antes de carregar a imagem!");
                return;
            }
            //else if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    imagemOriginal = openFileDialog1.FileName;

            //    imagemBase = "ImagensProdutos/baseProduto.png";
            //    imagemAjustada = "ImagensProdutos/" + Produto1.IDProduto.ToString() + ".png";
            //    using (var img = Imagem.AjustarImagem(imagemBase, imagemOriginal, imagemAjustada))
            //    {

            //        var pi = ProdutoImagem.CarregarPorProduto(Produto1.IDProduto.Value);
            //        if (pi.IDProdutoImagem.HasValue == false)
            //        {
            //            pi = new ProdutoImagemInformation { Produto = Produto1 };
            //            pi.Imagem = new ImagemInformation();
            //        }

            //        string extension = System.IO.Path.GetExtension(imagemBase).ToLower().Replace(".", "");

            //        var dados = new ConversorImagemParaByteArray(img).Dados;
            //        pi.Imagem.Dados = dados;
            //        pi.Imagem.Altura = img.Height;
            //        pi.Imagem.Largura = img.Width;
            //        pi.Imagem.Tamanho = dados.Length;
            //        pi.Imagem.Nome = Produto1.Nome;
            //        pi.Imagem.Extensao = extension;

            //        Imagem.Salvar(pi.Imagem);

            //        ProdutoImagem.Salvar(pi);

            //        //imagemBase = "ImagensProdutos/baseProdutoThumb.png";
            //        //imagemAjustada = "ImagensProdutos/" + Produto1.IDProduto.ToString() + "_thumb.png";
            //        //Imagem.AjustarImagem(imagemBase, imagemOriginal, imagemAjustada);

            //        Produto1.DtUltimaAlteracao = DateTime.Now;
            //        Produto.Salvar(Produto1);
            //    }
            //}
            else if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                var imagemBase = openFileDialog1.FileName;
                using (var img = Image.FromFile(openFileDialog1.FileName))
                {
                    string extensao = System.IO.Path.GetExtension(imagemBase).ToLower().Replace(".", "");
                    using (var bitmap = ImageUtil.RedimensionarCortandoCentralizado(img))
                    {
                        var pi = ProdutoImagem.CarregarPorProduto(Produto1.IDProduto.Value);
                        if (pi.IDProdutoImagem.HasValue == false)
                        {
                            pi = new ProdutoImagemInformation { Produto = Produto1 };
                            pi.Imagem = new ImagemInformation();
                        }


                        var dados = new ConversorImagemParaByteArray(bitmap).Dados;
                        pi.Imagem.Dados = dados;
                        pi.Imagem.Altura = bitmap.Height;
                        pi.Imagem.Largura = bitmap.Width;
                        pi.Imagem.Tamanho = dados.Length;
                        pi.Imagem.Nome = Produto1.Nome;
                        pi.Imagem.Extensao = extensao;

                        Imagem.Salvar(pi.Imagem);

                        ProdutoImagem.Salvar(pi);

                        //imagemBase = "ImagensProdutos/baseProdutoThumb.png";
                        //imagemAjustada = "ImagensProdutos/" + Produto1.IDProduto.ToString() + "_thumb.png";
                        //Imagem.AjustarImagem(imagemBase, imagemOriginal, imagemAjustada);

                        Produto1.DtUltimaAlteracao = DateTime.Now;
                        Produto.Salvar(Produto1);
                    }
                }
            }

            CarregarImagem();
        }

        private void CarregarImagem()
        {
            btnRemoverImagem.Enabled = false;
            var produtoImagem = ProdutoImagem.CarregarPorProduto(Produto1.IDProduto.Value);

            if (produtoImagem != null && produtoImagem.IDProdutoImagem.HasValue)
            {
                Imagem.CarregarDados(produtoImagem.Imagem);
                var imagem = new ConversorByteArrayParaImagem(produtoImagem.Imagem.Dados).Imagem;
                picImagemProduto.Image = imagem;
                btnRemoverImagem.Enabled = true;
            }

            //String imagemProduto = "ImagensProdutos/" + Produto1.IDProduto.ToString() + ".png";
            //if (File.Exists(imagemProduto) == true)
            //{
            //    picImagemProduto.ImageLocation = imagemProduto;
            //}
        }

        private void btnAdicionarCategoria_Click(object sender, EventArgs e)
        {
            if (cbbCategoriasProduto.SelectedIndex > 0)
            {
                foreach (var item in Produto1.ListaProdutoCategoria)
                {
                    if (item.CategoriaProduto.IDCategoriaProduto.Value == Convert.ToInt32(cbbCategoriasProduto.SelectedValue))
                        return;
                }

                int newID = -1;

                if (Produto1.ListaProdutoCategoria != null && Produto1.ListaProdutoCategoria.Count > 0)
                {
                    var ids = Produto1.ListaProdutoCategoria.Where(pcp => pcp.IDProdutoCategoriaProduto < 0).ToList();
                    if (ids.Count > 0)
                        newID = ids.Select(pcp => pcp.IDProdutoCategoriaProduto.Value).Min() - 1;
                }

                Int32 idPainelCategoria = Convert.ToInt32(cbbCategoriasProduto.SelectedValue);
                CategoriaProdutoInformation categoriaProduto = CategoriaProduto.Carregar(idPainelCategoria);

                ProdutoCategoriaProdutoInformation produtoCategoriaProduto = new ProdutoCategoriaProdutoInformation();
                produtoCategoriaProduto.CategoriaProduto = categoriaProduto;
                //produtoPainelModificacao.Ordem = ordem;
                produtoCategoriaProduto.Produto = Produto1;
                produtoCategoriaProduto.StatusModel = StatusModel.Novo;
                produtoCategoriaProduto.IDProdutoCategoriaProduto = newID;

                Produto1.ListaProdutoCategoria.Add(produtoCategoriaProduto);
                AtualizarListaCategoria();
            }
        }

        private void dgvCategoriasProduto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                switch (dgvCategoriasProduto.Columns[e.ColumnIndex].Name)
                {
                    case "RemoverCategoria":
                        var idProdutoCategoriaProduto = Convert.ToInt32(dgvCategoriasProduto["IDProdutoCategoriaProduto", e.RowIndex].Value);
                        var idCategoriaProduto = Convert.ToInt32(dgvCategoriasProduto[nameof(colIDCategoriaProduto), e.RowIndex].Value);
                        var produtoCategoriaProduto = Produto1.ListaProdutoCategoria.FirstOrDefault(c => c.IDProdutoCategoriaProduto == idProdutoCategoriaProduto);
                        if (produtoCategoriaProduto != null)
                            produtoCategoriaProduto.StatusModel = StatusModel.Excluido;
                        CategoriasProduto.Add(CategoriasSelecionadas.First(c => c.IDCategoriaProduto == idCategoriaProduto));
                        AtualizarListaCategoria();
                        break;
                }
            }
        }

        private void dgvTraducao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var idProdutoTraducao = Convert.ToInt32(dgvTraducao["IDProdutoTraducao", e.RowIndex].Value);
                var produtoTraducao = Produto1.ListaProdutoTraducao.First(pt => pt.IDProdutoTraducao.Value == idProdutoTraducao);
                switch (dgvTraducao.Columns[e.ColumnIndex].Name)
                {
                    case "RemoverTraducao":
                        produtoTraducao.StatusModel = StatusModel.Excluido;
                        AtualizarListaTraducoes();
                        break;

                    case "EditarTraducao":
                        produtoTraducao.Produto = Produto1;
                        var frmProdutoTraducao = new frmProdutoTraducao(produtoTraducao);
                        frmProdutoTraducao.ShowDialog();
                        if (frmProdutoTraducao.Cancelado == false)
                            AtualizarListaTraducoes();
                        break;
                }
            }
        }

        private void btnAdicionarTraducao_Click(object sender, EventArgs e)
        {

            if (cbbIdioma.SelectedIndex > 0)
            {
                if (Produto1.ListaProdutoTraducao != null && Produto1.ListaProdutoTraducao.Count > 0)
                {
                    if (Produto1.ListaProdutoTraducao.Where(pt => pt.StatusModel != StatusModel.Excluido).Select(pt => pt.Idioma.IDIdioma.Value).Any(id => id == cbbIdioma.SelectedIndex))
                        return;
                }

                var idiomaSelecionado = Idioma.Carregar(Convert.ToInt32(cbbIdioma.SelectedValue));
                var frmProdutoTraducao = new frmProdutoTraducao(Produto1, idiomaSelecionado);

                frmProdutoTraducao.ShowDialog();

                if (frmProdutoTraducao.Cancelado == false)
                {
                    var novaTraducao = frmProdutoTraducao.Traducao;

                    int newID = -1;
                    if (Produto1.ListaProdutoTraducao != null && Produto1.ListaProdutoTraducao.Count > 0)
                    {
                        var ids = Produto1.ListaProdutoTraducao.Where(pt => pt.IDProdutoTraducao.Value < 0).ToList();
                        if (ids.Count > 0)
                            newID = ids.Select(pt => pt.IDProdutoTraducao.Value).Min() - 1;
                    }
                    novaTraducao.IDProdutoTraducao = newID;
                    Produto1.ListaProdutoTraducao.Add(novaTraducao);
                    AtualizarListaTraducoes();
                    cbbIdioma.SelectedIndex = 0;
                }
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnRemoverImagem_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("Deseja realmente remover a imagem?\nEsta operação não pode ser desfeita.", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                btnRemoverImagem.Enabled = false;
                var pi = ProdutoImagem.CarregarPorProduto(Produto1.IDProduto.Value);
                if (pi != null)
                {
                    if (picImagemProduto.Image != null)
                    {
                        picImagemProduto.Image.Dispose();
                        picImagemProduto.Image = null;
                    }
                    ProdutoImagem.Excluir(pi);
                    Imagem.Excluir(pi.Imagem);
                    CarregarImagem();
                }
            }
        }

        private void frmProdutoEditar_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (picImagemProduto.Image != null)
                picImagemProduto.Image.Dispose();
        }

        private void salvarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pi = ProdutoImagem.CarregarPorProduto(Produto1.IDProduto.Value);
            if ((pi == null || pi.IDProdutoImagem.HasValue == false)
                || (pi.Imagem == null || pi.Imagem.IDImagem.HasValue == false))
                return;

            pi.Imagem = Imagem.CarregarCompleto(pi.Imagem.IDImagem.Value);

            var sfd = new SaveFileDialog();
            sfd.CheckPathExists = true;
            sfd.FileName = Produto1.Nome.Replace(" ", "_");
            if (sfd.FileName.Length > 50)
                sfd.FileName = sfd.FileName.Substring(0, 50);

            sfd.DefaultExt = pi.Imagem.Extensao;

            var resp = sfd.ShowDialog();

            if (resp == DialogResult.OK)
            {
                using (var img = new ConversorByteArrayParaImagem(pi.Imagem.Dados).Imagem)
                {
                    img.Save(sfd.FileName);
                }
            }
        }

        private void txtProdutoReceita_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nomeProduto = txtProdutoReceita.Text;
                var produto = Produtos.FirstOrDefault(p => p.Nome == nomeProduto);
                AdicionarProdutoReceita(produto);
                txtProdutoReceita.Text = string.Empty;
            }
        }

        private void InicializarBusca()
        {
            ProdutosRemovidos = new List<ProdutoInformation>();
            Produtos = Produtos
                            .Where(p => p.Excluido == false)
                            .Where(p => p.Ativo == true)
                            .Where(p => p.TipoProduto.IDTipoProduto.Value != 20)
                            .Where(p => p.IDProduto != Produto1.IDProduto)
                            .OrderBy(p => p.Nome)
                            .ToList();

            if (Produto1.IDProduto.HasValue)
            {
                var listaReceita = ProdutoReceita.ListarPorProduto(Produto1.IDProduto.Value);
                Produtos = Produtos.Where(p => !listaReceita
                        .Select(i => i.ProdutoIngrediente.IDProduto.Value)
                        .ToList()
                        .Any(i => i == p.IDProduto.Value))
                    .ToList();
            }

            AtualizarAutoComplete();
        }

        private void AtualizarAutoComplete()
        {
            var produtosPesquisa = Produtos.Select(p => p.Nome);
            txtProdutoReceita.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtProdutoReceita.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(produtosPesquisa.ToArray());
            txtProdutoReceita.AutoCompleteCustomSource = autoComplete;
        }

        private void AdicionarProdutoPesquisa(ProdutoInformation produto)
        {
            if (produto == null || BuscaInicializada == false)
                return;
            var produtoRemovido = ProdutosRemovidos.FirstOrDefault(p => p.IDProduto == produto.IDProduto);
            if (produtoRemovido != null)
                Produtos.Add(produtoRemovido);
            else
                Produtos.Add(produto);
            ProdutosRemovidos.Remove(produtoRemovido);
            AtualizarAutoComplete();
        }

        private void RemoverProdutoPesquisa(ProdutoInformation produto)
        {
            if (produto == null || BuscaInicializada == false)
                return;
            var produtoSelecionado = Produtos.FirstOrDefault(p => p.IDProduto == produto.IDProduto.Value);
            ProdutosRemovidos.Add(produtoSelecionado);
            Produtos.Remove(produtoSelecionado);
            AtualizarAutoComplete();
        }

        private void txtProdutoReceita_Enter(object sender, EventArgs e)
        {

        }

        private void AdicionarProdutoReceita(ProdutoInformation produto)
        {
            if (produto == null)
                return;

            produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);
            int id = -1;
            try
            {
                id = Produto1.ListaProdutoReceita.Where(pr => pr.IDProdutoReceita.Value < 0).Select(pr => pr.IDProdutoReceita).Min().Value;
                id--;
            }
            catch { }

            var linha = new object[] { null, id, produto.Nome, null, 1 };
            Produto1.ListaProdutoReceita.Add(new ProdutoReceitaInformation { IDProdutoReceita = id, Produto = Produto1, ProdutoIngrediente = produto, Quantidade = 1, StatusModel = StatusModel.Novo, Unidade = produto.Unidade });
            dgvReceita.Rows.Add(linha);
            RemoverProdutoPesquisa(produto);

            var unidade = produto.Unidade;
            var cb = dgvReceita.Rows[dgvReceita.Rows.Count - 1].Cells["colUnidade"] as DataGridViewComboBoxCell;

            if (cb != null && unidade != null)
            {
                var unidades = ConversaoUnidade.ListarUnidadesConversaoPara(unidade);
                unidades.Insert(0, unidade);
                cb.DisplayMember = "Nome";
                cb.ValueMember = "IDUnidade";
                cb.DataSource = unidades;
                cb.Value = unidade.IDUnidade.Value;
            }

        }

        private void dgvReceita_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var resp = MessageBox.Show("Deseja remover esse ingrediente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    var idProdutoReceita = dgvReceita.Rows[e.RowIndex].Cells["IDProdutoReceita"].Value as int?;
                    var produtoReceita = Produto1.ListaProdutoReceita.FirstOrDefault(pr => pr.IDProdutoReceita == idProdutoReceita.Value && pr.StatusModel != StatusModel.Excluido);
                    if (produtoReceita != null)
                    {
                        produtoReceita.StatusModel = StatusModel.Excluido;
                        AdicionarProdutoPesquisa(produtoReceita.ProdutoIngrediente);
                    }

                    dgvReceita.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.IndexOf(tabReceita) == tabControl1.SelectedIndex)
            {
                if (BuscaInicializada == false)
                {
                    BuscaInicializada = true;
                    Cursor = Cursors.WaitCursor;
                    InicializarBusca();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnProcurarProdutoReceita_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                var frm = new frmMovimentacaoTodosProdutos(Produtos);
                frm.ShowDialog();
                AdicionarProdutoReceita(frm.Produto);
            }
            else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvReceita_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var produtoReceita = ProdutoReceitaNaPosicao(e.RowIndex, e.ColumnIndex);
                var cb = dgvReceita.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                produtoReceita.StatusModel = StatusModel.Alterado;
                produtoReceita.Unidade = Unidade.Carregar(Convert.ToInt32(cb.Value));
            }
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                var produtoReceita = ProdutoReceitaNaPosicao(e.RowIndex, e.ColumnIndex);
                var quantidade = Convert.ToDecimal(dgvReceita.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                produtoReceita.StatusModel = StatusModel.Alterado;
                produtoReceita.Quantidade = quantidade;
            }
        }

        private ProdutoReceitaInformation ProdutoReceitaNaPosicao(int rowIndex, int columnIndex)
        {
            var row = dgvReceita.Rows[rowIndex];
            var idProdutoReceita = row.Cells["IDProdutoReceita"].Value as int?;
            var produtoReceita = Produto1.ListaProdutoReceita.FirstOrDefault(pr => pr.IDProdutoReceita == idProdutoReceita.Value && pr.StatusModel != StatusModel.Excluido);
            return produtoReceita;
        }

        private void btnSalvarEFechar_Click(object sender, EventArgs e)
        {
            Salvar(true);
        }

        private void AlterarTexto(GroupBox gb, int numero, string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                gb.Text = $"Integração {texto}";
            }
            else
            {
                gb.Text = $"Integração {numero}";
            }
        }

        private void cbbCategoriasProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbCategoriasProduto.SelectedIndex > 0)
            {
                foreach (var item in Produto1.ListaProdutoCategoria.Where(pcp => pcp.StatusModel != StatusModel.Excluido))
                {
                    if (item.CategoriaProduto.IDCategoriaProduto.Value == Convert.ToInt32(cbbCategoriasProduto.SelectedValue))
                        return;
                }

                int newID = -1;

                if (Produto1.ListaProdutoCategoria != null && Produto1.ListaProdutoCategoria.Count > 0)
                {
                    var ids = Produto1.ListaProdutoCategoria.Where(pcp => pcp.IDProdutoCategoriaProduto < 0).ToList();
                    if (ids.Count > 0)
                        newID = ids.Select(pcp => pcp.IDProdutoCategoriaProduto.Value).Min() - 1;
                }

                Int32 idPainelCategoria = Convert.ToInt32(cbbCategoriasProduto.SelectedValue);
                var categoriaProduto = CategoriasProduto.First(c => c.IDCategoriaProduto == idPainelCategoria);

                ProdutoCategoriaProdutoInformation produtoCategoriaProduto = new ProdutoCategoriaProdutoInformation();
                produtoCategoriaProduto.CategoriaProduto = categoriaProduto;
                //produtoPainelModificacao.Ordem = ordem;
                produtoCategoriaProduto.Produto = Produto1;
                produtoCategoriaProduto.StatusModel = StatusModel.Novo;
                produtoCategoriaProduto.IDProdutoCategoriaProduto = newID;

                Produto1.ListaProdutoCategoria.Add(produtoCategoriaProduto);
                AtualizarListaCategoria();
            }
        }

        private void cbbPainelModificacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbPainelModificacao.SelectedIndex > 0)
            {
                var idProdutoPainelModificacao = -1;
                Int32 ordem = 0;

                if (Produto1.ListaPainelModificacao.Count > 0)
                {
                    idProdutoPainelModificacao = (Produto1.ListaPainelModificacao.Where(l => l.IDProdutoPainelModificacao < 0).Min(l => l.IDProdutoPainelModificacao)) ?? -1;
                    ordem = Produto1.ListaPainelModificacao.Max(l => l.Ordem).Value + 2;
                }

                Int32 idPainelModificacao = Convert.ToInt32(cbbPainelModificacao.SelectedValue);
                PainelModificacaoInformation painelModificacao = PainelModificacao.CarregarCompleto(idPainelModificacao);

                ProdutoPainelModificacaoInformation produtoPainelModificacao = new ProdutoPainelModificacaoInformation();
                produtoPainelModificacao.IDProdutoPainelModificacao = idProdutoPainelModificacao;
                produtoPainelModificacao.Ordem = ordem;
                produtoPainelModificacao.PainelModificacao = painelModificacao;

                Produto1.ListaPainelModificacao.Add(produtoPainelModificacao);
                AtualizarListaPainelModificacao();
            }
        }

        private void dgvAreaProducao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                var idAreaImpressao = Convert.ToInt32(dgvAreaProducao[nameof(colIDAreaImpressao), e.RowIndex].Value);
                var idMapAreaImpressaoProduto = Convert.ToInt32(dgvAreaProducao[nameof(colIDMapAreaImpressaoProduto), e.RowIndex].Value);
                if (idMapAreaImpressaoProduto > 0)
                    AreasMapeadas.First(am => am.IDMapAreaImpressaoProduto == idMapAreaImpressaoProduto).Status = StatusModel.Excluido;
                else
                    AreasMapeadas = AreasMapeadas.Where(am => am.IDMapAreaImpressaoProduto != idMapAreaImpressaoProduto).ToList();
                AtualizarListaAreaImpressao();
            }
        }

        private void cbbAreaProducao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbAreaProducao.SelectedIndex > 0)
            {
                var idAreaImpressao = Convert.ToInt32(cbbAreaProducao.SelectedValue);
                if (AreasMapeadas.Any(am => am.AreaImpressao.IDAreaImpressao == idAreaImpressao && am.Status != StatusModel.Excluido))
                {
                    cbbAreaProducao.SelectedIndex = 0;
                    return;
                }

                var minId = -1;
                if (AreasMapeadas.Any(am => am.IDMapAreaImpressaoProduto < 0))
                    minId = AreasMapeadas.Min(am => am.IDMapAreaImpressaoProduto.Value) - 1;

                var areaImpressao = AreasImpressao.First(ai => ai.IDAreaImpressao == idAreaImpressao);

                AreasMapeadas.Add(new MapAreaImpressaoProdutoInformation
                {
                    IDMapAreaImpressaoProduto = minId,
                    Produto = Produto1,
                    AreaImpressao = areaImpressao,
                    Status = StatusModel.Novo
                });

                AtualizarListaAreaImpressao();
            }
        }

        private void btnTagAdicionar_Click(object sender, EventArgs e)
        {
            string chave;

            if(cbbTagChave.Visible == true)
            {
                chave = cbbTagChave.SelectedValue.ToString();
            }
            else
            {
                chave = txtTagChave.Text;
            }

            BLL.Tag.Adicionar(Produto1.GUIDIdentificacao, chave, txtTagValor.Text);

            cbbTagChave.SelectedIndex = 0;
            cbbTagChave.Visible = true;
            txtTagChave.Visible = false;

            txtTagChave.Text = "";
            txtTagValor.Text = "";

            AtualizarListaTags();
            AtualizarComboTags();
        }

        private void dgvTags_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvTags.Columns[e.ColumnIndex].Name == "excluir")
                {
                    Int32 idTag = Convert.ToInt32(dgvTags["IDTag", e.RowIndex].Value);
                    BLL.Tag.Excluir(idTag);

                    AtualizarListaTags();
                    AtualizarComboTags();
                }
            }
        }

        private void cbbTagChave_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbTagChave.SelectedValue.ToString() == "criar nova tag")
            {
                cbbTagChave.Visible = false;
                txtTagChave.Visible = true;
                txtTagChave.Focus();
            }
        }

        private void btnTagCancelar_Click(object sender, EventArgs e)
        {
            cbbTagChave.SelectedIndex = 0;
            cbbTagChave.Visible = true;
            txtTagChave.Visible = false;

            txtTagChave.Text = "";
            txtTagValor.Text = "";
        }
    }
}
