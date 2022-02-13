using a7D.PDV.Integracao.API2.Model;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ConfiguracaoAPI
    {
        private ClienteWS api;

        internal ConfiguracaoAPI(ClienteWS ws)
            => api = ws;

        public string Chave(string chave, int? idPDV = null, int? tipoPDV = null)
           => api.Get<ResultadoOuErro>($"api/configuracao/{chave}?idPDV={idPDV}&tipoPDV={tipoPDV}")?.Mensagem;
    }
}