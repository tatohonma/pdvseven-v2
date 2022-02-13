using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class Acao
    {
        public static List<AcaoInformation> Listar()
        {
            AcaoInformation objFiltro = new AcaoInformation();

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<AcaoInformation> lista = listaObj.ConvertAll(new Converter<Object, AcaoInformation>(AcaoInformation.ConverterObjeto));

            return lista;
        }

        public static AcaoInformation Carregar(Int32 idAcao)
        {
            AcaoInformation obj = new AcaoInformation { IDAcao = idAcao };
            obj = (AcaoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(AcaoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idAcao)
        {
            AcaoInformation objFiltro = new AcaoInformation { IDAcao = idAcao };
            CRUD.Excluir(objFiltro);
        }
    }
}
