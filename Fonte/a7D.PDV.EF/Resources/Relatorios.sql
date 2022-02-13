BEGIN TRANSACTION T1

SET IDENTITY_INSERT [dbo].[tbRelatorio] ON 

INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (1, 1, N'Resumo dos Fechamentos', NULL, N'', 1, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (2, 1, N'Resumo por Sexo', NULL, N'SELECT CASE cl.Sexo
           WHEN ''m'' THEN ''Homem''
           WHEN ''f'' THEN ''Mulher''
       END ''Tipo'' ,COUNT(DISTINCT(p.idPedido)) AS ''Qtd.'' ,
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
                               3)) ''Valor produto (R$)'' ,
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
                           3)) ''Valor entrada (R$)'' ,
                               SUM(p.ValorServico) ''Valor serviço (R$)'' ,
                                                   SUM(p.ValorDesconto) ''Valor desconto (R$)'' ,(
                                                                                                  (SELECT SUM(ValorUnitario*Quantidade)
                                                                                                   FROM tbPedidoProduto pp1
                                                                                                   INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
                                                                                                   INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
                                                                                                   INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
                                                                                                   WHERE pp1.Cancelado=0
                                                                                                     AND p1.idstatuspedido = 40
                                                                                                     AND c1.idFechamento=@idFechamento
                                                                                                     AND cl1.Sexo=cl.Sexo) + SUM(p.ValorServico) - SUM(p.ValorDesconto)) ''Valor total (R$)''
FROM tbPedido p (NOLOCK)
LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
LEFT JOIN tbCliente cl (NOLOCK) ON cl.idCliente=p.idCliente
WHERE c.idFechamento=@idFechamento
  AND p.idstatuspedido = 40
  AND Sexo IS NOT NULL
GROUP BY cl.Sexo', 20, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (4, 1, N'Resumo por Tipo Pagamento', NULL, N'SELECT tp.Nome ''Tipo pagamento'' ,
               COUNT(DISTINCT(pp.IDTipoPagamento)) AS ''Qtd.'' ,
               SUM(pp.Valor) ''Valor total (R$)''
FROM tbCaixa c
LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE idFechamento=@idFechamento
  AND pp.idTipoPagamento IS NOT NULL
  AND p.idStatusPedido = 40
  AND pp.Excluido=0
