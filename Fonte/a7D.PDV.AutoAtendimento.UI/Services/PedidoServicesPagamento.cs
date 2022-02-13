using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal partial class PedidoServices
    {
        internal bool Pagar()
        {
            try
            {
                App.Timeout = false; // PAGAMENTO (INICIO) !!! Desliga o controle de Timeout padrão

                string cPedidoID = DateTime.Now.ToString("HHmmss"); // MAXIMO 6 digitos por causa da PAGO!
                int nPedidoID = int.Parse(cPedidoID);

                pinpad.CriaTEF(nPedidoID, TotalAPagar);

                var modalWindow = new ModalTEFWindow(pinpad.TEF, Enviar, ConfirmarPagamento, FecharPedido);
                pinpad.Execute(modalWindow);

                if (modalWindow.DialogResult == true)
                {
                    Cancelar();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                return false;
            }
            finally
            {
                if (!ComandaCarregada && PedidoID > 0)
                {
                    var fechamento = new FechamentoPedido(PedidoID, PdvServices.PDVID, PdvServices.ChaveUsuario);
                    fechamento.Cancelar = true;
                    var result = apiPedido.Fechar(fechamento);
                    if (result.Mensagem != "OK")
                    {
                        var ex = new Exception($"Cancelamento do pedido {PedidoID} retornou: {result.Mensagem}");
                        ModalSimNaoWindow.Show(ex);
                    }
                }
                App.Timeout = true; // PAGAMENTO (FIM) !!! Religa o controle de Timeout padrão
            }
        }

        internal void Enviar()
        {
            if (TipoFluxo == EFluxo.ComandaPagamento)
                return;

            else if (TipoFluxo == EFluxo.CheckInEntrada)
            {
                var abrir = new ComandaAbrir()
                {
                    Comanda = ComandaNumero,
                    ClienteID = ClienteID,
                    PDVID = PdvServices.PDVID,
                    UsuarioID = PdvServices.UsuarioID
                };

                var resultado = apiPedido.ComandaAbrir(abrir);
                if (resultado?.Mensagem != "OK")
                    throw new Exception(resultado.Mensagem);

                PedidoComanda = apiPedido.ComandaItens(ComandaNumero, true);
                if (PedidoComanda == null || PedidoComanda.Itens == null)
                    throw new Exception("Pedido não foi criado corretamente");

                ComandaCarregada = true;
                PedidoID = PedidoComanda.IDPedido.Value;

                if (Itens.Count == 0)
                    return; // Sem creditos a adicionar
            }

            if (!string.IsNullOrEmpty(Comanda_LeitoraTAG) && Comanda_IDCliente == 0)
            {
                var novoCliente = InserirCliente(Comanda_ClienteDocumento, Comanda_ClienteNome, Comanda_ClienteTelefone);
                if (novoCliente.Id == 0)
                {
                    throw new Exception("ERRO: " + novoCliente.Mensagem);
                }
                ClienteID = Comanda_IDCliente = novoCliente.Id.Value;
            }

            if (!string.IsNullOrEmpty(Comanda_LeitoraTAG) && Comanda_IDCliente > 0)
            {
                ClienteID = Comanda_IDCliente;
                var cmdCliente = apiPedido.ComandaRegistraCliente(Comanda_LeitoraTAG, App.Pedido.Comanda_LeitoraTIPO, Comanda_IDCliente);
                if (cmdCliente.Id == 0)
                {
                    throw new Exception("ERRO: " + cmdCliente.Mensagem);
                }
            }

            // Por elimintório compra de produtos ou creditos
            var novoPedido = new AdicionarProdutos()
            {
                GUIDSolicitacao = Guid.NewGuid().ToString(),
                IDPDV = PdvServices.PDVID,
                IDTipoPedido = TipoPedido,
                IDUsuario = PdvServices.UsuarioID,
                GerarOrdemProducao = false // ATENÇÃO: A ordem de produção é fechado apos concluir o pagamento
            };

            if (TipoPedido == 20)
                novoPedido.Numero = ComandaNumero;
            else if (TipoPedido == 40 && ClienteID > 0)
                novoPedido.Numero = ClienteID;

            novoPedido.Itens = new List<Item>();
            foreach (var item in Itens)
            {
                novoPedido.Itens.Add(new Item()
                {
                    Qtd = item.Quantidade,
                    IDProduto = item.Produto.IDProduto,
                    Preco = item.ValorUnitario,
                    Modificacoes = item.Modificacoes
                });
            }

            var result = apiPedido.AdicionaPedido(novoPedido);
            if (result == null)
                throw new Exception("Sem resposta do servidor");

            else if (!(result.Id > 0))
                throw new Exception(result.Mensagem);

            if (TipoFluxo == EFluxo.CheckInEntrada
             || TipoFluxo == EFluxo.AdicionarCredito)
                TipoPedido = 40; // O Pedido vira pedido balcão

            PedidoID = result.Id.Value;
        }

        internal void ConfirmarPagamento()
        {
            var pagamentos = new List<Pagamento>();
            var autorizacao = pinpad.TEF.Autorizacao;
            var bandeira = pinpad.TEF.Bandeira;
            var contaRecebivel = pinpad.TEF.Adquirente;

            int idMetodo = 3; // padrão (MeiosPagamentos.Credito)
            if (pinpad.TEF.Debito)
                idMetodo = 4; // (int)MeiosPagamentos.Debito;

            var pagamento = new Pagamento()
            {
                TipoPagamento = new TipoPagamento(),
                Valor = pinpad.TEF.Valor,
                Autorizacao = autorizacao,
                ContaRecebivel = contaRecebivel,
                Bandeira = bandeira,
                IDMetodo = idMetodo,
            };

            if (PdvServices.MeioPagamento == "NTKDLL")
                pagamento.TipoPagamento.IDGateway = 2;
            else if (PdvServices.MeioPagamento == "GranitoTEF")
                pagamento.TipoPagamento.IDGateway = 23;
            else if (PdvServices.MeioPagamento == "NTKPayGo")
                pagamento.TipoPagamento.IDGateway = 1;
            else if (PdvServices.MeioPagamento == "STONE")
                pagamento.TipoPagamento.IDGateway = 9;
            else
                throw new Exception("Meio de pagamento não identificado");

            pagamentos.Add(pagamento);

            var numero = TipoPedido == 20 ? ComandaNumero : PedidoID;
            var item = (TipoPedido == 20 ? "comanda " : "pedido ") + numero;

            var data = new InsercaoPagamento(Guid.NewGuid().ToString(), TipoPedido, numero, PdvServices.UsuarioID, PdvServices.PDVID, pagamentos);
            var result = apiPedido.AdicionaPagamento(data);
            if (result.Mensagem != "OK")
                throw new Exception($"Pagamento {item} retornou:\n{result.Mensagem}");

            string conteudo = pinpad.TEF.ViaCliente;
            if (!string.IsNullOrEmpty(conteudo))
            {
                if (ComandaNumero > 0)
                    conteudo = "COMANDA: " + ComandaNumero + "\r\n" + conteudo;
                else
                    conteudo = item.ToUpper() + "\r\n" + conteudo;

                App.Impressora.ImprimirTexto(conteudo);
            }
        }

        internal void FecharPedido()
        {
            System.IO.Stream img;
            decimal totalItens = 0;
            var fechamento = new FechamentoPedido(PedidoID, PdvServices.PDVID, PdvServices.ChaveUsuario, PdvServices.OrdemImpressao, Comanda_ClienteDocumento, Comanda_ClienteDocumento);
            //if (TipoFluxo == EFluxo.CheckInEntrada || TipoFluxo == EFluxo.AdicionarCredito)
            //fechamento.ReabrirParaCredito = true;

            var result = apiPedido.Fechar(fechamento);
            if (result.Mensagem != "OK")
                throw new Exception($"Fechamento do pedido {PedidoID} retornou: {result.Mensagem}");

            if (PdvServices.Fiscal)
            {
                img = apiImpressao.ImageFiscal(PedidoID, ImpressoraServices.Width);
                App.Impressora.ImprimirImagem(img);
            }
            else
            {
                img = apiImpressao.ImagemConta(PedidoID, ImpressoraServices.Width);
                App.Impressora.ImprimirImagem(img);
            }

            if (PdvServices.ComandaComCredito)
                CarregaSaldoCreditos(ClienteID);

            if (TipoFluxo != EFluxo.Produto)
                return;

            for (int item = 0; item < Itens.Count; item++)
            {
                if (Itens[item].Produto.IDTipoProduto == 50)
                    continue;

                totalItens += Itens[item].Quantidade;
            }

            for (int item = 0; item < totalItens; item++)
            {
                img = apiImpressao.Ticket(PedidoID, item);
                if (img != null)
                    App.Impressora.ImprimirImagem(img);
            }
        }
    }
}
