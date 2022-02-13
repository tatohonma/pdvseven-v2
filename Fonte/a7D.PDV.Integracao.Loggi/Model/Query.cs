namespace a7D.PDV.Integracao.Loggi.Model
{
    public class WayPoint
    {
        public string cep;
        public string category;
        public string address;
        public string address_complement;
        public int number;
        public string instructions;
        public AddressData address_data;
        public string state;
        public string vicinity;
        public string zip;
        public WayPointGeometry geometry;
        public string formatted_address; // No retorno do Orçamento
        public string address_formated; // No retorno do Pedido

        public override string ToString()
            => formatted_address ?? address_formated ?? zip ?? cep ?? address;

        public WayPoint()
        {
        }

        public WayPoint(string cep, int numero, string complemento = null)
        {
            this.cep = cep;
            this.number = numero;
            this.address_complement = complemento;
        }

        public WayPoint(string ruaAv, string endereco, int numero, string complemento = null)
        {
            this.category = ruaAv;
            this.address = endereco;
            this.number = numero;
            this.address_complement = complemento;
        }
    }

    public class WayPointGeometry
    {
        public LocationGeometry location;
    }

    public class LocationGeometry
    {
        public decimal lat;
        public decimal lng;
    }
}