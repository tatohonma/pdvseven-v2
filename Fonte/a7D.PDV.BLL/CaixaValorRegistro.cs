using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class CaixaValorRegistro
    {
        public static CaixaValorRegistroInformation Carregar(Int32 idCaixaValorRegistro)
        {
            CaixaValorRegistroInformation obj = new CaixaValorRegistroInformation { IDCaixaValorRegistro = idCaixaValorRegistro };
            obj = (CaixaValorRegistroInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(CaixaValorRegistroInformation obj)
        {
            if (obj.IDCaixaValorRegistro == null)
            {
                CaixaValorRegistro.Adicionar(obj);
            }
            else
            {
                CaixaValorRegistro.Alterar(obj);
            }
        }
        public static void Adicionar(CaixaValorRegistroInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(CaixaValorRegistroInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static List<CaixaValorRegistroInformation> ListarPorCaixa(Int32 idCaixa)
        {
            CaixaValorRegistroInformation objFiltro = new CaixaValorRegistroInformation();
            objFiltro.Caixa = new CaixaInformation { IDCaixa = idCaixa };

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CaixaValorRegistroInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaValorRegistroInformation>(CaixaValorRegistroInformation.ConverterObjeto));

            foreach (var item in lista)
            {
                item.TipoPagamento = TipoPagamento.Carregar(item.TipoPagamento.IDTipoPagamento.Value);
            }

            return lista;
        }
    }
}
