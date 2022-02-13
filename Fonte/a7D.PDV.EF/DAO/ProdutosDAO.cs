using a7D.PDV.EF.Models;
using System;
using System.Linq;

namespace a7D.PDV.EF.DAO
{
    public static class ProdutosDAO
    {
        //https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/
        public static tbProduto[] Listar(int nID = 0, DateTime? dtAlteracao = null, int idTipo = 0, int idCategoria = 0, bool? bDisponivel = null, bool? bAtivo = null, int top = 0, int limit = 0)
        {
            using (var pdv7 = new pdv7Context())
            {
                var produtos = from p in pdv7.tbProdutoes
                                    .AsNoTracking()
                                    .Include(nameof(tbProduto.tbProdutoCategoriaProdutoes))
                                    .Include(nameof(tbProduto.tbProdutoImagems))
                                    .Include(nameof(tbProduto.tbMapAreaImpressaoProdutoes))
                                    .Include(nameof(tbProduto.tbProdutoPainelModificacaos))
                               select p;

                if (nID > 0)
                {
                    produtos = produtos.Where(p => p.IDProduto == nID);
                    return produtos.ToArray();
                }

                produtos = produtos.Where(p =>
                       (idTipo == 0 || (p.IDProduto >= 4 && p.IDTipoProduto == idTipo))
                    && (idCategoria == 0 || p.tbProdutoCategoriaProdutoes.Any(c => c.IDCategoriaProduto == idCategoria)));


                if (dtAlteracao != null)
                {
                    var date = dtAlteracao.Value;
                    produtos = produtos.Where(p => p.DtUltimaAlteracao >= date);
                }

                if (bDisponivel.HasValue)
                {
                    if (bDisponivel.Value)
                        produtos = produtos.Where(p => p.Disponibilidade == true);
                    else
                        produtos = produtos.Where(p => p.Disponibilidade == false);
                }

                if (bAtivo.HasValue)
                {
                    if (bAtivo.Value)
                        produtos = produtos.Where(p => p.Ativo == true);
                    else
                        produtos = produtos.Where(p => p.Ativo == false);
                }

                var result = produtos.OrderBy(p => p.DtUltimaAlteracao);

                if (top > 0 || limit > 0)
                {
                    return result.Skip(top).Take(limit).ToArray();
                }
                else
                {
                    return result.ToArray();
                }
            }
        }
    }
}
