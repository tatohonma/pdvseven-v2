using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class PainelModificacaoProduto
    {
        public static List<PainelModificacaoProdutoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new PainelModificacaoProdutoInformation());
            List<PainelModificacaoProdutoInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoProdutoInformation>(PainelModificacaoProdutoInformation.ConverterObjeto));

            return lista;
        }

        public static List<PainelModificacaoProdutoInformation> Listar(Int32 idPainelModificacao,List<ProdutoInformation> produtos)
        {
            PainelModificacaoProdutoInformation obj = new PainelModificacaoProdutoInformation();
            obj.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };

            List<Object> listaObj = CRUD.Listar(obj);
            List<PainelModificacaoProdutoInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoProdutoInformation>(PainelModificacaoProdutoInformation.ConverterObjeto));
            lista = lista.OrderBy(l => l.Ordem).ToList();

            //produtos.AddRange(Produto.ListarExcluidos());
            foreach (var item in lista)
                item.Produto = produtos.FirstOrDefault(p => p.IDProduto == item.Produto.IDProduto);

            return lista;
        }

        public static List<PainelModificacaoProdutoInformation> ListarCompleto()
        {
            List<PainelModificacaoProdutoInformation> listaPainelModificacaoProduto = Listar();

            foreach (var painelModificacaoProduto in listaPainelModificacaoProduto)
            {
                if (painelModificacaoProduto.PainelModificacao != null && painelModificacaoProduto.PainelModificacao.IDPainelModificacao.HasValue)
                    painelModificacaoProduto.PainelModificacao = PainelModificacao.Carregar(painelModificacaoProduto.PainelModificacao.IDPainelModificacao.Value);

                if (painelModificacaoProduto.Produto != null && painelModificacaoProduto.Produto.IDProduto.HasValue)
                    painelModificacaoProduto.Produto = Produto.Carregar(painelModificacaoProduto.Produto.IDProduto.Value);
            }
            return listaPainelModificacaoProduto;
        }

        public static List<PainelModificacaoProdutoInformation> ListarCompletoPorIdPainelModificacao(int iDPainelModificacao)
        {
            List<PainelModificacaoProdutoInformation> listaPainelModificacaoProduto = ListarCompleto();

            var lista = listaPainelModificacaoProduto.Where(l => l.PainelModificacao.IDPainelModificacao.Value == iDPainelModificacao).ToList();

            return lista;
        }

        public static PainelModificacaoProdutoInformation Carregar(Int32 idPainelModificacaoProduto)
        {
            PainelModificacaoProdutoInformation obj = new PainelModificacaoProdutoInformation { IDPainelModificacaoProduto = idPainelModificacaoProduto };
            obj = (PainelModificacaoProdutoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(PainelModificacaoProdutoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void SalvarLista(List<PainelModificacaoProdutoInformation> listaPainelModificacaoProduto)
        {
            foreach (PainelModificacaoProdutoInformation painelModificacaoProduto in listaPainelModificacaoProduto)
            {
                switch (painelModificacaoProduto.StatusModel)
                {
                    case StatusModel.Inalterado:
                        break;
                    case StatusModel.Novo:
                        Adicionar(painelModificacaoProduto);
                        break;
                    case StatusModel.Alterado:
                        Alterar(painelModificacaoProduto);
                        break;
                    case StatusModel.Excluido:
                        Excluir((int)painelModificacaoProduto.IDPainelModificacaoProduto);
                        break;
                }
            }
        }

        public static void Adicionar(PainelModificacaoProdutoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(PainelModificacaoProdutoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idPainelModificacaoProduto)
        {
            try
            {
                PainelModificacaoProdutoInformation obj = new PainelModificacaoProdutoInformation { IDPainelModificacaoProduto = idPainelModificacaoProduto };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        internal static void ExcluirPorPainel(Int32 idPainelModificacao)
        {
            PainelModificacaoProdutoInformation obj = new PainelModificacaoProdutoInformation();
            obj.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };

            CRUD.Excluir(obj);
        }
    }
}
