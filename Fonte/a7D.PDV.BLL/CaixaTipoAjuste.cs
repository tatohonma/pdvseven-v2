using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class CaixaTipoAjuste
    {
        public static CaixaTipoAjusteInformation Carregar(Int32 idCaixaTipoAjuste)
        {
            CaixaTipoAjusteInformation obj = new CaixaTipoAjusteInformation { IDCaixaTipoAjuste = idCaixaTipoAjuste };
            obj = (CaixaTipoAjusteInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(CaixaTipoAjusteInformation obj)
        {
            if (obj.IDCaixaTipoAjuste == null)
            {
                CaixaTipoAjuste.Adicionar(obj);
            }
            else
            {
                CaixaTipoAjuste.Alterar(obj);
            }
        }
        public static void Adicionar(CaixaTipoAjusteInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(CaixaTipoAjusteInformation obj)
        {
            CRUD.Alterar(obj);
        }
    }
}
