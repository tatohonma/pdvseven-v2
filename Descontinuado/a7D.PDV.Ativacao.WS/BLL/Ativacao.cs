using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Ativacao.WS.Model;

namespace a7D.PDV.Ativacao.WS.BLL
{
    public class Ativacao
    {
        public static AtivacaoInformation Carregar(String chaveAtivacao)
        {
            AtivacaoInformation obj = new AtivacaoInformation { ChaveAtivacao = chaveAtivacao };
            obj = (AtivacaoInformation)CRUD.Carregar(obj);

            return obj;
        }
    }
}