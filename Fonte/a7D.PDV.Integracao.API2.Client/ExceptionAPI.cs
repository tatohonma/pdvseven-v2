using a7D.PDV.Integracao.API2.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ExceptionAPI : Exception
    {
        public const string varData = "responseBody";

        public ResultadoOuErro Erro { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        private ExceptionAPI(string message, string body, ResultadoOuErro erro, HttpStatusCode status) : base(message)
        {
            Erro = erro;
            StatusCode = status;
            if (body != null)
                Data.Add(varData, body);
        }

        public string ResonseBody => Data[varData].ToString();

        public static ExceptionAPI Create(HttpResponseMessage response, string body)
        {
            ResultadoOuErro erro = null;
            string message = $"{(int)response.StatusCode}: {response.ReasonPhrase}";
            try
            {
                if (body!=null && body.StartsWith("{") && body.EndsWith("}"))
                {
                    erro = JsonConvert.DeserializeObject<ResultadoOuErro>(body);
                    message += " - " + erro.Mensagem;
                }
            }
            catch (Exception)
            {
            }
            return new ExceptionAPI(message, body, erro, response.StatusCode);
        }
    }
}