using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbComanda")]
    [Serializable]
    public class ComandaInformation
    {
        [CRUDParameterDAL(true, "IDComanda")]
        public Int32? IDComanda { get; set; }

        [CRUDParameterDAL(false, "IDStatusComanda", "IDStatusComanda")]
        public StatusComandaInformation StatusComanda { get; set; }

        public EStatusComanda ValorStatusComanda => (EStatusComanda)StatusComanda?.IDStatusComanda;

        [CRUDParameterDAL(false, "GUIDIdentificacao")]
        public String GUIDIdentificacao { get; set; }

        [CRUDParameterDAL(false, "Numero")]
        public Int32? Numero { get; set; }

        [CRUDParameterDAL(false, "Codigo")]
        public Int64? Codigo { get; set; }

        [CRUDParameterDAL(false, "IDCliente")]
        public Int32? IDCliente { get; set; }

        [CRUDParameterDAL(false, "Observacao")]
        public String Observacao { get; set; }

        public static ComandaInformation ConverterObjeto(Object obj)
        {
            return (ComandaInformation)obj;
        }

        public override string ToString()
            => $"{IDComanda}: {Numero} ({GUIDIdentificacao}) {StatusComanda?.IDStatusComanda}";
    }
}
