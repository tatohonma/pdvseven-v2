using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Integracao.iFood
{
    public partial class _IntegraIFood
    {
        private void InserePedido(Evento evento)
        {
            string log = "?";
            string jsonPedido = "?";
            try
            {
                var pedidoIFood = ifoodAPI.Pedido(evento.correlationId);
                jsonPedido = ifoodAPI.LastResult;

                log = DTO.CriaPedido(pedidoIFood, out int idPedido, ifoodPDV, ifoodCaixa, ifoodUsuario,
                    ifoodPagamento.IDTipoPagamento.Value, ifoodDinheiro.IDTipoPagamento.Value, ifoodDebito.IDTipoPagamento.Value, ifoodCredito.IDTipoPagamento.Value, ifoodRefeicao.IDTipoPagamento.Value, ifoodOutros.IDTipoPagamento.Value,
                    ifoodTaxa.IDTaxaEntrega.Value);

                if (!string.IsNullOrEmpty(log))
                {
                    Logs.Info(CodigoInfo.I005, log + "\r\n\r\nJSON:\r\n" + jsonPedido,
                        $"iFood {pedidoIFood.shortReference} Pedido {idPedido} Novo");

                    ifoodAPI.PedidoIntegrado(pedidoIFood.reference);
                    log += " => Integrado";
                    AddLog(log);

                    GA.PostEvento(ifoodPDV, "iFood");
                }
            }
            catch (Exception ex)
            {
                var ex2 = new Exception($"ERRO: {ex.Message} - {evento.code} - {evento.createdAt.ToString("HH:mm:ss")} - {evento.correlationId}", ex);
                ex2.Data.Add("log", log);
                ex2.Data.Add("jsonPedido", jsonPedido);
                if (jsonPedido != ifoodAPI.LastResult)
                    ex2.Data.Add("LastResult", ifoodAPI.LastResult);

                AddLog(new ExceptionPDV(CodigoErro.EE13, ex2), true);
            }
        }

        private void EnviaConfirmacoes()
        {
            try
            {
                var pedidos = BLL.Pedido.ListarDelivery6Horas();
                foreach (var pedido in pedidos)
                {
                    string log = pedido.IDPedido.ToString();
                    try
                    {
                        if (pedido.GUIDIdentificacao?.StartsWith("ifood#") != true
                         || pedido.GUIDAgrupamentoPedido?.StartsWith("ifood#") != true
                         || pedido.GUIDAgrupamentoPedido.Contains("#Cancelado"))
                            continue;

                        string reference = pedido.GUIDIdentificacao.Split('#')[1].Trim();
                        string idshort = pedido.GUIDAgrupamentoPedido.Split('#')[1].Trim();
                        log = $"iFood {idshort} Pedido {pedido.IDPedido}";

                        if (pedido.IDStatusPedido == (int)EStatusPedido.NaoConfirmado)
                        {
                            log += " => Aguardando Confirmação";
                            AddLog(log);
                            continue;
                        }
                        else if (pedido.IDStatusPedido == (int)EStatusPedido.Cancelado)
                        {
                            string cMotivo = IntegracaoPedido.ObterMotivo(pedido.IDPedido);
                            ifoodAPI.PedidoRejeitado(reference, cMotivo);
                            pedido.GUIDAgrupamentoPedido = "#Cancelado " + pedido.GUIDAgrupamentoPedido;
                            log += " => Cancelado";
                        }
                        else if (pedido.IDStatusPedido == (int)EStatusPedido.Aberto
                             || (pedido.IDStatusPedido == (int)EStatusPedido.Enviado && !pedido.GUIDAgrupamentoPedido.Contains("#Confirmado"))
                             || (pedido.IDStatusPedido == (int)EStatusPedido.Finalizado && !pedido.GUIDAgrupamentoPedido.Contains("#Confirmado")))
                        {
                            if (pedido.GUIDAgrupamentoPedido.Contains("#Confirmado"))
                                continue;

                            ifoodAPI.PedidoConfirmado(reference);
                            pedido.GUIDAgrupamentoPedido += "#Confirmado";
                            log += " => Confirmado";
                        }
                        else if (pedido.IDStatusPedido == (int)EStatusPedido.Enviado
                             || (pedido.IDStatusPedido == (int)EStatusPedido.Finalizado && !pedido.GUIDAgrupamentoPedido.Contains("#Enviado")))
                        {
                            if (pedido.GUIDAgrupamentoPedido.Contains("#Enviado"))
                                continue;

                            ifoodAPI.PedidoEnviado(reference);
                            pedido.GUIDAgrupamentoPedido += "#Enviado";
                            log += " => Enviado";
                        }
                        else if (pedido.IDStatusPedido == (int)EStatusPedido.Finalizado)
                        {
                            if (pedido.GUIDAgrupamentoPedido.Contains("#Finalizado"))
                                continue;

                            ifoodAPI.PedidoEntregue(reference);
                            pedido.GUIDAgrupamentoPedido += "#Finalizado";
                            log += " => Entregue";
                        }
                        EF.Repositorio.Atualizar(pedido);
                        AddLog(log);
                        Logs.Info(CodigoInfo.I007, null, log);
                    }
                    catch (Exception ex)
                    {
                        log += " ERRO: " + ex.Message;
                        ex.Data.Add("pedido", pedido.IDPedido);
                        Logs.Erro(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog(new ExceptionPDV(CodigoErro.EE12, ex), true);
            }
        }

        private void CancelaPedido(Evento evento)
        {
            string log = "?";
            string jsonPedido = "?";
            try
            {
                var pedidoIFood = ifoodAPI.Pedido(evento.correlationId);
                jsonPedido = ifoodAPI.LastResult;

                var pedidoPDV = IntegracaoPedido.CarregarPorIdentificacao("ifood#" + pedidoIFood.reference);
                if (pedidoPDV == null || pedidoPDV?.StatusPedido == null)
                {
                    AddLog($"Pedido iFood #{pedidoIFood.shortReference} não existe no sistema");
                    return;
                }
                else if (pedidoPDV.StatusPedido.StatusPedido == EStatusPedido.Cancelado
                 || pedidoPDV.StatusPedido.StatusPedido == EStatusPedido.EmCancelamento)
                {
                    AddLog($"Pedido {pedidoPDV.IDPedido} {pedidoPDV.StatusPedido.StatusPedido.ToString().ToUpper()} iFood #{pedidoIFood.shortReference} CANCELADO");
                    return;
                }
                else if (pedidoPDV.StatusPedido.StatusPedido != EStatusPedido.NaoConfirmado)
                {
                    AddLog($"Pedido {pedidoPDV.IDPedido} Cancelado diretamente por ainda estar aguardando confirmação! iFood #{pedidoIFood.shortReference} CANCELADO");
                    IntegracaoPedido.Cancelar(pedidoPDV, ifoodUsuario.IDUsuario.Value);
                }
                else if (pedidoPDV.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                {
                    // NÃO FAZ NADA!
                    // Em teoria o IFOOD não deve permitir um pedido em status de ENTREGA ou FINALIZADO ser cancelado
                    AddLog($"Pedido {pedidoPDV.IDPedido} {pedidoPDV.StatusPedido.StatusPedido.ToString().ToUpper()} iFood #{pedidoIFood.shortReference} CANCELADO => SITUAÇÃO INVÁLIDA");
                }
                else
                {
                    // Se o Pedido ainda não foi "entregue ou finalziado" ele pode ser cancelado
                    AddLog($"Pedido {pedidoPDV.IDPedido} iFood #{pedidoIFood.shortReference} foi CANCELADO pelo cliente, aguardando cancelamento no caixa");
                    IntegracaoPedido.AlterarStatus(pedidoPDV.IDPedido.Value, EStatusPedido.EmCancelamento);
                }
            }
            catch (Exception ex)
            {
                var ex2 = new Exception($"ERRO: {ex.Message} - {evento.code} - {evento.createdAt.ToString("HH:mm:ss")} - {evento.correlationId}", ex);
                ex2.Data.Add("log", log);
                ex2.Data.Add("jsonPedido", jsonPedido);
                if (jsonPedido != ifoodAPI.LastResult)
                    ex2.Data.Add("LastResult", ifoodAPI.LastResult);

                AddLog(new ExceptionPDV(CodigoErro.EE13, ex2), true);
            }
        }
    }
}
