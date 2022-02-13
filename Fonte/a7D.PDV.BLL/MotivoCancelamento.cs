using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class MotivoCancelamento
    {
        public static List<MotivoCancelamentoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new MotivoCancelamentoInformation());
            List<MotivoCancelamentoInformation> lista = listaObj.ConvertAll(new Converter<Object, MotivoCancelamentoInformation>(MotivoCancelamentoInformation.ConverterObjeto));

            return lista;
        }

        public static MotivoCancelamentoInformation Carregar(Int32 idMotivoCancelamento)
        {
            MotivoCancelamentoInformation obj = new MotivoCancelamentoInformation { IDMotivoCancelamento = idMotivoCancelamento };
            obj = (MotivoCancelamentoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(MotivoCancelamentoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idMotivoCancelamento)
        {
            try
            {
                MotivoCancelamentoInformation obj = new MotivoCancelamentoInformation { IDMotivoCancelamento = idMotivoCancelamento };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }            
        }
    }
}
