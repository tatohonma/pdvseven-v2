// FROM a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace a7D.PDV.Iaago.Web
{
    public class ClienteWS : IDisposable
    {
        private readonly Uri urlbase;
        private readonly HttpClient client;

        public ClienteWS(string urlBase)
        {
            client = new HttpClient();
            urlbase = new Uri(Uri.EscapeUriString(urlBase));
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public async Task<TResult> Get<TResult>(bool notFoundNull = false)
        {
            try
            {
                var result = await GetText();
                return JsonConvert.DeserializeObject<TResult>(result);
            }
            catch (ExceptionAPI ex)
            {
                if (notFoundNull && ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (TResult)(object)null;
                }
                else
                {
                    throw ex;
                }
            }
        }

        private async Task<string> GetText(bool notFoundNull = false)
        {
            var response = await client.GetAsync(urlbase);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (notFoundNull)
                {
                    return null;
                }
                else
                {
                    throw ExceptionAPI.Create(response, result);
                }
            }

            return result;
        }

        public async Task<TResult> Post<TResult>(object dado, bool notFoundNull = false)
        {
            try
            {
                var result = await Post(dado);
                return JsonConvert.DeserializeObject<TResult>(result);
            }
            catch (ExceptionAPI ex)
            {
                if (notFoundNull && ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (TResult)(object)null;
                }
                else
                {
                    throw ex;
                }
            }
        }

        private async Task<string> Post(object dado)
        {
            var response = await PostAsync(dado);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw ExceptionAPI.Create(response, result);
            }

            return result;
        }

        private async Task<HttpResponseMessage> PostAsync(object dado)
        {
            HttpContent content;
            if (dado is Dictionary<string, string> dic)
            {
                content = new FormUrlEncodedContent(dic.Select(c => new KeyValuePair<string, string>(c.Key, c.Value)));
            }
            else
            {
                var request = JsonConvert.SerializeObject(dado, Formatting.Indented);
                content = new StringContent(request, Encoding.UTF8, "application/json");
            }

            return await client.PostAsync(urlbase, content);
        }
    }
}