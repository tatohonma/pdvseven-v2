SELECT
     p1.Nome as 'PDV Cancelamento'
  ,u1.Nome as 'Usuário Cancelamento'
  ,SUM(pp.Quantidade) as 'Qtd.'
  ,SUM(pp.ValorUnitario*pp.Quantidade) as 'Valor total (R$)'
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1
GROUP BY
     p1.Nome
  ,u1.Nome