using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class ProdutoPainelModificacao
    {
        public static List<ProdutoPainelModificacaoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new ProdutoPainelModificacaoInformation());
            List<ProdutoPainelModificacaoInformation> lista = listaObj.ConvertAll(new Converter<Object, ProdutoPainelModificacaoInformation>(ProdutoPainelModificacaoInformation.ConverterObjeto));

            return lista;
        }

        public static List<ProdutoPainelModificacaoInformation> Listar(Int32 idProduto)
        {
            ProdutoPainelModificacaoInformation obj = new ProdutoPainelModificacaoInformation();
            obj.Produto = new ProdutoInformation { IDProduto = idProduto };

            List<Object> listaObj = CRUD.Listar(obj);
            List<ProdutoPainelModificacaoInformation> lista = listaObj.ConvertAll(new Converter<Object, ProdutoPainelModificacaoInformation>(ProdutoPainelModificacaoInformation.ConverterObjeto));
            lista = lista.OrderBy(l => l.Ordem).ToList();

            foreach (var item in lista)
                item.PainelModificacao = PainelModificacao.CarregarCompleto(item.PainelModificacao.IDPainelModificacao.Value);

            return lista;
        }

        public static List<ProdutoPainelModificacaoInformation> ListarCompleto()
        {
            List<ProdutoPainelModificacaoInformation> listaProdutoPainelModificacao = Listar();

            foreach (var produtoPainelModificacao in listaProdutoPainelModificacao)
            {
                if (produtoPainelModificacao.PainelModificacao != null && produtoPainelModificacao.PainelModificacao.IDPainelModificacao.HasValue)
                    produtoPainelModificacao.PainelModificacao = PainelModificacao.Carregar(produtoPainelModificacao.PainelModificacao.IDPainelModificacao.Value);

                if (produtoPainelModificacao.Produto != null && produtoPainelModificacao.Produto.IDProduto.HasValue)
                    produtoPainelModificacao.Produto = Produto.Carregar(produtoPainelModificacao.Produto.IDProduto.Value);

            }

            return listaProdutoPainelModificacao;
        }

        public static List<ProdutoPainelModificacaoInformation> ListarCompletoPorIdProduto(int idProduto)
        {
            List<ProdutoPainelModificacaoInformation> listaPainelModificacaoProduto = ListarCompleto();

            var lista = listaPainelModificacaoProduto.Where(l => l.Produto.IDProduto.Value == idProduto).ToList();

            return lista;
        }

        public static ProdutoPainelModificacaoInformation Carregar(Int32 idProdutoPainelModificacao)
        {
            ProdutoPainelModificacaoInformation obj = new ProdutoPainelModificacaoInformation { IDProdutoPainelModificacao = idProdutoPainelModificacao };
            obj = (ProdutoPainelModificacaoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(ProdutoPainelModificacaoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void SalvarLista(List<ProdutoPainelModificacaoInformation> listaProdutoPainelModificacao)
        {
            foreach (ProdutoPainelModificacaoInformation ProdutoPainelModificacao in listaProdutoPainelModificacao)
            {
                switch (ProdutoPainelModificacao.StatusModel)
                {
                    case StatusModel.Inalterado:
                        break;
                    case StatusModel.Novo:
                        Adicionar(ProdutoPainelModificacao);
                        break;
                    case StatusModel.Alterado:
                        Alterar(ProdutoPainelModificacao);
                        break;
                    case StatusModel.Excluido:
                        Excluir((int)ProdutoPainelModificacao.IDProdutoPainelModificacao);
                        break;
                }
            }
        }

        public static void Adicionar(ProdutoPainelModificacaoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ProdutoPainelModificacaoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idProdutoPainelModificacao)
        {
            try
            {
                ProdutoPainelModificacaoInformation obj = new ProdutoPainelModificacaoInformation { IDProdutoPainelModificacao = idProdutoPainelModificacao };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static void ExcluirPorProduto(Int32 idProduto)
        {
            ProdutoPainelModificacaoInformation obj = new ProdutoPainelModificacaoInformation();
            obj.Produto = new ProdutoInformation { IDProduto = idProduto };

            CRUD.Excluir(obj);
        }
    }
}
