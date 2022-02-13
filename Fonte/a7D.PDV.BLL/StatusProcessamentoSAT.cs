using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class StatusProcessamentoSAT
    {
        public static StatusProcessamentoSATInformation Carregar(int idStatusProcessamento)
        {
            var obj = new StatusProcessamentoSATInformation { IDStatusProcessamentoSAT = idStatusProcessamento };
            return (StatusProcessamentoSATInformation)CRUD.Carregar(obj);
        }

        public static StatusProcessamentoSATInformation Carregar(EStatusProcessamentoSAT status)
        {
            return Carregar((int)status);
        }
    }
}
