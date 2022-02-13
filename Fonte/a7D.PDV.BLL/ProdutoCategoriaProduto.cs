using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ProdutoCategoriaProduto
    {
        public static ProdutoCategoriaProdutoInformation Carregar(int idProdutoCategoriaProduto)
        {
            var obj = new ProdutoCategoriaProdutoInformation { IDProdutoCategoriaProduto = idProdutoCategoriaProduto };
            return CRUD.Carregar(obj) as ProdutoCategoriaProdutoInformation;
        }

        public static ProdutoCategoriaProdutoInformation CarregarCompleto(int idProdutoCategoriaProduto)
        {
            var produtoCategoriaProduto = Carregar(idProdutoCategoriaProduto);
            produtoCategoriaProduto.Produto = Produto.Carregar(produtoCategoriaProduto.Produto.IDProduto.Value);
            produtoCategoriaProduto.CategoriaProduto = CategoriaProduto.Carregar(produtoCategoriaProduto.CategoriaProduto.IDCategoriaProduto.Value);
            return produtoCategoriaProduto;
        }

        public static List<ProdutoCategoriaProdutoInformation> Listar(ProdutoCategoriaProdutoInformation obj = null)
        {
            if (obj == null)
                obj = new ProdutoCategoriaProdutoInformation();

            var lista = CRUD.Listar(obj).Cast<ProdutoCategoriaProdutoInformation>().ToList();
            return lista;
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarComCategoria(ProdutoCategoriaProdutoInformation obj = null)
        {
            var lista = Listar(obj);
            var retorno = new List<ProdutoCategoriaProdutoInformation>();
            var categorias = CategoriaProduto.Listar();
            foreach (var pc in lista)
            {
                pc.CategoriaProduto = categorias.FirstOrDefault(c => c.IDCategoriaProduto == pc.CategoriaProduto.IDCategoriaProduto.Value);
                retorno.Add(pc);
            }

            return retorno;
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarCompleto()
        {
            var obj = new ProdutoCategoriaProdutoInformation();
            var lista = CRUD.Listar(obj).Cast<ProdutoCategoriaProdutoInformation>().ToList();
            var retorno = new List<ProdutoCategoriaProdutoInformation>();
            var listaCategoria = CategoriaProduto.Listar();
            var listaProdutos = Produto.Listar(new ProdutoInformation() { Ativo = true, Excluido = false });
            foreach (var pc in lista)
            {
                pc.CategoriaProduto = listaCategoria.FirstOrDefault(c => c.IDCategoriaProduto == pc.CategoriaProduto?.IDCategoriaProduto);
                pc.Produto = listaProdutos.FirstOrDefault(p => p.IDProduto == pc.Produto?.IDProduto);
                retorno.Add(pc);
            }

            return retorno;
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarPorProduto(int idProduto)
        {
            var obj = new ProdutoCategoriaProdutoInformation { Produto = new ProdutoInformation { IDProduto = idProduto } };
            return CRUD.Listar(obj).Cast<ProdutoCategoriaProdutoInformation>().ToList();
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarPorCategoria(int idCategoria)
        {
            var obj = new ProdutoCategoriaProdutoInformation { CategoriaProduto = new CategoriaProdutoInformation { IDCategoriaProduto = idCategoria } };
            return CRUD.Listar(obj).Cast<ProdutoCategoriaProdutoInformation>().ToList();
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarPorCategoriaCompleto(int idCategoria)
        {
            var list = ListarPorCategoria(idCategoria);
            foreach (var item in list)
            {
                item.Produto = Produto.Carregar(item.Produto.IDProduto.Value);
                item.CategoriaProduto = CategoriaProduto.Carregar(item.CategoriaProduto.IDCategoriaProduto.Value);
            }
            return list;
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarPorCategoriaCompleto(CategoriaProdutoInformation categoria)
        {
            var list = ListarPorCategoria(categoria.IDCategoriaProduto.Value);
            foreach (var item in list)
            {
                item.Produto = Produto.Carregar(item.Produto.IDProduto.Value);
                item.CategoriaProduto = categoria;
            }
            return list;
        }

        public static List<ProdutoCategoriaProdutoInformation> ListarPorProdutoCompleto(int idProduto)
        {
            var lista = ListarPorProduto(idProduto);
            var retrono = new List<ProdutoCategoriaProdutoInformation>();
            foreach (var pcp in lista)
                retrono.Add(CarregarCompleto(pcp.IDProdutoCategoriaProduto.Value));

            return retrono;
        }

        public static string CategoriasPorProduto(int idProduto)
        {
            var listaCategorias = ListarPorProdutoCompleto(idProduto);
            string categorias = string.Empty;

            foreach (var pcp in listaCategorias)
            {
                categorias += pcp.CategoriaProduto.Nome + "\n";
            }

            if (categorias.Length > 1)
            {
                categorias = categorias.Substring(0, categorias.Length - 1);
            }

            return categorias;
        }

        public static void Salvar(ProdutoCategoriaProdutoInformation produtoCategoriaProduto)
        {
            CRUD.Salvar(produtoCategoriaProduto);
        }

        public static void SalvarLista(List<ProdutoCategoriaProdutoInformation> produtoCategoriaProdutos)
        {
            foreach (var item in produtoCategoriaProdutos)
            {
                switch (item.StatusModel)
                {
                    case StatusModel.Inalterado:
                        break;
                    case StatusModel.Novo:
                        item.IDProdutoCategoriaProduto = null;
                        Adicionar(item);
                        break;
                    case StatusModel.Alterado:
                        Alterar(item);
                        break;
                    case StatusModel.Excluido:
                        Excluir(item);
                        break;
                }
            }
        }

        private static void Alterar(ProdutoCategoriaProdutoInformation produtoCategoriaProduto)
        {
            CRUD.Alterar(produtoCategoriaProduto);
        }

        private static void Adicionar(ProdutoCategoriaProdutoInformation produtoCategoriaProduto)
        {
            CRUD.Adicionar(produtoCategoriaProduto);
        }

        public static void Excluir(ProdutoCategoriaProdutoInformation produtoCategoriaProduto)
        {
            CRUD.Excluir(produtoCategoriaProduto);
        }

        public static void Excluir(int idProdutoCategoriaProduto)
        {
            Excluir(new ProdutoCategoriaProdutoInformation { IDProdutoCategoriaProduto = idProdutoCategoriaProduto });
        }

        public static void ExcluirPorCategoria(int idCategoriaProduto)
        {
            Excluir(new ProdutoCategoriaProdutoInformation { CategoriaProduto = new CategoriaProdutoInformation { IDCategoriaProduto = idCategoriaProduto } });
        }
    }
}
