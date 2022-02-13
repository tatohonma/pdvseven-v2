using a7D.PDV.Integracao.Loggi.Model;
using a7D.PDV.Integracao.Loggi.ModelPro;
using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.Loggi
{
    public class APILoggi : APIJson
    {
        public ErroList LastErros { get; private set; }

        public APILoggi(string email, string token) :
            base(email.StartsWith("staging:") ? "https://staging.loggi.com/" : "https://www.loggi.com/")
        {
            if (email.StartsWith("staging:"))
                email = email.Substring(8);

            client.DefaultRequestHeaders.Add("authorization", "ApiKey " + email + ":" + token);
            jsonWriter.NullValueHandling = NullValueHandling.Ignore;
        }

        public override TResult ParseErro<TResult>()
        {
            if (typeof(TResult).IsSubclassOf(typeof(ErroLoggi)))
                return JsonConvert.DeserializeObject<TResult>(LastResult);

            var erro = JsonConvert.DeserializeObject<ErroLoggi>(LastResult);
            throw new Exception(erro.ToString());
        }

        public EstimativaResponse Estimativa(EstimativaRequest estimativa)
            => Send<EstimativaResponse>("api/v1/endereco/estimativa/", estimativa);

        public OrcamentoResponse Orcamento(OrcamentoRequest orcamento)
        {
            try
            {
                var or = Send<OrcamentoResponse>("api/v1/endereco/orcamento/", orcamento);
                or.last_request = this.LastRequest;
                or.last_result = this.LastResult;
                return or;
            }
            catch (Exception ex)
            {
                return new OrcamentoResponse()
                {
                    error_message = ex.Message,
                    last_request = this.LastRequest,
                    last_result = this.LastResult
                };
            }

        }

        public PedidoResponse PedidoConfirmar(string orcamentoID, int pagamentoID)
        {
            try
            {
                var pr = Send<PedidoResponse>($"api/v1/pedidos/{orcamentoID}/confirmar/", new PedidoRequest(pagamentoID));
                pr.last_request = this.LastRequest;
                pr.last_result = this.LastResult;
                return pr;
            }
            catch (Exception ex)
            {
                return new PedidoResponse()
                {
                    error_message = ex.Message,
                    last_request = this.LastRequest,
                    last_result = this.LastResult
                };
            }

        }

        public PedidoResponse PedidoStatus(int pedidoID)
            => Get<PedidoResponse>($"api/v1/pedidos-status/{pedidoID}/");

        public string PedidoListar()
            => Get<string>($"api/v1/pedidos-status/");

        public string PedidoCancelar(int pedidoID)
            => Send<string>($"api/v1/pedidos/{pedidoID}/cancelar/", new { });

        public string AutoComplete(string endereco)
            => Get<string>($"api/v1/autocomplete/?input={endereco}");

        //public OrcamentoResponse Pedido(OrcamentoRequest orcamento)
        //    => Send<OrcamentoResponse>("api/v1/pedidos/", orcamento);

        public MetodoPagamento[] MetodosPagamento()
        {
            LastErros = null;
            var result = Get<ListResponse<MetodoPagamento>>($"api/v1/metodos-de-pagamento/");
            LastErros = result?.errors;
            return result?.objects;
        }

        public void MetodosPagamentoCriar(MetodoPagamento pagamento)
            => Send<string>("api/v1/metodos-de-pagamento/", pagamento);

        // https://staging.loggi.com/graphiql
        // https://api.loggi.com/introduction/welcome
        public string GraphiQL(string query)
        {
            var result = Send<string>("graphql", new GraphiQL(query));

            return null;
        }
    }
}