using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Helpers
{
    public class SiteAtivacoesHelper
    {
        public static string ObterIdBroker(string chaveLicenca)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(new Uri(ConfigurationManager.AppSettings["urlApi"]), $"/api/ativacoes?filter[chaveAtivacao]={chaveLicenca}"),
                        Method = HttpMethod.Get
                    };
                    request.Headers.Add("x-auth-token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6InNlcnZlciIsImlzcyI6InNlbGYiLCJuYmYiOjE0NjM0MzIxODJ9.8Uw2JjVgPRxGOuuy9HPDwnubnuKNytuWvO_-ToQIPpw");
                    var response = client.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        dynamic obj = JArray.Parse(content);

                        if (obj.Count < 1)
                            return null;

                        return obj[0].Cliente.IDTiny.Value;

                    }
                    return null;
                }
            }
            catch { return null; }
        }
    }
}