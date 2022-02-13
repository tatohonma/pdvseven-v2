using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class PainelModificacaoCategoria
    {
        public static List<PainelModificacaoCategoriaInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new PainelModificacaoCategoriaInformation());
            List<PainelModificacaoCategoriaInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoCategoriaInformation>(PainelModificacaoCategoriaInformation.ConverterObjeto));

            return lista;
        }

        public static List<PainelModificacaoCategoriaInformation> Listar(Int32 idPainelModificacao)
        {
            PainelModificacaoCategoriaInformation obj = new PainelModificacaoCategoriaInformation();
            obj.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };

            List<Object> listaObj = CRUD.Listar(obj);
            List<PainelModificacaoCategoriaInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoCategoriaInformation>(PainelModificacaoCategoriaInformation.ConverterObjeto));
            lista = lista.OrderBy(l => l.Ordem).ToList();
            var categorias = new List<CategoriaProdutoInformation>(CategoriaProduto.Listar());
            categorias.AddRange(CategoriaProduto.ListarExcluidos());
            foreach (var item in lista)
                item.Categoria = categorias.FirstOrDefault(p => p.IDCategoriaProduto == item.Categoria.IDCategoriaProduto);

            return lista;
        }

        public static List<PainelModificacaoCategoriaInformation> ListarCompleto()
        {
            List<PainelModificacaoCategoriaInformation> listaPainelModificacaoCategoria = Listar();

            foreach (var PainelModificacaoCategoria in listaPainelModificacaoCategoria)
            {
                if (PainelModificacaoCategoria.PainelModificacao != null && PainelModificacaoCategoria.PainelModificacao.IDPainelModificacao.HasValue)
                    PainelModificacaoCategoria.PainelModificacao = PainelModificacao.Carregar(PainelModificacaoCategoria.PainelModificacao.IDPainelModificacao.Value);

                if (PainelModificacaoCategoria.Categoria != null && PainelModificacaoCategoria.Categoria.IDCategoriaProduto.HasValue)
                    PainelModificacaoCategoria.Categoria = CategoriaProduto.Carregar(PainelModificacaoCategoria.Categoria.IDCategoriaProduto.Value);
            }
            return listaPainelModificacaoCategoria;
        }

        public static List<PainelModificacaoCategoriaInformation> ListarCompletoPorIdPainelModificacao(int iDPainelModificacao)
        {
            List<PainelModificacaoCategoriaInformation> listaPainelModificacaoCategoria = ListarCompleto();

            var lista = listaPainelModificacaoCategoria.Where(l => l.PainelModificacao.IDPainelModificacao.Value == iDPainelModificacao).ToList();

            return lista;
        }

        public static PainelModificacaoCategoriaInformation Carregar(Int32 idPainelModificacaoCategoria)
        {
            PainelModificacaoCategoriaInformation obj = new PainelModificacaoCategoriaInformation { IDPainelModificacaoCategoria = idPainelModificacaoCategoria };
            obj = (PainelModificacaoCategoriaInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(PainelModificacaoCategoriaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void SalvarLista(List<PainelModificacaoCategoriaInformation> listaPainelModificacaoCategoria)
        {
            foreach (PainelModificacaoCategoriaInformation PainelModificacaoCategoria in listaPainelModificacaoCategoria)
            {
                switch (PainelModificacaoCategoria.StatusModel)
                {
                    case StatusModel.Inalterado:
                        break;
                    case StatusModel.Novo:
                        Adicionar(PainelModificacaoCategoria);
                        break;
                    case StatusModel.Alterado:
                        Alterar(PainelModificacaoCategoria);
                        break;
                    case StatusModel.Excluido:
                        Excluir((int)PainelModificacaoCategoria.IDPainelModificacaoCategoria);
                        break;
                }
            }
        }

        public static void Adicionar(PainelModificacaoCategoriaInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(PainelModificacaoCategoriaInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idPainelModificacaoCategoria)
        {
            try
            {
                PainelModificacaoCategoriaInformation obj = new PainelModificacaoCategoriaInformation { IDPainelModificacaoCategoria = idPainelModificacaoCategoria };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static void ExcluirPorPainel(Int32 idPainelModificacao)
        {
            PainelModificacaoCategoriaInformation obj = new PainelModificacaoCategoriaInformation();
            obj.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };

            CRUD.Excluir(obj);
        }
    }
}
