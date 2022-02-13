using System;
using System.Linq;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.ERPSige.Model;
using a7D.PDV.Model;

namespace a7D.PDV.Integracao.ERPSige
{
    internal static class DTO
    {
        public static string PedidoOrigemVenda = "Venda Direta";
        public static string PedidoSemCliente = "CONSUMIDOR NÃO IDENTIFICADO";
        public static string PedidoEmpresa = "Restaurante";
        public static string PedidoPlanoDeConta = "VENDA DE MERCADORIAS";
        public static string PedidoDeposito = "PADRÃO";
        public static string PedidoBanco = "Seu Banco";
        public static string PedidoFormaPagamento = "Caixa";

        internal static void ProdutoFill(ProdutoInformation produtoPDV, ref Produto produtoERP)
        {
            if (produtoERP == null)
            {
                produtoERP = new Produto()
                {
                    Codigo = produtoPDV.IDProduto.ToString(),
                    IgnorarEstoque = true,
                    EstoqueSaldo = produtoPDV.Quantidade ?? 0
                };
            }

            produtoERP.Nome = produtoPDV.Nome;
            produtoERP.PrecoVenda = produtoPDV.ValorUnitario.Value;
            produtoERP.EstoqueUnidade = produtoPDV.Unidade?.Nome ?? "UN";
            produtoERP.Genero = ProdutoGenero(produtoPDV.TipoProduto.IDTipoProduto.Value);
            produtoERP.Ativo = produtoPDV.Ativo.Value;
        }

        private static string ProdutoGenero(int tipoProduto)
        {
            if (tipoProduto == (int)ETipoProduto.Servico)
                return "09 – Serviços"; //Serviço
            else if (tipoProduto == (int)ETipoProduto.Modificacao)
                return "05 – Subproduto";
            else if (tipoProduto == (int)ETipoProduto.Ingrediente)
                return "05 – Subproduto"; // Sub Produto
            else
                return "04 – Produto Acabado"; // Produto Acabado
        }

        internal static Pessoa Pessoa(ClienteInformation clientePDV)
        {
            return new Pessoa()
            {
                NomeFantasia = clientePDV.NomeERP,
                CNPJ_CPF = clientePDV.Documento1,
                Telefone = clientePDV.Telefone1,
                Logradouro = clientePDV.Endereco,
                LogradouroNumero = clientePDV.EnderecoNumero,
                Bairro = clientePDV.Bairro,
                Cidade = clientePDV.Cidade,
                CEP = clientePDV.CEP.ToString() ?? "",
            };
        }

