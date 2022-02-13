using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class Estado
    {
        public static List<EstadoInformation> Listar()
        {
            EstadoInformation objFiltro = new EstadoInformation();

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<EstadoInformation> lista = listaObj.ConvertAll(new Converter<Object, EstadoInformation>(EstadoInformation.ConverterObjeto));

            return lista;
        }

        public static EstadoInformation Carregar(Int32 idEstado)
        {
            EstadoInformation obj = new EstadoInformation { IDEstado = idEstado };
            obj = (EstadoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(EstadoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idEstado)
        {
            EstadoInformation objFiltro = new EstadoInformation { IDEstado = idEstado };
            CRUD.Excluir(objFiltro);
        }
    }
}
