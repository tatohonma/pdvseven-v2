using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Loggi.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Loggi
{
    public class LoggiDelivery
    {
        private static readonly ConfiguracoesLoggi cfg;
        private static readonly APILoggi api;
        private static int idPagamento;
        private static string statusLoggi;

        static LoggiDelivery()
        {
            cfg = new ConfiguracoesLoggi();
            
            if(!cfg.IntegracaoLoggi)
            {
                statusLoggi = "Integração Loggi desligada";
                return;
            }
            else if (string.IsNullOrEmpty(cfg.EmailUsuario) || string.IsNullOrEmpty(cfg.TokenUsuario))
            {
                statusLoggi = "Loggi não configurada";
                return;
            }

            api = new APILoggi(cfg.EmailUsuario, cfg.TokenUsuario);
            idPagamento = 0;
        }

        public static string Estimar(string enderecoDestino)
        {
            if (api == null)
                return statusLoggi;
            try
            {
                var origem = new WayPointQuery(cfg.OrigemPedido);
                var destino = new WayPointQuery(enderecoDestino);
                var estimativa = new EstimativaRequest(origem, destino);
                var result = api.Estimativa(estimativa);
                if (result.errors != null)
                {
                    return $@"ERRO: {result}
Origem:
{cfg.OrigemPedido}
Destino:
{enderecoDestino}";
                }
                else
                {
                    return $"Distância: {result.normal.distance}\r\nCusto: {result.normal.estimated_cost}";
                }
            }
            catch (Exception ex)
            {
                return "Erro ao se conectar a Loggi\r\nVerifique a conexão com a internet ou as credenciais de acesso da Loggi\r\n" + ex.Message;
            }
        }

        public static Task<string> EstimarAsync(string enderecoDestino)
        {
            return Task.Factory.StartNew(() => Estimar(enderecoDestino));
        }

        public static OrcamentoResponse Orcamento(int value, string endereco, string infoPedido, string infoDestino)
        {
            if (api == null)
                return new OrcamentoResponse() { error_message = statusLoggi };

            var origem = new WayPointQuery(cfg.OrigemPedido);
            origem.query.instructions = infoPedido;

            var destino = new WayPointQuery(endereco);
            destino.query.instructions = infoDestino;

            var orcamento = new OrcamentoRequest(origem, destino);
            orcamento.package_type = ((ETamanhoPacote)value).ToString();

            var result = api.Orcamento(orcamento);
            return result;
        }

        public static PedidoResponse Confirmar(string idOrcamento)
        {
            if (api == null)
                return new PedidoResponse() { error_message = statusLoggi };

            if (idPagamento == 0)
            {
                var pagamentos = api.MetodosPagamento();
                if (pagamentos == null)
                    return new PedidoResponse() { errors = api.LastErros };

                var pagamento = pagamentos.FirstOrDefault(p => p.name == cfg.MeioPagamento);
                if (pagamento == null)
                    return new PedidoResponse() { errors = new ErroList($"Meio de pagamento '{cfg.MeioPagamento}' não encontrado") };

                idPagamento = pagamento.id.Value;
            }

            return api.PedidoConfirmar(idOrcamento, idPagamento);
        }

        //public static string GetLastRequestResult()
        //{
        //    return $"{api.LastRequest}\r\n{api.LastResult}";
        //}
    }
}