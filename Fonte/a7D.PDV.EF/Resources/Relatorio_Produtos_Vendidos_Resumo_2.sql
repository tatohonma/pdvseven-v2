SELECT
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = p.IDProduto) AS 'Categoria' ,
       p.Nome AS 'Item' ,
       SUM(pp.Quantidade) AS 'Qtd.' ,
       CAST(SUM(pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) 'Valor total'
FROM tbPedidoProduto pp
INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
INNER JOIn tbPedido pd ON pd.IDPedido=pp.IDPedido
WHERE pp.Cancelado=0
  AND DtInclusao BETWEEN @dtInicio AND @dtFim
  AND pd.IDStatusPedido = 40
GROUP BY p.IDProduto ,
         p.Nome
ORDER BY p.Nome