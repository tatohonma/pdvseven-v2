select
	tbProduto.IDProduto,
	tbProduto.Nome,
	sum(
		(case when Entrada = 1
		 then tbEntradaSaida.Quantidade
		 else tbEntradaSaida.Quantidade * -1
		 end)
		 ) Quantidade
from tbEntradaSaida (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
where not exists (select 1 from tbPedido (nolock) where GUIDMovimentacao = tbEntradaSaida.GUID_Origem)
and not exists (select 1 from tbMovimentacao (nolock) where GUID = tbEntradaSaida.GUID_Origem)
and not exists (select 1 from tbInventario (nolock) where GUID = tbEntradaSaida.GUID_Origem)
and Data BETWEEN @dtInicio AND @dtFim
group by 
	tbProduto.IDProduto,
	tbProduto.Nome