GROUP BY tp.Nome', 8, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (5, 1, N'Pedidos com Desconto', NULL, N'SELECT p.IDPedido AS ''Cód. Pedido'' ,
       u.Nome AS ''Usuário fechamento'' ,
       cl.NomeCompleto AS ''Cliente'' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto NOT IN (2, 3)), 0) AS ''Valor produto (R$)'' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto IN (2, 3)), 0) AS ''Valor entrada (R$)'' ,
       ISNULL(p.ValorServico, 0) AS ''Valor serviço (R$)'' ,
       ISNULL(p.ValorDesconto, 0) AS ''Valor desconto (R$)'' ,
       (ISNULL(
                 (SELECT SUM(ValorUnitario*Quantidade)
                  FROM tbPedidoProduto
                  WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) AS ''Valor total (R$)'' ,
       p.Observacoes
FROM tbPedido p
INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
WHERE ca.idFechamento=@idFechamento
  AND p.IDStatusPedido = 40
  AND p.ValorDesconto>0', 30, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (6, 1, N'Lista de Pedidos', NULL, N'SELECT p.IDPedido AS ''Cód. Pedido'' ,
       u.Nome AS ''Usuário fechamento'' ,
       cl.NomeCompleto AS ''Cliente'' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto NOT IN (2, 3)), 0) AS ''Valor produto (R$)'' ,
       ISNULL(
                (SELECT SUM(ValorUnitario*Quantidade)
                 FROM tbPedidoProduto
                 WHERE idPedido=p.idPedido
                   AND idProduto IN (2, 3)), 0) AS ''Valor entrada (R$)'' ,
       ISNULL(p.ValorServico, 0) AS ''Valor serviço (R$)'' ,
       ISNULL(p.ValorDesconto, 0) AS ''Valor desconto (R$)'' ,
       (ISNULL(
                 (SELECT SUM(ValorUnitario*Quantidade)
                  FROM tbPedidoProduto
                  WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) AS ''Valor total (R$)'' ,
       p.Observacoes
FROM tbPedido p
INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
WHERE ca.idFechamento=@idFechamento
and p.idstatuspedido = 40', 40, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (7, 1, N'Produtos Cancelados', NULL, N'SELECT
   p.idPedido as ''Cód. Pedido''
  ,cl.NomeCompleto as ''Cliente''
  ,p1.Nome as ''PDV lançamento''
  ,u1.Nome as ''Usuário lançamento''
  ,pp.DtInclusao as ''Data lançamento''
  ,p2.Nome as ''PDV cancelamento''
  ,u2.Nome as ''Usuário cancelamento''
  ,pp.DtCancelamento ''Data cancelamento''
  ,pr.Nome as ''Produto''
  ,pp.Quantidade as ''Qtd.''
  ,pp.ValorUnitario as ''Valor unitário (R$)''
    ,(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
  ,mc.Nome as ''Motivo Cancelamento''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
  LEFT JOIN tbMotivoCancelamento mc ON mc.IDMotivoCancelamento=pp.IDMotivoCancelamento
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1', 50, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (8, 1, N'Produtos Cancelados - Resumo por Usuário Lançamento', NULL, N'SELECT
     p1.Nome as ''PDV lançamento''
  ,u1.Nome as ''Usuário lançamento''
  ,SUM(pp.Quantidade) as ''Qtd.''
  ,SUM(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
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
  ,u1.Nome', 60, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (9, 1, N'Produtos Cancelados - Resumo por Usuário Cancelamento', NULL, N'SELECT
     p1.Nome as ''PDV Cancelamento''
  ,u1.Nome as ''Usuário Cancelamento''
  ,SUM(pp.Quantidade) as ''Qtd.''
  ,SUM(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
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
  ,u1.Nome', 70, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (10, 1, N'Produtos Vendidos', NULL, N'SELECT p.idPedido AS ''Cód. Pedido'' ,
       cl.NomeCompleto AS ''Cliente'' ,
       p1.Nome AS ''PDV lançamento'' ,
       u1.Nome AS ''Usuário lançamento'' ,
       pp.DtInclusao AS ''Data lançamento'' ,
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = pp.IDProduto) AS ''Categoria'' ,
       pr.Nome AS ''Produto'' ,
       pp.Quantidade AS ''Qtd.'' ,
       pp.ValorUnitario AS ''Valor unitário (R$)'' ,
       (pp.ValorUnitario*pp.Quantidade) AS ''Valor total (R$)''
FROM tbPedidoProduto pp
LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE c.idFechamento=@idFechamento
  AND pp.Cancelado=0
  AND p.idStatusPedido = 40', 80, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (11, 1, N'Produtos Vendidos - Resumo', NULL, N'SELECT
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = pp.IDProduto) AS ''Categoria'' ,
       pr.Nome AS ''Produto'' ,
       SUM(pp.Quantidade) AS ''Qtd.'' ,
       SUM(pp.ValorUnitario*pp.Quantidade) AS ''Valor total (R$)''
FROM tbPedidoProduto pp
LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE c.idFechamento=@idFechamento
  AND pp.Cancelado=0
  AND p.IDStatusPedido = 40
GROUP BY pp.IDProduto,
         pr.Nome', 90, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (12, 2, N'Lista de Clientes', NULL, N'SELECT
   NomeCompleto
  ,(''('' + CAST(ISNULL(Telefone1DDD, 11) as VARCHAR(2)) + '') '' + CAST(Telefone1Numero as VARCHAR(9))) as ''Telefone''
  ,(CAST(DAY(DataNascimento) as VARCHAR(2)) + ''/'' + CAST(MONTH(DataNascimento) as VARCHAR(2))) as ''Aniversário''
  ,case c.Sexo
    when ''m'' then ''Homem''
    when ''f'' then ''Mulher'' END ''Tipo''
  ,Documento1 as ''CPF/CNPJ''
  ,Endereco + '', '' + EnderecoNumero as ''Endereço''
  ,Bairro
  ,Cidade
  ,e.Nome as UF
  ,c.CEP
  ,DtInclusao as ''Data cadastro''
FROM
  tbCliente c (NOLOCK)
  LEFT JOIN tbEstado e (NOLOCK) ON e.idEstado=c.idEstado
WHERE
  DtInclusao BETWEEN @dtInicio AND @dtFim
ORDER BY
  NomeCompleto', 10, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (13, 2, N'Produtos Vendidos', NULL, N'SELECT
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = p.IDProduto) AS ''Categoria'' ,
       p.Nome AS ''Item'' ,
       pp.Quantidade AS ''Qtd.'' ,
       pp.ValorUnitario AS ''Valor unit.'' ,
       CAST((pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) ''Valor total'' ,
                                                                pp.DtInclusao AS ''Data'' ,
                                                                u.Nome AS ''Vendedor''
FROM tbPedidoProduto pp
INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
inner join tbpedido pd on pd.IDPedido = pp.IDPedido
WHERE pp.Cancelado=0
  AND DtInclusao BETWEEN @dtInicio AND @dtFim
  and pd.IDStatusPedido = 40', 20, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (14, 2, N'Produtos Vendidos - Resumo', NULL, N'SELECT
  (SELECT TOP 1 c.Nome
   FROM tbProdutoCategoriaProduto pcp
   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
   AND pcp.IDProduto = p.IDProduto) AS ''Categoria'' ,
       p.Nome AS ''Item'' ,
       SUM(pp.Quantidade) AS ''Qtd.'' ,
       CAST(SUM(pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) ''Valor total''
FROM tbPedidoProduto pp
INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
INNER JOIn tbPedido pd ON pd.IDPedido=pp.IDPedido
WHERE pp.Cancelado=0
  AND DtInclusao BETWEEN @dtInicio AND @dtFim
  AND pd.IDStatusPedido = 40
GROUP BY p.IDProduto ,
         p.Nome
ORDER BY p.Nome', 30, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (15, 1, N'Resumo por Tipo de Entrada', NULL, N'SELECT
   te.Nome as ''Tipo''
  ,COUNT(DISTINCT(p.idPedido)) as ''Qtd.''
FROM
  tbPedido p (NOLOCK)
  LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
  LEFT JOIN tbTipoEntrada te (NOLOCK) ON te.idTipoEntrada=p.idTipoEntrada
WHERE
  c.idFechamento=@idFechamento
  AND
  te.idTipoEntrada IS NOT NULL
GROUP BY
   te.idTipoEntrada
  ,te.Nome', 10, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (16, 2, N'Produtos Vendidos por Cliente', NULL, N'select
    isnull(tbCliente.NomeCompleto, ''Sem Cliente'') as ''Cliente''
  , tbProduto.Codigo as ''Codigo Produto''
  , tbProduto.Nome as ''Produto''
  , sum(tbPedidoProduto.Quantidade) as Quantidade
  , tbPedidoProduto.ValorUnitario
  , cast(sum(tbPedidoproduto.Quantidade * tbPedidoProduto.ValorUnitario) as decimal(10, 2)) as ''Valor''
  , cast(tbPedido.DtPedidoFechamento as date) as ''Data Fechamento Pedido''
from tbPedidoProduto (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbPedidoProduto.IDProduto
                and tbProduto.IDTipoProduto in (10, 20)
inner join tbPedido (nolock) on tbPedido.IDPedido = tbPedidoProduto.IDPedido
                and tbPedido.IDStatusPedido in (40)
left join tbCliente (nolock) on tbCliente.IDCliente = tbPedido.IDCliente
where
  tbPedidoProduto.Cancelado = 0
  and (tbProduto.IDTipoProduto = 10 or tbProduto.IDTipoProduto = 20 and tbPedidoProduto.ValorUnitario > 0)
group by
    isnull(tbCliente.NomeCompleto, ''Sem Cliente'')
  , tbProduto.Codigo
  , tbProduto.Nome
  , tbPedidoProduto.ValorUnitario
  , cast(tbPedido.DtPedidoFechamento as date)
order by
  cast(tbPedido.DtPedidoFechamento as date)
  , isnull(tbCliente.NomeCompleto, ''Sem Cliente'')', 40, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (17, 2, N'Taxa de serviço por garçom', NULL, N'SELECT
   u.Nome
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade) as money) as ''Vendido''
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*0.1) as money) as ''Serviço''  
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)) as money) as ''Vendido com Desconto''
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)*0.1) as money) as ''Serviço com Desconto''
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
  u.Nome', 100, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (18, 2, N'Taxa de Entrega Delivery', NULL, N'SELECT

  c.Nome as ''Nome Entregador''
  ,pp.Valor as ''Valor da Taxa''
  ,COUNT (p.ValorEntrega) as ''Quantidade''
  ,SUM (p.ValorEntrega) as ''Valor Total''
  
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
  ,ValorEntrega', 110, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (19, 1, N'Motivos de Desconto', NULL, N'SELECT 
   p.IDPedido as ''Cód. Pedido''
  ,u.Nome as ''Usuário fechamento''
  ,cl.NomeCompleto as ''Cliente''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as ''Valor produto (R$)''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as ''Valor entrada (R$)''
  ,ISNULL(p.ValorServico, 0) as ''Valor serviço (R$)''
  ,ISNULL(p.ValorDesconto, 0) as ''Valor desconto (R$)''
  ,(ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) as ''Valor total (R$)''
  ,p.Observacoes as ''Observações''
  ,a.Nome as ''Tipo Desconto''
  ,p.DtPedidoFechamento as ''Data''
FROM 
  tbPedido p 
  INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
  LEFT JOIN tbTipoDesconto a ON p.IDTipoDesconto=a.IDTipoDesconto
WHERE 
  ca.idFechamento=@idFechamento
  AND
  p.IDStatusPedido = 40
  AND
  p.ValorDesconto>0
  AND
  p.idtipodesconto is not NULL', 120, NULL)
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (20, 1, N'', NULL, N'SELECT 
   p.IDPedido as ''Cód. Pedido''
  ,u.Nome as ''Usuário fechamento''
  ,cl.NomeCompleto as ''Cliente''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as ''Valor produto (R$)''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as ''Valor entrada (R$)''
  ,ISNULL(p.ValorServico, 0) as ''Valor serviço (R$)''
  ,ISNULL(p.ValorDesconto, 0) as ''Valor desconto (R$)''
  ,(ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) as ''Valor total (R$)''
  ,p.Observacoes as ''Observações''
  ,a.Nome as ''Tipo Desconto''
  ,p.DtPedidoFechamento as ''Data''
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
  p.idtipodesconto is not NULL', 130, NULL)

SET IDENTITY_INSERT [dbo].[tbRelatorio] OFF

COMMIT TRANSACTION T1