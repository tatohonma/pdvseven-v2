using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.PixConta.API
{
    public class Invoice : APIJson
    {
        public Invoice(string token) : base("https://api.iugu.com", token)
        {
        }

        public Model.InvoiceInformation ConsultarStatus(string id) => Get<Model.InvoiceInformation>($"/v1/invoices/{id}");

        public void CancelarFatura(string id)
        {

        }
    }
}
