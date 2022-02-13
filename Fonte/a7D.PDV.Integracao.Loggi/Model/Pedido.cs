using System;

namespace a7D.PDV.Integracao.Loggi.Model
{
    // http://api.docs.dev.loggi.com/recursos/pedidos.html
    public class PedidoRequest
    {
        public int payment_method;

        public PedidoRequest(int pagamentoID)
        {
            payment_method = pagamentoID;
        }
    }

    public class PedidoResponse : ErroLoggi
    {
        public string completion_eta;
        public int id;
        public int progress;
        public string status;
        public WayPoint[] waypoints;

        public override string ToString()
            => errors?.ToString() ?? $"{id}: {status}";
    }
}