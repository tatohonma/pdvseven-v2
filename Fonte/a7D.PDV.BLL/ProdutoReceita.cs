using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class ProdutoReceita
    {
        public static List<ProdutoReceitaInformation> Listar()
        {
            List<ProdutoReceitaInformation> list = CRUD.Listar(new ProdutoReceitaInformation()).Cast<ProdutoReceitaInformation>().ToList();

            return list;
        }

        public static List<ProdutoReceitaInformation> ListarPorProduto(ProdutoInformation produto)
        {
            if (produto == null)
                return Enumerable.Empty<ProdutoReceitaInformation>().ToList();
            return ListarPorProduto(produto.IDProduto.Value);
        }

        public static List<ProdutoReceitaInformation> ListarPorProduto(int idProduto)
        {
            var lista = Listar().Where(pr => pr.Produto.IDProduto == idProduto).ToList();

            foreach (var item in lista)
            {
                item.Produto = Produto.Carregar(item.Produto.IDProduto.Value);
                item.Produto.Unidade = Unidade.Carregar(item.Produto.Unidade.IDUnidade.Value);

                item.ProdutoIngrediente = Produto.Carregar(item.ProdutoIngrediente.IDProduto.Value);
                item.ProdutoIngrediente.Unidade = Unidade.Carregar(item.ProdutoIngrediente.Unidade.IDUnidade.Value);

                item.Unidade = Unidade.Carregar(item.Unidade.IDUnidade.Value);
            }

            return lista;
        }

        public static ProdutoReceitaInformation Carregar(int idProdutoReceita)
        {
            ProdutoReceitaInformation obj = new ProdutoReceitaInformation { IDProdutoReceita = idProdutoReceita };
            return (ProdutoReceitaInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(ProdutoReceitaInformation obj)
        {
            if (obj.IDProdutoReceita.HasValue && obj.IDProdutoReceita.Value < 0)
                obj.IDProdutoReceita = null;
            switch (obj.StatusModel)
            {
                case StatusModel.Inalterado:
                case StatusModel.Novo:
                case StatusModel.Alterado:
                    CRUD.Salvar(obj);
                    break;
                case StatusModel.Excluido:
                    if (!obj.IDProdutoReceita.HasValue)
                        return;
                    Excluir(obj);
                    break;
                default:
                    break;
            }
        }

        public static void Adicionar(ProdutoReceitaInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ProdutoReceitaInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idProdutoReceita)
        {
            Excluir(Carregar(idProdutoReceita));
        }

        public static void Excluir(ProdutoReceitaInformation produtoReceita)
        {
            CRUD.Excluir(produtoReceita);
        }

        public static void Salvar(List<ProdutoReceitaInformation> listaProdutoReceita)
        {
            foreach (var obj in listaProdutoReceita)
                Salvar(obj);
        }
    }
}
