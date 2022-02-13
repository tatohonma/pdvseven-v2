using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace a7D.PDV.BLL
{
    public static class Movimentacao
    {
        public static List<MovimentacaoInformation> Listar()
        {
            List<MovimentacaoInformation> list = CRUD.Listar(new MovimentacaoInformation()).Cast<MovimentacaoInformation>().ToList();
            list.ForEach(obj =>
            {
                obj.TipoMovimentacao = TipoMovimentacao.Carregar(obj.TipoMovimentacao.IDTipoMovimentacao.Value);
            });

            return list;
        }

        public static MovimentacaoInformation Carregar(int idMovimentacao)
        {
            MovimentacaoInformation obj = new MovimentacaoInformation { IDMovimentacao = idMovimentacao };
            obj = (MovimentacaoInformation)CRUD.Carregar(obj);
            if (obj.TipoMovimentacao != null)
                obj.TipoMovimentacao = TipoMovimentacao.Carregar(obj.TipoMovimentacao.IDTipoMovimentacao.Value);
            obj.MovimentacaoProdutos = MovimentacaoProdutos.ListarPorMovimentacao(idMovimentacao);
            return obj;
        }

        public static void Salvar(MovimentacaoInformation obj)
        {
            CRUD.Salvar(obj);
            if (obj.MovimentacaoProdutos != null && obj.Excluido == false)
            {
                foreach (var mp in obj.MovimentacaoProdutos)
                {
                    mp.Movimentacao = obj;
                    MovimentacaoProdutos.Salvar(mp);
                }
            }
        }

        public static void Excluir(int idMovimentacao)
        {
            using (var scope = new TransactionScope())
            {
                var obj = Carregar(idMovimentacao);
                obj.Excluido = true;

                //if (obj.Processado == true)
                //{
                //    var reversa = new MovimentacaoInformation
                //    {
                //        MovimentacaoProdutos = new List<MovimentacaoProdutosInformation>(),
                //        TipoMovimentacao = new TipoMovimentacaoInformation
                //        {
                //            IDTipoMovimentacao = obj.TipoMovimentacao.IDTipoMovimentacao == 1 ? 2 : 1
                //        },
                //        DataMovimentacao = DateTime.Now,
                //        Reversa = true,
                //        Processado = false,
                //        GUID = Guid.NewGuid().ToString(),
                //        Excluido = false
                //    };
                //    foreach (var mp in obj.MovimentacaoProdutos)
                //    {
                //        reversa.MovimentacaoProdutos.Add(new MovimentacaoProdutosInformation
                //        {
                //            Movimentacao = reversa,
                //            Produto = mp.Produto,
                //            Quantidade = mp.Quantidade,
                //            StatusModel = StatusModel.Novo,
                //            Unidade = mp.Unidade
                //        });
                //    }
                //    Salvar(reversa);
                //    EntradaSaida.Movimentar(reversa);
                //    reversa.Processado = true;
                //    Salvar(reversa);
                //    obj.MovimentacaoReversa = reversa;
                //}

                Salvar(obj);
                scope.Complete();
            }
        }

        public static int MovimentacoesApos(DateTime data)
        {
            return Listar()
                .Where(p => p.Excluido == false)
                .Where(p => p.Processado == true)
                .Where(p => p.DataMovimentacao.Value >= data)
                .Count();
        }

        public static void Excluir(MovimentacaoInformation movimentacao)
        {
            if (movimentacao.IDMovimentacao.HasValue)
                Excluir(movimentacao.IDMovimentacao.Value);
        }
    }
}
