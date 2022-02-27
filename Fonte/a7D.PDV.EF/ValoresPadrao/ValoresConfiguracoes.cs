using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace a7D.PDV.EF.ValoresPadrao
{
    internal class ValoresConfiguracoes
    {
        internal static void Validar(pdv7Context context)
        {
            var allConfig = new List<tbConfiguracaoBD>();
            foreach (Type tp in ConfigAttribute.TypeList)
            {
                foreach (PropertyInfo prop in tp.GetProperties())
                {
                    var configs = prop.GetCustomAttributes();
                    if (configs != null)
                    {
                        foreach (var attr in configs)
                        {
                            if (attr is ConfigAttribute cfg)
                            {
                                allConfig.Add(cfg.ToDB(prop.Name));
                            }
                        }
                    }
                }
            }

            var allConfig2 = new List<tbConfiguracaoBD>
            {
                // TODO: Relatório Fechamento
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-PedidosDescontoDetalhe", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Detalhes de Descontos", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-PedidosDescontoResumo", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Resumo de Descontos", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ProdutosAbertos", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Produtos em Aberto", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ProdutosCancelados", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Produtos Cancelados", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ProdutosCanceladosAberto", Valor = "0", ValoresAceitos="0|1", Titulo = "Exibir Produtos Cancelados em Aberto", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ProdutosVendidos", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Produtos Vendidos", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ResumoCaixa", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Resumo por Caixa", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ResumoTipoPagamento", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Resumo por Tipo de Pagamento", Obrigatorio = true },
                new tbConfiguracaoBD() { Chave = "RelatorioFechamento-ResumoCreditoPagamento", Valor = "1", ValoresAceitos="0|1", Titulo = "Exibir Resumo de Conta Cliente", Obrigatorio = true }
            };

            allConfig.AddRange(allConfig2);

            var configValidas = allConfig.Select(c => c.Chave.ToLower());

            // Apaga as configurações descontinuadas
            var atualConfig = context.tbConfiguracoesBD.ToList();
            foreach (var atual in atualConfig)
            {
                if (!configValidas.Contains(atual.Chave.ToLower()))
                    context.tbConfiguracoesBD.Remove(atual);
            }

            while (allConfig.Count > 0)
            {
                var config = allConfig[0];
                var cfgAtualList = atualConfig.Where(c => c.IDTipoPDV == config.IDTipoPDV && string.Compare(c.Chave, config.Chave, StringComparison.InvariantCultureIgnoreCase) == 0);

                if (cfgAtualList.Count() == 0)
                {
                    context.tbConfiguracoesBD.Add(config);
                    atualConfig.Add(config); // para evitar inclusão dupla (necessário para classes derivadas)
                }
                else
                {
                    foreach (var cfgAtual in cfgAtualList)
                    {
                        if (cfgAtual.Titulo != config.Titulo)
                            cfgAtual.Titulo = config.Titulo;
                        if (cfgAtual.ValoresAceitos != config.ValoresAceitos)
                            cfgAtual.ValoresAceitos = config.ValoresAceitos;
                        if (cfgAtual.Obrigatorio != config.Obrigatorio)
                            cfgAtual.Obrigatorio = config.Obrigatorio;
                    }
                }
                allConfig.RemoveAt(0);
            }
        }
    }
}
