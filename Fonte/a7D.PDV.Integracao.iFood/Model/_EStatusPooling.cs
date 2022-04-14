using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.iFood
{
    //https://developer.ifood.com.br/v1.0/reference#eventspolling
    public enum EStatusPolling
    {
        PLACED, // Indica um pedido foi colocado no sistema.
        CONFIRMED, //Indica um pedido confirmado.
        INTEGRATED, // Indica um pedido que foi recebido pelo e-PDV.
        CANCELLED, // Indica um pedido que foi cancelado.
        DISPATCHED, // Indica um pedido que foi despachado ao cliente.
        DELIVERED, // Indica um pedido que foi entregue.
        CONCLUDED // Indica um pedido que foi concluído (Em até duas horas do fluxo normal)*
    }
}
