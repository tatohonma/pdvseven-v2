using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit; // https://www.nuget.org/packages/Refit
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    //https://reactiveui.github.io/refit/
    public interface IGitHubApi
    {
        [Get("/users/{user}")]
        Task<User> GetUser(string user);
        //Task<dynamic> GetUser(string user);
    }

    public class User
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
    }

    public interface IEasyChopp
    {
        [Get("/API/getCredito?key={key}&nrDocumento={documento}")]
        Task<ClienteEasyChopp> getCredito([Query] string key, [Query]string documento);
    }

    public class ClienteEasyChopp
    {
        public string dsError { get; set; }
        public bool stIntegracao { get; set; }
        public double? vlSaldoAtual { get; set; }
        public string dsNomeCliente { get; set; }
        public string dsEmail { get; set; }
        public DateTime dtNascimento { get; set; }
        public string dsSexo { get; set; }
        public string nrTelefone { get; set; }
        public string nrDocumento { get; set; }
        public string idTAG { get; set; }

        public override string ToString()
        {
            return stIntegracao ? $"OK {nrDocumento} {idTAG} {dsNomeCliente} {vlSaldoAtual}" : dsError;
        }
    }



    [TestClass]
    public class TestRefitGitHub
    {
        IGitHubApi git;
        IEasyChopp easy;

        [TestInitialize]
        public void ReFit_GitHub()
        {
            var client1 = new HttpClient() { BaseAddress = new Uri("https://api.github.com") };
            client1.DefaultRequestHeaders.Add("User-Agent", "Client PDVSeven API / 1.0.0 Integrador");
            client1.DefaultRequestHeaders.Add("Accept", "application/json");
            client1.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client1.DefaultRequestHeaders.Add("accept-encoding", "text");

            git = RestService.For<IGitHubApi>(client1);

            var client2 = new HttpClient() { BaseAddress = new Uri("https://apptst.easychopp.com.br") };
            client2.DefaultRequestHeaders.Add("User-Agent", "Client PDVSeven API / 1.0.0 Integrador");
            client2.DefaultRequestHeaders.Add("Accept", "application/json");
            client2.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client2.DefaultRequestHeaders.Add("accept-encoding", "text");
            //client.DefaultRequestHeaders.TransferEncodingChunked = false;
            //client.DefaultRequestHeaders.ExpectContinue = false;
            //client.DefaultRequestHeaders.ConnectionClose = true;

            easy = RestService.For<IEasyChopp>(client2);
        }

        [TestMethod]
        [TestCategory("ReFit-Test")]
        public void ReFit_GitHub_User()
        {
            Task.Run(async () =>
            {
                var user = await git.GetUser("impactro");
                Console.WriteLine($"{user.login}, {user.id}, {user.node_id}, {user.avatar_url}");
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("ReFit-Test")]
        public void ReFit_Easychopp_GetSaldo()
        {
            Task.Run(async () =>
            {
                var user = await easy.getCredito("4867A295D369819D010BA0818A8A533E", "19221149870");
                Console.WriteLine($"{user.stIntegracao} {user.dsError} {user.dsNomeCliente}, {user.vlSaldoAtual}");
            }).GetAwaiter().GetResult();
        }
    }
}