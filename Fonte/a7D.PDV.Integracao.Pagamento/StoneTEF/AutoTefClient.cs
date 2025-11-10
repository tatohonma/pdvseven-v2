using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.StoneTEF
{
    public sealed class AutoTefClient
    {
        readonly HttpClient _http;

        public AutoTefClient(string baseUrl, HttpMessageHandler handler = null)
        {
            _http = handler == null ? new HttpClient() : new HttpClient(handler);
            _http.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            _http.Timeout = TimeSpan.FromSeconds(90);
        }

        public class ActivateRequest
        {
            public string stoneCode { get; set; }
            public string partnerName { get; set; } 
            public ActivateRequest() { }
            public ActivateRequest(string stoneCode, string partnerName = null)
            {
                this.stoneCode = stoneCode;
                this.partnerName = partnerName;
            }
        }

        public class Installment
        {
            public int type { get; set; }
            public int number { get; set; }
            public Installment() { }
            public Installment(int type, int number)
            {
                this.type = type;
                this.number = number;
            }
        }

        public class PayRequest
        {
            public decimal amount { get; set; }
            public string accountType { get; set; } 
            public Installment installment { get; set; }
            public bool hasAlcoholicDrink { get; set; }
            public object splits { get; set; }
        }

        public class CancelRequest
        {
            public string acquirerTransactionKey { get; set; } 
            public decimal amount { get; set; }
            public int transactionType { get; set; }           
            public string panMask { get; set; }                
            public object splits { get; set; }
        }

        static StringContent AsJson(object obj)
        {
            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public Task<HttpResponseMessage> ActivateAsync(string stoneCode, string partnerName = null, CancellationToken ct = default(CancellationToken))
        {
            var body = new ActivateRequest(stoneCode, partnerName);
            return _http.PostAsync("api/Activate", AsJson(body), ct);
        }

        public Task<HttpResponseMessage> PayAsync(PayRequest req, CancellationToken ct = default(CancellationToken))
        {
            return _http.PostAsync("api/Pay", AsJson(req), ct);
        }

        public Task<HttpResponseMessage> CancelAsync(CancelRequest req, CancellationToken ct = default(CancellationToken))
        {
            return _http.PostAsync("api/Cancel", AsJson(req), ct);
        }

        public Task<HttpResponseMessage> HealthAsync(CancellationToken ct = default(CancellationToken))
        {
            return _http.GetAsync("api/Healthcheck", ct);
        }
    }
}
