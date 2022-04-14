using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core
{
    public abstract class APIJson : IDisposable
    {
        public string LastRequest { get; protected set; }
        public string LastResult { get; protected set; }

        protected Uri apiUri;
        protected HttpClient client;
        protected JsonSerializerSettings jsonWriter;

        public APIJson(string urlBase)
        {
            client = new HttpClient();
            apiUri = new Uri(urlBase);
            jsonWriter = new JsonSerializerSettings();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public APIJson(string urlBase, string token)
        {
            client = new HttpClient();
            apiUri = new Uri(urlBase);
            jsonWriter = new JsonSerializerSettings();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void Dispose() => client?.Dispose();

        public Uri Query(string query) => new Uri(apiUri, query);

        protected TResult MakeResponse<TResult>(HttpResponseMessage response) where TResult : class
        {
            LastResult = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(LastResult)
             || LastResult == "null")
            {
                LastResult = null;
                return null;
            }
            else if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)LastResult;
            }
            else if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<TResult>(LastResult);
                if (typeof(TResult).IsSubclassOf(typeof(apiError)))
                {
                    var objErr = (apiError)(object)obj;
                    if (objErr.error != null)
                        throw new Exception(objErr.error);
                }
                return obj;
            }

            if (LastResult.StartsWith("{"))
                return ParseErro<TResult>();
            else
                throw new Exception($"{(int)response.StatusCode}: {response.ReasonPhrase}");
        }

        public virtual TResult ParseErro<TResult>()
        {
            if (typeof(TResult).IsSubclassOf(typeof(apiError)))
                return JsonConvert.DeserializeObject<TResult>(LastResult);

            var erro = JsonConvert.DeserializeObject<apiError>(LastResult);
            throw new Exception(erro.GetMessage());
        }

        #region GET

        public TResult Get<TResult>(string query) where TResult : class
        {
            try
            {
                LastResult = null;

                var url = Query(query);
                LastRequest = url.ToString();

                var response = client.GetAsync(url).Result;

                return MakeResponse<TResult>(response);
            }
            catch (Exception exAPI)
            {
                exAPI.Data.Add("LastRequest", LastRequest);
                exAPI.Data.Add("LastResult", LastResult);
                throw exAPI;
            }
        }

        #endregion

        #region POST

        private Task<HttpResponseMessage> SendAsync(string query, object dado, string method)
        {
            var url = Query(query);
            var body = dado == null ? "{}" : JsonConvert.SerializeObject(dado, Formatting.Indented, jsonWriter);

            LastRequest = url.ToString() + Environment.NewLine + body;

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = url,
                Content = content
            };

            return client.SendAsync(req);
        }

        protected TResult Send<TResult>(string query, object dado = null, string method = "POST") where TResult : class
        {
            try
            {
                LastResult = null;

                var response = SendAsync(query, dado, method).Result;

                return MakeResponse<TResult>(response);
            }
            catch (Exception exAPI)
            {
                exAPI.Data.Add("LastRequest", LastRequest);
                exAPI.Data.Add("LastResult", LastResult);
                throw exAPI;
            }
        }

        #endregion
    }
}