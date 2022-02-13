using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    [Obsolete("", true)]
    public class CategoriaImposto
    {
        public static List<CategoriaImpostoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new CategoriaImpostoInformation());
            List<CategoriaImpostoInformation> lista = listaObj.ConvertAll(new Converter<Object, CategoriaImpostoInformation>(CategoriaImpostoInformation.ConverterObjeto));

            return lista;
        }

        public static CategoriaImpostoInformation Carregar(Int32 idCategoriaImposto)
        {
            CategoriaImpostoInformation obj = new CategoriaImpostoInformation { IDCategoriaImposto = idCategoriaImposto };
            obj = (CategoriaImpostoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(CategoriaImpostoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idCategoriaImposto)
        {
            try
            {
                CategoriaImpostoInformation obj = new CategoriaImpostoInformation { IDCategoriaImposto = idCategoriaImposto };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
