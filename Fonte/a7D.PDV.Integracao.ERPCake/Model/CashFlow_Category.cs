using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#categoria-de-fluxo-de-caixa
    public class CashFlow_Category : ModelCake
    {
        public string name;
        public string dre_account;
        public string subaccounts;

        public override string ToString()
        {
            return $"{id}: {name}";
        }
    }
}
