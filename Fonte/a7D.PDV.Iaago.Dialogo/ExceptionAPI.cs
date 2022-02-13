// FROM a7D.PDV.Integracao.API2.Model;
using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace a7D.PDV.Iaago.Web
{
    public class ExceptionAPI : Exception
    {
        public const string varData = "responseBody";

        public HttpStatusCode StatusCode { get; private set; }

        public string ResonseBody => Data[varData].ToString();

        private ExceptionAPI(string message, string body, HttpStatusCode status)
            : base(message)
        {
            StatusCode = status;
            if (body != null)
            {
                Data.Add(varData, body);
            }
        }


        public static ExceptionAPI Create(HttpResponseMessage response, string body)
        {
            string message = $"{(int)response.StatusCode}: {response.ReasonPhrase}";
            return new ExceptionAPI(message, body, response.StatusCode);
        }
    }
}