select 
	tp.Nome as 'Forma Pagamento', 
	m.Descricao as 'Metodo', 
	b.Nome 'Bandeira', 
	sum(pp.Valor) 'Valor Total (R$)'
FROM tbPedido p
INNER JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
INNER JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
INNER JOIN tbBandeira b ON pp.IDBandeira=b.IDBandeira
INNER JOIN tbMeioPagamentoSAT m ON m.IDMeioPagamentoSAT=pp.IDMetodo
WHERE pp.Excluido=0
AND p.DtPedidoFechamento BETWEEN @dtInicio AND @dtFim
AND p.idStatusPedido=40
GROUP BY tp.Nome, m.Descricao, b.Nome;
 