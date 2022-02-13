using System;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Models;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoPagamento")]
    [Serializable]
    public class TipoPagamentoInformation : IERP
    {
        [CRUDParameterDAL(true, "IDTipoPagamento")]
        public Int32? IDTipoPagamento { get; set; }

        [CRUDParameterDAL(false, "CodigoImpressoraFiscal")]
        public String CodigoImpressoraFiscal { get; set; }

        [CRUDParameterDAL(false, "CodigoERP")]
        public String CodigoERP { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "RegistrarValores")]
        public Boolean? RegistrarValores { get; set; }

        [CRUDParameterDAL(false, "PrazoCredito")]
        public Int32? PrazoCredito { get; set; }

        [CRUDParameterDAL(false, "IDGateway")]
        public Int32? IDGateway { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        [CRUDParameterDAL(false, "IDMeioPagamentoSAT", "IDMeioPagamentoSAT")] // Metodo de pagamento!!!
        public MeioPagamentoSATInformation MeioPagamentoSAT { get; set; }

        [CRUDParameterDAL(false, "IDContaRecebivel", "IDContaRecebivel")]
        public tbContaRecebivel ContaRecebivel { get; set; }

        [CRUDParameterDAL(false, "IDBandeira", "IDBandeira")]
        public tbBandeira Bandeira { get; set; }

        public EGateway Gateway => (EGateway)(IDGateway ?? 0);

        public static TipoPagamentoInformation ConverterObjeto(Object obj)
        {
            return (TipoPagamentoInformation)obj;
        }
    }
}
