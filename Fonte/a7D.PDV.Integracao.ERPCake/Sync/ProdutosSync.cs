using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Integracao.ERPCake.Sync
{
    internal static class ProdutosSync
    {
        static List<Product> ProdutosERP;

        internal static bool SincronizarProdutos(this IntegraERPCake cake)
        {
            //var dt = DateTime.Now; // Quando começou....

            cake.AddLog("Buscando produtos PDV");
            var produtosPDV = EF.Repositorio.Listar<tbProduto>();
            cake.AddLog($"Total de produtos no PDV: {produtosPDV.Count}");

            if (ProdutosERP == null)
            {
                ProdutosERP = new List<Product>();
                int offset = 0;
                int limit = 100;
                do
                {
                    cake.AddLog($"Buscando produtos no ERP {offset + 1}-{offset + limit}");
                    var partialProdutosERP = cake.api.All<Product>(offset, limit);
                    ProdutosERP.AddRange(partialProdutosERP);

                    if (partialProdutosERP.Length < limit)
                        break;

                    offset += limit;
                } while (true);
            }
            //TODO: Fazer busca por data de alteração/inclusão e baixar somente os itens alterados do ERP

            cake.AddLog($"Total de produtos no ERP: {ProdutosERP.Count}");

            int n = 0;
            foreach (var produtoPDV in produtosPDV)
            {
                var produtoERP = ProdutosERP.FirstOrDefault(p => p.code == produtoPDV.IDProduto.ToString());

                if (produtoERP != null
                 && !string.IsNullOrEmpty(produtoPDV.CodigoERP)
                 && produtoPDV.DtUltimaAlteracao < cake.UltimoSync)
                    continue;

                n++;
                string info = "";
                if (produtosPDV.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                    info = $"{(100 * n / (double)produtosPDV.Count).ToString("N1")}% ";

                //produtoPDV.Fill(ref produtoERP);
                var result = cake.api.Save(produtoERP);
                if (result != null)
                {
                    info += "OK ";
                    if (produtoPDV.CodigoERP != result.id.ToString())
                    {
                        produtoPDV.CodigoERP = result.id.ToString();
                        produtoPDV.DtUltimaAlteracao = dt; // se não irá refazer sempre
                        EF.Repositorio.Atualizar(produtoPDV);
                    }
                }
                else
                    info += "ERRO ";

                cake.AddLog($"{info} Produto {produtoPDV.IDProduto} ERP {produtoPDV.CodigoERP} - {produtoPDV.Nome}");
            }

            //cake.UpdateSync(dt);

            return true;
        }
    }
}
