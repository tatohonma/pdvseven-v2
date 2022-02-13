using a7D.PDV.BigData.Shared.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.Server.BigData
{
    public static class DTOextension
    {
        private static DateTime ArredondaSegundo(this DateTime dt)
        {
            if (dt.Millisecond == 0)
                return dt;
            else
                return dt
                    .AddMilliseconds(-dt.Millisecond)
                    .AddSeconds(1);
        }

        public static bdProduto ToBigData(this EF.Models.tbProduto prod)
        => new bdProduto()
        {
            IDProduto = prod.IDProduto,
            Nome = prod.Nome,
            EAN = prod.cEAN,
            Valor = prod.ValorUnitario,
            Custo = prod.ValorUltimaCompra ?? 0,
            Ativo = prod.Ativo && !prod.Excluido,
            ControlarEstoque = prod.ControlarEstoque,
            EstoqueIdeal = prod.EstoqueIdeal ?? 0,
            EstoqueMinimo = prod.EstoqueMinimo ?? 0,
            dtAlteracao = prod.DtUltimaAlteracao.ArredondaSegundo(),
            // O Estoque atual sobe tudo separadamente apos sincronizar os produtos
        };

        public static bdCliente ToBigData(this EF.Models.tbCliente cli)
        => new bdCliente()
        {
            IDCliente = cli.IDCliente,
            NomeCompleto = cli.NomeCompleto,
            DataNascimento = cli.DataNascimento,
            Sexo = cli.Sexo,
            dtAlteracao = cli.DtInclusao.ArredondaSegundo()
        };

        public static bdTipoPagamento ToBigData(this EF.Models.tbTipoPagamento tp)
        => new bdTipoPagamento()
        {
            IDTipoPagamento = tp.IDTipoPagamento,
            Nome = tp.Nome,
            dtAlteracao = DateTime.Now.ArredondaSegundo()
        };

        public static bdUsuario ToBigData(this EF.Models.tbUsuario usr)
        => new bdUsuario()
        {
            IDUsuario = usr.IDUsuario,
            Nome = usr.Nome,
            Senha = usr.Senha,
            Ativo = usr.Ativo && !usr.Excluido,
            dtAlteracao = usr.DtUltimaAlteracao.ArredondaSegundo()
        };

        public static bdPedido ToBigData(this EF.Models.tbPedido ped)
        {
            try
            {
                var pedido = new bdPedido()
                {
                    IDPedido = ped.IDPedido,
                    IDCliente = ped.IDCliente,
                    DtPedidoFechamento = ped.DtPedidoFechamento.Value.ArredondaSegundo(),
                    ValorTotal = ped.ValorTotal ?? 0,
                    ValorDesconto = ped.ValorDesconto ?? 0,
                    ValorEntrega = ped.ValorEntrega ?? 0
                };

                pedido.Pagamentos = ped.tbPedidoPagamentoes.Select(pag => new bdPedidoPagamento()
                {
                    IDPedidoPagamento = pag.IDPedidoPagamento,
                    IDPedido = ped.IDPedido,
                    IDTipoPagamento = pag.IDTipoPagamento,
                    Valor = pag.Valor
                }).ToArray();

                pedido.Produtos = ped.tbPedidoProdutoes.Select(pp => new bdPedidoProduto()
                {
                    IDPedidoProduto = pp.IDPedidoProduto,
                    IDPedido = ped.IDPedido,
                    IDProduto = pp.IDProduto,
                    IDPedidoProduto_pai = pp.IDPedidoProduto_pai,
                    ValorUnitario = pp.ValorUnitario,
                    Quantidade = pp.Quantidade,
                    DtInclusao = pp.DtInclusao.Value
                }).ToArray();

                return pedido;
            }
            catch (Exception ex)
            {
                throw new Exception("ERRO Pedido " + ped.IDPedido, ex);
            }
        }
    }
}
