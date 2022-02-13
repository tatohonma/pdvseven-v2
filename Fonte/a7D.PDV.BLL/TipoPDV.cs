using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public static class TipoPDV
    {
        public static TipoPDVInformation Carregar(ETipoPDV tipoPDV)
        {
            return Repositorio.Carregar<TipoPDVInformation>(t => t.IDTipoPDV == (int)tipoPDV);
            //return pdv7Context.DB.TipoPDVs.Find((int)tipoPDV);
            //TipoPDVInformation obj = new TipoPDVInformation { IDTipoPDV = idTipoPDV };
            //obj = (TipoPDVInformation)CRUD.Carregar(obj);
            //return obj;
        }
    }
}
