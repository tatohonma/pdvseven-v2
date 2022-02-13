select
    isnull(tbCliente.NomeCompleto, 'Sem Cliente') as 'Cliente'
  , tbProduto.Codigo as 'Codigo Produto'
  , tbProduto.Nome as 'Produto'
  , sum(tbPedidoProduto.Quantidade) as Quantidade
  , tbPedidoProduto.ValorUnitario
  , cast(sum(tbPedidoproduto.Quantidade * tbPedidoProduto.ValorUnitario) as decimal(10, 2)) as 'Valor'
  , cast(tbPedido.DtPedidoFechamento as date) as 'Data Fechamento Pedido'
from tbPedidoProduto (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbPedidoProduto.IDProduto
                and tbProduto.IDTipoProduto in (10, 20)
inner join tbPedido (nolock) on tbPedido.IDPedido = tbPedidoProduto.IDPedido
                and tbPedido.IDStatusPedido in (40)
left join tbCliente (nolock) on tbCliente.IDCliente = tbPedido.IDCliente
where
  tbPedidoProduto.Cancelado = 0
  and (tbProduto.IDTipoProduto = 10 or tbProduto.IDTipoProduto = 20 and tbPedidoProduto.ValorUnitario > 0)
group by
    isnull(tbCliente.NomeCompleto, 'Sem Cliente')
  , tbProduto.Codigo
  , tbProduto.Nome
  , tbPedidoProduto.ValorUnitario
  , cast(tbPedido.DtPedidoFechamento as date)
order by
  cast(tbPedido.DtPedidoFechamento as date)
  , isnull(tbCliente.NomeCompleto, 'Sem Cliente')