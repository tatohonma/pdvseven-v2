using System;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCaixa")]
    [Serializable]
    public class CaixaInformation
    {
        public CaixaInformation()
        {

        }
        [CRUDParameterDAL(true, "IDCaixa")]
        public Int32? IDCaixa { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "IDUsuario", "IDUsuario")]
        public UsuarioInformation Usuario { get; set; }

        [CRUDParameterDAL(false, "IDFechamento", "IDFechamento")]
        public FechamentoInformation Fechamento { get; set; }

        [CRUDParameterDAL(false, "DtAbertura")]
        public DateTime? DtAbertura { get; set; }

        [CRUDParameterDAL(false, "DtFechamento")]
        public DateTime? DtFechamento { get; set; }

        [CRUDParameterDAL(false, "SincERP")]
        public Boolean? SincERP { get; set; }
        
        public static CaixaInformation ConverterObjeto(Object obj)
        {
            return (CaixaInformation)obj;
        }
    }
}
