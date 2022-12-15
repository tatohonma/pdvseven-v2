using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.EF.ValoresPadrao;
using a7D.PDV.Model;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace a7D.PDV.EF.Migrations
{
    // https://docs.microsoft.com/pt-br/ef/core/managing-schemas/migrations/
    // EntityFramework\Add-Migration 2_20_MotivosPrincipais
    public sealed class Configuration : DbMigrationsConfiguration<pdv7Context>
    {
        bool EnableSeed;
        StringBuilder log;

        public Configuration() : this(false)
        {
        }

        public Configuration(bool lSeed)
        {
            AutomaticMigrationsEnabled = true;
            EnableSeed = lSeed;
            log = new StringBuilder();
            // AutomaticMigrationDataLossAllowed = true;
        }

        public string LogSeed => log.ToString();

        public static string VersionAssembly { get; internal set; }

        protected override void Seed(pdv7Context context)
        {
            if (!EnableSeed)
                return;

            var versao = context.Versoes.OrderByDescending(v => v.IDVersao).FirstOrDefault();

            if (versao != null)
            {
                if (versao.Versao == VersionAssembly)
                    return;

                else if (versao.ToVersion() > new Version(VersionAssembly))
                {
                    // Não bloqueia ultima versão
                    var v1 = versao.Versao.Split('.');
                    var v2 = VersionAssembly.Split('.');
                    if (v1.Length == 4 && v2.Length == 4 && v1[0] == v2[0] && v1[1] == v2[1])
                        return;
                    else
                        throw new Exception($"Banco de dados está mais atualizado que o aplicativo: {versao.Versao} > {VersionAssembly}")
                        {
                            Source = "VERSAO"
                        };
                }
            }

            // Tipos obrigatórios para os script (FK)
            if (versao == null)
                log.AppendLine($"Novo banco em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
            else
                log.AppendLine($"Migrando da versão {versao.Versao} instalada em {versao.Data.ToString("dd/MM/yyyy HH:mm")}");

            AjustesVersao2_17_9_0(context, versao); // Desativa IDs de gatways inativos

            context.Gateways.AddOrUpdate(g => g.IDGateway, ValueName.Convert<tbGateway>(typeof(EGateway)));
            context.TipoPDVs.AddOrUpdate(g => g.IDTipoPDV, ValueName.Convert<TipoPDVInformation>(typeof(ETipoPDV)));
            context.Bandeiras.AddOrUpdate(g => g.IDBandeira, ValueName.Convert<tbBandeira>(typeof(EBandeira)));
            context.tbStatusComandas.AddOrUpdate(g => g.IDStatusComanda, ValueName.Convert<tbStatusComanda>(typeof(EStatusComanda)));
            context.tbTipoPedidos.AddOrUpdate(g => g.IDTipoPedido, ValueName.Convert<tbTipoPedido>(typeof(ETipoPedido)));
            context.tbStatusPedidoes.AddOrUpdate(g => g.IDStatusPedido, ValueName.Convert<tbStatusPedido>(typeof(EStatusPedido)));
            context.tbStatusMesas.AddOrUpdate(g => g.IDStatusMesa, ValueName.Convert<tbStatusMesa>(typeof(EStatusMesa)));
            context.tbTipoProdutos.AddOrUpdate(g => g.IDTipoProduto, ValueName.Convert<tbTipoProduto>(typeof(ETipoProduto)));
            context.TamanhoPacotes.AddOrUpdate(g => g.IDTamanhoPacote, ValueName.Convert<tbTamanhoPacote>(typeof(ETamanhoPacote)));
            context.tbTipoAreaImpressoes.AddOrUpdate(g => g.IDTipoAreaImpressao, ValueName.Convert<tbTipoAreaImpressao>(typeof(ETipoAreaImpressao)));
            context.TipoIntegracoes.AddOrUpdate(g => g.IDTipoIntegracao, ValueName.Convert<tbTipoIntegracao>(typeof(ETipoIntegracao)));
            context.OrigemPedidos.AddOrUpdate(g => g.IDOrigemPedido, ValueName.Convert<tbOrigemPedido>(typeof(EOrigemPedido)));

            LogarAlteracoes(context);

            context.SaveChanges();

            if (!initial.Skip)
            {
                context.Database.ExecuteSqlCommand(Properties.Resources.Tipos);
                context.Database.ExecuteSqlCommand(Properties.Resources.Outros);
            }

            AjustesVersao2_17_0_3(context, versao);
            AjustesVersao2_17_10_0(context, versao);
            AjustesVersao2_17_17_5(context, versao);
            AjustesVersao2_19_3_2(context, versao);
            AjustesVersao2_20_3_0(context, versao);
            AjustesVersao2_25_0_0(context, versao);

            // Limpeza iFood e ERP: Remover futuramente
            if (versao != null && versao.ToVersion() < new Version("2.17.16.6"))
            {
                int qtd = context.Database.ExecuteSqlCommand("DELETE FROM tbConfiguracaoBD WHERE IDTipoPDV IN (150, 160)");
                log.AppendLine($"Limpesa de configurações do ERP e IFOOD: {qtd}");
            }

            ValoresConfiguracoes.Validar(context);
            ValoresProdutos.Validar(context);
            ValoresContasRecebiveis.Validar(context);
            ValoresRelatorios.Validar(context);
            ValoresTaxaEntregas.Validar(context);

            // Erros ou Alerta de versões antigas não fazem sentido e só poluem a base
            // Mas as informações de rastreamento devem ser mantidas
            context.Database.ExecuteSqlCommand("DELETE FROM tbLOG WHERE Codigo LIKE 'E%' OR Codigo LIKE 'A%'");


            LogarAlteracoes(context);

            // Histórico de Versões pelo qual o banco já passou!
            context.Versoes.AddOrUpdate(v => v.Versao, new tbVersao() { Versao = VersionAssembly, Data = DateTime.Now });
        }

        private void LogarAlteracoes(pdv7Context context)
        {
            var alteracoes = context.ChangeTracker.Entries().Where(e => e.State != System.Data.Entity.EntityState.Unchanged);
            foreach (var item in alteracoes)
            {
                log.AppendLine($"{item.Entity.GetType().Name} {item.State}");
                try
                {
                    if (item.State == System.Data.Entity.EntityState.Added)
                    {
                        foreach (string key in item.CurrentValues.PropertyNames)
                        {
                            if (!string.IsNullOrEmpty(item.CurrentValues[key]?.ToString()))
                                log.AppendLine($"\t{key}: => {item.CurrentValues[key]}");
                        }
                    }
                    else if (item.State == System.Data.Entity.EntityState.Deleted)
                    {
                        foreach (string key in item.OriginalValues.PropertyNames)
                        {
                            if (!string.IsNullOrEmpty(item.OriginalValues[key]?.ToString()))
                                log.AppendLine($"\t{key}: {item.OriginalValues[key]}");
                        }
                    }
                    else
                    {
                        foreach (string key in item.OriginalValues.PropertyNames)
                        {
                            if (item.OriginalValues[key]?.ToString() != item.CurrentValues[key]?.ToString())
                                log.AppendLine($"\t{key}: {item.OriginalValues[key]} => {item.CurrentValues[key]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.AppendLine("ERRO: " + ex.Message + ex.StackTrace);
                }
            }
        }

        private void AjustesVersao2_17_0_3(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.17.0.3"))
            {
                int qtd = context.Database.ExecuteSqlCommand("UPDATE tbPedidoPagamento SET Excluido=0 WHERE Excluido IS NULL");
                log.AppendLine($"AjustesVersao2_17_0_3: {qtd}");
            }
        }

        private void AjustesVersao2_17_9_0(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.17.9.0"))
            {
                int qtd = context.Database.ExecuteSqlCommand(@"update tbGateway set Nome='NÃO USAR';
delete from tbGateway where NOT(IDGateway in (select distinct IDGateway from tbTipoPagamento where IDGateway > 0 )); ");
                log.AppendLine($"AjustesVersao2_17_9_0: {qtd}");
            }
        }

        private void AjustesVersao2_17_10_0(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.17.10.0"))
            {
                int qtd1 = context.Database.ExecuteSqlCommand(@"UPDATE tbProduto set IDClassificacaoFiscal=2 WHERE IDClassificacaoFiscal IS NULL AND IDTipoProduto<5");
                int qtd2 = context.Database.ExecuteSqlCommand(@"UPDATE tbProduto set IDUnidade=1 WHERE IDUnidade IS NULL AND IDTipoProduto<5");
                log.AppendLine($"AjustesVersao2_17_10_0: {qtd1} {qtd2}");
            }
        }

        private void AjustesVersao2_17_17_5(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.17.17.5"))
            {
                int qtd1 = context.Database.ExecuteSqlCommand(@"UPDATE tbCliente SET Telefone1Numero=0, Telefone1DDD=0 WHERE Telefone1Numero IS NULL");
                log.AppendLine($"AjustesVersao2_17_17_5: {qtd1}");
            }
        }

        private void AjustesVersao2_19_3_2(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.19.3.2"))
            {
                int qtd1 = context.Database.ExecuteSqlCommand(@"DELETE FROM tbMensagem");
                log.AppendLine($"AjustesVersao2_19_3_2: {qtd1}");
            }
        }

        private void AjustesVersao2_20_3_0(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.20.3.0"))
            {
                int qtd1 = context.Database.ExecuteSqlCommand(@"UPDATE tbConfiguracaoBD SET Chave='Fiscal', valor='SAT' where chave='PossuiSAT' AND valor='1'");
                log.AppendLine($"AjustesVersao2_20_3_0: {qtd1}");
            }
        }

        private void AjustesVersao2_25_0_0(pdv7Context context, tbVersao versao)
        {
            if (versao == null || versao.ToVersion() < new Version("2.25.0.0"))
            {
                context.Database.ExecuteSqlCommand(@"DELETE tbRelatorio WHERE Nome='Taxa de Servico Com e Sem Desconto' AND IDTipoRelatorio='2'");
                log.AppendLine($"Ajuste Versao 2.25.0.0: Relatório 'Taxa de Servico Com e Sem Desconto' excluído!");
            }
        }
   }
}