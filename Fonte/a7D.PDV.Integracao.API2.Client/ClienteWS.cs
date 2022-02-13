using a7D.PDV.Integracao.API2.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ClienteWS : IDisposable
    {
        private Uri wsUri;
        private HttpClient client;

        public ClienteWS(string urlBase)
        {
            client = new HttpClient();
            wsUri = new Uri(urlBase);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public Uri Query(string query)
        {
            return new Uri(wsUri, query);
        }

        #region GET

        private Task<HttpResponseMessage> GetAsync(string query)
        {
            var endereco = Query(query);
            return client.GetAsync(endereco);
        }

        public string GetText(string query, bool notFoundNull = false)
        {
            var response = GetAsync(query).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (notFoundNull)
                    return null;
                else
                    throw ExceptionAPI.Create(response, result);
            }
            return result;
        }

        public Stream GetBytes(string query = "", bool notFoundNull = false)
        {
            var response = GetAsync(query).Result;
            if (!response.IsSuccessStatusCode)
            {
                if (notFoundNull)
                    return null;
                else
                    throw ExceptionAPI.Create(response, null);
            }

            return response.Content.ReadAsStreamAsync().Result;
        }

        public void Download(string query, string file, Action<long> actionProgress = null)
        {
            Task.WaitAll(DownloadAsync(query, file, actionProgress));
        }

        public async Task DownloadAsync(string query, string file, Action<long> actionProgress = null)
        {
            var endereco = Query(query);
            // https://github.com/dotnet/corefx/issues/6849
            using (var response = await client.GetAsync(endereco, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                int bufferSize = 8096;
                using (Stream
                    contentStream = await response.Content.ReadAsStreamAsync(),
                    fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
                {
                    var totalRead = 0L;
                    var buffer = new byte[bufferSize];
                    int read;
                    int loops = 0;
                    while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, read);
                        totalRead += read;
                        if (loops++ > 1000)
                        {
                            loops = 0;
                            actionProgress?.Invoke(totalRead);
                        }
                    }
                }
            }
        }

        public TResult Get<TResult>(string query, bool notFoundNull = false)
        {
            try
            {
                var result = GetText(query);
                return JsonConvert.DeserializeObject<TResult>(result);
            }
            catch (ExceptionAPI exAPI)
            {
                if (typeof(TResult) == typeof(ResultadoOuErro))
                    return (TResult)(object)exAPI.Erro;
                else if (notFoundNull && exAPI.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return (TResult)(object)null;
                else
                    throw exAPI;
            }
        }

        #endregion

        #region POST

        public Task<HttpResponseMessage> PostAsync(string query, object dado)
        {
            var endereco = Query(query);
            var request = JsonConvert.SerializeObject(dado, Formatting.Indented);
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            return client.PostAsync(endereco, content);
        }

        public string Post(string query, object dado)
        {
            var response = PostAsync(query, dado).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
                throw ExceptionAPI.Create(response, result);

            return result;
        }

        public TResult Post<TResult>(string query, object dado, bool notFoundNull = false)
        {
            try
            {
                var result = Post(query, dado);
                return JsonConvert.DeserializeObject<TResult>(result);
            }
            catch (ExceptionAPI exAPI)
            {
                if (typeof(TResult) == typeof(ResultadoOuErro))
                    return (TResult)(object)exAPI.Erro;
                else if (notFoundNull && exAPI.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return (TResult)(object)null;
                else
                    throw exAPI;
            }
        }

        #endregion
    }
}