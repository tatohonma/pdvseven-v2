select 
	 a.Nome as 'Nome'
	,b.Nome as 'Classificação Fiscal'
	,a.ValorUnitario as 'Valor'
	,b.NCM as 'NCM'
	,c.CFOP as 'CFOP'
	,b.IOF as 'IOF'
	,b.IPI as 'IPI'
	,b.PISPASEP as 'PISPASEP'
	,b.CIDE as 'CIDE'
	,b.COFINS as 'COFINS'
	,b.ICMS as 'ICMS'
	,b.ISS as 'ISS'

FROM
	tbproduto a
	INNER JOIN tbClassificacaoFiscal b ON a.IDClassificacaoFiscal=b.IDClassificacaoFiscal
	INNER JOIN tbTipoTributacao c ON c.IDTipoTributacao=b.IDTipoTributacao
WHERE
	 Excluido=0
	 AND
	 Ativo=1
	 AND
	 IdProduto>4
ORDER BY
	 a.Nome