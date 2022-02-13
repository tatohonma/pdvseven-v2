SELECT
   te.Nome as 'Tipo'
  ,COUNT(DISTINCT(p.idPedido)) as 'Qtd.'
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
  ,te.Nome