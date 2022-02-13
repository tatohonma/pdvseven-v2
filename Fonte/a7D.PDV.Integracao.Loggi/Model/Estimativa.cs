namespace a7D.PDV.Integracao.Loggi.Model
{
    // http://api.docs.dev.loggi.com/recursos/address-estimative.html
    public class EstimativaRequest
    {
        public int city = 1; // São Paulo, 2 Rio de Janeiro
        public int transport_type = 2; // 1 Moto, 2 Bicicleta
        public WayPointQuery[] addresses;

        public EstimativaRequest(WayPointQuery origem, WayPointQuery destino)
        {
            addresses = new WayPointQuery[2];
            addresses[0] = origem;
            addresses[1] = destino;
        }
    }

    public class EstimativaResponse : ErroLoggi
    {
        public string optimized;
        public string optimized_slo;
        public Destino normal_slo;
        public Destino normal;

        public override string ToString()
            => errors?.ToString() ?? $"normal {normal} / slo {normal_slo}";
    }
}