using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbSATServer")]
    public class SATServerInformation
    {
        [CRUDParameterDAL(true, "IDSATServer")]
        public int? IDSATServer { get; set; }

        [CRUDParameterDAL(false, "IDPedido", "IDPedido")]
        public PedidoInformation Pedido { get; set; }

        [CRUDParameterDAL(false, "IDUsuario", "IDUsuario")]
        public UsuarioInformation Usuario { get; set; }

        [CRUDParameterDAL(false, "IDPDV")]
        public int? IDPDV { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT", "IDRetornoSAT")]
        public RetornoSATInformation RetornoSAT { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT_cancelamento", "IDRetornoSAT")]
        public RetornoSATInformation RetornoSAT_cancelamento { get; set; }

        [CRUDParameterDAL(false, "TipoSolicitacao")]
        public int? TipoSolicitacao { get; set; }

        [CRUDParameterDAL(false, "Processado")]
        public bool? Processado { get; set; }

        [CRUDParameterDAL(false, "Erro")]
        public string Erro { get; set; }

        [CRUDParameterDAL(false, "ErroST")]
        public string ErroST { get; set; }
    }
}
