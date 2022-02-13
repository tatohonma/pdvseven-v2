using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.EF.ValoresPadrao
{
    internal class ValoresProdutos
    {
        internal static void Validar(pdv7Context context)
        {
            var produtosPadrao = new List<tbProduto>
            {
                new tbProduto() { IDProduto=1, IDTipoProduto=(int)ETipoProduto.Item, Codigo="pnc", Nome="Produto não cadastrado", ValorUnitario=0, CodigoAliquota="II", IDClassificacaoFiscal=13, IDUnidade=1, Disponibilidade=false, Ativo=true},
                new tbProduto() { IDProduto=2, IDTipoProduto=(int)ETipoProduto.Servico, Nome=" * Entrada", ValorUnitario=0, CodigoAliquota="II", IDUnidade=1, IDClassificacaoFiscal=2, Disponibilidade=false, Ativo=true},
                new tbProduto() { IDProduto=3, IDTipoProduto=(int)ETipoProduto.Servico, Nome=" * Entrada (CM)", ValorUnitario=0, CodigoAliquota="II", IDUnidade=1, IDClassificacaoFiscal=2, Disponibilidade=false, Ativo=true},
                new tbProduto() { IDProduto=4, IDTipoProduto=(int)ETipoProduto.Servico, Nome=" * Serviço", ValorUnitario=0, CodigoAliquota="II", IDUnidade=1, IDClassificacaoFiscal=2, Disponibilidade=false, Ativo=true},
                new tbProduto() { IDTipoProduto=(int)ETipoProduto.Credito, Nome=" * Crédito", ValorUnitario=0, CodigoAliquota="II", IDUnidade=1, IDClassificacaoFiscal=2, Disponibilidade=true, Ativo=true}
            };

            // TODO: Se for apagado os originais tem que apagar a tabela
            bool requestSave = false;
            while (produtosPadrao.Count > 0)
            {
                var prod = produtosPadrao[0];
                prod.DtAlteracaoDisponibilidade = DateTime.Now;
                prod.DtUltimaAlteracao = DateTime.Now;
                if (prod.IDProduto > 0)
                {
                    if (context.tbProdutoes.Find(prod.IDProduto) == null)
                    {
                        requestSave = true;
                        context.tbProdutoes.Add(prod);
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (context.tbProdutoes.Where(p => p.IDTipoProduto == prod.IDTipoProduto).Count() == 0)
                    {
                        if (requestSave)
                        {
                            requestSave = false;
                            // Para garantir criar com os ID Corretos!
                            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[tbProduto] ON");
                            context.SaveChanges();
                            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[tbProduto] OFF");
                        }
                        context.tbProdutoes.Add(prod);
                    }
                }
                produtosPadrao.RemoveAt(0);
                //transaction.Commit();
            }

        }
    }
}
