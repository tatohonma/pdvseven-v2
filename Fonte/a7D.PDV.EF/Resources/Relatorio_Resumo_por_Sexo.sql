SELECT CASE cl.Sexo
           WHEN 'm' THEN 'Homem'
           WHEN 'f' THEN 'Mulher'
       END 'Tipo' ,COUNT(DISTINCT(p.idPedido)) AS 'Qtd.' ,
  (SELECT SUM(ValorUnitario*Quantidade)
   FROM tbPedidoProduto pp1
   INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
   INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
   INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
   WHERE pp1.Cancelado=0
     AND c1.idFechamento=@idFechamento
     AND cl1.Sexo=cl.Sexo
     AND p1.idstatuspedido = 40
     AND pp1.idProduto NOT IN (2,
                               3)) 'Valor produto (R$)' ,
  (SELECT SUM(ValorUnitario*Quantidade)
   FROM tbPedidoProduto pp1
   INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
   INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
   INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
   WHERE pp1.Cancelado=0
     AND c1.idFechamento=@idFechamento
     AND cl1.Sexo=cl.Sexo
     AND p1.idstatuspedido = 40
     AND pp1.idProduto IN (2,
                           3)) 'Valor entrada (R$)' ,
                               SUM(p.ValorServico) 'Valor serviço (R$)' ,
                                                   SUM(p.ValorDesconto) 'Valor desconto (R$)' ,(
                                                                                                  (SELECT SUM(ValorUnitario*Quantidade)
                                                                                                   FROM tbPedidoProduto pp1
                                                                                                   INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
                                                                                                   INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
                                                                                                   INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
                                                                                                   WHERE pp1.Cancelado=0
                                                                                                     AND p1.idstatuspedido = 40
                                                                                                     AND c1.idFechamento=@idFechamento
                                                                                                     AND cl1.Sexo=cl.Sexo) + SUM(p.ValorServico) - SUM(p.ValorDesconto)) 'Valor total (R$)'
FROM tbPedido p (NOLOCK)
LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
LEFT JOIN tbCliente cl (NOLOCK) ON cl.idCliente=p.idCliente
WHERE c.idFechamento=@idFechamento
  AND p.idstatuspedido = 40
  AND Sexo IS NOT NULL
GROUP BY cl.Sexo