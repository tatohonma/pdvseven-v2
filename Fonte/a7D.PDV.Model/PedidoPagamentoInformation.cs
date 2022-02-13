using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Models;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPedidoPagamento")]
    [Serializable]
    public class PedidoPagamentoInformation
    {
        public PedidoPagamentoInformation()
        {
            Status = StatusModel.Novo;
        }

        [CRUDParameterDAL(true, "IDPedidoPagamento")]
        public Int32? IDPedidoPagamento { get; set; }

        [CRUDParameterDAL(false, "IDPedido", "IDPedido")]
        public PedidoInformation Pedido { get; set; }

        [CRUDParameterDAL(false, "IDTipoPagamento", "IDTipoPagamento")]
        public TipoPagamentoInformation TipoPagamento { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public Decimal? Valor { get; set; }

        [CRUDParameterDAL(false, "Autorizacao")]
        public String Autorizacao { get; set; }

        [CRUDParameterDAL(false, "IDGateway")]
        public int? IDGateway { get; set; }

        [CRUDParameterDAL(false, "IDMetodo", "IDMeioPagamentoSAT")]
        public MeioPagamentoSATInformation MeioPagamentoSAT { get; set; }

        [CRUDParameterDAL(false, "IDContaRecebivel", "IDContaRecebivel")]
        public tbContaRecebivel ContaRecebivel { get; set; }

        [CRUDParameterDAL(false, "IDBandeira", "IDBandeira")]
        public tbBandeira Bandeira { get; set; }

        [CRUDParameterDAL(false, "DataPagamento")]
        public DateTime? DataPagamento { get; set; }

        [CRUDParameterDAL(false, "IDUsuarioPagamento", "IDUsuario")]
        public UsuarioInformation UsuarioPagamento { get; set; }

        [CRUDParameterDAL(false, "DataCancelado")]
        public DateTime? DataCancelado { get; set; }

        [CRUDParameterDAL(false, "IDUsuarioCancelado", "IDUsuario")]
        public UsuarioInformation UsuarioCancelado { get; set; }

        [CRUDParameterDAL(false, "IDSaldoBaixa")]
        public int? IDSaldoBaixa { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public Boolean? Excluido { get; set; }

        public StatusModel Status { get; set; }

        public static PedidoPagamentoInformation ConverterObjeto(Object obj)
        {
            return (PedidoPagamentoInformation)obj;
        }

        public override int GetHashCode()
        {
            return (IDPedidoPagamento ?? 1) ^ (int)((Excluido == true ? 29 : 997) * (Valor ?? 1));
        }
    }
}
