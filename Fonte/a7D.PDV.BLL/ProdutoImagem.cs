using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public static class ProdutoImagem
    {
        public static void Salvar(ProdutoImagemInformation produtoImagem)
        {
            CRUD.Salvar(produtoImagem);
        }

        public static void Adicionar(ProdutoImagemInformation produtoImagem)
        {
            CRUD.Adicionar(produtoImagem);
        }

        public static List<ProdutoImagemInformation> Listar()
        {
            ProdutoImagemInformation obj = new ProdutoImagemInformation { Produto = new ProdutoInformation { Ativo = true } };
            return CRUD.Listar(obj).Cast<ProdutoImagemInformation>().ToList();
        }

        public static List<ProdutoImagemInformation> ListarCompleto()
        {
            ProdutoImagemInformation obj = new ProdutoImagemInformation { Produto = new ProdutoInformation { Ativo = true } };
            var lista = CRUD.Listar(obj).Cast<ProdutoImagemInformation>().ToList();
            foreach (var pi in lista)
            {
                pi.Imagem = Imagem.CarregarCompleto(pi.Imagem.IDImagem.Value);
            }
            return lista;
        }

        public static ProdutoImagemInformation CarregarPorProduto(int idProduto)
        {
            var obj = new ProdutoImagemInformation { Produto = new ProdutoInformation { IDProduto = idProduto } };
            return CRUD.Carregar(obj) as ProdutoImagemInformation;
        }

        public static ProdutoImagemInformation Carregar(int idProdutoImagem)
        {
            var obj = new ProdutoImagemInformation { IDProdutoImagem = idProdutoImagem };
            return CRUD.Carregar(obj) as ProdutoImagemInformation;
        }

        public static void Excluir(ProdutoImagemInformation produtoInformation)
        {
            CRUD.Excluir(produtoInformation);
        }

        private static void Excluir(int idProdutoImagem)
        {
            CRUD.Excluir(new ProdutoImagemInformation { IDProdutoImagem = idProdutoImagem });
        }

        public static bool Existe(int idProduto)
        {
            var produtoImagem = CarregarPorProduto(idProduto);
            return produtoImagem != null && produtoImagem.Imagem != null && produtoImagem.Imagem.IDImagem.HasValue;
        }

    }
}
