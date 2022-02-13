SELECT p.IDPedido AS 'Cód. Pedido' ,
       u.Nome AS 'Usuário fechamento' ,
       cl.NomeCompleto AS 'Cliente' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto NOT IN (2, 3)), 0) AS 'Valor produto (R$)' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto IN (2, 3)), 0) AS 'Valor entrada (R$)' ,
       ISNULL(p.ValorServico, 0) AS 'Valor serviço (R$)' ,
       ISNULL(p.ValorDesconto, 0) AS 'Valor desconto (R$)' ,
       (ISNULL(
                 (SELECT SUM(ValorUnitario*Quantidade)
                  FROM tbPedidoProduto
                  WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) AS 'Valor total (R$)' ,
       p.Observacoes
FROM tbPedido p
INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
WHERE ca.idFechamento=@idFechamento
  AND p.IDStatusPedido = 40
  AND p.ValorDesconto>0