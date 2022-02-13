using a7D.PDV.EF.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.MyFinance.Models
{
    public class ContaRecebivel
    {

        [JsonProperty(PropertyName = "id")]
        public string ID { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "entity_id")]
        public string EntityID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "provider")]
        public string Provider { get; private set; }

        [JsonProperty(PropertyName = "payment_methods")]
        public string PaymentMethods { get; private set; }

        public ContaRecebivel()
        {
            PaymentMethods = "";
        }

        public void SetProvider(API2.Model.ContaRecebivel contaRecebivelAPI2)
        {
            if (contaRecebivelAPI2.IDContaRecebivel == (int)EContaRecebivel.Dinheiro)
                Provider = MyFinanceProviders.cash_register.ToString();
            else if (contaRecebivelAPI2.IDContaRecebivel == (int)EContaRecebivel.Rede)
                Provider = MyFinanceProviders.redecard.ToString();
            else
            {
                Provider = contaRecebivelAPI2.Nome.ToLower();

                bool existeNoMyFinance = false;

                foreach (MyFinanceProviders item in Enum.GetValues(typeof(MyFinanceProviders)))
                {
                    if (Provider.ToLower() == item.ToString().ToLower())
                    {
                        existeNoMyFinance = true;
                        break;
                    }                        
                }

                if (!existeNoMyFinance)
                {
                    Provider = MyFinanceProviders.bank_account.ToString();
                }
            }
               


        }

        public void AdicionarPaymentMethods(PaymentMethodsMyFinance metodo)
        {
            var metodos = PaymentMethods.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            metodos.Add(metodo.ToString());
            PaymentMethods = String.Join(",", metodos);
        }

        public List<PaymentMethodsMyFinance> ObterListaPaymentMethods()
        {
            var lista = new List<PaymentMethodsMyFinance>();
            var metodos = PaymentMethods.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var item in metodos)
            {
                var metodo = (PaymentMethodsMyFinance)Enum.Parse(typeof(PaymentMethodsMyFinance), item);
                lista.Add(metodo);
            }
            return lista;
        }

        public string JsonPostContaRecebivel()
        {
            dynamic obj = new { sale_account = this };
            return JsonConvert.SerializeObject(obj);
        }

    }

    public enum MyFinanceProviders
    {
        cash_register, redecard, ticket, cielo,elavon, elo, getnet, pagseguro, stone, vero, bank_account
    }


}
