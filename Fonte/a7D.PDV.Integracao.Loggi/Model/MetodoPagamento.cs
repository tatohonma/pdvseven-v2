namespace a7D.PDV.Integracao.Loggi.Model
{
    // http://api.docs.dev.loggi.com/recursos/metodos-de-pagamento.html
    public class MetodoPagamento
    {
        public int? id;
        public string name;
        public string type;
        public bool valid;
        public int expiration_year;
        public string cvv;
        public string number;
        public string expiration_month;
        public string billing_name;

        public override string ToString()
            => $"{id}: {name}";
    }
}