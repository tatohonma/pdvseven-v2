namespace a7D.PDV.Integracao.Loggi.Model
{
    // http://api.docs.dev.loggi.com/recursos/address-orcamentos.html
    public class OrcamentoRequest
    {
        public int city = 1; // São Paulo, 2 Rio de Janeiro
        public int transport_type = 2; // 1 Moto, 2 Bicicleta
        public int? payment_method;
        public int slo = 1; // 1 entrega já (padrão); 2: entrega expressa, será finalizada em até quatro horas
        public string package_type = "medium_box"; // large_box
        public WayPointQuery[] waypoints;

        public OrcamentoRequest(WayPointQuery origem, WayPointQuery destino)
        {
            waypoints = new WayPointQuery[2];
            waypoints[0] = origem;
            waypoints[1] = destino;
        }
    }

    public class OrcamentoResponse : ErroLoggi
    {
        public int city;
        public string city_name;
        // customer
        // driver
        public string id;
        public string path_suggested_gencoded;
        public OrcamentoPrice pricing;
        public int slo;
        public string total_time;
        public string transport_type;
        public WayPoint[] waypoints;

        public override string ToString()
            => errors?.ToString() ?? $"{id}: {pricing}";
    }

    public class OrcamentoPrice
    {
        public string[] applied_bonuses;
        public string bonuses;
        public string discount;
        public int id;
        public string inquiry;
        public string sum_ride_cm;
        public string sum_wait_cm;
        public string total_cm;
        public string total_cm_gross;

        public override string ToString()
            => $"{total_cm}";
    }
}