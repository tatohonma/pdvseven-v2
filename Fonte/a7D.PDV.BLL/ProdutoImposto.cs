using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    [Obsolete("", true)]
    public class ProdutoImposto
    {
        public static List<ProdutoImpostoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new ProdutoImpostoInformation());
            List<ProdutoImpostoInformation> lista = listaObj.ConvertAll(new Converter<Object, ProdutoImpostoInformation>(ProdutoImpostoInformation.ConverterObjeto));

            return lista;
        }

        public static ProdutoImpostoInformation Carregar(Int32 idProdutoImposto)
        {
            ProdutoImpostoInformation obj = new ProdutoImpostoInformation { IDProdutoImposto = idProdutoImposto };
            obj = (ProdutoImpostoInformation)CRUD.Carregar(obj);

            if (obj.CategoriaImposto != null && obj.CategoriaImposto.IDCategoriaImposto != null)
                obj.CategoriaImposto = CategoriaImposto.Carregar(obj.CategoriaImposto.IDCategoriaImposto.Value);

            return obj;
        }

        public static void Salvar(ProdutoImpostoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idProdutoImposto)
        {
            try
            {
                ProdutoImpostoInformation obj = new ProdutoImpostoInformation { IDProdutoImposto = idProdutoImposto };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
