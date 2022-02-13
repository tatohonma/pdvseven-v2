SELECT
   u.Nome
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade) as money) as 'Vendido'
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*0.1) as money) as 'Serviço'  
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)) as money) as 'Vendido com Desconto'
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)*0.1) as money) as 'Serviço com Desconto'
FROM 
  tbPedido p
  LEFT JOIN tbPedidoProduto pp ON pp.IDPedido=p.IDPedido AND pp.Cancelado=0 AND pp.IDProduto<>4
  LEFT JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
WHERE 
    p.DtPedidoFechamento BETWEEN @dtInicio AND @dtFim
    AND
  IDStatusPedido=40
  AND
  ValorTotal>0
  AND
  ValorServico>0
GROUP BY  
  u.IDUsuario, u.Nome
ORDER BY
  u.Nome