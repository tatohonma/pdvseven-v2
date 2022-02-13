using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class TemaCardapio
    {
        public static List<TemaCardapioInformation> Listar()
        {
            List<object> listaObj = CRUD.Listar(new TemaCardapioInformation());
            List<TemaCardapioInformation> lista = listaObj.Cast<TemaCardapioInformation>().ToList();

            return lista;
        }

        public static TemaCardapioInformation Carregar(int idTemaCardapio)
        {
            TemaCardapioInformation obj = new TemaCardapioInformation { IDTemaCardapio = idTemaCardapio };
            obj = (TemaCardapioInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static TemaCardapioInformation CarregarPorNome(string nomeTema)
        {
            TemaCardapioInformation obj = new TemaCardapioInformation { Nome = nomeTema };
            obj = (TemaCardapioInformation)CRUD.Carregar(obj);

            if (obj.IDTemaCardapio.HasValue)
                return obj;

            return null;
        }

        public static void Salvar(TemaCardapioInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(int idTemaCardapio)
        {
            try
            {
                TemaCardapioInformation obj = new TemaCardapioInformation { IDTemaCardapio = idTemaCardapio };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
