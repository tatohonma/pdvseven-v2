using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL.Utils
{
    public static class ApiClient
    {
        public static async Task<string> PostJsonAsync(string apiUrl, string parametro, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(parametro, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro na solicitação HTTP: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
