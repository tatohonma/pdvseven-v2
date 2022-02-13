using a7D.PDV.EF.Models;
using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Services;
using Refit;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

// https://github.com/reactiveui/refit
namespace a7D.PDV.Integracao.Server
{
    public static class Ativador
    {
        public static IAPIAtivador API;

        static Ativador()
        {
            var client = new HttpClient()
            {
                //BaseAddress = new Uri("http://localhost:7700"),
                BaseAddress = new Uri("https://apipdvseven.azurewebsites.net"),
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-auth-token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6InNlcnZlciIsImlzcyI6InNlbGYiLCJuYmYiOjE0NjM0MzIxODJ9.8Uw2JjVgPRxGOuuy9HPDwnubnuKNytuWvO_-ToQIPpw");

            API = RestService.For<IAPIAtivador>(client);
        }
    }

    public interface IAPIAtivador
    {
        //[Obsolete]
        //[Get("/api/mensagens/receber?chave={chave}")]
        //Task<MensagemRecebida[]> Mensagens([Query] string chave); // Só recebe mensagem com cuja o destino seja o Integrador

        [Get("/api/mensagens/syncmsg?chave={chave}")]
        Task<MensagemRecebida[]> SyncMensage(
            [Query] string chave, 
            [Query, TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))] EOrigemDestinoMensagem to, 
            [Query] string versao, 
            [Query] string status);

        [Post("/api/mensagens/enviar")]
        Task<int> Enviar([Body] MensagemNova chave);
    }
}
