using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ProdutoTraducao
    {
        public static List<ProdutoTraducaoInformation> Listar()
        {
            List<Object> listObj = CRUD.Listar(new ProdutoTraducaoInformation());
            List<ProdutoTraducaoInformation> list = listObj.ConvertAll(new Converter<Object, ProdutoTraducaoInformation>(ProdutoTraducaoInformation.ConverterObjeto));

            return list;
        }

        public static List<ProdutoTraducaoInformation> ListarIdiomaCompleto()
        {
            List<Object> listObj = CRUD.Listar(new ProdutoTraducaoInformation());
            List<ProdutoTraducaoInformation> listaProdutoTraducao = Listar();

            foreach (var item in listaProdutoTraducao)
            {
                if (item.Idioma != null && item.Idioma.IDIdioma.HasValue)
                    item.Idioma = Idioma.Carregar(item.Idioma.IDIdioma.Value);
            }
            return listaProdutoTraducao;
        }

        public static ProdutoTraducaoInformation Carregar(Int32 idProdutoTraducao)
        {
            ProdutoTraducaoInformation obj = new ProdutoTraducaoInformation { IDProdutoTraducao = idProdutoTraducao };
            return (ProdutoTraducaoInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(ProdutoTraducaoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(ProdutoTraducaoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ProdutoTraducaoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idProdutoTraducao)
        {
            ProdutoTraducaoInformation obj = new ProdutoTraducaoInformation { IDProdutoTraducao = idProdutoTraducao };
            CRUD.Excluir(obj);
        }

        public static List<ProdutoTraducaoInformation> ListarCompletoPorProduto(int idProduto)
        {
            List<ProdutoTraducaoInformation> listaProdutoTraducao = ListarIdiomaCompleto();

            var lista = listaProdutoTraducao.Where(l => l.Produto.IDProduto.Value == idProduto).ToList();

            return lista;
        }

        public static List<ProdutoTraducaoInformation> ListarPorProduto(int idProduto)
        {
            var lista = Listar().Where(pt => pt.Produto.IDProduto.Value == idProduto).ToList();
            foreach (var item in lista)
                item.Idioma = Idioma.Carregar(item.Idioma.IDIdioma.Value);
            return lista;
        }

        public static void SalvarLista(List<ProdutoTraducaoInformation> listaProdutoTraducao)
        {
            foreach (ProdutoTraducaoInformation produtotraducao in listaProdutoTraducao)
            {
                switch (produtotraducao.StatusModel)
                {
                    case StatusModel.Inalterado:
                        break;
                    case StatusModel.Novo:
                        Adicionar(produtotraducao);
                        break;
                    case StatusModel.Alterado:
                        Alterar(produtotraducao);
                        break;
                    case StatusModel.Excluido:
                        Excluir((int)produtotraducao.IDProdutoTraducao);
                        break;
                }
            }
        }
    }
}
