using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Comunicacao.SAT;
using a7D.PDV.Fiscal.NFCe;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
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
        private const string requestUrlSATVenda = "/api/sat/enviarvenda?numeroSessao={0}&codigoDeAtivacao={1}";
        private const string requestUrlSATCancelamento = "/api/sat/cancelarvenda?codigoDeAtivacao={0}";
        private const string requestUrlSATConsulta = "/api/sat/consultarsessao/{0}/?codigoDeAtivacao={1}";

        public bool IsNFCe { get; set; }
        public string XMLJSON { get; set; }

        private readonly JavaScriptSerializer jsSerializer;
        private Task<HttpResponseMessage> request;
        private readonly HttpClient _client;
        private string codigoDeAtivacao;

        private ETipoSolicitacaoSAT _operacao = ETipoSolicitacaoSAT.SEM_TIPO;

        // Campos para inutilização (NFC-e)
        private int _serie;
        private int _numeroInicial;
        private int _numeroFinal;
        private string _justificativa;

        public FiscalApiClient(string endereco)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(endereco),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

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
                    switch (_operacao)
                    {
                        case ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA:
                            return NFeFacade.EnviarAssinada(XMLJSON);

                        case ETipoSolicitacaoSAT.INUTILIZAR_NUMERACAO:
                            return NFeFacade.InutilizarNumeracaoToString(_serie, _numeroInicial, _numeroFinal, _justificativa);
                    }
                }

                var response = request.Result;
                var obj = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = jsSerializer.Deserialize<RetApi>(obj);
                    return ret.RetStr;
                }

                var ex = new ExceptionPDV(CodigoErro.E508, _client.BaseAddress.ToString());
                ex.Data.Add("response.Result", obj);
                throw ex;
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
                throw;
            }
        }

        public IFiscalApiClient VendaClient(string codigoDeAtivacao, ICFeVenda nf, int numeroSessao)
        {
            _operacao = ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA;

            if (nf is NFCe.NFCe)
            {
                IsNFCe = true;
                XMLJSON = nf.GerarXMLVenda();
            }
            else // SAT
            {
                IsNFCe = false;

                this.codigoDeAtivacao = codigoDeAtivacao;
                XMLJSON = Encoding.GetEncoding("iso-8859-1").GetString(
                    Encoding.UTF8.GetBytes(nf.GerarXMLVenda())
                );

                var content = new StringContent(XMLJSON, Encoding.UTF8, "text/plain");
                request = _client.PostAsync(string.Format(requestUrlSATVenda, numeroSessao, codigoDeAtivacao), content);
            }

            return this;
        }

        public IFiscalApiClient CancelamentoClient(string codigoDeAtivacao, int numeroSessao, string chave, string dadosCancelamento)
        {
            _operacao = ETipoSolicitacaoSAT.CANCELAR_VENDA;

            // SAT
            IsNFCe = false;

            this.codigoDeAtivacao = codigoDeAtivacao;

            XMLJSON = jsSerializer.Serialize(new
            {
                NumeroSessao = numeroSessao,
                Chave = chave,
                DadosCancelamento = dadosCancelamento
            });

            var content = new StringContent(XMLJSON, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            request = _client.PostAsync(string.Format(requestUrlSATCancelamento, codigoDeAtivacao), content);
            return this;
        }

        public IFiscalApiClient ConsultaClient(string codigoDeAtivacao, int numeroSessao)
        {
            _operacao = ETipoSolicitacaoSAT.SEM_TIPO;

            // SAT
            IsNFCe = false;

            this.codigoDeAtivacao = codigoDeAtivacao;
            request = _client.GetAsync(string.Format(requestUrlSATConsulta, numeroSessao, codigoDeAtivacao));
            return this;
        }

        public IFiscalApiClient InutilizacaoClient(int serie, int numeroInicial, int numeroFinal, string justificativa, int numeroSessao)
        {
            _operacao = ETipoSolicitacaoSAT.INUTILIZAR_NUMERACAO;
            IsNFCe = true;

            justificativa = (justificativa ?? string.Empty).Trim();

            if (serie <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Série inválida.");

            if (numeroInicial <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Número inicial inválido.");

            if (numeroFinal <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Número final inválido.");

            if (numeroFinal < numeroInicial)
                throw new ExceptionPDV(CodigoErro.E100, "O número final não pode ser menor que o número inicial.");

            // Regra SEFAZ comum: justificativa mínima (em geral 15 chars). Você pode aumentar para 20 se quiser.
            if (justificativa.Length < 15)
                throw new ExceptionPDV(CodigoErro.E520, "Justificativa deve ter pelo menos 15 caracteres.");

            // Guarda para o Enviar()
            _serie = serie;
            _numeroInicial = numeroInicial;
            _numeroFinal = numeroFinal;
            _justificativa = justificativa;

            return this;
        }

    }
}
