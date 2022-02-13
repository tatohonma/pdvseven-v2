using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.ERPSige.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.ERPSige
{
    internal static class DTO2
    {
        internal static Pedido PedidoHoje(DateTime dt)
        {
            var pedido = new Pedido()
            {
                Codigo = int.Parse(dt.ToString("yyyyMMdd")),
                Categoria = "Venda PDV7",
                Status = "Finalizado",
                Data = dt,
                DataEnvio = dt,
                DataFaturamento = dt,
                OrigemVenda = "Venda Direta",
                ValorFrete = 0,
                Cliente = "CONSUMIDOR NÃO IDENTIFICADO",
                Empresa = "Restaurante",
                PlanoDeConta = "VENDA DE MERCADORIAS",
                Deposito = "PADRÃO",
                Banco = "Seu Banco",
                FormaPagamento = "Caixa",
                Lancado = false,
                Finalizado = true,
                Enviado = true,
                LancarComissaoVendedor = false,
            };

            //pedido.ValorFinal = pedidoPDV.ValorTotal.Value,

            var itens = BLL.PedidoProduto.ListarPorDataPedido(dt);

            pedido.Items = itens
                .GroupBy(p => p.Produto.IDProduto)
                .Select(pi => new PedidoItem()
                {
                    Codigo = pi.Key.Value.ToString(),
                    Unidade = "UN",
                    Descricao = pi.First().Produto.Nome,
                    Quantidade = pi.Sum(i => i.Quantidade.Value),
                    ValorUnitario = pi.First().ValorUnitario.Value,
                    ValorTotal = pi.Sum(i => i.Quantidade.Value * i.ValorUnitario.Value),
                }).ToArray();

            pedido.ValorFinal = pedido.Items.Sum(i => i.ValorTotal);

            //pedido.Pagamentos = pedidoPDV.ListaPagamento.Select(pg => new PedidoPagamento()
            //{
            //    DataTransacao = pg.DataPagamento.Value,
            //    FormaPagamento = pg.TipoPagamento?.Nome,
            //    CredenciadoraCartao = pg.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMeioPagamento.Dinheiro ? null : pg.ContaRecebivel.Nome,
            //    CredenciadoraCNPJ = "05343346000106",
            //    DescricaoPagamento = pg.TipoPagamento?.Gateway > 0 ? pg.TipoPagamento?.Gateway.ToString() : null,
            //    BandeiraCartao = Bandeira(pg.Bandeira?.IDBandeira ?? 0),
            //    NumeroTerminal = pedidoPDV.Caixa?.PDV?.IDPDV.ToString(),
            //    CV_NSU = pg.Autorizacao ?? "123456",
            //    Parcelas = 1,
            //    PeriodoParcelas = 1,
            //    ValorPagamento = pg.Valor.Value,
            //    CondicaoPagamento = pg.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMeioPagamento.Dinheiro ? 0 : 1 // a Vista //pg.MeioPagamentoSAT.Codigo
            //}).ToArray();

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