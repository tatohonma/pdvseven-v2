using a7D.PDV.EF.Migrations;
using a7D.PDV.EF.Models.Mapping;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace a7D.PDV.EF.Models
{
    public partial class pdv7Context : DbContext
    {
        static pdv7Context()
        {
            Database.SetInitializer<pdv7Context>(null);
        }

        public pdv7Context() : base("Name=connectionString")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public pdv7Context(bool fast) : this()
        {
            if (fast)
            {
                this.Configuration.AutoDetectChangesEnabled = false;
                this.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                this.Configuration.ValidateOnSaveEnabled = false;
            }
        }

        public T UnProxy<T>(T proxyObject) where T : class
        {
            var proxyCreationEnabled = this.Configuration.ProxyCreationEnabled;
            try
            {
                this.Configuration.ProxyCreationEnabled = false;
                T poco = this.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        public IEnumerable<T> UnProxy<T>(IEnumerable<T> proxyObjects) where T : class
        {
            var proxyCreationEnabled = this.Configuration.ProxyCreationEnabled;
            try
            {
                foreach (var obj in proxyObjects)
                    yield return UnProxy(obj);
            }
            finally
            {
                this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        public static string LastLog { get; private set; }

        public static void ClearLastLog()
        {
            LastLog = null;
        }

        public static string VerificaVersao()
        {
            var asm = Assembly.GetCallingAssembly();
            var path = new FileInfo(asm.Location).Directory.FullName;
            Migrations.Configuration.VersionAssembly = asm.GetName().Version.ToString();
            string log = Migrations.Configuration.VersionAssembly;

            var cn = new SqlConnection();
            try
            {
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao conectar-se com o SQL", ex);
            }

            try
            {
                var cmd = new SqlCommand("SELECT Count(1) FROM tbAreaImpressao", cn);
                cmd.ExecuteScalar();
                initial.Skip = true;
                log += "\r\nSkip initial";
            }
            catch (Exception ex)
            {
                initial.Skip = false;
                log += "\r\nTeste 'initial' falhou: " + ex.Message;
            }

            if (initial.Skip)
            {
                try
                {
                    var cmd = new SqlCommand("SELECT Versao FROM tbVersao ORDER BY IDVersao DESC", cn);
                    log += "\r\nVerificando versão";
                    var ultimaVersao = cmd.ExecuteScalar();
                    if (ultimaVersao?.ToString() == Migrations.Configuration.VersionAssembly)
                        return null;
                }
                catch (Exception ex)
                {
                    log += "\r\nErro ao obter versão: " + ex.Message;
                }
            }
            cn.Close();

            // EF!!!
            var file = new FileInfo(Path.Combine(path, string.Format("update-{0:yyyyMMdd-HHmmss}.txt", DateTime.Now)));
            string script = "?";
            try
            {
                // Apenas verifica para gerar o log
                var migrator = new DbMigrator(new Migrations.Configuration(false));
                var scriptor = new MigratorScriptingDecorator(migrator);
                foreach (string migration in migrator.GetPendingMigrations())
                    log += "\r\n" + migration;

                script = scriptor.ScriptUpdate(null, null);
                if (!string.IsNullOrEmpty(script))
                    // Pode não haver migração de estrutura, mas pode existir novos registros (Dados)
                    File.WriteAllText(file.FullName, log + "\r\n\r\n" + script);

                // Cria as tabelas
                migrator = new DbMigrator(new Migrations.Configuration(false));
                migrator.Update();

                // Revalida a migrção executando o SEED
                var config = new Migrations.Configuration(true);
                migrator = new DbMigrator(config);
                migrator.Update();

                LastLog = config.LogSeed;
                if (string.IsNullOrEmpty(LastLog))
                    return null;

                log += "\r\n\r\nSEED LOG\r\n" + LastLog;

                File.AppendAllText(file.FullName, "\r\n\r\nSEED LOG\r\n" + LastLog);
                log += "\r\nHistórico registrado em: " + file.FullName + "\r\n\r\n";

                if (!string.IsNullOrEmpty(script))
                    LastLog += "\r\n\r\n" + script;

            }
            catch (Exception ex) when (ex.Source == "VERSAO")
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ex.Data.Add("log", log);
                ex.Data.Add("file", file);
                ex.Data.Add("script", script);
                throw new Exception("Erro ao validar executar migração, veja mais no log do windows:\n" + ex.Message, ex);
            }

            return log;

        }

        internal DbSet<TEntity> DbSet<TEntity>() where TEntity : class, new()
        {
            var entitys = GetType()
                .GetProperties()
                .First(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments().Contains(typeof(TEntity)));

            return (DbSet<TEntity>)entitys.GetValue(this);
        }

        //public DbSet<tbAcao> tbAcaos { get; set; }
        public DbSet<tbAreaImpressao> tbAreasImpressao { get; set; }
        public DbSet<tbCaixa> tbCaixas { get; set; }
        public DbSet<tbCaixaAjuste> tbCaixaAjustes { get; set; }
        public DbSet<tbCaixaTipoAjuste> tbCaixaTipoAjustes { get; set; }
        public DbSet<tbCaixaValorRegistro> tbCaixaValorRegistros { get; set; }
        public DbSet<tbCategoriaProduto> tbCategoriaProdutos { get; set; }
        public DbSet<tbClassificacaoFiscal> tbClassificacaoFiscais { get; set; }
        public DbSet<tbCliente> tbClientes { get; set; }
        public DbSet<tbComanda> tbComandas { get; set; }
        public DbSet<tbConfiguracaoBD> tbConfiguracoesBD { get; set; }
        public DbSet<tbConversaoUnidade> tbConversaoUnidades { get; set; }
        public DbSet<tbEntradaSaida> tbEntradaSaidas { get; set; }
        public DbSet<tbEntregador> tbEntregadores { get; set; }
        public DbSet<tbEstado> tbEstados { get; set; }
        public DbSet<tbFechamento> tbFechamentos { get; set; }
        public DbSet<tbIdioma> tbIdiomas { get; set; }
        public DbSet<tbImagem> tbImagems { get; set; }
        public DbSet<tbImagemTema> tbImagemTemas { get; set; }
        public DbSet<tbInventario> tbInventarios { get; set; }
        public DbSet<tbInventarioProduto> tbInventarioProdutos { get; set; }
        public DbSet<tbMapAreaImpressaoProduto> tbMapAreaImpressaoProdutos { get; set; }
        public DbSet<tbMeioPagamento> tbMeioPagamentoSATs { get; set; }
        public DbSet<tbMesa> tbMesas { get; set; }
        public DbSet<tbMotivoCancelamento> tbMotivoCancelamentos { get; set; }
        public DbSet<tbMovimentacao> tbMovimentacoes { get; set; }
        public DbSet<tbMovimentacaoProduto> tbMovimentacaoProdutos { get; set; }
        public DbSet<tbOrdemImpressao> tbOrdemImpressoes { get; set; }
        public DbSet<tbPainelModificacao> tbPainelModificacoes { get; set; }
        public DbSet<tbPainelModificacaoCategoria> tbPainelModificacaoCategorias { get; set; }
        public DbSet<tbPainelModificacaoOperacao> tbPainelModificacaoOperacoes { get; set; }
        public DbSet<tbPainelModificacaoProduto> tbPainelModificacaoProdutos { get; set; }
        public DbSet<tbPainelModificacaoRelacionado> tbPainelModificacaoRelacionados { get; set; }
        public DbSet<PDVInformation> tbPDVs { get; set; }
        public DbSet<tbPedido> tbPedidos { get; set; }
        public DbSet<tbPedidoPagamento> tbPedidoPagamentos { get; set; }
        public DbSet<tbPedidoProduto> tbPedidoProdutos { get; set; }
        public DbSet<tbProcessamentoSAT> tbProcessamentoSATs { get; set; }
        public DbSet<tbProduto> tbProdutoes { get; set; }
        public DbSet<tbProdutoCategoriaProduto> tbProdutoCategoriaProdutos { get; set; }
        public DbSet<tbProdutoImagem> tbProdutoImagens { get; set; }
        public DbSet<tbProdutoPainelModificacao> tbProdutoPainelModificacoes { get; set; }
        public DbSet<tbProdutoReceita> tbProdutoReceitas { get; set; }
        public DbSet<tbProdutoTraducao> tbProdutoTraducaos { get; set; }
        public DbSet<tbRelatorio> tbRelatorios { get; set; }
        public DbSet<tbRetornoSAT> tbRetornoSATs { get; set; }
        public DbSet<tbStatusComanda> tbStatusComandas { get; set; }
        public DbSet<tbStatusMesa> tbStatusMesas { get; set; }
        public DbSet<tbStatusPedido> tbStatusPedidoes { get; set; }
        public DbSet<tbStatusProcessamentoSAT> tbStatusProcessamentoSATs { get; set; }
        public DbSet<tbTaxaEntrega> tbTaxaEntregas { get; set; }
        public DbSet<tbTemaCardapio> tbTemaCardapios { get; set; }
        public DbSet<tbTemaCardapioPDV> tbTemaCardapioPDVs { get; set; }
        public DbSet<tbTipoAreaImpressao> tbTipoAreaImpressoes { get; set; }
        public DbSet<tbTipoDesconto> tbTipoDescontos { get; set; }
        public DbSet<tbTipoEntrada> tbTipoEntradas { get; set; }
        public DbSet<tbTipoMovimentacao> tbTipoMovimentacoes { get; set; }
        public DbSet<tbTipoPagamento> tbTipoPagamentos { get; set; }
        public DbSet<TipoPDVInformation> TipoPDVs { get; set; }
        public DbSet<tbTipoPedido> tbTipoPedidos { get; set; }
        public DbSet<tbTipoProduto> tbTipoProdutos { get; set; }
        public DbSet<tbTipoRelatorio> tbTipoRelatorios { get; set; }
        public DbSet<tbTipoSolicitacaoSAT> tbTipoSolicitacaoSATs { get; set; }
        public DbSet<tbTipoTributacao> tbTipoTributacoes { get; set; }
        public DbSet<tbUnidade> tbUnidades { get; set; }
        public DbSet<tbUsuario> tbUsuarios { get; set; }
        public DbSet<tbGateway> Gateways { get; set; }
        public DbSet<tbVersao> Versoes { get; set; }
        public DbSet<tbContaRecebivel> ContaRecebiveis { get; set; }
        public DbSet<tbBandeira> Bandeiras { get; set; }
        public DbSet<tbSaldo> Saldos { get; set; }
        public DbSet<tbLOGInformation> LOGs { get; set; }
        public DbSet<tbPesquisa> Pesquisas { get; set; }
        public DbSet<tbTamanhoPacote> TamanhoPacotes { get; set; }
        public DbSet<tbMensagem> Mensagens { get; set; }
        public DbSet<tbTicket> Tickets { get; set; }
        public DbSet<tbIntegracao> Integracoes { get; set; }
        public DbSet<tbTipoIntegracao> TipoIntegracoes { get; set; }
        public DbSet<tbTag> Tags { get; set; }
        public DbSet<tbOrigemPedido> OrigemPedidos { get; set; }
        public DbSet<tbFaturaPixConta> FaturasPixConta { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new tbAreaImpressaoMap());
            modelBuilder.Configurations.Add(new tbCaixaMap());
            modelBuilder.Configurations.Add(new tbCaixaAjusteMap());
            modelBuilder.Configurations.Add(new tbCaixaTipoAjusteMap());
            modelBuilder.Configurations.Add(new tbCaixaValorRegistroMap());
            modelBuilder.Configurations.Add(new tbCategoriaProdutoMap());
            modelBuilder.Configurations.Add(new tbClassificacaoFiscalMap());
            modelBuilder.Configurations.Add(new tbClienteMap());
            modelBuilder.Configurations.Add(new tbComandaMap());
            modelBuilder.Configurations.Add(new tbConfiguracaoBDMap());
            modelBuilder.Configurations.Add(new tbConversaoUnidadeMap());
            modelBuilder.Configurations.Add(new tbEntradaSaidaMap());
            modelBuilder.Configurations.Add(new tbEntregadorMap());
            modelBuilder.Configurations.Add(new tbEstadoMap());
            modelBuilder.Configurations.Add(new tbFechamentoMap());
            modelBuilder.Configurations.Add(new tbIdiomaMap());
            modelBuilder.Configurations.Add(new tbImagemMap());
            modelBuilder.Configurations.Add(new tbImagemTemaMap());
            modelBuilder.Configurations.Add(new tbInventarioMap());
            modelBuilder.Configurations.Add(new tbInventarioProdutoMap());
            modelBuilder.Configurations.Add(new tbMapAreaImpressaoProdutoMap());
            modelBuilder.Configurations.Add(new tbMeioPagamentoSATMap());
            modelBuilder.Configurations.Add(new tbMesaMap());
            modelBuilder.Configurations.Add(new tbMotivoCancelamentoMap());
            modelBuilder.Configurations.Add(new tbMovimentacaoMap());
            modelBuilder.Configurations.Add(new tbMovimentacaoProdutoMap());
            modelBuilder.Configurations.Add(new tbOrdemImpressaoMap());
            modelBuilder.Configurations.Add(new tbPainelModificacaoMap());
            modelBuilder.Configurations.Add(new tbPainelModificacaoCategoriaMap());
            modelBuilder.Configurations.Add(new tbPainelModificacaoOperacaoMap());
            modelBuilder.Configurations.Add(new tbPainelModificacaoProdutoMap());
            modelBuilder.Configurations.Add(new tbPainelModificacaoRelacionadoMap());
            modelBuilder.Configurations.Add(new tbPDVMap());
            modelBuilder.Configurations.Add(new tbPedidoMap());
            modelBuilder.Configurations.Add(new tbPedidoPagamentoMap());
            modelBuilder.Configurations.Add(new tbPedidoProdutoMap());
            modelBuilder.Configurations.Add(new tbProcessamentoSATMap());
            modelBuilder.Configurations.Add(new tbProdutoMap());
            modelBuilder.Configurations.Add(new tbProdutoCategoriaProdutoMap());
            modelBuilder.Configurations.Add(new tbProdutoImagemMap());
            modelBuilder.Configurations.Add(new tbProdutoPainelModificacaoMap());
            modelBuilder.Configurations.Add(new tbProdutoReceitaMap());
            modelBuilder.Configurations.Add(new tbProdutoTraducaoMap());
            modelBuilder.Configurations.Add(new tbRelatorioMap());
            modelBuilder.Configurations.Add(new tbRetornoSATMap());
            modelBuilder.Configurations.Add(new tbStatusComandaMap());
            modelBuilder.Configurations.Add(new tbStatusMesaMap());
            modelBuilder.Configurations.Add(new tbStatusPedidoMap());
            modelBuilder.Configurations.Add(new tbStatusProcessamentoSATMap());
            modelBuilder.Configurations.Add(new tbTaxaEntregaMap());
            modelBuilder.Configurations.Add(new tbTemaCardapioMap());
            modelBuilder.Configurations.Add(new tbTemaCardapioPDVMap());
            modelBuilder.Configurations.Add(new tbTipoAreaImpressaoMap());
            modelBuilder.Configurations.Add(new tbTipoDescontoMap());
            modelBuilder.Configurations.Add(new tbTipoEntradaMap());
            modelBuilder.Configurations.Add(new tbTipoMovimentacaoMap());
            modelBuilder.Configurations.Add(new tbTipoPagamentoMap());
            modelBuilder.Configurations.Add(new tbTipoPDVMap());
            modelBuilder.Configurations.Add(new tbTipoPedidoMap());
            modelBuilder.Configurations.Add(new tbTipoProdutoMap());
            modelBuilder.Configurations.Add(new tbTipoRelatorioMap());
            modelBuilder.Configurations.Add(new tbTipoSolicitacaoSATMap());
            modelBuilder.Configurations.Add(new tbTipoTributacaoMap());
            modelBuilder.Configurations.Add(new tbUnidadeMap());
            modelBuilder.Configurations.Add(new tbUsuarioMap());
            modelBuilder.Configurations.Add(new tbGatewayMap());
            modelBuilder.Configurations.Add(new tbVersaoMap());
            modelBuilder.Configurations.Add(new tbContaRecebivelMap());
            modelBuilder.Configurations.Add(new tbBandeiraMap());
            modelBuilder.Configurations.Add(new tbSaldoMap());
            modelBuilder.Configurations.Add(new tbLOGMap());
            modelBuilder.Configurations.Add(new PesquisaMAP());
            modelBuilder.Configurations.Add(new tbTamanhoPacoteMAP());
            modelBuilder.Configurations.Add(new tbHorarioDeliveryMap());
            modelBuilder.Configurations.Add(new tbTagMap());
            modelBuilder.Configurations.Add(new tbOrigemPedidoMap());
            modelBuilder.Configurations.Add(new tbFaturaPixContaMap());
        }
    }
}
