using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class RetornoSAT
    {
        public static List<RetornoSATInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new RetornoSATInformation());
            List<RetornoSATInformation> lista = listaObj.ConvertAll(new Converter<Object, RetornoSATInformation>(RetornoSATInformation.ConverterObjeto));

            return lista;
        }

        public static List<RetornoSATInformation> ListarCompleto()
        {
            var lista = Listar();
            foreach (var item in lista)
            {
                if (item?.RetornoSATCancelamento?.IDRetornoSAT != null)
                {
                    item.RetornoSATCancelamento = Carregar(item.RetornoSATCancelamento.IDRetornoSAT.Value);
                }
            }

            return lista;
        }

        public static RetornoSATInformation Carregar(Int32 idPedidoRetornoSAT)
        {
            RetornoSATInformation obj = new RetornoSATInformation { IDRetornoSAT = idPedidoRetornoSAT };
            obj = (RetornoSATInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(RetornoSATInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idPedidoRetornoSAT)
        {
            try
            {
                RetornoSATInformation obj = new RetornoSATInformation { IDRetornoSAT = idPedidoRetornoSAT };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