        internal static Pedido Pedido(PedidoInformation pedidoPDV)
        {
            var pedido = new Pedido()
            {
                Codigo = pedidoPDV.IDPedido.Value,
                //StatusSistema = "Pedido",
                //StatusSistema = "Pedido Faturado",
                Categoria = "Venda PDV7",
                Status = "Finalizado",
                Data = pedidoPDV.DtPedido.Value,
                DataEnvio = pedidoPDV.DtEnvio ?? pedidoPDV.DtPedido ?? DateTime.MinValue,
                DataFaturamento = pedidoPDV.DtPedidoFechamento.Value,
                //OrigemVenda = "PDV Offline",
                OrigemVenda = PedidoOrigemVenda,
                ValorFrete = pedidoPDV.ValorEntrega ?? 0,
                //OutrasDespesas = pedidoPDV.ValorServico ?? 0 - pedidoPDV.ValorDesconto ?? 0,
                ValorFinal = pedidoPDV.ValorTotal.Value,
                Cliente = pedidoPDV.Cliente?.NomeERP ?? PedidoSemCliente,
                Empresa = PedidoEmpresa,
                //PlanoDeConta = "10.01",
                PlanoDeConta = PedidoPlanoDeConta,
                Deposito = PedidoDeposito,
                //Banco = PedidoBanco,
                FormaPagamento = PedidoFormaPagamento,
                //Lancado = false,
                //Finalizado = false,
                //Enviado = false,
                //LancarComissaoVendedor = false,

                // Cliente
                Logradouro = pedidoPDV.Cliente?.Endereco,
                LogradouroNumero = pedidoPDV.Cliente?.EnderecoNumero,
                Bairro = pedidoPDV.Cliente?.Bairro,
                Municipio = pedidoPDV.Cliente?.Cidade,
                CEP = pedidoPDV.Cliente?.CEP?.ToString()
            };

            if (pedidoPDV.TipoPedido.TipoPedido == ETipoPedido.Mesa)
                pedido.Descricao = "Mesa " + BLL.Mesa.CarregarPorGUID(pedidoPDV.GUIDIdentificacao)?.Numero;
            else if (pedidoPDV.TipoPedido.TipoPedido == ETipoPedido.Comanda)
                pedido.Descricao = "Comanda " + BLL.Comanda.CarregarPorGUID(pedidoPDV.GUIDIdentificacao)?.Numero;
            else
                pedido.Descricao = pedidoPDV.TipoPedido.TipoPedido.ToString();

            pedido.Descricao += $"\r\n{pedidoPDV.Observacoes}";

            //NumeroNFe = (pedidoPDV.RetornoSAT_venda??.IDRetornoSAT ?? 0) ? "": BLL.RetornoSAT.Carregar(pedidoPDV.RetornoSAT_venda.IDRetornoSAT.Value),

            pedido.Items = pedidoPDV.ListaProduto
                //.Where(p => p.Produto.IDProduto > 4)
                .Select(pi => new PedidoItem()
                {
                    Codigo = pi.Produto.IDProduto.ToString(),
                    Unidade = pi.Produto.Unidade?.Nome ?? "UN",
                    Descricao = pi.Produto.Nome,
                    Quantidade = pi.Quantidade.Value,
                    ValorUnitario = pi.ValorUnitario.Value,
                    ValorTotal = pi.Quantidade.Value * pi.ValorUnitario.Value,
                    //Composicoes = new ProdutoComposicao[] {
                    //    new ProdutoComposicao()
                    //    {
                    //        Produto="Limão",
                    //        Deposito="PADRÃO",
                    //        Consumo=0.25m,
                    //        ConsumoTotal=0.25m,
                    //    },
                    //    new ProdutoComposicao()
                    //    {
                    //        Produto="Vodka",
                    //        Deposito="PADRÃO",
                    //        Consumo=0.003m,
                    //        ConsumoTotal=0.003m,
                    //    }
                    //}
                }).ToArray();

            pedido.Pagamentos = pedidoPDV.ListaPagamento.Select(pg => new PedidoPagamento()
            {
                DataTransacao = pg.DataPagamento ?? pedido.DataFaturamento,
                FormaPagamento = pg.TipoPagamento?.Nome,
                Banco="Rede",
                CredenciadoraCartao = pg.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMeioPagamento.Dinheiro ? null : pg.ContaRecebivel?.Nome,
                DescricaoPagamento = pg.TipoPagamento?.Gateway > 0 ? pg.TipoPagamento?.Gateway.ToString() : null,
                BandeiraCartao = Bandeira(pg.Bandeira?.IDBandeira ?? 0),
                NumeroTerminal = pedidoPDV.Caixa?.PDV?.IDPDV.ToString(),
                CV_NSU = pg.Autorizacao ?? pedido.Codigo.ToString(),
                //Parcelas = 1,
                //PeriodoParcelas = 10,
                ValorPagamento = pg.Valor.Value,
                CondicaoPagamento = pg.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMeioPagamento.Dinheiro ? 0 : 1, // a Vista //pg.MeioPagamentoSAT.Codigo
                Quitar = true

            }).ToArray();

            return pedido;
        }

        private static string Bandeira(int bandeira)
        {
            /* A bandeira do cartão dentre as opções Visa = 01, Mastercard = 02, American Express = 03, Sorocred = 04, Outros = 99 */
            switch ((EBandeira)bandeira)
            {
                case EBandeira.Visa:
                    return "01";
                case EBandeira.MasterCard:
                    return "02";
                case EBandeira.Amex:
                    return "03";
                case EBandeira.Sorocred:
                    return "04";
                default:
                    return "99";
            }
        }
    }
}