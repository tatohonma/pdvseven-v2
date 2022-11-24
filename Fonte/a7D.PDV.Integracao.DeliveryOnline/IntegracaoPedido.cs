using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.DAL;
using a7D.PDV.EF.Enum;
//using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    public partial class IntegraDeliveryOnline
    {
        //private void ConfirmarCancelamentoPedido(Model.Order.Event evento)
        //{
        //    PedidoInformation pedido = CarregarPedidoPorOrderId(evento.orderId);
        //    if (pedido != null && pedido.IDPedido != null)
        //    {
        //        AlterarStatusPedido(pedido.IDPedido.Value, EStatusPedido.Cancelado);
        //    }
        //    else
        //    {
        //        AddLog("Pedido " + evento.orderId + " não encontrado!");
        //    }
        //}

        //private void CancelarPedido(Model.Order.Event evento)
        //{
        //    PedidoInformation pedido = CarregarPedidoPorOrderId(evento.orderId);

        //    if (pedido != null && pedido.IDPedido != null)
        //    {
        //        if (pedido.StatusPedido.StatusPedido == EStatusPedido.Aberto)
        //        {
        //            AlterarStatusPedido(pedido.IDPedido.Value, EStatusPedido.EmCancelamento);
        //        }
        //        else
        //        {
        //            AlterarStatusPedido(pedido.IDPedido.Value, EStatusPedido.Cancelado);
        //        }

        //        TagInformation tagStatus = new TagInformation();
        //        tagStatus.GUIDIdentificacao = pedido.GUIDIdentificacao;
        //        tagStatus.Chave = "ifood-status";
        //        CRUD.Carregar(tagStatus);

        //        tagStatus.Valor = evento.code;
        //        CRUD.Alterar(tagStatus);
        //    }
        //    else
        //    {
        //        AddLog("Pedido " + evento.orderId + " não encontrado!");
        //    }
        //}

        //private void FinalizarPedido(Model.Order.Event evento)
        //{
        //    PedidoInformation pedido = CarregarPedidoPorOrderId(evento.orderId);

        //    if (pedido != null && pedido.IDPedido != null)
        //    {
        //        if (ConfigIFood.FinalizacaoAutomatica)
        //        {
        //            AlterarStatusPedido(pedido.IDPedido.Value, EStatusPedido.Finalizado);
        //        }

        //        TagInformation tagStatus = new TagInformation();
        //        tagStatus.GUIDIdentificacao = pedido.GUIDIdentificacao;
        //        tagStatus.Chave = "ifood-status";
        //        CRUD.Carregar(tagStatus);

        //        tagStatus.Valor = "CON";
        //        CRUD.Alterar(tagStatus);
        //    }
        //    else
        //    {
        //        AddLog("Pedido " + evento.orderId + " não encontrado!");
        //    }
        //}

        private void AdicionarPedido(Model.Orders.DataInformation pedidoApi)
        {
            PedidoInformation pedido = new PedidoInformation();
            string voucherDesconto;
            //string tipoTaxaAdicional;

            AddLog(JsonConvert.SerializeObject(pedidoApi));

            if (CarregarPedidoPorOrderId(pedidoApi.id) != null)
            {
                AddLog($"Pedido {pedidoApi.id} já importado");

                //AlterarStatusPedidoAPI(pedidoApi.id, 1);
                return;
            }

            pedido.Cliente = AdicionarCliente(pedidoApi);
            pedido.Observacoes += "Cliente: " + pedido.Cliente.NomeCompleto + "\r\n\r\n";
            pedido.Observacoes += "Endereço: " + pedido.Cliente.EnderecoCompleto + "\r\n\r\n";

            //pedido.DocumentoCliente = orderDetails.customer.documentNumber;

            pedido.TipoPedido = new TipoPedidoInformation();
            pedido.TipoPedido.IDTipoPedido = 30;

            pedido.StatusPedido = new StatusPedidoInformation();
            pedido.StatusPedido.IDStatusPedido = (int)EStatusPedido.NaoConfirmado;

            pedido.OrigemPedido = new OrigemPedidoInformation();
            pedido.OrigemPedido.IDOrigemPedido = (int)EOrigemPedido.deliveryOnline;

            //pedido.Caixa = CaixaPDV;

            pedido.GUIDIdentificacao = Guid.NewGuid().ToString();
            //pedido.DocumentoCliente = orderDetails.customer.documentNumber;
            pedido.DtPedido = DateTime.Now;
            pedido.PermitirAlterar = false;

            pedido.TaxaEntrega = new TaxaEntregaInformation();
            pedido.TaxaEntrega = TaxaEntregaDO;
            string taxaEntrega = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "delivery").value;
            pedido.ValorEntrega = Convert.ToDecimal(taxaEntrega, new CultureInfo("en-us"));
            pedido.TaxaServicoPadrao = 0;

            var cupom = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "coupon");
            if (cupom != null)
            {
                pedido.TipoDesconto = TipoDescontoDO;

                voucherDesconto = "CUPOM DE DESCONTO\r\n";
                voucherDesconto += " > " + cupom.title + ": R$ " + Convert.ToDecimal(cupom.value, new CultureInfo("en-us")) + "\r\n";

                pedido.Observacoes += voucherDesconto + "\r\n";
                pedido.ObservacaoCupom += voucherDesconto + "\r\n";

                pedido.ValorDesconto = Convert.ToDecimal(cupom.value, new CultureInfo("en-us")) * -1;
            }
            else
            {
                pedido.ValorDesconto = 0;
            }

            //pedido.ValorServico = orderDetails.total.additionalFees;
            string strValorTotal = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "total").value;
            pedido.ValorTotal = Convert.ToDecimal(strValorTotal, new CultureInfo("en-us"));

            //if (orderDetails.orderTiming == "SCHEDULED")
            //{
            //    pedido.Observacoes += "AGENDADO PARA " + orderDetails.schedule.deliveryDateTimeEnd + "\r\n\r\n";
            //    pedido.ObservacaoCupom += "AGENDADO PARA " + orderDetails.schedule.deliveryDateTimeEnd + "\r\n\r\n";
            //    AddLog("AGENDADO PARA " + orderDetails.schedule.deliveryDateTimeEnd);
            //}

            //if (orderDetails.customer.ordersCountOnMerchant == 0)
            //{
            //    pedido.Observacoes += "NOVO CLIENTE " + "\r\n\r\n";
            //    pedido.ObservacaoCupom += "NOVO CLIENTE " + "\r\n\r\n";
            //}
            //else
            //{
            //    pedido.Observacoes += "FIDELIDADE " + orderDetails.customer.ordersCountOnMerchant.ToString() + "\r\n\r\n";
            //    pedido.ObservacaoCupom += "FIDELIDADE " + orderDetails.customer.ordersCountOnMerchant.ToString() + "\r\n\r\n";
            //}

            CRUD.Adicionar(pedido);

            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-order_id", pedidoApi.id);
            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-order_type", pedidoApi.attributes.order_type);
            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-status_id", pedidoApi.attributes.status.status_id.ToString());

            //if (orderDetails.total.additionalFees > 0)
            //{
            //    pedido.Observacoes += "TAXAS ADICIONAIS\r\n";
            //    foreach (var fee in orderDetails.additionalFees)
            //    {
            //        tipoTaxaAdicional = CarregarTipoTaxaAdicional(fee.type);
            //        AdicionarPedidoProduto(pedido.IDPedido.Value, null, TaxaAdicional.IDProduto.ToString(), tipoTaxaAdicional, fee.value, 1, tipoTaxaAdicional);
            //        pedido.Observacoes += " - " + tipoTaxaAdicional + ": R$ " + fee.value.ToString("#,##0.00") + "\r\n";
            //    }

            //    pedido.Observacoes += "\r\n";
            //}

            ProdutoInformation produto;
            Int32 idProduto;
            String observacaoProduto = "";

            ProdutoInformation modificacao;
            Int32 idProduto_modificacao;
            String observacaoModificacao = "";

            Int32 idPedidoProduto;
            Decimal valorUnitario;


            pedido.Observacoes += "ITENS\r\n";
            foreach (var p in pedidoApi.attributes.order_menus)
            {
                produto = CarregarProdutoPorMenuId(p.menu_id);
                CarregarProduto(produto, p.name, out idProduto, out observacaoProduto);

                if (!String.IsNullOrWhiteSpace(observacaoProduto) && !String.IsNullOrWhiteSpace(p.comment))
                    observacaoProduto = observacaoProduto + "\r\n" + p.comment;
                else if(String.IsNullOrWhiteSpace(observacaoProduto) && !String.IsNullOrWhiteSpace(p.comment))
                    observacaoProduto = p.comment;

                valorUnitario = Convert.ToDecimal(p.price, new CultureInfo("en-us"));
                idPedidoProduto = AdicionarPedidoProduto(pedido.IDPedido.Value, null, idProduto, valorUnitario, p.quantity, observacaoProduto);
                pedido.Observacoes += " - " + p.name + " (qtd " + p.quantity + ")\r\n";

                if (p.menu_options != null)
                {
                    foreach (var m in p.menu_options)
                    {
                        modificacao = CarregarModificacaoPorNome(m.menu_id, m.order_option_name);
                        CarregarProduto(modificacao, m.order_option_name, out idProduto_modificacao, out observacaoModificacao);

                        valorUnitario = Convert.ToDecimal(m.order_option_price, new CultureInfo("en-us"));
                        AdicionarPedidoProduto(pedido.IDPedido.Value, idPedidoProduto, idProduto_modificacao, valorUnitario, m.quantity, observacaoModificacao);
                        pedido.Observacoes += " -- " + m.order_option_name + " (qtd " + m.quantity + ")\r\n";
                    }
                }
            }

            Int32 idTipoPagamento = Convert.ToInt32(pedidoApi.attributes.payment);
            AdicionarPedidoPagamento(pedido.IDPedido.Value, idTipoPagamento, pedido.ValorTotal.Value);

            CRUD.Salvar(pedido);

            if (ConfigDO.ConfirmacaoAutomatica == true)
            {
                ConfirmarPedido(pedidoApi);
            }
        }

        //private string CarregarTipoTaxaAdicional(string feeType)
        //{
        //    string descricao;
        //    switch (feeType)
        //    {
        //        case "SMALL_ORDER_FEE":
        //            descricao = "pedido abaixo do valor mínimo";
        //            break;
        //        default:
        //            descricao = feeType;
        //            break;
        //    }

        //    return descricao;
        //}

        public void GerarOrdemProducao(Int32 idPedido)
        {
            try
            {
                PedidoInformation pedido = Pedido.CarregarCompleto(idPedido);
                OrdemProducaoServices.GerarOrdemProducao(pedido.ListaProduto, false);
                AddLog("Confirmação automatica e Ordem de Produção gerada!");

                if (ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "NOVO") // Aprovação automatica
                {
                    OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, ConfiguracoesSistema.Valores.IDAreaViaExpedicao);
                    AddLog("Via de expedição gerada!");
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro: " + ex.Message);
            }
        }

        private ClienteInformation AdicionarCliente(Model.Orders.DataInformation pedidoApi)
        {
            bool novoCliente = false;
            ClienteInformation cliente = CarregarCliente(pedidoApi.attributes.customer_id);

            if (cliente.IDCliente == null)
            {
                novoCliente = true;
                cliente.GUIDIdentificacao = Guid.NewGuid().ToString();
                cliente.DtInclusao = DateTime.Now;
            }

            cliente.NomeCompleto = pedidoApi.attributes.first_name;
            cliente.Telefone1Numero = Convert.ToInt32(pedidoApi.attributes.telephone.Substring(2));
            //cliente.Documento1 = orderDetails.customer.documentNumber;

            if (pedidoApi.attributes.order_type == "delivery")
            {
                var location = APILocations.GetLocations(pedidoApi.attributes.location_id).data;

                cliente.Endereco = location.attributes.location_address_1;
                cliente.EnderecoNumero = location.attributes.location_address_2;
                //cliente.Complemento = location.attributes.;
                //cliente.Bairro = location.attributes.;
                cliente.Cidade = location.attributes.location_city;
                //cliente.Estado = location.attributes.location_state;
                cliente.CEP = Convert.ToInt32(location.attributes.location_postcode);
                //cliente.EnderecoReferencia = location.attributes.;
            }
            //    else if (orderDetails.orderType == "TAKEOUT")
            //    {
            //        cliente.Endereco = "RETIRADA";
            //        cliente.EnderecoNumero = "";
            //        cliente.Complemento = "";
            //        cliente.Bairro = "";
            //        cliente.Cidade = "";
            //        cliente.CEP = 0;
            //        cliente.EnderecoReferencia = "";
            //    }
            //    else if (orderDetails.orderType == "INDOOR")
            //    {
            //        cliente.Endereco = "PEDIDO NA MESA " + orderDetails.indoor.table;
            //        cliente.EnderecoNumero = "";
            //        cliente.Complemento = "";
            //        cliente.Bairro = "";
            //        cliente.Cidade = "";
            //        cliente.CEP = 0;
            //        cliente.EnderecoReferencia = "";
            //    }
            cliente.Bloqueado = false;

            CRUD.Salvar(cliente);

            if (novoCliente)
            {
                Tag.Adicionar(cliente.GUIDIdentificacao, "deliveryOnline-customer_id", pedidoApi.attributes.customer_id.ToString());
            }

            return cliente;
        }

        private ClienteInformation CarregarCliente(Int32 customer_id)
        {
            ClienteInformation cliente = new ClienteInformation();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                    SELECT 
                        IDCliente
                    FROM
                        tbCliente c
                        INNER JOIN tbTag t ON t.GUIDIdentificacao = c.GUIDIdentificacao
                    WHERE
                        Chave = 'deliveryOnline-customer_id' AND
                        Valor = @customer_id
                ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@customer_id", customer_id);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                cliente.IDCliente = Convert.ToInt32(dt.Rows[0]["IDCliente"]);
                CRUD.Carregar(cliente);
            }

            return cliente;
        }

        private Int32 AdicionarPedidoProduto(Int32 idPedido, Int32? idPedidoProduto_pai, Int32 idProduto, decimal valorUnitario, decimal quantidade, string notas)
        {
            PedidoProdutoInformation pedidoProduto = new PedidoProdutoInformation();

            pedidoProduto.Produto = new ProdutoInformation();
            pedidoProduto.Produto.IDProduto = idProduto;

            pedidoProduto.Pedido = new PedidoInformation();
            pedidoProduto.Pedido.IDPedido = idPedido;

            if (idPedidoProduto_pai != null)
            {
                pedidoProduto.PedidoProdutoPai = new PedidoProdutoInformation();
                pedidoProduto.PedidoProdutoPai.IDPedidoProduto = idPedidoProduto_pai;
            }

            pedidoProduto.PDV = PDVDO;
            pedidoProduto.Usuario = UsuarioDO;

            pedidoProduto.Quantidade = quantidade;
            pedidoProduto.ValorUnitario = valorUnitario;
            pedidoProduto.DtInclusao = DateTime.Now;
            pedidoProduto.CodigoAliquota = "";

            pedidoProduto.Notas = notas;

            pedidoProduto.Cancelado = false;
            pedidoProduto.RetornarAoEstoque = false;

            CRUD.Adicionar(pedidoProduto);

            return pedidoProduto.IDPedidoProduto.Value;
        }

        private void CarregarProduto(ProdutoInformation produto, String nome, out Int32 idProduto, out String observacaoProduto)
        {
            if (produto == null || produto.Nome == null)
            {
                idProduto = 1;
                observacaoProduto = $"(não existe: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else if (produto.Excluido == true)
            {
                idProduto = 1;
                observacaoProduto = $"(excluido: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else if (produto.Ativo == false)
            {
                idProduto = 1;
                observacaoProduto = $"(inativo: {nome})";
                //log += $"\r\n{idPdod}: {notaProduto}";
            }
            else
            {
                idProduto = produto.IDProduto.Value;
                observacaoProduto = "";
            }
        }

        private void AdicionarPedidoPagamento(Int32 idPedido, Int32 idTipoPagamento, Decimal valor)
        {
            PedidoPagamentoInformation pedidoPagamento = new PedidoPagamentoInformation();

            pedidoPagamento.TipoPagamento = TipoPagamento.Carregar(idTipoPagamento);

            pedidoPagamento.Pedido = new PedidoInformation();
            pedidoPagamento.Pedido.IDPedido = idPedido;

            pedidoPagamento.Valor = valor;
            pedidoPagamento.DataPagamento = DateTime.Now;
            pedidoPagamento.UsuarioPagamento = UsuarioDO;
            pedidoPagamento.Excluido = false;

            pedidoPagamento.MeioPagamentoSAT = pedidoPagamento.TipoPagamento.MeioPagamentoSAT;
            pedidoPagamento.IDGateway = (int?)pedidoPagamento.TipoPagamento.Gateway;

            //if (paymentMethod.method == "CASH" && paymentMethod.cash.changeFor > 0)
            //    pedidoPagamento.Valor = paymentMethod.cash.changeFor;

            CRUD.Adicionar(pedidoPagamento);
        }

        private void AlterarStatusPedidoSistema(int idPedido, EStatusPedido statusPedido)
        {
            var query = @"UPDATE tbPedido set IDStatusPedido=@idStatusPedido WHERE idPedido=@idPedido";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    cmd.Parameters.AddWithValue("@idStatusPedido", (int)statusPedido);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private PedidoInformation CarregarPedidoPorOrderId(string order_id)
        {
            PedidoInformation pedido = new PedidoInformation();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
                    IDPedido
                FROM
                    tbPedido p
                    INNER JOIN tbTag t ON t.GUIDIdentificacao = p.GUIDIdentificacao
                WHERE
                    Chave = 'DeliveryOnline-order_id' AND
                    Valor = @order_id
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@order_id", order_id);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                pedido = Pedido.Carregar(Convert.ToInt32(dt.Rows[0]["IDPedido"]));
                return pedido;
            }
            else
            {
                return null;
            }
        }

        private DataRowCollection ListarPedidosNaoSincronizados()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                p.GUIDIdentificacao,
	                p.IDPedido,
	                p.IDStatusPedido,
	                t1.Valor as status_id,
	                t2.Valor as order_id
                FROM 
	                tbPedido p
	                INNER JOIN tbTag t1 ON p.GUIDIdentificacao=t1.GUIDIdentificacao AND t1.Chave='DeliveryOnline-status_id'
	                INNER JOIN tbTag t2 ON p.GUIDIdentificacao=t2.GUIDIdentificacao AND t2.Chave='DeliveryOnline-order_id'
                WHERE
	                (p.IDStatusPedido=10 AND t1.Valor<>3)
	                OR
	                (p.IDStatusPedido=20 AND t1.Valor<>4)
	                OR
	                (p.IDStatusPedido=40 AND t1.Valor<>5)
	                OR
	                (p.IDStatusPedido=50 AND t1.Valor<>9)
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];

            return dt.Rows;
        }

        private ProdutoInformation CarregarProdutoPorMenuId(int menu_id)
        {
            ProdutoInformation produto = new ProdutoInformation();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
                    IDProduto
                FROM
                    tbProduto p
                    INNER JOIN tbTag t ON t.GUIDIdentificacao = p.GUIDIdentificacao
                WHERE
                    Chave = 'DeliveryOnline-menu_id' AND
                    Valor = @menu_id
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@menu_id", menu_id);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                produto = Produto.Carregar(Convert.ToInt32(dt.Rows[0]["IDProduto"]));
                return produto;
            }
            else
            {
                return null;
            }
        }

        private ProdutoInformation CarregarModificacaoPorNome(int menu_id, string nome)
        {
            ProdutoInformation produto = new ProdutoInformation();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                m.IDProduto
                FROM 
	                tbProduto m
	                INNER JOIN tbPainelModificacaoProduto pmp ON pmp.IDProduto=m.IDProduto
	                INNER JOIN tbProdutoPainelModificacao ppm ON ppm.IDPainelModificacao=pmp.IDPainelModificacao
	                INNER JOIN tbProduto p ON p.IDProduto=ppm.IDProduto
	                INNER JOIN tbTag t ON t.GUIDIdentificacao=p.GUIDIdentificacao AND t.Chave='DeliveryOnline-menu_id'
                WHERE
	                m.Nome=@nome
	                AND
	                t.Valor=@menu_id
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@menu_id", menu_id);
            da.SelectCommand.Parameters.AddWithValue("@nome", nome);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                produto = Produto.Carregar(Convert.ToInt32(dt.Rows[0]["IDProduto"]));
                return produto;
            }
            else
            {
                return null;
            }
        }

        private void ConfirmarPedido(Model.Orders.DataInformation pedidoApi)
        {
            PedidoInformation pedido = CarregarPedidoPorOrderId(pedidoApi.id);

            if (pedido != null && pedido.IDPedido != null)
            {
                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                {
                    AlterarStatusPedidoSistema(pedido.IDPedido.Value, EStatusPedido.Aberto);
                    AlterarStatusPedidoAPI(pedidoApi.id, 3);
                    GerarOrdemProducao(pedido.IDPedido.Value);
                }
                else
                {
                    AddLog("Pedido já estava confirmado");
                }
            }
        }

        private void AlterarStatusPedidoAPI(string orderId, int statusId)
        {
            APIOrders.UpdateStatus(orderId, statusId);
        }
    }
}