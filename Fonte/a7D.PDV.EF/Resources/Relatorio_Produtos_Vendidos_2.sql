SELECT
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = p.IDProduto) AS 'Categoria' ,
       p.Nome AS 'Item' ,
       pp.Quantidade AS 'Qtd.' ,
       pp.ValorUnitario AS 'Valor unit.' ,
       CAST((pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) 'Valor total' ,
                                                                pp.DtInclusao AS 'Data' ,
                                                                u.Nome AS 'Vendedor'
FROM tbPedidoProduto pp
INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
inner join tbpedido pd on pd.IDPedido = pp.IDPedido
WHERE pp.Cancelado=0
  AND DtInclusao BETWEEN @dtInicio AND @dtFim
  and pd.IDStatusPedido = 40