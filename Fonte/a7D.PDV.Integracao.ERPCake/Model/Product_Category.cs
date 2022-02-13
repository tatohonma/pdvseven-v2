namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#categorias-de-produtos
    public class Product_Category : ModelCake
    {
        public string name;
        public string symbol;
        public string code_cest;
        public string image;
        public string markup;
        public string tax_id;
        public string external_code;

        public override void SetCode(string code) => this.external_code = code;

        public override string ToString() => $"{id}: {name}";
    }
}