using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class GranitoController : ApiController
    {
        private readonly string DicName = "ExecPDictionary";
        private readonly string FieldIdTag = "tag";
        private readonly string FieldIdDocument = "document";
        private readonly string NextConsultaOK = "004";
        private readonly string NextConsultaErro = "005";
        private readonly string NextPagamento = "999";
        private readonly int ContaRecebivelGranito = 150;
        private readonly int UsuarioPOS = 2; // autoatendimento
        private readonly int IdProdutoCredito = 5; // Produto crédito

        private readonly string formatoNumero = "R$ #,##0.00";

        [HttpPost]
        [Route("api/granito/pos/consultar")]
        public IHttpActionResult ConsultarCliente([FromBody] GranitoFieldValue fieldValue)
        {
            ValidarParametrosConsulta(fieldValue, 0);

            string nomeCliente, cpfCliente, saldo;
            decimal credito = 0;
            ClienteInformation cliente = null;

            try
            {
                cliente = ValidarCliente(fieldValue);
            }
            catch (Exception e)
            {
                return ResponseErro(e.Message, NextConsultaErro);
            }

            if (fieldValue.FieldId == FieldIdTag)
            {
                var comanda = BLL.Comanda.CarregarPorNumeroOuCodigo(long.Parse(fieldValue.Value));

                if (comanda.IDCliente.HasValue)
                {
                    cliente = BLL.Cliente.Carregar(comanda.IDCliente.Value);
                    if (cliente == null)
                    {
                        return ResponseErro("Mensagem de erro|Tag sem cliente|", NextConsultaErro);
                    }
                }
            }

            if (fieldValue.FieldId == FieldIdDocument)
            {
                var clientes = BLL.Cliente.BuscarCliente(null, null, fieldValue.Value, null, 1, 10);
                cliente = clientes.FirstOrDefault(c => c.Documento1 == fieldValue.Value);
            }

            if (cliente != null)
            {
                nomeCliente = cliente.NomeCompleto;
                cpfCliente = cliente.Documento1;

                credito = BLL.Saldo.ClienteSaldoLiquido(cliente.IDCliente.Value);
                saldo = credito.ToString(formatoNumero);

                return ResponseSucesso(nomeCliente, cpfCliente, saldo, string.Empty, NextConsultaOK, false);
            }
            else
            {
                return ResponseErro("Mensagem de erro|Cliente não encontrado|", NextConsultaErro);
            }
        }

        [HttpPost]
        [Route("api/granito/pos/pagar")]
        public IHttpActionResult RegistrarPedido([FromBody] GranitoPagamentoRequest pagamento)
        {
            string nomeCliente, cpfCliente, saldo;
            decimal valorTotal = int.Parse(pagamento.Amount) / 100m;
            string guidSolicitacao = Guid.NewGuid().ToString();

            ValidarParametrosPagamento(pagamento);
            PDVInformation pdv;
            try
            {
                pdv = ValidarLicencas(pagamento.serial);
            }
            catch (Exception e)
            {
                return ResponseErro(e.Message, NextPagamento);
            }

            ClienteInformation cliente;
            try
            {
                cliente = ValidarCliente(pagamento.Fields[0]);
            }
            catch (Exception e)
            {
                return ResponseErro(e.Message, NextPagamento);
            }


            var caixa = BLL.Caixa.UsaOuAbre(pdv.IDPDV.Value, UsuarioPOS);

            PedidoInformation pedido = new PedidoInformation
            {
                Cliente = cliente,
                TipoPedido = TipoPedido.Carregar((int)ETipoPedido.Balcao),
                StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Aberto },
                Caixa = caixa,
            };

            try
            {
                Task.WaitAll(Executando(guidSolicitacao));
                BLL.Pedido.Adicionar(pedido);

                pedido.ListaProduto = new List<PedidoProdutoInformation>
                {
                    new PedidoProdutoInformation
                    {
                        Quantidade = 1,
                        IDPedidoProduto = IdProdutoCredito,
                        Produto = BLL.Produto.Carregar(IdProdutoCredito),
                        ValorUnitario = int.Parse(pagamento.Amount) / 100m,
                        Cancelado = false,
                        PDV = pdv,
                    }
                };

                var tipoPagamento = BLL.TipoPagamento.CarregarPorGateway((int)EGateway.GranitoPOS, false);
                pedido.ListaPagamento = new List<PedidoPagamentoInformation>
                {
                    new PedidoPagamentoInformation
                    {
                        TipoPagamento = tipoPagamento,
                        Valor = valorTotal,
                        Excluido = false,
                        Bandeira = BLL.Bandeira.Carregar(BandeiraGranito(pagamento.BrandCode)),
                        ContaRecebivel = BLL.ContaRecebivel.Carregar(ContaRecebivelGranito),
                    }
                };

                BLL.Pedido.FecharVendaDB(pedido, pedido.Caixa, UsuarioPOS);
            }
            catch (Exception ex)
            {
                return ResponseErro($"Mensagem de erro|{ex.Message}|", NextPagamento);
            }
            finally
            {
                Exec.Remove(guidSolicitacao);
            }

            nomeCliente = cliente.NomeCompleto;
            cpfCliente = cliente.Documento1;
            decimal credito = Saldo.ClienteSaldoLiquido(cliente.IDCliente.Value);

            if (cliente.Credito.HasValue)
            {
                credito = cliente.Credito.Value;
            }

            saldo = credito.ToString(formatoNumero);

            return ResponseSucesso(nomeCliente, cpfCliente, saldo, valorTotal.ToString(formatoNumero), NextPagamento, true);
        }

        #region Metodos auxiliares

        /// <summary>
        /// 1 - Baseado no serial, consultar o idpdv
        /// 1.1 - Encontrou, segue
        /// 1.2 = N encontrou, verifica se existe uma licenca para esse idTipoPDV
        /// 1.2.1 Se existir, recupera e vincula
        /// 1.2.2 Se n existir,tem que dar excecao de licencas excedidas
        /// </summary>
        /// <param name="chaveHardware">Serial do equipamento</param>
        /// <returns></returns>
        private PDVInformation ValidarLicencas(string chaveHardware)
        {
            if (string.IsNullOrWhiteSpace(chaveHardware))
                throw new ExceptionPDV(CodigoErro.E08E);

            using (var pdvServico = new Licencas())
            {
                var gw = EF.Repositorio.Carregar<tbTipoPagamento>(t => t.IDGateway == (int)EGateway.GranitoPOS);
                if (gw == null)
                    throw new ExceptionPDV(CodigoErro.EESD);

                var pdv = pdvServico.Carregar(ETipoPDV.POS_INTEGRADO_GRANITO, chaveHardware);

                GA.PostEvento(pdv, $"Validar PDV: {ETipoPDV.POS_INTEGRADO_GRANITO} {chaveHardware} 0.1");

                return pdv;
            }
        }

        private ClienteInformation ValidarCliente(GranitoFieldValue fv)
        {
            ClienteInformation cliente = null;

            if (fv.FieldId == FieldIdTag)
            {
                var comanda = BLL.Comanda.CarregarPorNumeroOuCodigoOuHex(fv.Value, false);

                if (comanda != null && comanda.IDComanda.HasValue)
                {
                    if (comanda.ValorStatusComanda == EStatusComanda.Liberada)
                    {
                        throw new Exception("Mensagem de erro|Comanda não aberta|");
                    }

                    if (comanda.ValorStatusComanda == EStatusComanda.Cancelada)
                    {
                        throw new Exception("Mensagem de erro|Comanda cancelada|");
                    }

                    if (comanda.ValorStatusComanda == EStatusComanda.Perdida)
                    {
                        throw new Exception("Mensagem de erro|Comanda Perdida|");
                    }

                    var pedidoComanda = BLL.Pedido.ObterPedidoAbertoPorComanda(comanda.Numero.Value);
                    if (pedidoComanda != null && pedidoComanda.IDPedido.HasValue && pedidoComanda.Cliente != null && pedidoComanda.Cliente.IDCliente.HasValue)
                    {
                        cliente = BLL.Cliente.Carregar(pedidoComanda.Cliente.IDCliente.Value);
                        if (cliente == null)
                        {
                            throw new Exception("Mensagem de erro|Pedido sem cliente|");
                        }
                    }
                    else
                    {
                        throw new Exception("Mensagem de erro|Comanda não aberta|");
                    }
                }
                else
                {
                    throw new Exception("Mensagem de erro|Comanda não cadastrada|");
                }
            }

            if (fv.FieldId == FieldIdDocument)
            {
                var clientes = BLL.Cliente.BuscarCliente(null, null, fv.Value, null, 1, 10);
                cliente = clientes.FirstOrDefault(c => c.Documento1 == fv.Value);
                if (cliente == null)
                {
                    throw new Exception("Mensagem de erro|Cliente não encontrado|");
                }
            }

            return cliente;
        }

        private int BandeiraGranito(int bandeira)
        {
            switch (bandeira)
            {
                case 1:
                    return (int)EBandeira.Visa;
                case 2:
                    return (int)EBandeira.VisaElectro;
                case 3:
                    return (int)EBandeira.MasterCard;
                case 4:
                    return (int)EBandeira.MastercardD;
                case 5:
                    return (int)EBandeira.EloCredito;
                case 6:
                    return (int)EBandeira.Elo;
                case 9:
                    return (int)EBandeira.Amex;
                case 82:
                case 83:
                    return (int)EBandeira.Hipercard;
                default:
                    throw new ArgumentOutOfRangeException($"cardBrand: fora dos valores esperados [{bandeira}]");
            }
        }

        private void ValidarParametrosPagamento(GranitoPagamentoRequest pagamento)
        {
            if (pagamento.MerchantId == 0)
            {
                throw new ArgumentNullException("merchantId");
            }

            if (pagamento.PDVId == 0)
            {
                throw new ArgumentNullException("pdv");
            }

            if (pagamento.OperationType != 1)
            {
                throw new NotImplementedException("operationType: no momento, somente 1 - venda");
            }

            if (pagamento.CardNumber == 0)
            {
                throw new ArgumentNullException("cardNumber");
            }

            if (pagamento.BrandCode == 0)
            {
                throw new ArgumentNullException("brandCode");
            }

            if (pagamento.NetworkCode == 0)
            {
                throw new ArgumentNullException("networkCode");
            }

            if (pagamento.PaymentMethodType == 0)
            {
                throw new ArgumentNullException("paymentMethodType");
            }

            if (string.IsNullOrWhiteSpace(pagamento.Amount))
            {
                throw new ArgumentNullException("amount");
            }

            if (string.IsNullOrWhiteSpace(pagamento.ProofOfSale))
            {
                throw new ArgumentNullException("proofOfSale");
            }

            if (string.IsNullOrWhiteSpace(pagamento.TransactionCode))
            {
                throw new ArgumentNullException("transactionCode");
            }

            if (string.IsNullOrWhiteSpace(pagamento.TransactionDate))
            {
                throw new ArgumentNullException("transactionDate");
            }

            if (!DateTime.TryParseExact(pagamento.TransactionDate, "yyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                throw new ArgumentOutOfRangeException("transactionDate: data invalida");
            }

            if (pagamento.Fields.Length != 1)
            {
                throw new ArgumentOutOfRangeException("Esperado um registro em 'fields'");
            }

            int idx = 0;
            foreach (var fieldValue in pagamento.Fields)
            {
                ValidarParametrosConsulta(fieldValue, idx++);
            }
        }

        private ResponseMessageResult ResponseSucesso(string nome, string cpf, string saldo, string credito, string next, bool adicionado)
        {
            string mensagem = adicionado ?
                $"{nome}|Saldo adicionado {saldo}|Boa festa :)|" :
                $"{nome}|Saldo {saldo}|Boa festa :)|";

            if (!string.IsNullOrWhiteSpace(credito))
            {
                mensagem = $"{nome}|Crédito adicionado {credito}|Saldo atual {saldo}|Boa festa :)|";
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new GranitoResponse
            {
                ResponseCode = 0,
                Message = mensagem,
                Next = next
            }));
        }


        private ResponseMessageResult ResponseErro(string message, string next)
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new GranitoResponse
            {
                ResponseCode = 1,
                Message = message,
                Next = next
            }));
        }

        private void ValidarParametrosConsulta(GranitoFieldValue cliente, int idx)
        {
            if (string.IsNullOrWhiteSpace(cliente.FieldId))
            {
                throw new ArgumentNullException($"fieldId[{idx}]");
            }

            if (cliente.FieldId != FieldIdTag && cliente.FieldId != FieldIdDocument)
            {
                throw new ArgumentOutOfRangeException($"fieldId[{idx}]: Deve ser 'tag' ou 'document'");
            }

            if (string.IsNullOrWhiteSpace(cliente.Value))
            {
                throw new ArgumentNullException($"value[{idx}]");
            }

            if (!long.TryParse(cliente.Value, out long x))
            {
                throw new ArgumentOutOfRangeException($"value[{idx}]: Deve ser um número");
            }
        }

        private HashSet<string> Exec
        {
            get
            {
                if (HttpContext.Current.Application.Get(DicName) == null)
                {
                    HttpContext.Current.Application.Set(DicName, new HashSet<string>());
                }
                return (HashSet<string>)HttpContext.Current.Application.Get(DicName);
            }
        }
        
        private static object @lock = new object();

        private async Task Executando(string guid)
        {
            while (Exec.Contains(guid))
            {
                await Task.Delay(100);
            }
            lock (@lock) Exec.Add(guid);
        }

        #endregion
    }
}
