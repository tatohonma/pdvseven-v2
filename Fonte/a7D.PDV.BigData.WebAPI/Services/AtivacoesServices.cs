using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace a7D.PDV.BigData.WebAPI.Services
{
    public static class AtivacoesServices
    {

        private static IAtivacaoAPI API;

        static AtivacoesServices()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://apipdvseven.azurewebsites.net"),
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            API = RestService.For<IAtivacaoAPI>(client);
        }

        public static async Task<ClienteAtivacaoResult> ClientePorChave(string chave)
        {
            var cliente = await API.GetAtivacaoPorChave(chave);
            if (string.IsNullOrEmpty(cliente.CNPJ) || cliente.CNPJ.Length < 5)
                cliente.CNPJ = "";
            else if (cliente.CNPJ.Length < 11) // CPF
                cliente.CNPJ = cliente.CNPJ.PadLeft(11, '0');
            else if (cliente.CNPJ.Length > 11 && cliente.CNPJ.Length < 14) //CNPJ
                cliente.CNPJ = cliente.CNPJ.PadLeft(14, '0');

            return cliente;
        }
    }

    public interface IAtivacaoAPI
    {
        // TODO: Substituir por autenticação incluindo a chave de hardware
        [Get("/api/ativacoes/cliente/{chave}")]
        Task<ClienteAtivacaoResult> GetAtivacaoPorChave(string chave);
    }

    public class ClienteAtivacaoResult
    {
        public string Estabelecimento { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
    };
}