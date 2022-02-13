namespace a7D.PDV.Integracao.Loggi.Model
{
    public class Destino
    {
        public string estimated_cost;
        public int distance;
        public int original_eta;
        public string path_suggested_gencoded;

        public override string ToString()
            => $"cost: {estimated_cost} distance: {distance}";
    }
}