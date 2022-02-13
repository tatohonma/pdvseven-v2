using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#clientes
    public class State : ModelCake
    {
        public string name;
        public string country_iso;
        public string symbol;
        public string uf_code;

        public override string ToString() => $"{id}: {symbol} {name}";
    }
}
