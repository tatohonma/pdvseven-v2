using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbProcessamentoSAT")]
    public class ProcessamentoSATInformation
    {
        [CRUDParameterDAL(true, "IDProcessamentoSAT")]
        public int? IDProcessamentoSAT { get; set; }

        [CRUDParameterDAL(false, "IDStatusProcessamentoSAT")]
        public int? IDStatusProcessamentoSAT { get; set; }

        [CRUDParameterDAL(false, "IDTipoSolicitacaoSAT")]
        public int? IDTipoSolicitacaoSAT { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT")]
        public int? IDRetornoSAT { get; set; }

        [CRUDParameterDAL(false, "XMLEnvio")]
        public string XMLEnvio { get; set; }

        [CRUDParameterDAL(false, "GUID")]
        public string GUID { get; set; }

        [CRUDParameterDAL(false, "NumeroSessao")]
        public int? NumeroSessao { get; set; }

        [CRUDParameterDAL(false, "DataSolicitacao")]
        public DateTime? DataSolicitacao { get; set; }
    }
}
