using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class PainelModificacao
    {
        public static List<PainelModificacaoInformation> Listar()
        {
            PainelModificacaoInformation obj = new PainelModificacaoInformation();
            obj.Excluido = false;

            List<Object> listaObj = CRUD.Listar(obj);
            List<PainelModificacaoInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoInformation>(PainelModificacaoInformation.ConverterObjeto));

            return lista;
        }

        public static List<PainelModificacaoInformation> ListarExcluidos()
        {
            PainelModificacaoInformation obj = new PainelModificacaoInformation();
            obj.Excluido = true;

            List<Object> listaObj = CRUD.Listar(obj);
            List<PainelModificacaoInformation> lista = listaObj.ConvertAll(new Converter<Object, PainelModificacaoInformation>(PainelModificacaoInformation.ConverterObjeto));

            return lista;
        }

        public static PainelModificacaoInformation CarregarCompleto(Int32 idPainelModificacao)
        {
            PainelModificacaoInformation obj = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };
            obj = (PainelModificacaoInformation)CRUD.Carregar(obj);

            var produtos = Produto.Listar(new ProdutoInformation() { Excluido = false, Ativo = true });
            obj.ListaProduto = PainelModificacaoProduto.Listar(obj.IDPainelModificacao.Value, produtos);
            obj.ListaCategoria = PainelModificacaoCategoria.Listar(obj.IDPainelModificacao.Value);
            obj.PainelModificacaoOperacao = PainelModificacaoOperacao.Carregar(obj.PainelModificacaoOperacao.IDPainelModificacaoOperacao.Value);
            obj.PaineisRelacionados = PainelModificacaoRelacionado.ListarCompletoPorPainel1(obj.IDPainelModificacao.Value);
            return obj;
        }

        public static PainelModificacaoInformation Carregar(Int32 idPainelModificacao)
        {
            PainelModificacaoInformation obj = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };
            return (PainelModificacaoInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(PainelModificacaoInformation obj)
        {
            obj.DtUltimaAlteracao = DateTime.Now;
            obj.Excluido = false;
            CRUD.Salvar(obj);

            PainelModificacaoProduto.ExcluirPorPainel(obj.IDPainelModificacao.Value);
            PainelModificacaoCategoria.ExcluirPorPainel(obj.IDPainelModificacao.Value);
            if (obj.ListaProduto != null)
            {

                foreach (var item in obj.ListaProduto)
                {
                    item.IDPainelModificacaoProduto = null;
                    item.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = obj.IDPainelModificacao };

                    PainelModificacaoProduto.Salvar(item);
                }

            }
            else if (obj.ListaCategoria != null)
            {

                foreach (var item in obj.ListaCategoria)
                {
                    item.IDPainelModificacaoCategoria = null;

                    PainelModificacaoCategoria.Salvar(item);
                }

            }

            if (obj.PaineisRelacionados != null)
            {
                PainelModificacaoRelacionado.ExcluirPorPainel1(obj.IDPainelModificacao.Value);
                foreach (var item in obj.PaineisRelacionados)
                {

                    PainelModificacaoRelacionado.Salvar(item);
                }
            }



        }

        public static void Excluir(Int32 idPainelModificacao)
        {
            try
            {
                //PainelModificacaoInformation obj = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao };
                //CRUD.Excluir(obj);

                PainelModificacaoInformation obj = CarregarCompleto(idPainelModificacao);
                obj.Excluido = true;
                obj.DtUltimaAlteracao = DateTime.Now;

                CRUD.Salvar(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static List<PainelModificacaoInformation> ListarCompleto()
        {
            List<PainelModificacaoInformation> lista = Listar();

            var produtos = Produto.Listar(new ProdutoInformation() { Excluido = false })
                .OrderByDescending(p => p.Ativo).ToList();

            foreach (var item in lista)
            {
                item.ListaProduto = PainelModificacaoProduto.Listar(item.IDPainelModificacao.Value, produtos);
                item.ListaCategoria = PainelModificacaoCategoria.Listar(item.IDPainelModificacao.Value);
                item.PainelModificacaoOperacao = PainelModificacaoOperacao.Carregar(item.PainelModificacaoOperacao.IDPainelModificacaoOperacao.Value);
                item.PaineisRelacionados = PainelModificacaoRelacionado.ListarCompletoPorPainel1(item.IDPainelModificacao.Value);
            }

            return lista;
        }
    }
}
