using a7D.PDV.Integracao.API2.Model;

namespace a7D.PDV.Integracao.API2.Client
{
    public class PainelMesasAPI
    {
        private ClienteWS api;

        internal PainelMesasAPI(ClienteWS ws)
            => api = ws;

        public MesaComandasTotal[] LerMesas()
            => api.Get<MesaComandasTotal[]>($"api/painelmesas");

        public NumeroQuantidadeTotalPrimeiro[] LerComandasPorMesa(int mesa)
            => api.Get<NumeroQuantidadeTotalPrimeiro[]>($"api/painelmesas/{mesa}");
    }
}