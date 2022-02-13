using a7D.PDV.Fiscal.Comunicacao.SAT;
using a7D.PDV.Fiscal.NFCe;
using a7D.PDV.Fiscal.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace a7D.PDV.Fiscal
{
    public class FiscalApiClient : IFiscalApiClient
    {
        const string requestUrlSATVenda = "/api/sat/enviarvenda?numeroSessao={0}&codigoDeAtivacao={1}";
        const string requestUrlSATCancelamento = "/api/sat/cancelarvenda?codigoDeAtivacao={0}";
        const string requestUrlSATConsulta = "/api/sat/consultarsessao/{0}/?codigoDeAtivacao={1}";

        public bool IsNFCe { get; set; }
        public string XMLJSON { get; set; }

        private JavaScriptSerializer jsSerializer;
        private Task<HttpResponseMessage> request;
        private HttpClient _client;
        private string codigoDeAtivacao;

        public FiscalApiClient(string endereco)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(endereco),
                Timeout = TimeSpan.FromSeconds(30)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            jsSerializer = new JavaScriptSerializer();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public string Enviar()
        {
            try
            {
                if (IsNFCe)
                {
                    return NFeFacade.EnviarAssinada(XMLJSON);
                }
                else
                {
                    var response = request.Result;
                    var obj = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var ret = jsSerializer.Deserialize<RetApi>(obj);
                        return ret.RetStr;
                    }
                    var ex = new BLL.ExceptionPDV(BLL.CodigoErro.E508, _client.BaseAddress.ToString());
                    ex.Data.Add("response.Result", obj);
                    throw ex;
                }
            }
            catch (AggregateException ex)
            {
                ex.Data.Add("_client.BaseAddress", _client.BaseAddress);
                var msg = string.Empty;

                foreach (var e in ex.InnerExceptions)
                    msg += e.Message;

                throw new Exception(msg, ex);
            }
            catch (Exception ex)
            {
                ex.Data.Add("_client.BaseAddress", _client.BaseAddress);
                throw ex;
            }
        }

        public IFiscalApiClient VendaClient(string codigoDeAtivacao, ICFeVenda nf, int numeroSessao)
        {
            if (nf is NFCe.NFCe _nfce)
            {
                IsNFCe = true;
                XMLJSON = nf.GerarXMLVenda();
                //System.IO.File.WriteAllText(@"C:\PDV7\Venda-NFCe.xml", XMLJSON);
            }
            else // SAT
            {
                this.codigoDeAtivacao = codigoDeAtivacao;
                XMLJSON = Encoding.GetEncoding("iso-8859-1").GetString(Encoding.UTF8.GetBytes(nf.GerarXMLVenda()));
                var content = new StringContent(XMLJSON, Encoding.UTF8, "text/plain");
                request = _client.PostAsync(string.Format(requestUrlSATVenda, numeroSessao, codigoDeAtivacao), content);
            }
            return this;
        }

        public string Venda(string venda, int numeroSessao)
        {
            var content = new StringContent(venda, Encoding.UTF8, "text/plain");
            var result = _client.PostAsync(string.Format(requestUrlSATVenda, numeroSessao, FiscalServices.ConfigSAT.InfCFe_codigoAtivacao), content).Result;
            return result.Content.ReadAsStringAsync().Result;
        }

        public IFiscalApiClient CancelamentoClient(string codigoDeAtivacao, int numeroSessao, string chave, string dadosCancelamento)
        {
            this.codigoDeAtivacao = codigoDeAtivacao;
            XMLJSON = jsSerializer.Serialize(new { NumeroSessao = numeroSessao, Chave = chave, DadosCancelamento = dadosCancelamento });
            var content = new StringContent(XMLJSON, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            request = _client.PostAsync(string.Format(requestUrlSATCancelamento, codigoDeAtivacao), content);
            return this;
        }

        public IFiscalApiClient ConsultaClient(string codigoDeAtivacao, int numeroSessao)
        {
            this.codigoDeAtivacao = codigoDeAtivacao;
            request = _client.GetAsync(string.Format(requestUrlSATConsulta, numeroSessao, codigoDeAtivacao));
            return this;
        }
    }
}
