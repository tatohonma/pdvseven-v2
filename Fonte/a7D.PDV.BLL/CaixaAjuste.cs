using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class CaixaAjuste
    {
        public static CaixaAjusteInformation Carregar(Int32 idCaixaAjuste)
        {
            CaixaAjusteInformation obj = new CaixaAjusteInformation { IDCaixaAjuste = idCaixaAjuste };
            obj = (CaixaAjusteInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(CaixaAjusteInformation obj)
        {
            if (obj.IDCaixaAjuste == null)
            {
                CaixaAjuste.Adicionar(obj);
            }
            else
            {
                CaixaAjuste.Alterar(obj);
            }
        }
        public static void Adicionar(CaixaAjusteInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(CaixaAjusteInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static List<CaixaAjusteInformation> ListarPorCaixa(Int32 idCaixa)
        {
            CaixaAjusteInformation objFiltro = new CaixaAjusteInformation();
            objFiltro.Caixa = new CaixaInformation { IDCaixa = idCaixa };

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CaixaAjusteInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaAjusteInformation>(CaixaAjusteInformation.ConverterObjeto));

            foreach (var item in lista)
                item.CaixaTipoAjuste = CaixaTipoAjuste.Carregar(item.CaixaTipoAjuste.IDCaixaTipoAjuste.Value);

            return lista;
        }
    }
}
