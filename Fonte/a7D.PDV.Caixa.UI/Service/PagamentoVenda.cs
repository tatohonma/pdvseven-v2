using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.Pagamento;
using a7D.PDV.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public static class PagamentoVenda
    {
        public static void ConfiguraDinheiro(this PedidoPagamentoInformation pagamento)
        {
            try
            {
                pagamento.Bandeira = new tbBandeira() { IDBandeira = pagamento.TipoPagamento.Bandeira?.IDBandeira ?? (int)EBandeira.Desconhecida };
                pagamento.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = pagamento.TipoPagamento.ContaRecebivel?.IDContaRecebivel ?? (int)EContaRecebivel.Dinheiro };
                pagamento.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = pagamento.TipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT ?? (int)EMetodoPagamento.Dinheiro };
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EESA, ex);
            }
        }

        public static bool Efetiva(PedidoInformation pedido, PedidoPagamentoInformation pagamento, decimal ValorPendente)
        {
            if (frmPrincipal.ContaCliente // Se está com conta cliente tem que verificar compra isolada de creditos
             && pedido.ListaProduto.Any(p => p.Produto.Excluido != true && p.Produto.TipoProduto.IDTipoProduto == (int)ETipoProduto.Credito)
             && pedido.ListaProduto.Any(p => p.Produto.Excluido != true && p.Produto.TipoProduto.IDTipoProduto != (int)ETipoProduto.Credito))
            {
                // TODO: Por enquanto, depois tem que gerar dois pedidos desmembrando o credito
                // Isso pois fiscalmente porde gerar impostos desnecessários?
                Logs.ErroBox(CodigoErro.AE31);
                return false;
            }
            string viaCliente = null;
            string viaEstabelecimento = null;
            string mensagemOperador = "";

            try
            {
                pagamento.Bandeira = pagamento.TipoPagamento.Bandeira;
                pagamento.ContaRecebivel = pagamento.TipoPagamento.ContaRecebivel;
                pagamento.MeioPagamentoSAT = pagamento.TipoPagamento.MeioPagamentoSAT;

                if (pagamento.TipoPagamento.IDGateway > 0)
                {
                    pagamento.IDGateway = pagamento.TipoPagamento.IDGateway;
                    pagamento.Pedido = pedido;
                    if (ObterPagamentoPorGateway(pedido, pagamento, ValorPendente, out viaCliente, out viaEstabelecimento, out mensagemOperador))
                    {
                        // Todas as validações ocorrem antes, aqui é só gravar!
                        PedidoPagamento.Salvar(pagamento, AC.Usuario.IDUsuario.Value);
                        if (pagamento.IDGateway == (int)EGateway.ContaCliente)
                        {
                            int idSaldo = Saldo.RegistrarDebitoComoPagamento(pagamento);
                            if (pedido.ValorSaldoCliente >= 0)
                            {
                                // Pode emitir nota!
                                pagamento.IDSaldoBaixa = idSaldo;
                                PedidoPagamento.Salvar(pagamento, AC.Usuario.IDUsuario.Value);
                            }
                        }

                        return true;
                    }
                }
                else if (pagamento.TipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMetodoPagamento.Dinheiro)
                {
                    pagamento.ConfiguraDinheiro();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("IDPedido", pedido.IDPedido);
                ex.Data.Add("IDTipoPagamento", pagamento.TipoPagamento?.IDTipoPagamento);
                ex.Data.Add("IDGateway", pagamento.TipoPagamento?.IDGateway);
                Logs.ErroBox(CodigoErro.E900, ex, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(viaEstabelecimento))
                    ImpressoraWindows.ImprimirTexto(ConfiguracoesCaixa.Valores.ModeloImpressora, true, viaEstabelecimento);

                if (!String.IsNullOrWhiteSpace(viaCliente))
                    ImpressoraWindows.ImprimirTexto(ConfiguracoesCaixa.Valores.ModeloImpressora, true, viaCliente);

                if (!String.IsNullOrWhiteSpace(mensagemOperador))
                    MessageBox.Show(mensagemOperador);
            }
            return false;
        }

        private static bool ObterPagamentoPorGateway(PedidoInformation pedido, PedidoPagamentoInformation pagamento, decimal valorPendente, out string viaCliente, out string viaEstabelecimento, out string mensagemOperador)
        {
            switch (pagamento.TipoPagamento.Gateway)
            {
                case EGateway.NTKTEF:
                    return ObterPagamento(TipoTEF.NTKPayGo, pedido, pagamento, out viaCliente, out viaEstabelecimento, out mensagemOperador);
                case EGateway.GranitoTEF:
                    return ObterPagamento(TipoTEF.GRANITO, pedido, pagamento, out viaCliente, out viaEstabelecimento, out mensagemOperador);
                case EGateway.StoneTEF:
                    return ObterPagamento(TipoTEF.STONE, pedido, pagamento, out viaCliente, out viaEstabelecimento, out mensagemOperador);
                case EGateway.ContaCliente:
                    mensagemOperador = "";
                    return ObterPagamentoContaCliente(pedido, pagamento, valorPendente, out viaCliente, out viaEstabelecimento);
                //case EGateway.TodoCartoes:
                //    viaCliente = viaEstabelecimento = null;
                //    return ObterPagamentoTodoCartoes(pedido, pagamento);
                default:
                    mensagemOperador = "Metodo de pagamento não encontrado!";
                    viaCliente = viaEstabelecimento = null;
                    return false;
            }
        }

        private static bool ObterPagamento(TipoTEF tipo, PedidoInformation pedido, PedidoPagamentoInformation pagamento, out string viaCliente, out string viaEstabelecimento, out string mensagemOperador)
        {
            string celular = pagamento?.Pedido?.Cliente?.Telefone1DDD?.ToString();
            if (celular != null)
                celular += pagamento?.Pedido?.Cliente?.Telefone1Numero?.ToString();

            bool contemAlcoolicos = Pedido.ContemAlcoolico(pedido.IDPedido.Value);

            var tefPagamento = PinpadTEF.Pagar(tipo, pagamento.Valor.Value, pedido.IDPedido.Value, "Loja", AC.PDV.Nome, AC.PDV.IDPDV.Value, celular, 1, contemAlcoolicos);
            mensagemOperador = tefPagamento.MensagemOperador;

            if (tefPagamento.Confirmado)
            {

                int idMetodo = (int)EMetodoPagamento.Credito;
                if (tefPagamento.Debito)
                    idMetodo = (int)EMetodoPagamento.Debito;

                pagamento.Autorizacao = tefPagamento.LocRef + "-" + tefPagamento.Autorizacao;
                pagamento.Bandeira = new tbBandeira() { IDBandeira = Bandeira.CarregarPorNome(tefPagamento.Bandeira)?.IDBandeira ?? (int)EF.Enum.EBandeira.Desconhecida };
                pagamento.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = ContaRecebivel.CarregarPorNome(tefPagamento.ContaRecebivel.Replace("CARD", ""))?.IDContaRecebivel ?? (int)EF.Enum.EContaRecebivel.Dinheiro };
                pagamento.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = idMetodo };

                viaEstabelecimento = tefPagamento.ViaEstabelecimento;
                viaCliente = tefPagamento.ViaCliente;

                return true;
            }
            else
            {
                viaEstabelecimento = "";
                viaCliente = "";

                return false;
            }
        }

        private static bool ObterPagamentoContaCliente(PedidoInformation pedido, PedidoPagamentoInformation pagamento, decimal valorPendente, out string viaCliente, out string viaEstabelecimento)
        {
            viaCliente = viaEstabelecimento = null;

            // para credito usar não é necessário de cliente
            if (pedido.ListaProduto.Any(p => p.Produto.Excluido != true && p.Produto.TipoProduto.TipoProduto == ETipoProduto.Credito))
            {
                Logs.ErroBox(CodigoErro.AE33);
                return false;
            }
            else if (pedido.ListaPagamento.Any(p => p.IDGateway != (int)EGateway.ContaCliente) || valorPendente != 0)
            {
                Logs.ErroBox(CodigoErro.AE34);
                return false;
            }

            var frm = new frmContaCliente
            {
                Documento = pedido.Cliente?.Documento1 ?? pedido.DocumentoCliente,
                Valor = pagamento.Valor.Value,
                NaoValidarLimite = pedido.TipoPedido.TipoPedido == ETipoPedido.Comanda && ConfiguracoesSistema.Valores.ComandaComCredito
            };

            if (frm.ShowDialog() != DialogResult.OK)
                return false;

            pedido.Cliente = Cliente.Carregar(frm.IDCliente);
            pedido.ValorContaCliente = pagamento.Valor.Value;
            pedido.ValorSaldoCliente = frm.SaldoAtual - pagamento.Valor.Value;
            pagamento.Autorizacao = "Saldo: " + frm.SaldoAtual;

            viaCliente = "RECIBO DE MOVIMENTAÇÃO DE SALDO"
                + $"\r\nCliente {pedido.Cliente.IDCliente}: {pedido.Cliente.NomeCompleto}"
                + "\r\nSaldo Anterior: " + frm.SaldoAtual.ToString("C")
                + $"\r\nDébito: " + pagamento.Valor.Value.ToString("C")
                + "\r\nSaldo Final: " + pedido.ValorSaldoCliente.Value.ToString("C");

            if (pedido.ValorSaldoCliente < 0)
            {
                viaCliente += "\r\nO recibo fiscal será emitido após a quitação do débito";
            }

            return true;
        }

        private static bool ObterPagamentoTodoCartoes(PedidoInformation pedido, PedidoPagamentoInformation pagamento)
        {
            MessageBox.Show("Não implementado");
            return false;

            //var frm = new frmFidelidade("Loja", AC.PDV.Nome, pedido.IDPedido.Value, pagamento.Valor.Value);
            //if (frm.ShowDialog() != DialogResult.OK)
            //    return false;

            //pagamento.Autorizacao = frm.Pagamento.Autorizacao;
            //pagamento.Bandeira = new tbBandeira() { IDBandeira = Bandeira.CarregarPorNome(frm.Pagamento.Bandeira)?.IDBandeira ?? (int)EF.Enum.EBandeira.Desconhecida };
            //pagamento.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = ContaRecebivel.CarregarPorNome(frm.Pagamento.ContaRecebivel.Replace("CARD", ""))?.IDContaRecebivel ?? (int)EF.Enum.EContaRecebivel.Dinheiro };
            //pagamento.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = (int)EMetodoPagamento.Presente };

            //return true;
        }
    }
}
