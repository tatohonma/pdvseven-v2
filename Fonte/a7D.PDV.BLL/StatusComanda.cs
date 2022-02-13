using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public static class StatusComanda
    {
        public static string CarregarNome(EStatusComanda status)
        {
            if (status == EStatusComanda.ContaSolicitada)
                return "Conta Solicitada";
            else
                return status.ToString();
        }

        public static  StatusComandaInformation ToObjInfo(this EStatusComanda status)
        {
            return Carregar(status);
        }

        public static StatusComandaInformation Carregar(EStatusComanda status)
        {
            //StatusComandaInformation obj = new StatusComandaInformation { IDStatusComanda = (int)status };
            //return (StatusComandaInformation)CRUD.Carregar(obj);
            return new StatusComandaInformation()
            {
                IDStatusComanda = (int)status,
                Nome = CarregarNome(status)
            };
        }
    }
}
