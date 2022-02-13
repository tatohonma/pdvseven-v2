using a7D.PDV.Integracao.EasyChopp.Model;
using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.EasyChopp
{
    public class APIEasyChopp : APIJson
    {
        public APIEasyChopp() : base("http://apptst.easychopp.com.br")
        {
        }

        public Cliente getCredito(string key, string nrDocumento)
        {
            return Get<Cliente>($"/API/getCredito?key={key}&nrDocumento={nrDocumento}");
        }
    }
}