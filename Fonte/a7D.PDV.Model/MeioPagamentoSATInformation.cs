using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbMeioPagamentoSAT")]
    public class MeioPagamentoSATInformation
    {
        [CRUDParameterDAL(true, "IDMeioPagamentoSAT")]
        public int? IDMeioPagamentoSAT { get; set; }

        [CRUDParameterDAL(false, "Codigo")]
        public string Codigo { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }
    }
}