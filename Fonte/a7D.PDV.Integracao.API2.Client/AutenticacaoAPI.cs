using a7D.PDV.Integracao.API2.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.API2.Client
{
    public class AutenticacaoAPI
    {
        private ClienteWS api;

        private static string _versao;

        public static string Versao => _versao ?? (_versao = Assembly.GetEntryAssembly().FullName.Split(',')[1].Split('=')[1]);

        public AutenticacaoAPI(ClienteWS ws)
            => api = ws;

        public Task<HttpResponseMessage> AutenticarPDVAsync(int tipo, string hardware)
            => api.PostAsync("api/autenticacao/pdv", new pdvRequest(tipo, hardware, Versao));

        public pdvResult AutenticarPDV(int tipo, string hardware)
            => api.Post<pdvResult>("api/autenticacao/pdv", new pdvRequest(tipo, hardware, Versao));

        public usuarioResult AutenticarUsuario(string senha)
            => api.Post<usuarioResult>("api/autenticacao/usuario", new usuarioRequest(senha));

        public T DeserializeObject<T>(string valor)
            => JsonConvert.DeserializeObject<T>(valor);
    }
}