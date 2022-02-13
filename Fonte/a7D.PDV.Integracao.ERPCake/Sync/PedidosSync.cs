using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.ERPCake.DTO;
using a7D.PDV.Integracao.ERPCake.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Integracao.ERPCake.Sync
{
    public static class PedidosSync
    {
        private static int idOoperacaoFiscal;
        
        private static IDictionary<int, Payment_Form_Condition> condicoes;

        public static bool SincronizarPedidos(this ICakeBase cake)
        {
            if (idOoperacaoFiscal == 0)
            {
                var listaOperacaoFiscal = cake.api.All<Fiscal_Operation>();
                var operacaoFiscal = listaOperacaoFiscal.FirstOrDefault(o => o.name == "Venda");
                if (operacaoFiscal == null)
                {
                    cake.AddLog("Operação fiscal 'Venda' não encontrada");
                    return false;
                }
                idOoperacaoFiscal = operacaoFiscal.id.Value;
            }

            condicoes = cake.CacheGetAll<Payment_Form_Condition>();
            if (condicoes == null || condicoes.Count == 0)
            {
                cake.AddLog("Falta cadastrar as condições de pagamento");
                return false;
            }

            var formas = cake.CacheGetAll<Payment_Form>();
            var cartoes = formas.Values.Where(p => p.IsCardType());

            foreach (var cartao in cartoes)
            {
                var qtd = condicoes.Values.Count(c => c.payment_form == cartao.id);
                if (qtd == 0)
                {
                    cake.AddLog("Não há condições de pagamento configuradas para: " + cartao.name);
                    return false;
                }
                else if (qtd > 1)
                {
                    cake.AddLog("Há mais de uma condições de pagamento configuradas para: " + cartao.name);
                    return false;
                }
            }

            // Subir as vendas
            var pedidos = Pedido.ListarNaoSincERP(cake.DataInicial);

            if (!pedidos.Any())
            {
                cake.AddLog("não há novos pedidos a sincronizar");
                return true;
            }

            int n = 0;
            cake.AddLog("Enviando pedidos");
            bool ok = true;

            foreach (var pedidoPDV in pedidos)
            {
                n++;
                string info = "";
                try
                {
                    //if (pedidos.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                    //    info = $"{(100 * n / pedidos.Count).ToString("N1")}% ";

                    info += $"Pedido {pedidoPDV.IDPedido} ({pedidoPDV.DtPedido.Value.ToString("dd/MM/yy HH:mm:ss")}) ";

                    pedidoPDV.ListaProduto = PedidoProduto.ListarPorPedido(pedidoPDV.IDPedido.Value);
                    if (pedidoPDV.ListaProduto.Count == 0)
                    {
                        info += " (sem produtos)";
                        pedidoPDV.SincERP = true;
                        Pedido.Salvar(pedidoPDV);
                        continue;
                    }

                    var itens = new List<Sales_Order_Item>();
                    if (!cake.SincronizarListaProdutos(pedidoPDV.ListaProduto, itens, ref info))
                    {
                        cake.AddLog(info);
                        ok = false;
                        continue;
                    }

                    pedidoPDV.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(pedidoPDV.IDPedido.Value);
                    var pagamentos = new List<Sales_Order_Parcel>();
                    var pagamentosP1 = new List<Sales_Order_Parcel_Group>();
                    pedidoPDV.ListaPagamento.ForEach(p =>
                    {
                        if (p.DataPagamento == null)
                            p.DataPagamento = pedidoPDV.DtPedidoFechamento.Value;
                    });

                    if (!cake.SincronizarListaPagamentos(pedidoPDV.ListaPagamento, pagamentos, pagamentosP1, ref info))
                    {
                        cake.AddLog(info);
                        ok = false;
                        continue;
                    }

                    if (pagamentos.Count == 0 && pedidoPDV.ValorTotal == 0 && pedidoPDV.ValorDesconto == 0)
                        info += "vazio";
                    else
                    {
                        var pedidoERP = pedidoPDV.Convert();

                        if (pedidoPDV.Cliente != null)
                        {
                            pedidoPDV.Cliente = Cliente.Carregar(pedidoPDV.Cliente.IDCliente.Value);
                            if (!int.TryParse(pedidoPDV.Cliente.CodigoERP, out int nCustomer))
                            {
                                cake.AddLog(info + " Cliente ainda não foi sincronizado no ERP");
                                ok = false;
                                continue;
                            }
                            pedidoERP.customer = nCustomer;
                        }
                        else
                            pedidoERP.customer = cake.DefaultCustomer;

                        var cliente = GenericSync.GetByID<Customer>(pedidoERP.customer);
                        if (cliente == null)
                        {
                            cake.AddLog(info + " Cliente não existe no ERP");
                            ok = false;
                            continue;
                        }

                        pedidoPDV.Caixa = Caixa.Carregar(pedidoPDV.Caixa.IDCaixa.Value);
                        pedidoERP.internal_obs = $"Caixa: {pedidoPDV.Caixa.IDCaixa} PDV: {pedidoPDV.Caixa.PDV.IDPDV}";
                        pedidoERP.invoice_model = 59; // SAT

                        if (pedidoPDV.ListaPagamento.Count > 0)
                            pedidoERP.seller = GenericSync.FirstOrDefault<Seller>(c => c.code == pedidoPDV.ListaPagamento[0].UsuarioPagamento?.IDUsuario.ToString())?.id;

                        pedidoERP.fiscal_operation = idOoperacaoFiscal;
                        pedidoERP.subtotal = itens.Sum(i => i.total);

                        var order = cake.api.Save(pedidoERP);
                        if (order == null)
                        {
                            cake.AddLog(info + "Erro ao gerar o pedido");
                            ok = false;
                            continue;
                        }
                        info += $" ERP: {order}\r\n{cliente.name}\r\n";

                        foreach (var item in itens)
                        {
                            item.sales_order_id = order.id.Value;
                            var result = cake.api.Save(item);
                            info += $"\t{result}\r\n";
                        }

                        if (pagamentos.Count == 0)
                            info += "Sem Pagamentos\r\n";
                        else
                        {
                            info += "Pagmentos\r\n";
                            for (int i = 0; i < pagamentos.Count; i++)
                            {
                                var itemP1 = pagamentosP1[i];
                                itemP1.sales_order_id = order.id.Value;
                                var resultP1 = cake.api.Save(itemP1);

                                var item = pagamentos[i];
                                item.sales_order_id = order.id.Value;
                                item.sales_parcel_group = resultP1.id.Value;
                                var result = cake.api.Save(item);

                                info += $"\t{result.id}:{resultP1}\r\n";
                            }

                            try
                            {
                                if (pedidoPDV.ListaPagamento.Any(p => p.IDGateway == (int)EGateway.ContaCliente && p.IDSaldoBaixa == null))
                                {
                                    // Só fatura o que está quitado
                                    info += "Aguardando quitação para faturar\r\n";
                                }
                                else
                                {
                                    var result2 = cake.api.BillOrder(order.id.Value);
                                    info += "Faturado\r\n";
                                }
                            }
                            catch (Exception ex)
                            {
                                info += $"Erro ao Faturar: {ex.Message}\r\n{cake.api.LastRequest}\r\n{cake.api.LastResult ?? ex.StackTrace}\r\n";
                            }
                        }
                    }

                    pedidoPDV.SincERP = true;
                    Pedido.Salvar(pedidoPDV);

                    cake.AddLog(info);

                    Logs.Info(CodigoInfo.I010, info);
                }
                catch (Exception ex)
                {
                    cake.AddLog(info + "!!! ERRO !!!\r\n");

                    var exPDV = new ExceptionPDV(CodigoErro.EE03, ex);
                    exPDV.Data.Add("info", info);
                    cake.AddLog(exPDV, true);
                }
            }
            return ok;
        }

        internal static bool SincronizarListaProdutos(this ICakeBase cake, List<PedidoProdutoInformation> ListaProdutos, List<Sales_Order_Item> itens, ref string info)
        {
            bool ok = true;

            foreach (var pp in ListaProdutos)
            {
                try
                {
                    if (GenericSync.ContainsID<Product>(pp.Produto.CodigoERP))
                        itens.Add(pp.Convert());
                    else
                    {
                        info += ($"\r\n\tProduto: '{pp.Produto.Nome}' não contem código no ERP");
                        ok = false;
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    info += $"\tERRO PedidoProduto {pp.IDPedidoProduto} {ex.Message}";
                    ok = false;
                }
            }
            return ok;
        }

        internal static bool SincronizarListaPagamentos(this ICakeBase cake, List<PedidoPagamentoInformation> listaPagamentos, List<Sales_Order_Parcel> pagamentos, List<Sales_Order_Parcel_Group> pagamentosP1, ref string info)
        {
            bool ok = true;
            // paymentForms
            foreach (var pag in listaPagamentos)
            {
                try
                {
                    // http://app.cakeerp.com/api_docs/servicos.html#formas-de-pagamento

                    Payment_Form form;
                    if (pag.TipoPagamento.IDGateway > 0 && pag.TipoPagamento.CodigoERP?.Contains(",") == true)
                    {
                        // ATENÇÃO: Credito, Debito - Exatamente nesta sequencia
                        var ids = pag.TipoPagamento.CodigoERP.Split(',');
                        if (pag.MeioPagamentoSAT.IDMeioPagamentoSAT == (int)EMetodoPagamento.Credito)
                            form = GenericSync.GetByID<Payment_Form>(ids[0]);
                        else if (pag.MeioPagamentoSAT.IDMeioPagamentoSAT == (int)EMetodoPagamento.Debito)
                            form = GenericSync.GetByID<Payment_Form>(ids[1]);
                        else if (pag.TipoPagamento.IDGateway > 0)
                            form = GenericSync.GetByID<Payment_Form>(ids[0]); // Como credito
                        else
                        {
                            info += ($"\r\n\tTipo de Pagamento: '{pag.TipoPagamento.Nome}' incompatível com o sistema");
                            ok = false;
                            continue;
                        }
                    }
                    else
                        form = GenericSync.GetByID<Payment_Form>(pag.TipoPagamento.CodigoERP);

                    if (form == null)
                    {
                        info += ($"\r\n\tTipo de Pagamento: '{pag.TipoPagamento.Nome}' não contem código no ERP");
                        ok = false;
                        continue;
                    }

                    var condicao = condicoes.Values.FirstOrDefault(c => c.payment_form == form.id);
                    if (condicao == null && form.IsCardType())
                    {
                        info += ($"\r\n\tNão há condição de pagamento válido para: '{pag.TipoPagamento.Nome}'");
                        ok = false;
                        continue;
                    }

                    var parcel = pag.Convert(form.id.Value, condicao?.days_parcel ?? 0);
                    pagamentos.Add(parcel);

                    var parcelP1 = pag.ConvertP1(form.id.Value, condicao?.percentage ?? 0);
                    pagamentosP1.Add(parcelP1);
                }
                catch (Exception ex)
                {
                    info += $"\tERRO Pagamento {pag.IDPedidoPagamento} {ex.Message}";
                    ok = false;
                }
            }
            return ok;
        }
    }
}
