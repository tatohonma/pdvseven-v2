using System;
using System.Collections.Generic;
using System.Linq;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public static class PrecoAssistente
    {
        public static void FinalizaNivel(PedidoProdutoInformation pedidoProduto, bool valorRedefinido)
        {
            var listaPainelModificacao = pedidoProduto.Produto.ListaPainelModificacao;
            var listaProduto = pedidoProduto.ListaModificacao;

            // cria uma cópia para ir agrupando os valores quando necessário
            var listaPaineis = listaPainelModificacao.ToList();
            while (listaPaineis.Count > 0)
            {
                var painel = listaPaineis[0];
                if (painel.PainelModificacao.PainelModificacaoOperacao.IDPainelModificacaoOperacao == 1) // Soma (embora seja padrão, tem que processar os paineis relacionados)
                    CalculaSoma(painel, listaPaineis, listaProduto);
                
                else if (painel.PainelModificacao.PainelModificacaoOperacao.IDPainelModificacaoOperacao == 2) // Maior Valor
                    EscolheMaiorValor(painel, listaPaineis, listaProduto);
                
                else if (painel.PainelModificacao.PainelModificacaoOperacao.IDPainelModificacaoOperacao == 3) // Média
                    CalculaMedia(painel, listaPaineis, listaProduto);

                else
                    throw new Exception("Forma de calculo invalida");

                if (valorRedefinido == false && painel.PainelModificacao.IgnorarValorItem == true)
                {
                    valorRedefinido = true; // só redefine 1 vez!
                    pedidoProduto.ValorUnitario = 0;
                }

                listaPaineis.RemoveAt(0); // apaga o painel
            }
        }

        private static void CalculaSoma(ProdutoPainelModificacaoInformation painel, List<ProdutoPainelModificacaoInformation> listaPaineis, List<PedidoProdutoInformation> listaProduto)
        {
            DefinePrecoPainel(painel, painel.PainelModificacao.IDPainelModificacao.Value, listaProduto);

            // só busca os paineis relacionados e os remove dos calculos, pois tudo é somado por padrão
            for (int i = 0; i < painel.PainelModificacao.PaineisRelacionados.Count; i++)
            {
                int prID = listaPaineis.FindIndex(pr => painel.PainelModificacao.PaineisRelacionados[i].PainelModificacao2.IDPainelModificacao == pr.PainelModificacao.IDPainelModificacao);
                // se está sempre no zero que dizer ser outro que não seja ele mesmo
                if (prID > 0)
                {
                    // somente os produtos do painel relacionado 
                    DefinePrecoPainel(painel, listaPaineis[prID].PainelModificacao.IDPainelModificacao.Value, listaProduto);
                    // Remove o painel da lista de paineis pois já foi processado e não deve ser mais usado para nada
                    listaPaineis.RemoveAt(prID);
                }
            }
        }

        private static void CalculaMedia(ProdutoPainelModificacaoInformation painel, List<ProdutoPainelModificacaoInformation> listaPaineis, List<PedidoProdutoInformation> listaProduto)
        {
            decimal soma = 0;
            int qtd = 0;
            var idMedia = new List<int>();

            // Etapa 1) soma e conta os valor do painel atual 
            for (int prod = 0; prod < listaProduto.Count; prod++)
            {
                if (listaProduto[prod].PainelModificacao.IDPainelModificacao == painel.PainelModificacao.IDPainelModificacao)
                {
                    idMedia.Add(prod); // memoriza todos ids relacionados
                    listaProduto[prod].ValorUnitario = ProdutoValor(listaProduto[prod].Produto, painel.PainelModificacao);
                    soma += listaProduto[prod].ValorUnitario.Value;
                    qtd++;
                }
            }

            // Etapa 2) busca valores no painel relacionado
            for (int i = 0; i < painel.PainelModificacao.PaineisRelacionados.Count; i++)
            {
                int prID = listaPaineis.FindIndex(pr => painel.PainelModificacao.PaineisRelacionados[i].PainelModificacao2.IDPainelModificacao == pr.PainelModificacao.IDPainelModificacao);
                // se está sempre no zero que dizer ser outro que não seja ele mesmo
                if (prID > 0)
                {
                    // somente os produtos do painel relacionado 
                    for (int prod = 0; prod < listaProduto.Count; prod++)
                    {
                        if (listaProduto[prod].PainelModificacao.IDPainelModificacao == listaPaineis[prID].PainelModificacao.IDPainelModificacao)
                        {
                            idMedia.Add(prod); // memoriza todos ids relacionados
                            listaProduto[prod].ValorUnitario = ProdutoValor(listaProduto[prod].Produto, painel.PainelModificacao);
                            soma += listaProduto[prod].ValorUnitario.Value;
                            qtd++;
                        }
                    }
                    // Remove o painel da lista de paineis pois já foi processado e não deve ser mais usado para nada
                    listaPaineis.RemoveAt(prID);
                }
            }

            // Etapa 3) tendo itens selecionado, a média é distribuida entre os produtos
            if (qtd > 0)
            {
                decimal media = (soma / qtd) / qtd;
                foreach (var prod in idMedia)
                {
                    listaProduto[prod].ValorUnitario = media;
                }
            }
        }

        private static void EscolheMaiorValor(ProdutoPainelModificacaoInformation painel, List<ProdutoPainelModificacaoInformation> listaPaineis, List<PedidoProdutoInformation> listaProduto)
        {
            int selecionado = 0;
            decimal valor = 0;
            var idZerar = new List<int>();

            // Etapa 1) seleciona o maior valor do painel atual 
            for (int prod = 0; prod < listaProduto.Count; prod++)
            {
                var item = listaProduto[prod];
                if (item.PainelModificacao.IDPainelModificacao == painel.PainelModificacao.IDPainelModificacao)
                {
                    idZerar.Add(prod); // memoriza todos ids relacionados
                    if (item.ValorUnitario.Value > valor)
                    {
                        item.ValorUnitario = ProdutoValor(item.Produto, painel.PainelModificacao);
                        valor = item.ValorUnitario.Value;
                        selecionado = prod;
                    }
                }
            }

            // Etapa 2) seleciona o maior valor entre os outros paineis relacionados
            for (int i = 0; i < painel.PainelModificacao.PaineisRelacionados.Count; i++)
            {
                int prID = listaPaineis.FindIndex(pr => painel.PainelModificacao.PaineisRelacionados[i].PainelModificacao2.IDPainelModificacao == pr.PainelModificacao.IDPainelModificacao);
                // se está sempre no zero que dizer ser outro que não seja ele mesmo
                if (prID > 0)
                {
                    // somente os produtos do painel relacionado 
                    for (int prod = 0; prod < listaProduto.Count; prod++)
                    {
                        var item = listaProduto[prod];
                        if (item.PainelModificacao.IDPainelModificacao == listaPaineis[prID].PainelModificacao.IDPainelModificacao)
                        {
                            idZerar.Add(prod); // memoriza todos ids relacionados
                            if (item.ValorUnitario.Value > valor)
                            {
                                item.ValorUnitario = ProdutoValor(item.Produto, painel.PainelModificacao);
                                valor = item.ValorUnitario.Value;
                                selecionado = prod;
                            }
                        }
                    }
                    // Remove o painel da lista de paineis pois já foi processado e não deve ser mais usado para nada
                    listaPaineis.RemoveAt(prID);
                }
            }

            // Etapa 3) exclui os valores dos itens que não foram selecionados
            foreach (var prod in idZerar)
            {
                if (prod != selecionado)
                {
                    listaProduto[prod].ValorUnitario = 0;
                }
            }
        }

        private static void DefinePrecoPainel(ProdutoPainelModificacaoInformation painel, int idPainel, List<PedidoProdutoInformation> listaProduto)
        {
            // Coloca precos no do painel no Painel específico pir idPainel
            var itensProdutosPainel = listaProduto.Where(p => p.PainelModificacao.IDPainelModificacao == idPainel).ToArray();
            for (int i = 0; i < itensProdutosPainel.Length; i++)
            {
                var item = itensProdutosPainel[i];
                item.ValorUnitario = ProdutoValor(item.Produto, painel.PainelModificacao);
            }
        }

        public static decimal? ProdutoValor(ProdutoInformation produto, PainelModificacaoInformation painel)
        {
            if (painel.IDValorUtilizado == 2 && produto.ValorUnitario2.HasValue)
                return produto.ValorUnitario2.Value;
            else if (painel.IDValorUtilizado == 3 && produto.ValorUnitario3.HasValue)
                return produto.ValorUnitario3.Value;
            else
                return produto.ValorUnitario.Value;
        }
    }
}