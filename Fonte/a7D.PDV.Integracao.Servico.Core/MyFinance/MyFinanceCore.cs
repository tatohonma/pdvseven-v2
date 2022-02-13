using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Servico.Core.MyFinance.Models;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.MyFinance
{
    public class MyFinanceCore
    {      
        public List<Recebivel> MontarRecebiveis(List<API2.Model.Pedido> pedidos)
        {
            List<Recebivel> listaRecebiveis = new List<Recebivel>();
            foreach (var pedido in pedidos)
            {
                foreach (var pagamento in pedido.Pagamentos)
                {
                    var pedidoFlat = new API2.Model.Pedido
                    {
                        IDPedido = pedido.IDPedido,
                        DtPedidoFechamento = pedido.DtPedidoFechamento,
                        Pagamentos = new List<API2.Model.Pagamento> { { pagamento } }
                    };

                    var recebivel = new Recebivel(pedidoFlat);
                    recebivel.Description = $"Pedido {pedido.IDPedido} - Pagamento {pagamento.ID} - {pagamento.TipoPagamento.Nome} - {pedido.DtPedidoFechamento}";
                    recebivel.NominalAmount = pagamento.Valor.Value;
                    recebivel.OcurredAt = pedido.DtPedidoFechamento.Value;
                    recebivel.ConfirmedAt = pedido.DtPedidoFechamento.Value;
                    listaRecebiveis.Add(recebivel);
                }
            }
            return listaRecebiveis;
        }

        public static List<Models.ContaRecebivel> DeserializarContasRecebiveis(string json)
        {
            var lista = JsonConvert.DeserializeObject<List<ContaRecebivelJson>>(json);
            return lista.Select(i => i.ContaRecebivel).ToList();
        }

        public static List<Models.ContaRecebivel> ListarContasRecebiveis()
        {
            var api = new MyFinanceAPI();
            var result = api.ListaContasRecebivel();
            var resultBody = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao listar contas de Recebiveis do MyFinance.
Codigo do erro: {(int)result.StatusCode} - {result.ReasonPhrase} - {resultBody}
");            
            }
            return DeserializarContasRecebiveis(resultBody);
        }

        public static Models.ContaRecebivel ConsultarContaRecebivel(string id)
        {
            var api = new MyFinanceAPI();
            var result = api.ConsultarContaRecebivel(id);
            var resultBody = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                throw new Exception($@"Erro ao consultar conta de Recebível do MyFinance.
Codigo do erro: {(int)result.StatusCode} - {result.ReasonPhrase} - {resultBody}
");
            }
            var contaJson = JsonConvert.DeserializeObject<ContaRecebivelJson>(resultBody);
            return contaJson.ContaRecebivel;

        }

        public static EntidadeItem ObterEntidadeID()
        {
            var api = new MyFinanceAPI();
            var result = api.ObterEntidade();
            var resultBody = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao obter o ID da Entidade do MyFinance.
Codigo do erro: {(int)result.StatusCode} - {result.ReasonPhrase} - {resultBody}
");
            }
            var entidadeJson = JsonConvert.DeserializeObject<List<Entidade>>(resultBody);
            return entidadeJson[0].entidade;
        }

        public static Models.ContaRecebivel EnviarContaDeRecebivel(Models.ContaRecebivel contaRevebivemMyFinance)
        {
            var apiMyFinance = new MyFinanceAPI();
            var response = apiMyFinance.PostContaRecebivel(contaRevebivemMyFinance);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao enviar conta de Recebível ao MyFinance. 
Codigo do erro: {(int)response.StatusCode} - {response.ReasonPhrase} - {responseBody}
Conta de recebivel: {JsonConvert.SerializeObject(contaRevebivemMyFinance)}");
            }
            var contaJson = JsonConvert.DeserializeObject<Models.ContaRecebivelJson>(responseBody);
            return contaJson.ContaRecebivel;
        }

        public static void AlterarContaRecebivel(Models.ContaRecebivel contaRecebivel)
        {
            var apiMyFinance = new MyFinanceAPI();
            var response = apiMyFinance.AlterarContaRecebivel(contaRecebivel);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($@"Erro ao enviar conta de Recebível ao MyFinance. 
Codigo do erro: {(int)response.StatusCode} - {response.ReasonPhrase} - {responseBody}
Conta de recebivel: {JsonConvert.SerializeObject(contaRecebivel)}");
            }
            //var contaJson = JsonConvert.DeserializeObject<Models.ContaRecebivelJson>(responseBody);
        }

        public static PaymentMethodsMyFinance TraduzirMeioMetodoPagamento(int idMeioPagamento)
        {
            switch (idMeioPagamento)
            {
                case (int)EMeioPagamento.Dinheiro:
                    return PaymentMethodsMyFinance.cash;
                case (int)EMeioPagamento.Credito:
                case (int)EMeioPagamento.Loja:
                    return PaymentMethodsMyFinance.credit;
                default:
                    return PaymentMethodsMyFinance.debit;
            }
        }

   



    }

    public enum PaymentMethodsMyFinance
    {
        cash,credit,debit
    }
}
