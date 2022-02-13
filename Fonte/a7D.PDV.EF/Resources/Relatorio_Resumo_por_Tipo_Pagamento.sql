SELECT tp.Nome 'Tipo pagamento' ,
               COUNT(DISTINCT(pp.IDTipoPagamento)) AS 'Qtd.' ,
               SUM(pp.Valor) 'Valor total (R$)'
FROM tbCaixa c
LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE idFechamento=@idFechamento
  AND pp.idTipoPagamento IS NOT NULL
  AND p.idStatusPedido = 40
  AND pp.Excluido=0
GROUP BY tp.Nome