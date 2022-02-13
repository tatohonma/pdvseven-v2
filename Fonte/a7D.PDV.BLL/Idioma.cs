using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class Idioma
    {
        public static List<IdiomaInformation> Listar()
        {
            List<Object> listObj = CRUD.Listar(new IdiomaInformation());
            List<IdiomaInformation> list = listObj.ConvertAll(new Converter<Object, IdiomaInformation>(IdiomaInformation.ConverterObjeto));

            return list;
        }

        public static IdiomaInformation Carregar(Int32 idIdioma)
        {
            IdiomaInformation obj = new IdiomaInformation { IDIdioma = idIdioma };
            return (IdiomaInformation)CRUD.Carregar(obj);
        }

        public static IdiomaInformation CarregarPorCodigo(string codigo)
        {
            IdiomaInformation obj = new IdiomaInformation { Codigo = codigo };
            obj = (IdiomaInformation)CRUD.Carregar(obj);

            if (obj.IDIdioma.HasValue)
                return obj;

            return null;
        }

        public static void Salvar(IdiomaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(IdiomaInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(IdiomaInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idIdioma)
        {
            IdiomaInformation obj = new IdiomaInformation { IDIdioma = idIdioma };
            CRUD.Excluir(obj);
        }
    }
}
