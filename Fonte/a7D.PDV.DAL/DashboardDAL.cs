using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace a7D.PDV.DAL
{
    public static class DashboardDAL
    {
        public static IEnumerable<FaturamentoDia> FaturamentoPorDia2Meses()
        {
            var query = @"SELECT
	   dateadd(DAY, 0, datediff(DAY, 0, p.DtPedido)) as dtFechamento,
       SUM(p.ValorTotal) AS 'valor'
FROM tbPedido p
WHERE p.IDStatusPedido = 40
and p.DtPedido IS NOT NULL
and p.DtPedido >= @data
GROUP by dateadd(DAY, 0, datediff(DAY, 0, p.DtPedido))
order by dateadd(DAY, 0, datediff(DAY, 0, p.DtPedido))";
            var _2meses = DateTime.Now.AddMonths(-1);
            return ExecutarConsulta(query, new Dictionary<string, object> { { "@data", new DateTime(_2meses.Year, _2meses.Month, 1) } });
        }

        private static IEnumerable<FaturamentoDia> ExecutarConsulta(string query, Dictionary<string, object> parametros = null)
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parametros != null)
                    {
                        cmd.Parameters.AddRange(parametros.Select(p => new SqlParameter(p.Key, p.Value)).ToArray());
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new FaturamentoDia
                            {
                                Data = reader.Def<DateTime>(reader.GetOrdinal("dtFechamento")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<FaturamentoMensal> FaturamentoMensalUltimos2Anos()
        {
            var query = @"select
	  datepart(year, p.DtPedido) as ano
	, datepart(month, p.DtPedido) as mes
	, sum(p.ValorTotal) as valor
from tbPedido p
where 
	p.IDStatusPedido = 40
	and p.DtPedido is not null
	and p.DtPedido >= @data
group by 
	datepart(year, p.DtPedido)
	, datepart(month, p.DtPedido)
order by 
	1
	, 2";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", new DateTime(DateTime.Now.Year - 1, 1, 1));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new FaturamentoMensal
                            {
                                Ano = reader.Def<int>(reader.GetOrdinal("ano")),
                                Mes = reader.Def<int>(reader.GetOrdinal("mes")),
                                Total = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<FaturamentoDetalhe> FaturamentoDetalhado(DateTime data, DateTime? dataFim)
        {
            var query = @"select
	  p.DtPedido as data
	, t.Nome as tipo
	, isnull(p.ValorTotal, 0) as valor
from tbPedido p
inner join tbTipoPedido t on t.IDTipoPedido = p.IDTipoPedido
where p.IDStatusPedido = 40
and p.DtPedido is not null
and p.DtPedido >= @data";
            if (dataFim.HasValue)
                query += " and p.DtPedido <= @dataFim";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);
                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new FaturamentoDetalhe
                            {
                                Data = reader.Def<DateTime>(reader.GetOrdinal("data")),
                                Tipo = reader.Ref<string>(reader.GetOrdinal("tipo")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<MotivoCancelamento> MotivosCancelamento(DateTime data, DateTime? dataFim)
        {
            var query = @"select
	  isnull(m.Nome, 'Cancelamento SAT') as motivo
	, sum(pp.Quantidade * pp.ValorUnitario) as valor
from tbPedidoProduto pp
left join tbMotivoCancelamento m on m.IDMotivoCancelamento = pp.IDMotivoCancelamento
inner join tbPedido p on p.IDPedido = pp.IDPedido
where p.IDStatusPedido = 50
and p.DtPedido is not null
and p.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and p.DtPedido <= @dataFim";
            query += " group by m.nome";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new MotivoCancelamento
                            {
                                Motivo = reader.Ref<string>(reader.GetOrdinal("motivo")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<Categoria> ObterFaturamentoPorCategoria(DateTime data, DateTime? dataFim)
        {
            var query = @"select
	isnull(cp.Nome, 'Sem categoria') as categoria,
	sum(pp.Quantidade * pp.ValorUnitario) as valor
from tbPedidoProduto pp
inner join tbProduto p on p.IDProduto = pp.IDProduto
inner join tbPedido pd on pd.IDPedido = pp.IDPedido
left join tbProdutoCategoriaProduto pcp on pcp.IDProduto = p.IDProduto
left join tbCategoriaProduto cp on cp.IDCategoriaProduto = pcp.IDCategoriaProduto
where pp.Cancelado = 0
and pd.IDStatusPedido = 40
and pd.DtPedido is not null
and (p.IDProduto = 1 or p.IDProduto > 4)
and pd.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and pd.DtPedido <= @dataFim";

            query += @" group by cp.Nome
order by 2 desc";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Categoria
                            {
                                _Categoria = reader.Ref<string>(reader.GetOrdinal("categoria")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<TipoPagamento> ObterFaturamentoTipoPagamento(DateTime data, DateTime? dataFim)
        {
            var query = @"select
	  tp.Nome as tipo
	, sum(pp.Valor) as valor
from tbPedidoPagamento pp 
inner join tbPedido p on p.IDPedido = pp.IDPedido
inner join tbTipoPagamento tp on tp.IDTipoPagamento = pp.IDTipoPagamento
where p.IDStatusPedido = 40
and p.DtPedido is not null
and p.DtPedido >= @data
and pp.Excluido=0";

            if (dataFim.HasValue)
                query += " and p.DtPedido <= @dataFim";

            query += @" group by tp.Nome
order by 2 desc";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new TipoPagamento
                            {
                                Tipo = reader.Ref<string>(reader.GetOrdinal("tipo")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<Garcom> ObterFaturamentoPorGarcom(DateTime data, DateTime? dataFim)
        {
            var query = @"SELECT
      c.Nome as nome
    , SUM(pp.ValorUnitario*pp.Quantidade) as valor
FROM
    tbPedidoProduto pp
    INNER JOIN tbPedido p ON p.idPedido=pp.idPedido
    INNER JOIN tbUsuario c ON c.IDUsuario=pp.IDUsuario
WHERE
    pp.Cancelado=0
    AND
    p.IDStatusPedido=40
    AND
    p.ValorTotal>0
	and p.DtPedido is not null
	and p.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and p.DtPedido <= @dataFim";

            query += @" GROUP BY c.Nome
order by 2 desc";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Garcom
                            {
                                Nome = reader.Ref<string>(reader.GetOrdinal("nome")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<RankingProduto> ObterRankingProdutos(TipoRanking tipoRanking, DirecaoRanking direcaoRanking, DateTime data, DateTime? dataFim)
        {
            var query = @"select
	top 50
	tbProduto.Nome as produto
	, sum(tbpedidoproduto.quantidade) as quantidade
	, sum(tbpedidoproduto.quantidade * tbPedidoProduto.ValorUnitario) as valor
from tbPedidoProduto
inner join tbPedido on tbPedido.IDPedido = tbPedidoProduto.IDPedido
inner join tbProduto on tbProduto.IDProduto = tbPedidoProduto.IDProduto
where
tbPedidoProduto.Cancelado = 0
and tbProduto.IDProduto = 1 or tbProduto.IDProduto > 4
and tbProduto.IDTipoProduto = 10
and tbPedido.IDStatusPedido = 40
and tbPedido.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and tbPedido.DtPedido <= @dataFim";

            query += @" group by tbProduto.Nome
order by {0} {1}, 1 asc";
            query = string.Format(query, (tipoRanking == TipoRanking.Quantidade ? 2 : 3), (direcaoRanking == DirecaoRanking.Mais ? "desc" : "asc"));

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new RankingProduto
                            {
                                Produto = reader.Ref<string>(reader.GetOrdinal("produto")),
                                Quantidade = reader.Def<decimal>(reader.GetOrdinal("quantidade")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static int QuantidadeProdutosVendidos()
        {
            var query = @"select count(*) as qtd from (
select
	tbProduto.IDProduto
from tbPedidoProduto
inner join tbPedido on tbPedido.IDPedido = tbPedidoProduto.IDPedido
inner join tbProduto on tbProduto.IDProduto = tbPedidoProduto.IDProduto
where
tbPedidoProduto.Cancelado = 0
and tbProduto.IDProduto = 1 or tbProduto.IDProduto > 4
and tbProduto.IDTipoProduto = 10
and tbPedido.IDStatusPedido = 40
group by tbProduto.IDProduto) as a";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        reader.Read();
                        return reader.Def<int>(reader.GetOrdinal("qtd"));
                    }
                }
            }
        }

        public static decimal ValorProdutosVendidos()
        {
            var query = @"select
	sum(tbpedidoproduto.quantidade * tbPedidoProduto.ValorUnitario) as valor
from tbPedidoProduto
inner join tbPedido on tbPedido.IDPedido = tbPedidoProduto.IDPedido
inner join tbProduto on tbProduto.IDProduto = tbPedidoProduto.IDProduto
where
tbPedidoProduto.Cancelado = 0
and tbProduto.IDProduto = 1 or tbProduto.IDProduto > 4
and tbProduto.IDTipoProduto = 10
and tbPedido.IDStatusPedido = 40";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        reader.Read();
                        return reader.Def<decimal>(reader.GetOrdinal("valor"));
                    }
                }
            }
        }

        public static IEnumerable<Cliente> MelhoresClientes(DateTime data, DateTime? dataFim)
        {
            var query = @"select
	top 50
	tbCliente.NomeCompleto as nome,
	count(*) as qtdVisitas,
	sum(tbPedido.ValorTotal) as totalVendas,
	sum(tbpedido.valorTotal)/count(*) as ticketMedio,
	max(tbPedido.DtPedido) as ultimaVisita
from tbPedido
inner join tbCliente on tbCliente.IDCliente = tbPedido.IDCliente
where IDStatusPedido = 40
and tbPedido.ValorTotal is not null
and tbPedido.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and tbPedido.DtPedido <= @dataFim";

            query += @" group by tbCliente.NomeCompleto
order by 3 desc";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Cliente
                            {
                                Nome = reader.Ref<string>(reader.GetOrdinal("nome")),
                                QtdVisitas = reader.Def<int>(reader.GetOrdinal("qtdVisitas")),
                                TotalVendas = reader.Def<decimal>(reader.GetOrdinal("totalVendas")),
                                TicketMedio = reader.Def<decimal>(reader.GetOrdinal("ticketMedio")),
                                UltimaVisita = reader.Def<DateTime>(reader.GetOrdinal("ultimaVisita"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<FechamentoTipo> ObterFechamentoTipo(int idFechamento)
        {
            var query = @"select 
	tbTipoPedido.Nome as tipo
	, tbPedido.IDPedido
	, sum(isnull(tbPedido.ValorDesconto, 0)) as desconto
	, sum(isnull(tbPedido.ValorTotal, 0)) as valor
from tbPedido
inner join tbCaixa on tbCaixa.IDCaixa = tbPedido.IDCaixa
inner join tbTipoPedido on tbTipoPedido.IDTipoPedido = tbPedido.IDTipoPedido
where tbCaixa.IDFechamento = @idFechamento
AND tbPedido.IDStatusPedido = 40
group by tbPedido.idPedido, tbTipoPedido.Nome";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@idFechamento", idFechamento);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new FechamentoTipo
                            {
                                IDPedido = reader.Def<int>(reader.GetOrdinal("IDPedido")),
                                Tipo = reader.Ref<string>(reader.GetOrdinal("tipo")),
                                Desconto = reader.Def<decimal>(reader.GetOrdinal("desconto")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor"))
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<FechamentoCaixa> ObterFechamentoCaixa(int idFechamento)
        {
            var query = @"SELECT      
	 c.IDCaixa as 'idcaixa'
	,c.DtAbertura as 'dataabertura'
	,c.DtFechamento as 'datafechamento'
	,p1.Nome as 'pdv'
	,u.Nome as 'usuario'
	,tp.Nome as 'tipopagamento'
	,ValorAbertura as 'valorabertura'
	,ValorFechamento as 'valorfechamento'
	,(SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDCaixa=cvr.IDCaixa AND pp.IDTipoPagamento=cvr.IDTipoPagamento and p.IDStatusPedido = 40 and pp.Excluido=0) 'valorrecebido'
    , ValorFechamento - (ValorAbertura + (SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDStatusPedido = 40 and p.IDCaixa=cvr.IDCaixa AND pp.IDTipoPagamento=cvr.IDTipoPagamento AND pp.Excluido=0)) 'diferenca'
FROM 
	tbCaixa c
	LEFT JOIN tbUsuario u ON u.IDUsuario=c.IDUsuario
	LEFT JOIN tbPDV p1 ON p1.IDPDV=c.IDPDV
	LEFT JOIN tbCaixaValorRegistro cvr ON cvr.IDCaixa=c.IDCaixa
	LEFT JOIN tbTipoPagamento tp ON tp.IDTipoPagamento=cvr.IDTipoPagamento
WHERE
    c.IDFechamento=@idFechamento";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@idFechamento", idFechamento);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new FechamentoCaixa
                            {
                                IDCaixa = reader.Def<int>(reader.GetOrdinal("idcaixa")),
                                DataAbertura = reader.Def<DateTime>(reader.GetOrdinal("dataabertura")),
                                DataFechamento = reader.Def<DateTime>(reader.GetOrdinal("datafechamento")),
                                PDV = reader.Ref<string>(reader.GetOrdinal("pdv")),
                                Usuario = reader.Ref<string>(reader.GetOrdinal("usuario")),
                                TipoPagamento = reader.Ref<string>(reader.GetOrdinal("tipopagamento")),
                                ValorAbertura = reader.Def<decimal>(reader.GetOrdinal("valorabertura")),
                                ValorFechamento = reader.Def<decimal>(reader.GetOrdinal("valorfechamento")),
                                ValorRecebido = reader.Def<decimal>(reader.GetOrdinal("valorrecebido")),
                                Diferenca = reader.Def<decimal>(reader.GetOrdinal("diferenca")),
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<Garcom> ObterFechamentoGarcom(int idFechamento)
        {
            var query = @"SELECT
      c.Nome as nome
    , SUM(pp.ValorUnitario*pp.Quantidade) as valor
FROM
    tbPedidoProduto pp
    INNER JOIN tbPedido p ON p.idPedido=pp.idPedido
    INNER JOIN tbUsuario c ON c.IDUsuario=pp.IDUsuario
	INNER JOIN tbCaixa cx ON cx.IDCaixa = p.IDCaixa
WHERE
    pp.Cancelado=0
    AND
    p.IDStatusPedido=40
    AND
    p.ValorTotal>0
	and cx.IDFechamento = @idFechamento
GROUP BY c.Nome";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@idFechamento", idFechamento);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Garcom
                            {
                                Nome = reader.Ref<string>(reader.GetOrdinal("nome")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor")),
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<Resumo> ObterDadosResumo(DateTime data, DateTime? dataFim, TipoResumo tipo)
        {
            var pedido = "count(1)";
            var cliente = "sum(case ISNULL(tbPedido.NumeroPessoas, 0) when 0 then 1 else tbPedido.NumeroPessoas end)";

            var query = @"select
	tbTipoPedido.IDTipoPedido,  tbTipoPedido.Nome as tipo,
	{0} as quantidade,
	sum(isnull(ValorTotal, 0)) as valor,
	sum(isnull(ValorTotal, 0)) / {0} as
	 ticketmedio
from tbPedido
inner join tbTipoPedido on tbTipoPedido.IDTipoPedido = tbPedido.IDTipoPedido
where IDStatusPedido = 40
and isnull(tbPedido.ValorTotal, 0) > 0
and tbPedido.DtPedido >= @data";

            if (dataFim.HasValue)
                query += " and tbPedido.DtPedido <= @dataFim";

            query += @" group by tbTipoPedido.IDTipoPedido,  tbTipoPedido.Nome
order by tbTipoPedido.IDTipoPedido";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = String.Format(query, tipo == TipoResumo.Cliente ? cliente : pedido);
                    cmd.Parameters.AddWithValue("@data", data.Date);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Resumo
                            {
                                IDTipoPedido = reader.Def<int>(reader.GetOrdinal("IDTipoPedido")),
                                Tipo = reader.Ref<string>(reader.GetOrdinal("tipo")),
                                Quantidade = reader.Def<int>(reader.GetOrdinal("quantidade")),
                                Valor = reader.Def<decimal>(reader.GetOrdinal("valor")),
                                TicketMedio = reader.Def<decimal>(reader.GetOrdinal("ticketmedio")),
                            };
                        }
                    }
                }
            }
        }

        public partial class FaturamentoDia
        {
            public DateTime Data { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class FaturamentoMensal
        {
            public int Ano { get; set; }
            public int Mes { get; set; }
            public decimal Total { get; set; }
        }

        public partial class FaturamentoDetalhe
        {
            public DateTime Data { get; set; }
            public string Tipo { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class MotivoCancelamento
        {
            public string Motivo { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class Categoria
        {
            public string _Categoria { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class TipoPagamento
        {
            public string Tipo { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class RankingProduto
        {
            public string Produto { get; set; }
            public decimal Quantidade { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class Cliente
        {
            public string Nome { get; set; }
            public long QtdVisitas { get; set; }
            public decimal TotalVendas { get; set; }
            public decimal TicketMedio { get; set; }
            public DateTime UltimaVisita { get; set; }
        }

        public partial class FechamentoTipo
        {
            public int IDPedido { get; set; }
            public string Tipo { get; set; }
            public decimal Desconto { get; set; }
            public decimal Valor { get; set; }
        }

        public partial class FechamentoCaixa
        {
            public int IDCaixa { get; set; }
            public DateTime DataAbertura { get; set; }
            public DateTime DataFechamento { get; set; }
            public string PDV { get; set; }
            public string Usuario { get; set; }
            public string TipoPagamento { get; set; }
            public decimal ValorAbertura { get; set; }
            public decimal ValorFechamento { get; set; }
            public decimal ValorRecebido { get; set; }
            public decimal Diferenca { get; set; }
        }

        public partial class Garcom
        {
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public decimal Percentual { get; set; }
        }

        public partial class Resumo
        {
            public int IDTipoPedido { get; set; }
            public string Tipo { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
            public decimal TicketMedio { get; set; }
        }

        public enum TipoRanking
        {
            Quantidade,
            Valor
        }

        public enum DirecaoRanking
        {
            Mais,
            Menos
        }

        public enum TipoResumo
        {
            Pedido,
            Cliente
        }
    }
}