using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class TipoEntrada
    {
        public static List<TipoEntradaInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new TipoEntradaInformation());
            List<TipoEntradaInformation> lista = listaObj.ConvertAll(new Converter<Object, TipoEntradaInformation>(TipoEntradaInformation.ConverterObjeto));

            return lista;
        }

        public static TipoEntradaInformation Carregar(Int32 idTipoEntrada)
        {
            TipoEntradaInformation obj = new TipoEntradaInformation { IDTipoEntrada = idTipoEntrada };
            obj = (TipoEntradaInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(TipoEntradaInformation obj)
        {
            if (obj.IDTipoEntrada.HasValue)
            {
                var padraoAntigo = BuscarPadrao();
                if (padraoAntigo != null && padraoAntigo?.IDTipoEntrada != obj.IDTipoEntrada.Value && obj.Padrao == true)
                {
                    padraoAntigo.Padrao = false;
                    CRUD.Salvar(padraoAntigo);
                }
            }
            CRUD.Salvar(obj);
        }

        public static TipoEntradaInformation BuscarPadrao()
        {
            return CRUD.Listar(new TipoEntradaInformation { Padrao = true })
                .Cast<TipoEntradaInformation>().FirstOrDefault();
        }

        public static void Excluir(Int32 idTipoEntrada)
        {
            try
            {
                TipoEntradaInformation obj = new TipoEntradaInformation { IDTipoEntrada = idTipoEntrada };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
