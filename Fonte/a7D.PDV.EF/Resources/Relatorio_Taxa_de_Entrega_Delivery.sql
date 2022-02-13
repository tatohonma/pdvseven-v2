SELECT

  c.Nome as 'Nome Entregador'
  ,pp.Valor as 'Valor da Taxa'
  ,COUNT (p.ValorEntrega) as 'Quantidade'
  ,SUM (p.ValorEntrega) as 'Valor Total'
  
FROM
  tbpedido p
  INNER JOIN tbTaxaEntrega pp ON p.IDTaxaEntrega=pp.IDTaxaEntrega
  INNER JOIN tbEntregador c ON p.IDEntregador=c.IDEntregador

WHERE
  p.IDStatusPedido=40
  AND
  p.IDTipoPedido=30
  AND
  p.DtPedido BETWEEN @dtInicio AND @dtFim
GROUP BY
  c.Nome
  ,pp.Valor
  ,p.ValorEntrega
  ,ValorEntrega