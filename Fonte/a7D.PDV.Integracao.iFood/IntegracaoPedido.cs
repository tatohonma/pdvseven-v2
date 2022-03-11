using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.iFood
{
    public class IntegracaoPedido
    {
        API.Order APIOrder;
        ConfiguracoesIFood ConfigIFood;
        CaixaInformation CaixaIFood;
        PDVInformation PDVIFood;
        UsuarioInformation UsuarioIfood;
        TipoPagamentoInformation PagamentoIFood;
        TipoPagamentoInformation PagamentoDinheiro;
        TipoPagamentoInformation PagamentoCredito;
        TipoPagamentoInformation PagamentoDebito;
        TipoPagamentoInformation PagamentoRefeicao;
        TipoPagamentoInformation PagamentoOutros;
        TaxaEntregaInformation TaxaEntregaIFood;

        public IntegracaoPedido(string accessToken,
            ConfiguracoesIFood configIFood,
            CaixaInformation caixaIFood,
            PDVInformation pdvIFood,
            UsuarioInformation usuarioIfood,
            TipoPagamentoInformation pagamentoIFood,
            TipoPagamentoInformation pagamentoDinheiro,
            TipoPagamentoInformation pagamentoCredito,
            TipoPagamentoInformation pagamentoDebito,
            TipoPagamentoInformation pagamentoRefeicao,
            TipoPagamentoInformation pagamentoOutros,
            TaxaEntregaInformation taxaEntregaIFood)
        {
            APIOrder = new API.Order(accessToken);

            ConfigIFood = configIFood;
            CaixaIFood = caixaIFood;
            PDVIFood = pdvIFood;
            UsuarioIfood = usuarioIfood;
            PagamentoIFood = pagamentoIFood;
            PagamentoDinheiro = pagamentoDinheiro;
            PagamentoCredito = pagamentoCredito;
            PagamentoDebito = pagamentoDebito;
            PagamentoRefeicao = pagamentoRefeicao;
            PagamentoOutros = pagamentoOutros;
            TaxaEntregaIFood = taxaEntregaIFood;
        }

        public void ImportarPedidos()
        {
            var eventos = APIOrder.EventsPolling();
            if (eventos == null)
                return;

            foreach (var evento in eventos)
            {
                try
                {
                    if (evento.code == "PLC")
                        AdicionarPedido(evento);

                    APIOrder.Acknowledgment(new Model.Order.Event[] { evento });
                }
                catch (Exception ex)
                {
                }

            }
        }

        private void AdicionarPedido(Model.Order.Event evento)
        {
            PedidoInformation pedido = new PedidoInformation();

            var orderDetails = APIOrder.OrderDetails(evento.orderId);

            pedido.Cliente = AdicionarCliente(orderDetails);
            pedido.DocumentoCliente = orderDetails.customer.documentNumber;

            pedido.TipoPedido = new TipoPedidoInformation();
            pedido.TipoPedido.IDTipoPedido = 30;

            pedido.StatusPedido = new StatusPedidoInformation();
            pedido.StatusPedido.IDStatusPedido = 10;

            pedido.Caixa = CaixaIFood;

            pedido.TaxaEntrega = new TaxaEntregaInformation();
            pedido.TaxaEntrega = TaxaEntregaIFood;

            pedido.GUIDIdentificacao = Guid.NewGuid().ToString();
            pedido.DocumentoCliente = orderDetails.customer.documentNumber;
            pedido.DtPedido = DateTime.Now;
            pedido.GUIDMovimentacao = Guid.NewGuid().ToString();
            pedido.ValorEntrega = Convert.ToDecimal(orderDetails.total.deliveryFee);
            pedido.TAG = "{ \"iFood_orderId\":\"" + orderDetails.id + "\"}";

            if (orderDetails.customer.ordersCountOnMerchant == 0)
                pedido.ObservacaoCupom = "NOVO CLIENTE IFOOD";
            else
                pedido.ObservacaoCupom = "FIDELIDADE " + orderDetails.customer.ordersCountOnMerchant.ToString();

            CRUD.Adicionar(pedido);

            foreach (var item in orderDetails.items)
            {
                AdicionarPedidoProduto(pedido.IDPedido.Value, item.externalCode, item.name, item.unitPrice, item.quantity);

                if (item.options != null)
                {
                    foreach (var modificacoes in item.options)
                    {
                        AdicionarPedidoProduto(pedido.IDPedido.Value, modificacoes.externalCode, modificacoes.name, modificacoes.unitPrice, modificacoes.quantity);
                    }
                }
            }

            foreach (var pagamento in orderDetails.payments.methods)
            {
                AdicionarPedidoPagamento(pedido.IDPedido.Value, pagamento);
            }
        }

        private ClienteInformation AdicionarCliente(Model.Order.OrderDetails orderDetails)
        {
            ClienteInformation cliente = CarregarCliente(orderDetails.customer.id);

            cliente.NomeCompleto = orderDetails.customer.name;
            cliente.Telefone1Numero = Convert.ToInt32(orderDetails.customer.phone.localizer);
            cliente.Documento1 = orderDetails.customer.documentNumber;
            cliente.Endereco = orderDetails.delivery.deliveryAddress.streetName;
            cliente.EnderecoNumero = orderDetails.delivery.deliveryAddress.streetNumber;
            cliente.Complemento = orderDetails.delivery.deliveryAddress.complement;
            cliente.Bairro = orderDetails.delivery.deliveryAddress.neighborhood;
            cliente.Cidade = orderDetails.delivery.deliveryAddress.city;
            cliente.CEP = Convert.ToInt32(orderDetails.delivery.deliveryAddress.postalCode);
            cliente.EnderecoReferencia = orderDetails.delivery.deliveryAddress.reference;
            cliente.Bloqueado = false;

            if (cliente.IDCliente == null)
            {
                cliente.TAG = "{ \"ifood_customer-id\":\"" + orderDetails.customer.id + "\" }";
                cliente.DtInclusao = DateTime.Now;
            }

            CRUD.Salvar(cliente);

            return cliente;
        }

        private ClienteInformation CarregarCliente(string customerId)
        {
            ClienteInformation cliente = new ClienteInformation();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = "SELECT IDCliente FROM tbCliente WHERE TAG LIKE '%\"ifood_customer-id\":\"' + @customerId + '\"%'";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@customerId", customerId);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                cliente.IDCliente = Convert.ToInt32(dt.Rows[0]["IDCliente"]);
                CRUD.Carregar(cliente);
            }

            return cliente;
        }

        private void AdicionarPedidoProduto(Int32 idPedido, string externalCode, string nome, decimal valorUnitario, decimal quantidade)
        {
            PedidoProdutoInformation pedidoProduto = new PedidoProdutoInformation();
            Int32 idProduto = 0;
            String notas = "";

            CarregarProduto(externalCode, nome, out idProduto, out notas);

            pedidoProduto.Produto = new ProdutoInformation();
            pedidoProduto.Produto.IDProduto = idProduto;

            pedidoProduto.Pedido = new PedidoInformation();
            pedidoProduto.Pedido.IDPedido = idPedido;

            pedidoProduto.PDV = PDVIFood;
            pedidoProduto.Usuario = UsuarioIfood;

            pedidoProduto.Quantidade = quantidade;
            pedidoProduto.ValorUnitario = valorUnitario;
            pedidoProduto.DtInclusao = DateTime.Now;
            pedidoProduto.CodigoAliquota = "";

            pedidoProduto.Notas = notas;

            pedidoProduto.Cancelado = false;
            pedidoProduto.RetornarAoEstoque = false;

            CRUD.Adicionar(pedidoProduto);
        }

        private void CarregarProduto(string externalCode, string nome, out Int32 idProduto, out String notas)
        {
            if (externalCode == "")
            {
                idProduto = 1;
                notas = $"(sem código: {nome})";
                //    log += $"\r\n{idPdod}: {notaProduto}";
            }

            ProdutoInformation produto = Produto.Carregar(Convert.ToInt32(externalCode));

            if (produto == null || produto.Nome == null)
            {
                idProduto = 1;
                notas = $"(não existe: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else if (produto.Excluido == true)
            {
                idProduto = 1;
                notas = $"(excluido: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else if (produto.Ativo == false)
            {
                idProduto = 1;
                notas = $"(inativo: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else
            {
                idProduto = produto.IDProduto.Value;
                notas = "";
            }
        }

        private void AdicionarPedidoPagamento(Int32 idPedido, Model.Order.PaymentMethod paymentMethod)
        {
            PedidoPagamentoInformation pedidoPagamento = new PedidoPagamentoInformation();

            pedidoPagamento.TipoPagamento = new TipoPagamentoInformation();
            pedidoPagamento.TipoPagamento.IDTipoPagamento = CarregarTipoPagamento(paymentMethod.type, paymentMethod.method);

            pedidoPagamento.Pedido = new PedidoInformation();
            pedidoPagamento.Pedido.IDPedido = idPedido;

            pedidoPagamento.Valor = paymentMethod.value;
            pedidoPagamento.DataPagamento = DateTime.Now;
            pedidoPagamento.UsuarioPagamento = UsuarioIfood;

            //pedido.@IDMetodo = ''--payments.methods.method;
            //pedido.IDContaRecebivel;
            //pedido.@IDBandeira = ''--payments.methods.card.brand;
            //pedido.IDGateway

            CRUD.Adicionar(pedidoPagamento);
        }

        private Int32 CarregarTipoPagamento(string type, string method)
        {
            Int32 idTipoPagamento;

            if (type == "OFFLINE")
            {
                switch(method)
                {
                    case "DEBIT":
                        idTipoPagamento = PagamentoDebito.IDTipoPagamento.Value;
                        break;
                    default:
                        idTipoPagamento = PagamentoOutros.IDTipoPagamento.Value;
                        break;
                }
            }
            else
            {
                return PagamentoIFood.IDTipoPagamento.Value;
            }

            return idTipoPagamento;
        }

        private void AlterarStatus(Model.Order.Event evento)
        {
            //var query = @"update tbproduto set Disponibilidade = @disponivel
            //    from tbProdutoCategoriaProduto pcp
            //    inner join tbProduto on tbProduto.IDProduto = pcp.IDProduto
            //    where pcp.IDCategoriaProduto = @idCategoria
            //    and tbProduto.Ativo = 1 and tbProduto.Excluido = 0";

            //using (var conn = new SqlConnection(DB.ConnectionString))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = query;
            //        //cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
            //        //cmd.Parameters.AddWithValue("@disponivel", disponivel);

            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }
        public DataTable ListarPedidosAlterados()
        {
            //SqlDataAdapter da;
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();

            //String querySql = @"";

            //da = new SqlDataAdapter(querySql, DB.ConnectionString);

            //da.Fill(ds);
            //dt = ds.Tables[0];

            //return dt;

            return null;
        }
        public void ExportarAlteracaoStatus()
        {
            Int32 idPedido;

            DataTable listaPedidos = ListarPedidosAlterados();

            for (int i = 0; i < listaPedidos.Rows.Count; i++)
            {
                idPedido = Convert.ToInt32(listaPedidos.Rows[i]["IDPedido"]);
            }
        }
    }
}
