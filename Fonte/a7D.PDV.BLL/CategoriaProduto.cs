using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model.DTO;

namespace a7D.PDV.BLL
{
    public class CategoriaProduto
    {
        public static CategoriaProdutoInformation Carregar(Int32 idCategoriaProduto)
        {
            CategoriaProdutoInformation obj = new CategoriaProdutoInformation { IDCategoriaProduto = idCategoriaProduto };
            obj = (CategoriaProdutoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(CategoriaProdutoInformation obj)
        {
            obj.DtUltimaAlteracao = DateTime.Now;
            obj.Excluido = false;
            if (obj.IDCategoriaProduto == null)
            {
                obj.Disponibilidade = true;
                obj.DtAlteracaoDisponibilidade = DateTime.Now;
            }
            CRUD.Salvar(obj);
        }

        public static List<CategoriaProdutoInformation> Listar(CategoriaProdutoInformation objFiltro = null)
        {
            if (objFiltro == null)
            {
                objFiltro = new CategoriaProdutoInformation();
                objFiltro.Excluido = false;
            }

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CategoriaProdutoInformation> lista = listaObj.ConvertAll(new Converter<Object, CategoriaProdutoInformation>(CategoriaProdutoInformation.ConverterObjeto));

            return lista;
        }



        public static List<CategoriaProdutoInformation> ListarExcluidos()
        {
            CategoriaProdutoInformation objFiltro = new CategoriaProdutoInformation();
            objFiltro.Excluido = true;

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CategoriaProdutoInformation> lista = listaObj.ConvertAll(new Converter<Object, CategoriaProdutoInformation>(CategoriaProdutoInformation.ConverterObjeto));

            return lista;
        }

        public static void Excluir(Int32 idCategoriaProduto)
        {
            try
            {
                //CategoriaProdutoInformation obj = new CategoriaProdutoInformation { IDCategoriaProduto = idCategoriaProduto };
                //CRUD.Excluir(obj);

                CategoriaProdutoInformation obj = Carregar(idCategoriaProduto);
                obj.Excluido = true;
                obj.DtUltimaAlteracao = DateTime.Now;

                CRUD.Salvar(obj);
                ProdutoCategoriaProduto.ExcluirPorCategoria(idCategoriaProduto);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static IEnumerable<CategoriaAtiva> CategoriasAtivas()
        {
            return DAL.CategoriaProdutoDAL.CategoriasAtivas();
        }

        public static void AlterarDisponibilidade(int idCategoria, bool disponivel)
        {
            DAL.CategoriaProdutoDAL.AlterarDisponibilidade(idCategoria, disponivel);
        }
    }
}
