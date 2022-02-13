SELECT
   p.idPedido as 'Cód. Pedido'
  ,cl.NomeCompleto as 'Cliente'
  ,p1.Nome as 'PDV lançamento'
  ,u1.Nome as 'Usuário lançamento'
  ,pp.DtInclusao as 'Data lançamento'
  ,p2.Nome as 'PDV cancelamento'
  ,u2.Nome as 'Usuário cancelamento'
  ,pp.DtCancelamento 'Data cancelamento'
  ,pr.Nome as 'Produto'
  ,pp.Quantidade as 'Qtd.'
  ,pp.ValorUnitario as 'Valor unitário (R$)'
    ,(pp.ValorUnitario*pp.Quantidade) as 'Valor total (R$)'
  ,mc.Nome as 'Motivo Cancelamento'
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
  LEFT JOIN tbMotivoCancelamento mc ON mc.IDMotivoCancelamento=pp.IDMotivoCancelamento
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1