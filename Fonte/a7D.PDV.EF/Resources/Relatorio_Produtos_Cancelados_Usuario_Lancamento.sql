SELECT
     p1.Nome as 'PDV lançamento'
  ,u1.Nome as 'Usuário lançamento'
  ,SUM(pp.Quantidade) as 'Qtd.'
  ,SUM(pp.ValorUnitario*pp.Quantidade) as 'Valor total (R$)'
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1
GROUP BY
     p1.Nome
  ,u1.Nome