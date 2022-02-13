SELECT 
   p.IDPedido as 'Cód. Pedido'
  ,u.Nome as 'Usuário fechamento'
  ,cl.NomeCompleto as 'Cliente'
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as 'Valor produto (R$)'
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as 'Valor entrada (R$)'
  ,ISNULL(p.ValorServico, 0) as 'Valor serviço (R$)'
  ,ISNULL(p.ValorDesconto, 0) as 'Valor desconto (R$)'
  ,(ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) as 'Valor total (R$)'
  ,p.Observacoes as 'Observações'
  ,a.Nome as 'Tipo Desconto'
  ,p.DtPedidoFechamento as 'Data'
FROM 
  tbPedido p 
  INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
  LEFT JOIN tbTipoDesconto a ON p.IDTipoDesconto=a.IDTipoDesconto
WHERE 
  ca.idFechamento=@idFechamento
  AND
  p.IDTipoPedido = 40
  AND
  p.ValorDesconto>0
  AND
  p.idtipodesconto is not NULL