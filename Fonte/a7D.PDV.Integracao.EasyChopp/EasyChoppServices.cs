using a7D.PDV.Integracao.EasyChopp.Model;
using Refit;
using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.EasyChopp
{
    // TODO: os metodos de inclusão por TAG não estão funcionando, somente por documento

    public static class EasyChoppServices
    {
        static readonly CultureInfo cultureENUS = CultureInfo.GetCultureInfo("en-US");

        private static IEasyChopp API;

        private static string ChaveSeguranca;

        private static string EmailLogin;

        public static void ConfigEasyChoppServices(string url, string chave, string email)
        {
            ChaveSeguranca = chave;
            EmailLogin = email;

            var client = new HttpClient() { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Add("User-Agent", "Client PDVSeven API / 1.0.0 Integrador");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("accept-encoding", "text");
            client.DefaultRequestHeaders.TransferEncodingChunked = false;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.ConnectionClose = true;

            API = RestService.For<IEasyChopp>(client);
        }

        private static string toMD5Hex(this string valor)
        {
            var bytes = UTF32Encoding.ASCII.GetBytes(valor);
            var sb = new StringBuilder();

            using (var md5 = MD5.Create())
            {
                byte[] byteHashedPassword = md5.ComputeHash(bytes);
                foreach (var bt in byteHashedPassword)
                    sb.Append(bt.ToString("X2"));
            }

            return sb.ToString();
        }

        public static Cliente GetClienteDocumento(string documento)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            return API.getClienteDocumento(hash, documento).Result;
        }

        //public static Cliente GetClienteTAG(string tag)
        //{
        //    string key = ChaveSeguranca + tag;
        //    string hash = key.toMD5Hex();
        //    return API.getClienteTAG(hash, tag).Result;
        //}

        public static Cliente[] GetClientes(DateTime dt, out string dsErro)
        {
            string dtCadastro = dt.ToString("dd/MM/yyyy hh:mm:ss");
            string key = ChaveSeguranca + dt.ToString("ddMMyyyy");
            string hash = key.toMD5Hex();
            var result = API.getClientes(hash, dtCadastro).Result;
            if (!result.stIntegracao)
            {
                dsErro = result.dsError;
                return null;
            }

            dsErro = null;
            foreach (var cliente in result.Clientes)
                cliente.stIntegracao = true;

            return result.Clientes;
        }

        public static Retorno AddClienteDocumento(string documento, string nome, string email, string sexo, DateTime? nascimento)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            string dtNascimento = nascimento.HasValue ? nascimento.Value.ToString("dd/MM/yyyy") : "01/01/2000";
            return API.addClienteDocumento(hash, documento, nome, email ?? "", dtNascimento, sexo ?? "M").Result;
        }

        //public static Retorno AddClienteTAG(string tag, string nome, string email, string sexo, DateTime? nascimento)
        //{
        //    string key = ChaveSeguranca + tag;
        //    string hash = key.toMD5Hex();
        //    string dtNascimento = nascimento.HasValue ? nascimento.Value.ToString("dd/MM/yyyy") : "01/01/2000";
        //    return API.addClienteTAG(hash, tag, nome, email, dtNascimento, sexo).Result;
        //}

        public static Retorno DefineClienteTAG(string documento, string tagHEX)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            // Sempre prépago, sem tarifas
            return API.addTAG(hash, documento, tagHEX, "0", "0", EmailLogin).Result;
        }

        public static Retorno RemoveClienteTAG(string documento, string tagHEX)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            return API.removeTAG(hash, documento, tagHEX).Result;
        }

        public static Task<Cliente> GetCreditoDocumento(string documento)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            return API.getCreditoDocumento(hash, documento);
        }

        //public static Task<Cliente> GetCreditoTAG(string tag)
        //{
        //    string key = ChaveSeguranca + tag;
        //    string hash = key.toMD5Hex();
        //    return API.getCreditoTAG(hash, tag);
        //}

        public static Retorno AddCreditoDocumento(string documento, FormaPagamento formaPagamento, decimal valor, string autorizacao, string nrCupom)
        {
            string key = ChaveSeguranca + documento;
            string hash = key.toMD5Hex();
            return API.addCreditoDocumento(hash, documento, valor.ToString("N2", cultureENUS), autorizacao, EmailLogin, true, (int)formaPagamento, nrCupom, "API PDVSeven").Result;
        }

        //public static Retorno AddCreditoTAG(string tag, FormaPagamento formaPagamento, decimal valor, string autorizacao, string nrCupom)
        //{
        //    string key = ChaveSeguranca + tag;
        //    string hash = key.toMD5Hex();
        //    return API.addCreditoTAG(hash, tag, valor.ToString("N2", cultureENUS), autorizacao, EmailLogin, true, (int)formaPagamento, nrCupom, "API PDVSeven").Result;
        //}
    }

    public enum FormaPagamento
    {
        Dinheiro = 1,
        Debito = 2,
        Credito = 3,
        Consumo = 4,
        Cheque = 5,
        OnLine = 6,
        Outros = 9
    }
}
