using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class MovimentacaoProdutos
    {
        public static List<MovimentacaoProdutosInformation> Listar()
        {
            List<MovimentacaoProdutosInformation> list = CRUD.Listar(new MovimentacaoProdutosInformation()).Cast<MovimentacaoProdutosInformation>().ToList();

            return list;
        }

        public static List<MovimentacaoProdutosInformation> ListarPorMovimentacao(int idMovimentacao)
        {
            var lista = Listar().Where(mp => mp.Movimentacao.IDMovimentacao == idMovimentacao).ToList();

            foreach (var mp in lista)
            {
                mp.Produto = Produto.Carregar(mp.Produto.IDProduto.Value);
                mp.Unidade = Unidade.Carregar(mp.Unidade.IDUnidade.Value);
            };

            return lista;
        }

        public static List<MovimentacaoProdutosInformation> ListarPorMovimentacao(MovimentacaoInformation movimentacao)
        {
            if (movimentacao.IDMovimentacao.HasValue)
                return ListarPorMovimentacao(movimentacao.IDMovimentacao.Value);
            return new List<MovimentacaoProdutosInformation>();
        }

        public static MovimentacaoProdutosInformation Carregar(int idMovimentacaoProdutos)
        {
            MovimentacaoProdutosInformation obj = new MovimentacaoProdutosInformation { IDMovimentacaoProdutos = idMovimentacaoProdutos };
            return (MovimentacaoProdutosInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(MovimentacaoProdutosInformation obj)
        {
            if (obj.IDMovimentacaoProdutos.HasValue && obj.IDMovimentacaoProdutos.Value < 0)
                obj.IDMovimentacaoProdutos = null;
            switch (obj.StatusModel)
            {
                case StatusModel.Inalterado:
                case StatusModel.Novo:
                case StatusModel.Alterado:
                    CRUD.Salvar(obj);
                    break;
                case StatusModel.Excluido:
                    Excluir(obj);
                    break;
                default:
                    break;
            }
        }

        private static void Excluir(MovimentacaoProdutosInformation obj)
        {
            CRUD.Excluir(obj);
        }

        public static void Adicionar(MovimentacaoProdutosInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(MovimentacaoProdutosInformation obj)
        {
            CRUD.Alterar(obj);
        }
    }
}
