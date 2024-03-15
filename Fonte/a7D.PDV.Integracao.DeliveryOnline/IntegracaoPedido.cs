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
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    public partial class IntegraDeliveryOnline
    {
        private void AdicionarPedido(Model.Orders.DataInformation pedidoApi, Model.Orders.AttributesAddessInformation endereco)
        {
            PedidoInformation pedido = new PedidoInformation();
            string voucherDesconto;

            AddLog(JsonConvert.SerializeObject(pedidoApi));

            if (CarregarPedidoPorOrderId(pedidoApi.id) != null)
            {
                AddLog($"Pedido {pedidoApi.id} já importado");

                return;
            }

            pedido.Cliente = AdicionarCliente(pedidoApi, endereco);
            pedido.Observacoes += "Cliente: " + pedido.Cliente.NomeCompleto + "\r\n\r\n";
            pedido.Observacoes += "Endereço: " + pedido.Cliente.EnderecoCompleto + "\r\n\r\n";

            pedido.TipoPedido = new TipoPedidoInformation();
            pedido.TipoPedido.IDTipoPedido = 30;

            pedido.StatusPedido = new StatusPedidoInformation();
            pedido.StatusPedido.IDStatusPedido = (int)EStatusPedido.NaoConfirmado;

            pedido.OrigemPedido = new OrigemPedidoInformation();
            pedido.OrigemPedido.IDOrigemPedido = (int)EOrigemPedido.deliveryOnline;

            pedido.GUIDMovimentacao = Guid.NewGuid().ToString();
            pedido.GUIDIdentificacao = Guid.NewGuid().ToString();
            pedido.DtPedido = DateTime.Now;
            pedido.PermitirAlterar = false;

            pedido.TaxaEntrega = new TaxaEntregaInformation();
            pedido.TaxaEntrega = TaxaEntregaDO;
            if (pedidoApi.attributes.order_type == "delivery")
            {
                string taxaEntrega = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "delivery").value;
                pedido.ValorEntrega = Convert.ToDecimal(taxaEntrega, new CultureInfo("en-us"));
            }
            else
            {
                pedido.ValorEntrega = 0;
            }
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

            string strValorTotal = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "total").value;
            pedido.ValorTotal = Convert.ToDecimal(strValorTotal, new CultureInfo("en-us"));

            CRUD.Adicionar(pedido);

            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-order_id", pedidoApi.id);
            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-order_type", pedidoApi.attributes.order_type);
            Tag.Adicionar(pedido.GUIDIdentificacao, "DeliveryOnline-status_id", pedidoApi.attributes.status.status_id.ToString());

            var taxa = pedidoApi.attributes.order_totals.FirstOrDefault(total => total.code == "paymentFee");
            if (taxa != null)
            {
                decimal valorTaxa = Convert.ToDecimal(taxa.value, new CultureInfo("en-us"));

                pedido.Observacoes += "TAXA ADICIONAL\r\n";
                pedido.Observacoes += " - " + taxa.title + ": R$ " + valorTaxa.ToString("#,##0.00") + "\r\n";
                pedido.Observacoes += "\r\n";

                AdicionarPedidoProduto(pedido.IDPedido.Value, null, TaxaAdicional.IDProduto.Value, valorTaxa, 1, "");
            }

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
                else if (String.IsNullOrWhiteSpace(observacaoProduto) && !String.IsNullOrWhiteSpace(p.comment))
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

            AdicionarPedidoPagamento(pedido.IDPedido.Value, pedidoApi.attributes.payment, pedido.ValorTotal.Value);

            if (!String.IsNullOrEmpty(pedidoApi.attributes.comment))
            {
                pedido.Observacoes += "\r\nOBSERVAÇÔES: " + pedidoApi.attributes.comment;
                pedido.ObservacaoCupom += "\r\nOBSERVAÇÔES: " + pedidoApi.attributes.comment;
            }

            CRUD.Salvar(pedido);

            if (ConfigDO.ConfirmacaoAutomatica == true)
            {
                ConfirmarPedido(pedidoApi);
            }
        }

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

        private ClienteInformation AdicionarCliente(Model.Orders.DataInformation pedidoApi, Model.Orders.AttributesAddessInformation endereco)
        {
            bool novoCliente = false;
            ClienteInformation cliente = CarregarCliente(pedidoApi.attributes.customer_id);

            if (cliente.IDCliente == null)
            {
                novoCliente = true;

                cliente.Bloqueado = false;
                cliente.GUIDIdentificacao = Guid.NewGuid().ToString();
                cliente.DtInclusao = DateTime.Now;
            }

            cliente.NomeCompleto = pedidoApi.attributes.first_name.PadLeft(50).Trim();

            long telefone;
            if (pedidoApi.attributes.telephone != null &&
                pedidoApi.attributes.telephone != "" &&
                long.TryParse(pedidoApi.attributes.telephone.Substring(3), out telefone))
            {
                if (pedidoApi.attributes.telephone.Substring(3).Length == 8 ||
                    pedidoApi.attributes.telephone.Substring(3).Length == 9)
                {
                    cliente.Telefone1DDD = 0;
                    cliente.Telefone1Numero = Convert.ToInt32(pedidoApi.attributes.telephone.Substring(3));
                }
                else if (pedidoApi.attributes.telephone.Substring(3).Length == 10 ||
                    pedidoApi.attributes.telephone.Substring(3).Length == 11)
                {
                    cliente.Telefone1DDD = Convert.ToInt32(pedidoApi.attributes.telephone.Substring(3, 2));
                    cliente.Telefone1Numero = Convert.ToInt32(pedidoApi.attributes.telephone.Substring(5));
                }
                else if (cliente.Telefone1Numero == null)
                {
                    cliente.Telefone1DDD = 0;
                    cliente.Telefone1Numero = Convert.ToInt32(pedidoApi.attributes.customer_id);
                }
            }
            else if(cliente.Telefone1Numero == null)
            {
                cliente.Telefone1DDD = 0;
                cliente.Telefone1Numero = Convert.ToInt32(pedidoApi.attributes.customer_id);
            }

            if (pedidoApi.attributes.order_type == "delivery")
            {
                cliente.Endereco = endereco.address_1.PadLeft(500).Trim();
                cliente.Complemento = endereco.address_2.PadLeft(500).Trim(); ;
                cliente.Cidade = endereco.city.PadLeft(500).Trim();
                cliente.Bairro = endereco.state.PadLeft(500).Trim();

                int cep;
                if (endereco.postcode != null &&
                    endereco.postcode != "" &&
                    endereco.postcode.Replace("-", "").Length == 8 &&
                    int.TryParse(endereco.postcode.Replace("-", ""), out cep))
                {
                    cliente.CEP = cep;
                }
                else
                {
                    cliente.CEP = 0;
                }
            } else
            {
                cliente.Endereco = "RETIRADA";
                cliente.EnderecoNumero = "";
                cliente.Complemento = "";
                cliente.Bairro = "";
                cliente.Cidade = "";
                cliente.CEP = 0;
                cliente.EnderecoReferencia = "";
            }

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

        private void AdicionarPedidoPagamento(Int32 idPedido, String tipoPagamento, Decimal valor)
        {
            PedidoPagamentoInformation pedidoPagamento = new PedidoPagamentoInformation();

            pedidoPagamento.TipoPagamento = CarregarTipoPagamento(tipoPagamento);

            pedidoPagamento.Pedido = new PedidoInformation();
            pedidoPagamento.Pedido.IDPedido = idPedido;

            pedidoPagamento.Valor = valor;
            pedidoPagamento.DataPagamento = DateTime.Now;
            pedidoPagamento.UsuarioPagamento = UsuarioDO;
            pedidoPagamento.Excluido = false;

            pedidoPagamento.MeioPagamentoSAT = pedidoPagamento.TipoPagamento.MeioPagamentoSAT;
            pedidoPagamento.IDGateway = (int?)pedidoPagamento.TipoPagamento.Gateway;

            CRUD.Adicionar(pedidoPagamento);
        }

        private TipoPagamentoInformation CarregarTipoPagamento(string tipoPagamento)
        {
            Int32 IntTipoPagamento;
            if (!int.TryParse(tipoPagamento, out IntTipoPagamento))
            {
                return PagamentoOutros;
            }

            IntTipoPagamento = Convert.ToInt32(tipoPagamento);

            if (IntTipoPagamento >= 100 && IntTipoPagamento <= 199)
            {
                return PagamentoDinheiro;
            }
            else if (IntTipoPagamento >= 200 && IntTipoPagamento <= 299)
            {
                return PagamentoCredito;
            }
            else if (IntTipoPagamento >= 300 && IntTipoPagamento <= 399)
            {
                return PagamentoDebito;
            }
            else if (IntTipoPagamento >= 400 && IntTipoPagamento <= 499)
            {
                return PagamentoRefeicao;
            }
            else if (IntTipoPagamento >= 500 && IntTipoPagamento <= 599)
            {
                return PagamentoPixConta;
            }
            else
                return PagamentoOutros;
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