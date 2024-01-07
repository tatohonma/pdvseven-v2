using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.DAL;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace a7D.PDV.BLL
{
    public class Pedido
    {
        public static Decimal ValorTotalProdutos(Int32 idPedido)
        {
            List<PedidoProdutoInformation> listaPedidoProduto = PedidoProduto.ListarPorPedido(idPedido);
            return PedidoProdutoInformation.SomaValorTotal(listaPedidoProduto);
        }

        public static bool TotalPago(Int32 idPedido)
        {
            //TODO: 
            return true;
        }

        public static Decimal ValorPendente(Int32 idPedido)
        {
            return 0;
        }

        public static PedidoInformation PreparaConta(int id, int pessoas)
        {
            var pedido = BLL.Pedido.CarregarCompleto(id);
            if (pedido == null)
                return null;

            if (pessoas > 0 && pedido.NumeroPessoas != pessoas)
            {
                pedido.NumeroPessoas = pessoas;
                Salvar(pedido);
            }

            AdicionarProdutoServico(pedido, true, new PDVInformation() { IDPDV = 1 }, new UsuarioInformation() { IDUsuario = 1 });
            AdicionarProdutoConsumacaoMinima(pedido, new PDVInformation() { IDPDV = 1 }, new UsuarioInformation() { IDUsuario = 1 });

            return pedido;
        }
        public static decimal? IncluirAcrescimo(List<PedidoProdutoInformation> listaPedidoProdutos)
        {
            if (!ConfiguracoesSistema.Valores.ServicoComoItem)
            {
                var produtoServico = listaPedidoProdutos.FirstOrDefault(pp => pp.Produto.IDProduto == ProdutoInformation.IDProdutoServico);
                decimal? acrescimo = 0m;

                if (produtoServico != null)
                    acrescimo = produtoServico.ValorUnitario;

                return acrescimo;
            }
            else
            {
                return 0m;
            }


        }

        public static PedidoInformation NovoPedidoMesa(String guidIdentificacao)
        {
            PedidoInformation pedido = new PedidoInformation();
            pedido.GUIDMovimentacao = Guid.NewGuid().ToString();
            pedido.TipoPedido = new TipoPedidoInformation { IDTipoPedido = (int)ETipoPedido.Mesa };
            pedido.StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Aberto };
            pedido.GUIDIdentificacao = guidIdentificacao;
            pedido.DtPedido = DateTime.Now;
            pedido.TaxaServicoPadrao = TipoPedido.RetornarTaxaServico(ETipoPedido.Mesa);

            Pedido.Salvar(pedido);
            Mesa.AlterarStatus(guidIdentificacao, EStatusMesa.EmAtendimento);

            return pedido;
        }

        public static PedidoInformation NovoPedidoComanda(String guidIdentificacao, ClienteInformation cliente, TipoEntradaInformation tipoEntrada)
        {
            if (tipoEntrada == null)
                tipoEntrada = TipoEntrada.BuscarPadrao();
            if (tipoEntrada == null)
                throw new ExceptionPDV(CodigoErro.EF78);

            PedidoInformation pedido = new PedidoInformation
            {
                GUIDMovimentacao = Guid.NewGuid().ToString(),
                Cliente = cliente,
                TipoPedido = new TipoPedidoInformation { IDTipoPedido = (int)ETipoPedido.Comanda },
                StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Aberto },
                GUIDIdentificacao = guidIdentificacao,
                TipoEntrada = new TipoEntradaInformation { IDTipoEntrada = tipoEntrada.IDTipoEntrada },
                DtPedido = DateTime.Now,
                TaxaServicoPadrao = TipoPedido.RetornarTaxaServico(ETipoPedido.Comanda),
                ValorConsumacaoMinima = tipoEntrada.ValorConsumacaoMinima
            };

            if (pedido.ValorConsumacaoMinima > 0)
                pedido.TaxaServicoPadrao = 0;

            // Apenas na abertura de pedidos!
            // Itens são aberto como venda "balcão", mas muda o tipo para comanda
            //if (ConfiguracoesSistema.Valores.ComandaComCredito)
            //{
            //    pedido.GUIDAgrupamentoPedido = Guid.NewGuid().ToString();
            //    pedido.TaxaServicoPadrao = 0;
            //}

            Pedido.Salvar(pedido);
            Comanda.AlterarStatus(pedido.GUIDIdentificacao, EStatusComanda.EmUso);

            return pedido;
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosPorCliente(ClienteInformation cliente)
        {
            if (cliente?.IDCliente > 0)
            {
                return PedidoDAL.ListarFinalizadosPorCliente(cliente.IDCliente.Value);
            }
            else
            {
                return Enumerable.Empty<PedidoInformation>();
            }
        }

        public static PedidoInformation NovoPedidoBalcao(ClienteInformation cliente = null)
        {
            var pedido = new PedidoInformation
            {
                Cliente = cliente,
                GUIDMovimentacao = Guid.NewGuid().ToString(),
                //GUIDAgrupamentoPedido = pedidoOriginal?.GUIDAgrupamentoPedido,
                TipoPedido = new TipoPedidoInformation { IDTipoPedido = (int)ETipoPedido.Balcao },
                StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Aberto },
                DtPedido = DateTime.Now,
                TaxaServicoPadrao = TipoPedido.RetornarTaxaServico(ETipoPedido.Balcao),
                ListaProduto = new List<PedidoProdutoInformation>()
            };
            Pedido.Salvar(pedido);

            return pedido;
        }


        public static string ObterMotivo(int iDPedido)
        {
            string cMotivo = "Produto indiponível";
            var item = EF.Repositorio.Carregar<EF.Models.tbPedidoProduto>(pp => pp.IDPedido == iDPedido);
            if (item != null)
            {
                var motivo = EF.Repositorio.Carregar<EF.Models.tbMotivoCancelamento>(m => m.IDMotivoCancelamento == item.IDMotivoCancelamento);
                if (motivo != null)
                    cMotivo = motivo.Nome;
            }
            return cMotivo;
        }

        public static Exception Compare(string hashPedido, List<object> i1, PedidoInformation pedidoAtual)
        {
            var hashPedidoAtual = Pedido.GetHash(pedidoAtual, out List<object> i2);
            if (hashPedido == hashPedidoAtual)
                return null;

            var dif = new StringBuilder();
            dif.AppendLine($"{hashPedido} / {hashPedidoAtual}");
            dif.AppendLine($"{i1.Count} / {i2.Count}");
            for (int i = 0; i < i1.Count && i < i2.Count; i++)
            {
                if (i1[i]?.ToString() != i2[i]?.ToString())
                    dif.AppendLine($"{i}: {i1[i]} <> {i2[i]}");
            }

            var ex = new Exception("HASH Diferentes");
            ex.Data.Add("diferencas", dif.ToString());
            return ex;
        }

        public static PedidoInformation NovoPedidoDelivery(CaixaInformation caixa)
        {
            var pedido = new PedidoInformation
            {
                GUIDMovimentacao = Guid.NewGuid().ToString(),
                TipoPedido = new TipoPedidoInformation { IDTipoPedido = (int)ETipoPedido.Delivery },
                StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Aberto },
                DtPedido = DateTime.Now,
                TaxaServicoPadrao = TipoPedido.RetornarTaxaServico(ETipoPedido.Delivery),
                GUIDIdentificacao = Guid.NewGuid().ToString(),
                ListaPagamento = new List<PedidoPagamentoInformation>(),
                ListaProduto = new List<PedidoProdutoInformation>(),
                ValorDesconto = 0,
                ValorServico = 0,
                Caixa = caixa
            };
            // Não é salvo, pois será ainda peenchido os itens para salvar tudo de uma vez
            return pedido;
        }

        public static PedidoInformation AdicionarProduto(ETipoPedido tipoPedido, String guidIdentificacao, Int32 idUsuario, Int32 idPDV, List<PedidoProdutoInformation> listaPedidoProduto, String referenciaLocalizacao)
        {
            PedidoInformation pedido = AdicionarProduto(tipoPedido, guidIdentificacao, idUsuario, idPDV, listaPedidoProduto);

            if (!string.IsNullOrWhiteSpace(referenciaLocalizacao))
            {
                pedido = Carregar(pedido.IDPedido.Value);
                pedido.ReferenciaLocalizacao = referenciaLocalizacao;
                Salvar(pedido);
            }

            return pedido;
        }

        public static PedidoInformation AdicionarProduto(ETipoPedido tipoPedido, String guidIdentificacao, Int32 idUsuario, Int32 idPDV, List<PedidoProdutoInformation> listaNovosProdutos, bool validarLimite = true, ClienteInformation cliente = null)
        {
            PedidoInformation pedido = null;
            if (guidIdentificacao != null)
            {
                pedido = PedidoDAL.CarregarUltimoPedido(guidIdentificacao);

                if (tipoPedido == ETipoPedido.Comanda)
                {
                    decimal taxaServico = 0m;
                    Decimal valorTotalPedido = 0;
                    Decimal valorNovosItens = PedidoProdutoInformation.SomaValorTotal(listaNovosProdutos);

                    if (pedido?.IDPedido.Value > 0)
                        valorTotalPedido = Pedido.ValorTotalProdutos(pedido.IDPedido.Value);

                    // TODO: Metodo 'PedidoDAL.CarregarUltimoPedido' não traz a taxa, por algum motivo  a ser descoberto e revalidado no futuro
                    decimal taxaComanda = TipoPedido.RetornarTaxaServico(ETipoPedido.Comanda);
                    if (taxaComanda > 0)
                        taxaServico = (valorNovosItens + valorTotalPedido) * taxaComanda / 100m;

                    if (ConfiguracoesSistema.Valores.ComandaComCredito)
                    {
                        if (pedido.Cliente == null)
                            throw new ExceptionPDV(CodigoErro.AA10);

                        if (validarLimite)
                        {
                            decimal saldo = Saldo.ClienteSaldoLiquido(pedido.Cliente.IDCliente.Value, pedido?.IDPedido);

                            if ((valorNovosItens + valorTotalPedido + taxaServico) > saldo)
                                throw new ExceptionPDV(CodigoErro.AA20, "Saldo: " + saldo.ToString("C"));
                        }
                    }
                    else if (validarLimite)
                    {
                        // pedido.Cliente?.Limite?.Value ?? 
                        int limiteComanda = ConfiguracoesSistema.Valores.LimiteComanda;
                        if (valorTotalPedido == 0 && listaNovosProdutos.Count == 1) // Primeiro item apenas
#pragma warning disable CS0642 // Possible mistaken empty statement
                            ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                        else if ((valorTotalPedido + valorNovosItens + taxaServico) > limiteComanda)
                            throw new ExceptionPDV(CodigoErro.AA30, limiteComanda.ToString("C"));
                    }
                }
            }

            if (pedido != null && pedido.IDPedido != null && pedido.StatusPedido.StatusPedido == EStatusPedido.Enviado)
                throw new ExceptionPDV(CodigoErro.EC80);

            else if (pedido?.IDPedido == null)
            {
                switch (tipoPedido)
                {
                    case ETipoPedido.Mesa:
                        pedido = NovoPedidoMesa(guidIdentificacao);
                        break;
                    case ETipoPedido.Comanda:
                        pedido = NovoPedidoComanda(guidIdentificacao, null, null);
                        break;
                    case ETipoPedido.Balcao:
                        pedido = NovoPedidoBalcao(cliente);
                        break;
                    default:
                        throw new ExceptionPDV(CodigoErro.EC40, "tipoPedido: " + (int)tipoPedido);
                }
            }

            foreach (var item in listaNovosProdutos)
            {
                item.Produto = Produto.Carregar(item.Produto.IDProduto.Value);
                item.Pedido = pedido;
                item.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                item.PDV = new PDVInformation { IDPDV = idPDV };

                var quantidadeInteira = item.Quantidade % 1 == 0;

                if (quantidadeInteira)
                {
                    for (int i = 0; i < item.Quantidade; i++)
                    {
                        var pedidoProduto_new = (PedidoProdutoInformation)item.Clone();
                        pedidoProduto_new.IDPedidoProduto = null;
                        pedidoProduto_new.Pedido = pedido;
                        pedidoProduto_new.CodigoAliquota = item.CodigoAliquota;
                        pedidoProduto_new.Quantidade = 1;
                        PedidoProduto.Salvar(pedidoProduto_new);
                        SalvarModificacoes(idUsuario, idPDV, pedido, item, pedidoProduto_new);
                    }
                }
                else
                {
                    var pedidoProduto_new = (PedidoProdutoInformation)item.Clone();
                    pedidoProduto_new.IDPedidoProduto = null;
                    pedidoProduto_new.Pedido = pedido;
                    pedidoProduto_new.CodigoAliquota = item.CodigoAliquota;
                    pedidoProduto_new.Quantidade = item.Quantidade;
                    PedidoProduto.Salvar(pedidoProduto_new);
                    SalvarModificacoes(idUsuario, idPDV, pedido, item, pedidoProduto_new);
                }
            }

            return pedido;
        }

        public static bool TentaFecharPorTransferencia(PedidoInformation Pedido1, int idCaixa)
        {
            // Obtem a lista de produtos atuais após a transferencia
            Pedido1.ListaProduto = PedidoProduto.ListarPorPedido(Pedido1.IDPedido.Value);

            var servico = Pedido1.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoServico);
            if (servico != null)
            {
                servico.Pedido = Pedido1;
                PedidoProduto.Salvar(servico);
            }

            var entradaCM = Pedido1.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM);
            if (entradaCM != null)
            {
                entradaCM.Pedido = Pedido1;
                PedidoProduto.Salvar(entradaCM);
            }

            Pedido1.Caixa = new CaixaInformation { IDCaixa = idCaixa };
            Pedido1.DtPedidoFechamento = DateTime.Now;
            Pedido1.SincERP = false;
            Pedido1.ValorTotal = Pedido1.ValorTotalProdutosServicos - Pedido1.ValorDesconto;

            if (Pedido1.ValorTotal == 0)
                Pedido1.StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Finalizado };

            Pedido.Salvar(Pedido1);

            return Pedido1.ValorTotal == 0;
        }


        public static void FecharVendaDB(PedidoInformation pedido, CaixaInformation caixa, int idUsuario) //, bool manterAberto)
        {
            try
            {
                bool novoDebito = false;
                foreach (var item in pedido.ListaPagamento) //.Where(l => l.TipoPagamento.IDTipoPagamento != 1))
                {
                    item.Pedido = pedido;
                    bool novo = item.IDPedidoPagamento == null;
                    PedidoPagamento.Salvar(item, idUsuario);

                    if (novo && item.IDGateway == (int)EGateway.ContaCliente)
                    {
                        novoDebito = true;

                        // Neste caso sempre poderá salvar pois foi gerado quando há saldo
                        item.IDSaldoBaixa = Saldo.RegistrarDebitoComoPagamento(item);

                        // Antes de chegar aqui passou na validação se o pedido não foi alterado!
                        PedidoPagamento.Salvar(item, idUsuario);
                    }
                }

                var servico = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoServico);
                if (servico != null)
                {
                    servico.Pedido = pedido;
                    PedidoProduto.Salvar(servico);
                }

                var entradaCM = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM);
                if (entradaCM != null)
                {
                    entradaCM.Pedido = pedido;
                    PedidoProduto.Salvar(entradaCM);
                }

                pedido.Caixa = caixa;
                pedido.DtPedidoFechamento = DateTime.Now;
                pedido.SincERP = false;
                pedido.ValorTotal = pedido.ValorTotalProdutosServicos - pedido.ValorDesconto +
                    (pedido.ValorEntrega != null ? pedido.ValorEntrega.Value : 0);

                pedido.StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Finalizado };

                bool novoCredito = Saldo.AdicionarCreditoSeExistir(pedido);
                Pedido.Salvar(pedido);

                GA.PostTransacao(pedido.Caixa.PDV, pedido.IDPedido.Value, pedido.ValorTotal ?? 0);

                if (novoCredito || novoDebito)
                {
                    // Se há novos Creditos ou Débitos precisa liquidar para poder dar baixa
                    Saldo.LiquidarDebitos(pedido.Cliente.IDCliente.Value);
                }
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(ex);

                //pedido.StatusPedido = new StatusPedidoInformation { StatusPedido = EStatusPedido.Cancelado };
                Pedido.Salvar(pedido);
            }
        }

        private static void SalvarModificacoes(int idUsuario, int idPDV, PedidoInformation pedido, PedidoProdutoInformation item, PedidoProdutoInformation pedidoProduto_new)
        {
            if (pedidoProduto_new.ListaModificacao != null && pedidoProduto_new.ListaModificacao.Count > 0)
            {
                // Nova lista para ter a ordem e os novos pedidos
                var modificacoes = pedidoProduto_new.ListaModificacao.OrderByDescending(m => m.PedidoProdutoPai.IDPedidoProduto).ToList();
                foreach (var modificacao in modificacoes)
                {
                    if (modificacao.PedidoProdutoPai.IDPedidoProduto_Original != null) // Se não é o produto raiz, busca na lista do que já foi criado!
                        modificacao.PedidoProdutoPai = modificacoes.First(m => m.IDPedidoProduto_Original == modificacao.PedidoProdutoPai.IDPedidoProduto_Original);
                    else
                        modificacao.PedidoProdutoPai = pedidoProduto_new;

                    modificacao.Produto = Produto.Carregar(modificacao.Produto.IDProduto.Value);
                    modificacao.CodigoAliquota = item.CodigoAliquota;
                    modificacao.Pedido = pedido;

                    modificacao.Quantidade = modificacao.Quantidade ?? 1;
                    modificacao.IDPedidoProduto = null;
                    modificacao.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                    modificacao.PDV = new PDVInformation { IDPDV = idPDV };

                    PedidoProduto.Salvar(modificacao);
                }
            }
        }

        private static void SalvarModificacao(PedidoProdutoInformation modificacao, int idUsuario, int idPDV, PedidoInformation pedido, PedidoProdutoInformation item, PedidoProdutoInformation pedidoProduto_new, decimal? valor = null, decimal quantidade = 1)
        {
            modificacao.Produto = Produto.Carregar(modificacao.Produto.IDProduto.Value);
            modificacao.CodigoAliquota = item.CodigoAliquota;
            modificacao.Pedido = pedido;
            modificacao.PedidoProdutoPai = pedidoProduto_new;
            modificacao.Quantidade = quantidade;
            modificacao.IDPedidoProduto = null;
            modificacao.Pedido = pedido;
            modificacao.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
            modificacao.PDV = new PDVInformation { IDPDV = idPDV };
            if (valor.HasValue)
                modificacao.ValorUnitario = valor;

            PedidoProduto.Salvar(modificacao);
        }

        public static PedidoInformation Carregar(int idPedido)
        {
            PedidoInformation obj = new PedidoInformation { IDPedido = idPedido };
            obj = (PedidoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static decimal ValorTotalOuCredito(PedidoInformation pedido, out bool retornouCredito)
        {
            var valorTotal = pedido.ValorTotalTemp;
            if (pedido.TipoPedido?.IDTipoPedido == (int)ETipoPedido.Comanda
             && pedido.Cliente?.IDCliente > 0
             && ConfiguracoesSistema.Valores.ComandaComCredito)
            {
                retornouCredito = true;
                decimal saldo = Saldo.ClienteSaldoLiquido(pedido.Cliente.IDCliente.Value, pedido?.IDPedido);

                return saldo - valorTotal;
            }
            else
            {
                retornouCredito = false;
                return valorTotal;
            }
        }
        public static PedidoInformation CarregarUltimoPedido(string guidIdentificacao)
        {
            Int32? idPedido = null;
            PedidoInformation pedido = null;

            if (!string.IsNullOrWhiteSpace(guidIdentificacao))
                idPedido = PedidoDAL.RetornarIDPedido(guidIdentificacao);

            if (idPedido != null)
            {
                pedido = CarregarCompleto(idPedido.Value);
            }
            else
            {
                pedido = new PedidoInformation();
                pedido.GUIDMovimentacao = Guid.NewGuid().ToString();
                pedido.AplicarDesconto = false;
                pedido.ListaProduto = new List<PedidoProdutoInformation>();
                pedido.ListaPagamento = new List<PedidoPagamentoInformation>();
            }

            return pedido;
        }
        public static PedidoInformation CarregarCompleto(Int32 idPedido)
        {
            PedidoInformation pedido = PedidoDAL.CarregarCompleto(idPedido);

            if (pedido == null)
                return null;

            pedido.ListaProduto = PedidoProduto.ListarPorPedido(idPedido);
            pedido.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(idPedido);

            if (pedido.Cliente != null)
                pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);
            if (pedido.TipoEntrada != null)
                pedido.TipoEntrada = TipoEntrada.Carregar(pedido.TipoEntrada.IDTipoEntrada.Value);
            if (pedido.Entregador != null)
                pedido.Entregador = Entregador.Carregar(pedido.Entregador.IDEntregador.Value);
            if (pedido.Caixa != null)
                pedido.Caixa = Caixa.Carregar(pedido.Caixa.IDCaixa.Value);
            if (pedido.TaxaEntrega?.IDTaxaEntrega != null)
                pedido.TaxaEntrega = TaxaEntrega.Carregar(pedido.TaxaEntrega.IDTaxaEntrega.Value);
            if (pedido.UsuarioDesconto?.IDUsuario != null)
                pedido.UsuarioDesconto = Usuario.Carregar(pedido.UsuarioDesconto.IDUsuario.Value);
            if (pedido.UsuarioTaxaServico?.IDUsuario != null)
                pedido.UsuarioTaxaServico = Usuario.Carregar(pedido.UsuarioTaxaServico.IDUsuario.Value);
            if (pedido.ValorDesconto == null)
                pedido.ValorDesconto = 0;

            return pedido;
        }

        public static void Salvar(PedidoInformation obj)
        {
            if (obj.Entregador != null && obj.Entregador.IDEntregador.Value == 0)
                obj.Entregador = null;

            if (obj.DtEntrega != null && obj.DtEntrega.Value.Year < 1980)
                obj.DtEntrega = new DateTime(1980, 1, 1);

            if (obj.IDPedido == null)
            {
                Pedido.Adicionar(obj);
            }
            else
            {
                Pedido.Alterar(obj);
            }
        }
        public static void Adicionar(PedidoInformation obj)
        {
            CRUD.Salvar(obj);
        }
        public static void Alterar(PedidoInformation obj)
        {
            CRUD.Alterar(obj);
        }
        public static void AlterarSincERP(Int32 idPedido, Boolean sincERP)
        {
            PedidoInformation pedido = Carregar(idPedido);

            pedido.SincERP = sincERP;

            Salvar(pedido);
        }
        public static void SalvarRetornoSATVenda(Int32 idPedido, Int32 idRetornoSAT)
        {
            PedidoInformation pedido = Carregar(idPedido);

            pedido.RetornoSAT_venda = new RetornoSATInformation { IDRetornoSAT = idRetornoSAT };
            Salvar(pedido);
        }
        public static void SalvarRetornoSATCancelamento(Int32 idPedido, Int32 idRetornoSAT)
        {
            PedidoInformation pedido = Carregar(idPedido);

            pedido.RetornoSAT_cancelamento = new RetornoSATInformation { IDRetornoSAT = idRetornoSAT };
            Salvar(pedido);
        }

        public static List<PedidoInformation> ListarPendentes()
        {
            List<Object> listaObj = CRUD.Listar(new PedidoInformation());
            List<PedidoInformation> lista = listaObj.ConvertAll(new Converter<Object, PedidoInformation>(PedidoInformation.ConverterObjeto));

            var lista1 = lista.Where(l => l.DtPedidoFechamento == null);

            return lista1.ToList();
        }

        public static void CancelarBalcaoPendentes(int idpdv, int idusuario, ref int idCaixa)
        {
            using (var pdv = new pdv7Context())
            {
                var pedidos = pdv.tbPedidos.Where(p => p.IDStatusPedido == (int)EStatusPedido.Aberto && p.IDTipoPedido == (int)ETipoPedido.Balcao).ToList();
                if (pedidos.Count == 0)
                    return;

                Caixa.UsaOuAbreRefID(idpdv, idusuario, ref idCaixa);

                var log = new StringBuilder();
                log.AppendLine("Pedidos cancelados em venda balcão não finalizada");
                foreach (var pedido in pedidos)
                {
                    log.AppendLine($"\t{pedido.IDPedido}: {pedido.DtPedido.Value.ToString("dd/MM/yyyy HH:mm")}");
                    pedido.IDCaixa = idCaixa;
                    pedido.IDStatusPedido = (int)EStatusPedido.Cancelado;
                }
                pdv.SaveChanges();
                Logs.Info(CodigoInfo.I002, log.ToString());
            }
        }

        public static void FechaCancelados(int idpdv, int idusuario, ref int idCaixa)
        {
            using (var pdv = new pdv7Context())
            {
                var pedidos = pdv.tbPedidos.Where(p => p.IDCaixa == null && p.IDStatusPedido == (int)EStatusPedido.Cancelado).ToList();
                if (pedidos.Count == 0)
                    return;

                Caixa.UsaOuAbreRefID(idpdv, idusuario, ref idCaixa);

                var log = new StringBuilder();
                log.AppendLine("Pedidos cancelados não fechados");
                foreach (var pedido in pedidos)
                {
                    log.AppendLine($"\t{pedido.IDPedido}: {pedido.DtPedido.Value.ToString("dd/MM/yyyy HH:mm")}");
                    pedido.IDCaixa = idCaixa;

                    if (pedido.DtPedidoFechamento == null)
                        pedido.DtPedidoFechamento = DateTime.Now;
                }
                pdv.SaveChanges();
                Logs.Info(CodigoInfo.I004, log.ToString());
            }
        }

        /// <summary>
        /// Conta o numero de pedidos abertor no delivery
        /// </summary>
        /// <remarks>
        /// Teste: T13
        /// </remarks>
        public static int QtdDeliveryAberto()
        {
            using (var pdv = new pdv7Context())
            {

                var qtd = pdv.tbPedidos.Count(
                    p => p.IDTipoPedido == (int)ETipoPedido.Delivery
                     && (p.IDStatusPedido != (int)EStatusPedido.Cancelado && p.IDStatusPedido != (int)EStatusPedido.Finalizado));
                return qtd;
            }
        }

        public static IEnumerable<PedidoInformation> ListarNaoSincERP(DateTime dt)
        {
            return PedidoDAL.ListarPedidosSemSyncERP(dt);
        }

        public static List<tbPedido> ListarDelivery6Horas()
        {
            var dtLimite = DateTime.Now.AddHours(-6);
            var lista = Repositorio.Listar<tbPedido>(p =>
                p.IDTipoPedido == (int)ETipoPedido.Delivery &&
                p.DtPedido > dtLimite).ToList();

            return lista;
        }
        public static List<PedidoInformation> ListarPorCaixa(Int32 idCaixa)
        {
            PedidoInformation pedidoFiltro = new PedidoInformation();
            pedidoFiltro.Caixa = new CaixaInformation { IDCaixa = idCaixa };

            List<Object> listaObj = CRUD.Listar(pedidoFiltro);
            List<PedidoInformation> lista = listaObj.ConvertAll(new Converter<Object, PedidoInformation>(PedidoInformation.ConverterObjeto));

            return lista.ToList();
        }

        public static List<PedidoInformation> ListarFinalizadosUltimaHora()
        {
            return PedidoDAL.ListarFinalizadosUltimaHora();
        }

        public static IEnumerable<PedidoInformation> ListarPedidoSemEstoque()
        {
            return PedidoDAL.ListarPedidoSemEstoque();
        }

        public static PedidosCFe[] ListarFinalizados(DateTime de, DateTime ate)
        {
            var sql = @"SELECT 
	p.IDPedido, p.DtPedidoFechamento, p.DocumentoCliente, p.ValorTotal,
	rs.IDRetornoSAT, rs.IDTipoSolicitacaoSAT, rs.ChaveConsulta
FROM tbPedido p (nolock) 
LEFT JOIN tbRetornoSAT rs ON p.IDRetornoSAT_venda = rs.IDRetornoSAT
WHERE p.IDStatusPedido = 40
AND (p.DtPedidoFechamento BETWEEN @dataMin AND @dataMax)
ORDER BY p.DtPedidoFechamento";

            return Repositorio.Query<PedidosCFe>(sql,
                new SqlParameter("dataMin", de),
                new SqlParameter("dataMax", ate));
        }

        public static List<PedidoInformation> ListarFinalizadosNoIntervalo(DateTime de, DateTime ate, int idCliente, int skip, int limit, CancellationToken ct)
        {
            var lista = PedidoDAL.ListarFinalizadosNoIntervalo(de, ate, idCliente, skip, limit).ToList();
            foreach (var pedido in lista)
            {
                if (ct.IsCancellationRequested)
                    break;

                if (pedido.Cliente?.IDCliente != null)
                    pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                if (pedido.Caixa?.IDCaixa != null)
                    pedido.Caixa = Caixa.Carregar(pedido.Caixa.IDCaixa.Value);

                pedido.ListaProduto = PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                pedido.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(pedido.IDPedido.Value);
            }
            return lista;
        }

        public static int CountFinalizadosNoIntervalo(DateTime dtFechamentoMin, DateTime dtFechamentoMax, int idCliente)
        {
            var lista = PedidoDAL.ListarFinalizadosAPartirDe(dtFechamentoMin).Where(p => p.DtPedidoFechamento <= dtFechamentoMax);
            if (idCliente != 0)
                lista = lista.Where(p => p.Cliente?.IDCliente == idCliente);

            return lista.Count();
        }

        public static DataTable ListarDeliveryPendentes()
        {
            return PedidoDAL.ListarDeliveryPendentes();
        }

        public static void AlterarStatusSinc(Int32 idPedido, Boolean statusSinc)
        {
            PedidoInformation pedido = Carregar(idPedido);
            pedido.SincERP = statusSinc;

            Salvar(pedido);
        }

        public static PedidoInformation CarregarPorIdentificacao(String Identificacao)
        {
            PedidoInformation obj = new PedidoInformation { GUIDIdentificacao = Identificacao };
            obj = (PedidoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static PedidoInformation CarregarPorIdRetornoSatVenda(Int32 idRetornoSat)
        {
            var obj = new PedidoInformation { RetornoSAT_venda = new RetornoSATInformation { IDRetornoSAT = idRetornoSat } };
            obj = (PedidoInformation)CRUD.Carregar(obj);

            if (obj.IDPedido.HasValue)
                return obj;

            return null;
        }

        public static void CancelarProduto(Int32 idPDV, Int32 idUsuario, Int32 idPedidoProduto, Int32 idMotivoCancelamento, String observacoesCancelamento, Boolean retornarAoEstoque)
        {
            PedidoProdutoInformation pedidoProduto = PedidoProduto.Carregar(idPedidoProduto);
            pedidoProduto.RetornarAoEstoque = retornarAoEstoque;
            pedidoProduto.Cancelado = true;
            pedidoProduto.PDVCancelamento = new PDVInformation { IDPDV = idPDV };
            pedidoProduto.UsuarioCancelamento = new UsuarioInformation { IDUsuario = idUsuario };
            if (idMotivoCancelamento > 0)
                pedidoProduto.MotivoCancelamento = new MotivoCancelamentoInformation { IDMotivoCancelamento = idMotivoCancelamento };
            pedidoProduto.ObservacoesCancelamento = idMotivoCancelamento > 0 ? observacoesCancelamento : "Cancelamento SAT";
            pedidoProduto.DtCancelamento = DateTime.Now;

            PedidoProduto.Salvar(pedidoProduto);
            Logs.Info(CodigoInfo.I008, pedidoProduto.ToString() + "\r\n" + observacoesCancelamento, null, idPDV, idUsuario);

            foreach (var item in pedidoProduto.ListaModificacao)
            {
                item.Cancelado = true;
                item.PDVCancelamento = new PDVInformation { IDPDV = idPDV };
                item.UsuarioCancelamento = new UsuarioInformation { IDUsuario = idUsuario };
                item.DtCancelamento = DateTime.Now;

                PedidoProduto.Salvar(item);
            }
        }

        public static void AlterarStatus(Int32 idPedido, EStatusPedido idStatusPedido)
        {
            PedidoInformation pedido = Pedido.Carregar(idPedido);
            pedido.StatusPedido.StatusPedido = idStatusPedido;

            Pedido.Salvar(pedido);
        }

        public static void Cancelar(PedidoInformation pedido, int usuarioCancelamento)
        {
            pedido.StatusPedido.StatusPedido = EStatusPedido.Cancelado;
            Pedido.Salvar(pedido);

            PedidoPagamento.CancelarPorPedido(pedido.IDPedido.Value, usuarioCancelamento);

            PedidoProduto.CancelarPorPedido(pedido.IDPedido.Value, usuarioCancelamento);
        }

        public static void AdicionarProdutoServico(PedidoInformation pedido, Boolean incluirServico, PDVInformation pdv, UsuarioInformation usuario)
        {
            PedidoProdutoInformation servico;

            if (incluirServico == true && pedido.TaxaServicoPadrao > 0)
            {
                pedido.ValorServico = pedido.ValorServicoTemp;
                servico = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoServico);
                if (servico == null)
                {
                    var produto = Produto.Carregar(ProdutoInformation.IDProdutoServico);
                    servico = new PedidoProdutoInformation
                    {
                        Produto = produto,
                        Quantidade = 1,
                        Cancelado = false,
                        ValorUnitario = pedido.ValorServico,
                        PDV = pdv,
                        Usuario = usuario,
                        Pedido = pedido,
                        CodigoAliquota = produto.CodigoAliquota
                    };
                    pedido.ListaProduto.Insert(0, servico);
                }
                else
                {
                    servico.ValorUnitario = pedido.ValorServico;
                }
            }
            else
            {
                pedido.ValorServico = 0;
                servico = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == 4);
                if (servico != null)
                    pedido.ListaProduto.Remove(servico);
            }
        }

        public static void AdicionarProdutoTaxaEntrega(PedidoInformation pedido, bool incluirTaxaEntrega, PDVInformation pdv, UsuarioInformation usuario)
        {
            var taxaEntrega = pedido.ListaProduto.FirstOrDefault(p => p.Produto.IDProduto == 164170145);

            if (incluirTaxaEntrega && pedido.ValorEntrega > 0)
            {
                if (taxaEntrega == null)
                {
                    taxaEntrega = new PedidoProdutoInformation
                    {
                        Produto = Produto.Carregar(164170145),
                        Quantidade = 1,
                        Cancelado = false,
                        ValorUnitario = pedido.ValorEntrega,
                        PDV = pdv,
                        Usuario = usuario
                    };
                    pedido.ListaProduto.Insert(0, taxaEntrega);
                }
                else
                {
                    taxaEntrega.ValorUnitario = pedido.ValorEntrega;
                }
            }
            else
            {
                pedido.ValorEntrega = 0;
                if (taxaEntrega != null)
                    pedido.ListaProduto.Remove(taxaEntrega);
            }
        }

        public static void AdicionarProdutoConsumacaoMinima(PedidoInformation pedido, PDVInformation pdv, UsuarioInformation usuario, decimal? valorConsumacaoMinima = null)
        {
            if (pedido.TipoPedido.TipoPedido != ETipoPedido.Comanda)
                return;

            PedidoProdutoInformation entradaCM;
            decimal diferencaConsumacaoMinima = pedido.DiferencaConsumacaoMinima;

            if ((valorConsumacaoMinima.HasValue && valorConsumacaoMinima.Value > 0) || diferencaConsumacaoMinima > 0)
            {
                entradaCM = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM);
                if (entradaCM == null)
                {
                    var produto = Produto.Carregar(ProdutoInformation.IDProdutoEntracaCM);
                    entradaCM = new PedidoProdutoInformation
                    {
                        Produto = produto,
                        Quantidade = 1,
                        Cancelado = false,
                        ValorUnitario = valorConsumacaoMinima.HasValue ? valorConsumacaoMinima.Value : diferencaConsumacaoMinima,
                        PDV = pdv,
                        Usuario = usuario,
                        Pedido = pedido,
                        CodigoAliquota = produto.CodigoAliquota
                    };
                    pedido.ListaProduto.Insert(0, entradaCM);
                }
                else
                {
                    entradaCM.ValorUnitario = valorConsumacaoMinima.HasValue ? valorConsumacaoMinima.Value : diferencaConsumacaoMinima;
                }
            }
            else
            {
                entradaCM = pedido.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM);
                if (entradaCM != null)
                    pedido.ListaProduto.Remove(entradaCM);
            }
        }

        public static void AlterarTipoEntrada(Int32 idPedido, Int32 idTipoEntrada, Int32 idPDV, Int32 idUsuario)
        {
            PedidoProdutoInformation entrada = PedidoProduto.ListarPorPedido(idPedido).FirstOrDefault(l => l.Produto.IDProduto == 2 && l.Cancelado == false);
            if (entrada != null)
            {
                entrada.Cancelado = true;
                entrada.UsuarioCancelamento = new UsuarioInformation { IDUsuario = idUsuario };
                entrada.PDVCancelamento = new PDVInformation { IDPDV = idPDV };
                entrada.DtCancelamento = DateTime.Now;
                PedidoProduto.Salvar(entrada);
            }

            #region Nova entrada
            TipoEntradaInformation tipoEntradaNova = TipoEntrada.Carregar(idTipoEntrada);

            PedidoProdutoInformation entradaNova = new PedidoProdutoInformation();
            entradaNova.Pedido = new PedidoInformation { IDPedido = idPedido };
            entradaNova.Produto = Produto.Carregar(ProdutoInformation.IDProdutoEntrada);
            entradaNova.Quantidade = 1;
            entradaNova.CodigoAliquota = entradaNova.Produto.CodigoAliquota;
            entradaNova.Cancelado = false;
            entradaNova.ValorUnitario = tipoEntradaNova.ValorEntrada;
            entradaNova.PDV = new PDVInformation { IDPDV = idPDV };
            entradaNova.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
            entradaNova.DtInclusao = DateTime.Now;
            entradaNova.RetornarAoEstoque = false;
            entradaNova.IDTipoEntrada = tipoEntradaNova.IDTipoEntrada;
            entradaNova.Notas = tipoEntradaNova.Nome;

            PedidoProduto.Adicionar(entradaNova);
            #endregion

            #region Pedido
            PedidoInformation pedido = Pedido.Carregar(idPedido);
            pedido.TipoEntrada = new TipoEntradaInformation { IDTipoEntrada = idTipoEntrada };
            pedido.ValorConsumacaoMinima = tipoEntradaNova.ValorConsumacaoMinima;
            Pedido.Salvar(pedido);
            #endregion
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosPorDataAberturaAPartirDe(DateTime de)
        {
            return PedidoDAL.ListarFinalizadosPorDataAberturaAPartirDe(de);
        }

        public static List<PedidoInformation> ListarEnviados()
        {
            return PedidoDAL.ListarEnviados();
        }

        public static PedidoInformation ObterPedidoAbertoPorMesa(int numeroMesa)
        {
            return PedidoDAL.ObterPedidoAbertoPorMesa(numeroMesa);
        }

        public static PedidoInformation ObterPedidoAbertoPorComanda(int numeroComanda)
        {
            return PedidoDAL.ObterPedidoAbertoPorComanda(numeroComanda);
        }

        public static string GetHash(PedidoInformation pedido, out List<object> itens)
        {
            itens = new List<object>
            {
                pedido.IDPedido,
                pedido.StatusPedido?.IDStatusPedido,
                pedido.DtPedido,
                pedido.DtPedidoFechamento,
                pedido.GUIDIdentificacao
            };

            if (pedido.ListaPagamento == null)
                itens.Add(1111);
            else
                itens.AddRange(pedido.ListaPagamento.Where(pp => pp.IDPedidoPagamento > 0).Select(pp => (object)("p" + pp.GetHashCode().ToString())));

            if (pedido.ListaProduto == null)
                itens.Add(2222);
            else
            {
                var prods = pedido.ListaProduto.Where(p => p.IDPedidoProduto > 0);
                foreach (var prod in prods)
                    itens.Add("i" + prod.GetHashCode());
            }

            return UtilSha256Hash.GenerateSHA256String(itens);
        }

        public static bool ContemAlcoolico(Int32 idPedido)
        {
            string categoriasAlcoolicas = ConfiguracoesSistema.Valores.CategoriasAlcoolicas;

            if (String.IsNullOrEmpty(categoriasAlcoolicas))
                return false;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                idPedido 
                FROM 
	                tbPedidoProduto pp
	                INNER JOIN tbProdutoCategoriaProduto pcp ON pcp.IDProduto=pp.IDProduto
                WHERE
	                pp.IDPedido=@idPedido AND
	                PCP.IDCategoriaProduto IN (" + categoriasAlcoolicas + @")
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idPedido", idPedido);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
