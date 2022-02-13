using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class TemaCardapioPDV
    {
        public static List<TemaCardapioPDVInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new TemaCardapioPDVInformation());
            List<TemaCardapioPDVInformation> lista = listaObj.ConvertAll(new Converter<Object, TemaCardapioPDVInformation>(TemaCardapioPDVInformation.ConverterObjeto));

            foreach (var item in lista)
                if (item.PDV != null)
                    item.PDV = PDV.Carregar(item.PDV.IDPDV.Value);

            foreach (var item in lista)
                item.TemaCardapio = TemaCardapio.Carregar(item.TemaCardapio.IDTemaCardapio.Value);

            return lista;
        }

        public static TemaCardapioPDVInformation Carregar(Int32 idTemaCardapioPDV)
        {
            TemaCardapioPDVInformation obj = new TemaCardapioPDVInformation { IDTemaCardapioPDV = idTemaCardapioPDV };
            obj = (TemaCardapioPDVInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(TemaCardapioPDVInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idTemaCardapioPDV)
        {
            try
            {
                TemaCardapioPDVInformation obj = new TemaCardapioPDVInformation { IDTemaCardapioPDV = idTemaCardapioPDV };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static void AtualizarUltimaAlteracao()
        {
            List<TemaCardapioPDVInformation> lista = TemaCardapioPDV.Listar();
            foreach (var temaCardapioPDV in lista)
            {
                temaCardapioPDV.DtUltimaAlteracao = DateTime.Now;
                Salvar(temaCardapioPDV);
            }
        }

        public static void AtualizarUltimaAlteracao(Int32 idTemaCardapioPDV)
        {
            TemaCardapioPDVInformation temaCardapioPDV = TemaCardapioPDV.Carregar(idTemaCardapioPDV);
            temaCardapioPDV.DtUltimaAlteracao = DateTime.Now;
            Salvar(temaCardapioPDV);
        }
    }
}
