using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using System.Linq;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela2_MenuPagamento()
        {
            string titulo;
            string info = "\r";

            if (Terminal.RequerDesconectar)
            {
                Terminal.RequerDesconectar = false;
                onTerminalEvent(Terminal, "Desconectando e enviando confirmações (Tela2)");
                termDisconnect(Terminal.terminalId, 0);
                return Tela2_MenuPagamento;
            }

            if (Terminal.tipo == (int)ETipoPedido.Mesa)
            {
                titulo = "Mesa " + Terminal.numero;
                termDisplayMessageCenter(Terminal.terminalId, "Carregando\r\r" + titulo, 10);
                var mesa = pedido.MesaStatus(Terminal.numero, true);
                if (mesa == null)
                {
                    termDisplayMessageCenter(Terminal.terminalId, (titulo + "\r\rnão existe"));
                    return Tela1_MenuPrincipal;
                }
                else if (mesa.status == (int)EStatusMesa.Liberada)
                {
                    termDisplayMessageCenter(Terminal.terminalId, (titulo + "\r\rlivre"));
                    return Tela1_MenuPrincipal;
                }
                else if (mesa.ValorTotal == 0)
                {
                    termDisplayMessageCenter(Terminal.terminalId, (titulo + "\r\rsem valor"));
                    return Tela1_MenuPrincipal;
                }
                // Só tem valor quando ha pedido
                info += "\rPedido " + mesa.idPedidoAberto.Value;
                Terminal.pedido = mesa.idPedidoAberto.Value;
                Terminal.valorTotal = (mesa.ValorTotal.Value);
                Terminal.valorPendente = Terminal.valorTotal - (mesa.ValorPago ?? 0);
                Terminal.valorServico = (mesa.ValorServico ?? 0);
                if (!string.IsNullOrEmpty(mesa.ClienteNome))
                {
                    Terminal.documentoFidelidade = mesa.ClienteDocumento;
                    Terminal.documentoFiscal = mesa.DocumentoFiscal;
                    info += "\r\r" + mesa.ClienteNome.Trim().Split(' ')[0] + "\r" + mesa.ClienteDocumento;
                }
            }
            else if (Terminal.tipo == (int)ETipoPedido.Comanda)
            {
                titulo = "Comanda " + Terminal.numero;
                termDisplayMessageCenter(Terminal.terminalId, "Carregando\r\r" + titulo, 10);
                var comanda = pedido.ComandaStatus(Terminal.numero, true);
                if (comanda == null)
                {
                    termDisplayMessageCenter(Terminal.terminalId, titulo + "\r\rnão existe");
                    return Tela1_MenuPrincipal;
                }
                else if (comanda.status == (int)EStatusComanda.Liberada)
                {
                    termDisplayMessageCenter(Terminal.terminalId, titulo + "\r\rlivre");
                    return Tela1_MenuPrincipal;
                }
                else if (comanda.ValorTotal == 0)
                {
                    termDisplayMessageCenter(Terminal.terminalId, titulo + "\r\rsem valor");
                    return Tela1_MenuPrincipal;
                }

                // Só tem valor quando ha pedido
                info += "\rPedido " + comanda.idPedidoAberto.Value;
                Terminal.pedido = comanda.idPedidoAberto.Value;
                Terminal.valorTotal = (comanda.ValorTotal.Value);
                Terminal.valorPendente = Terminal.valorTotal - (comanda.ValorPago ?? 0);
                Terminal.valorServico = (comanda.ValorServico ?? 0);
                if (!string.IsNullOrEmpty(comanda.ClienteNome))
                {
                    Terminal.documentoFidelidade = comanda.ClienteDocumento;
                    Terminal.documentoFiscal = comanda.DocumentoFiscal;
                    info += "\r\r" + comanda.ClienteNome + "\r" + comanda.ClienteDocumento;
                }
            }
            else
            {
                if (Terminal.tipo == (int)ETipoPedido.Delivery)
                    titulo = "Delivery " + Terminal.pedido;
                else
                    titulo = "Balcao " + Terminal.pedido;

                termDisplayMessageCenter(Terminal.terminalId, "Carregando\r\r" + titulo, 10);
                var ped = pedido.Pedido(Terminal.pedido);

                if (ped == null
                || !ped.ValorTotal.HasValue
                || ped.StatusPedido != (int)EStatusPedido.Aberto)
                {
                    if (ped == null)
                        termDisplayMessageCenter(Terminal.terminalId, titulo + " inválido");
                    else if (ped.StatusPedido == (int)EStatusPedido.Finalizado)
                        termDisplayMessageCenter(Terminal.terminalId, titulo + " finalizado");
                    else if (ped.StatusPedido == (int)EStatusPedido.Cancelado)
                        termDisplayMessageCenter(Terminal.terminalId, titulo + " cancelado");
                    if (ped.ValorTotal == null)
                        termDisplayMessageCenter(Terminal.terminalId, titulo + " sem valor");

                    return Tela1_MenuPrincipal;
                }

                Terminal.valorTotal = (ped.ValorTotal.Value);
                Terminal.valorPendente = Terminal.valorTotal - ped.Pagamentos.Sum(v => v.Valor.Value);
                Terminal.valorServico = (ped.ValorServico ?? 0);

                if (!string.IsNullOrEmpty(ped.Cliente?.NomeCompleto))
                {
                    Terminal.documentoFidelidade = ped.Cliente.Documento1;
                    Terminal.documentoFiscal = ped.DocumentoFiscal;
                    info += "\r\r" + ped.Cliente.NomeCompleto + "\r" + ped.Cliente.Documento1;
                }
            }

            if (Terminal.valorPendente != Terminal.valorTotal)
                info += "\r\r" + ("Pendente: R$ " + Terminal.valorPendente.ToString("N2"));

            info += "\r\r" + ("Total: R$ " + Terminal.valorTotal.ToString("N2"));

            termDisplayMessageCenter(Terminal.terminalId, titulo + info, 1000);

            string[] menu = {
                "Imprimir Conta",       // 0
                "Pagamento Parcial",    // 1
                "Pagamento Total",      // 2
                //"Usar Pontos Fidelidade",// 3
                //"Pagar R$ " + Terminal.valorPendente.ToString("N2"),
                "Voltar"
            };

            if (Terminal.valorTotal > 100 && Terminal.tipo == (int)ETipoPedido.Comanda)
                titulo = "Cmd " + Terminal.numero;

            onTerminalEvent(Terminal, titulo);

            Terminal.valorTEF = 0;
            if (Terminal.valorPendente == 0)
                return TelaA_FinalizaPedido;

            //titulo += " R$ " + Terminal.valorTotal.ToString("N2");
            if (Terminal.valorTotal == Terminal.valorPendente)
                menu[2] = "Total: " + Terminal.valorPendente.ToString("N2");
            else // if (Terminal.valorTotal != Terminal.valorPendente)
                menu[2] = "Pendente: " + Terminal.valorPendente.ToString("N2");

            Terminal.ret = 0;
            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, titulo, menu);

            switch (selectedMenu)
            {
                case (int)PTIRET.NOCONN:
                case (int)PTIRET.PROTOCOLERR:
                    onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                    return Tela2_MenuPagamento;

                case 0:
                    return Tela3_Imprimir;

                case 1:
                    return Tela4_PagamentoParcial;

                case 2:
                    Terminal.valorTEF = Terminal.valorPendente;
                    return Tela5_TEF;

                //case 3:
                //    return Tela6_FidelidadeCPF;

                default:
                    return Tela1_MenuPrincipal;
            }
        }
    }
}