SELECT c.NomeCompleto, s.dtMovimento [Data Pedido], s.IDPedido [N.Pedido], S.Valor Credito,
	(SELECT sum(valor) FROM tbsaldo d WHERE d.tipo='D' AND d.IDPai=s.idSaldo) Debito,
	S.Valor - (SELECT sum(valor) FROM tbsaldo d WHERE d.tipo='D' AND d.IDPai=s.idSaldo) Saldo
FROM tbsaldo s
INNER JOIN tbCliente c ON c.IDCliente=s.IDCliente
WHERE s.tipo='C'
AND s.dtMovimento BETWEEN @dtInicio AND @dtFim
ORDER BY c.NomeCompleto;