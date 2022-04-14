using a7D.PDV.EF.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public class RelatorioDAL
    {
        #region Relatorios BackOffice - Fechamento VERIFICAR
        public static DataTable ResumoFechamento(Int32 idFechamento)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT      
	 c.IDCaixa
	,c.DtAbertura as 'Data abertura'
	,c.DtFechamento as 'Data fechamento'
	,p1.Nome as 'PDV'
	,u.Nome as 'Usuário'
	,tp.Nome as 'Tipo pagamento'
	,ValorAbertura as 'Valor abertura (R$)'
	,ValorFechamento as 'Valor fechamento (R$)'
	,(SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDCaixa=cvr.IDCaixa AND pp.IDTipoPagamento=cvr.IDTipoPagamento AND pp.Excluido=0) 'Valor recebido (R$)'
    , ValorFechamento - (ValorAbertura + (SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDCaixa=cvr.IDCaixa AND pp.IDTipoPagamento=cvr.IDTipoPagamento AND pp.Excluido=0)) 'Diferença (R$)'
FROM 
	tbCaixa c
	LEFT JOIN tbUsuario u ON u.IDUsuario=c.IDUsuario
	LEFT JOIN tbPDV p1 ON p1.IDPDV=c.IDPDV
	LEFT JOIN tbCaixaValorRegistro cvr ON cvr.IDCaixa=c.IDCaixa
	LEFT JOIN tbTipoPagamento tp ON tp.IDTipoPagamento=cvr.IDTipoPagamento
WHERE
    c.IDFechamento=@idFechamento

UNION

SELECT      
	 c.IDCaixa
	,c.DtAbertura as 'Data abertura'
	,c.DtFechamento as 'Data fechamento'
	,p1.Nome as 'PDV'
	,u.Nome as 'Usuário'
	,' Total' as 'Tipo pagamento'
	,SUM(ValorAbertura) as 'Valor abertura (R$)'
	,SUM(ValorFechamento) as 'Valor fechamento (R$)'
	,(SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDCaixa=c.IDCaixa AND pp.Excluido=0) 'Valor recebido (R$)'
    ,SUM(ValorFechamento) - (SUM(ValorAbertura) + (SELECT ISNULL(SUM(Valor), 0) FROM tbPedidoPagamento pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE p.IDCaixa=c.IDCaixa AND pp.Excluido=0)) 'Diferença (R$)'
FROM 
	tbCaixa c
	LEFT JOIN tbUsuario u ON u.IDUsuario=c.IDUsuario
	LEFT JOIN tbPDV p1 ON p1.IDPDV=c.IDPDV
	LEFT JOIN tbCaixaValorRegistro cvr ON cvr.IDCaixa=c.IDCaixa
WHERE
    c.IDFechamento=@idFechamento
GROUP BY
	 c.IDCaixa
	,c.DtAbertura
	,c.DtFechamento
	,p1.Nome
	,u.Nome
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idFechamento", idFechamento);

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            String idCaixa = "";
            foreach (DataRow item in dt.Rows)
            {
                if (idCaixa == item["IDCaixa"].ToString())
                {
                    item["Data abertura"] = DBNull.Value;
                    item["Data fechamento"] = DBNull.Value; ;
                    item["PDV"] = "";
                    item["Usuário"] = "";
                }

                idCaixa = item["IDCaixa"].ToString();
            }

            dt.Columns.RemoveAt(0);
            return dt;
        }
        #endregion

        #region Relatorios Caixa
        public static DataTable ProdutosVendidos(Int32? idFechamento)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
	 sp.Nome as Status
	,pr.Nome as Produto
	,SUM(pp.Quantidade) Quantidade
	,SUM(pp.Quantidade*pp.ValorUnitario) as ValorTotal
FROM 
	tbPedido p (NOLOCK) 
	INNER JOIN tbPedidoProduto pp (NOLOCK) ON pp.idPedido=p.idPedido
	INNER JOIN tbProduto pr (NOLOCK) ON pr.idProduto=pp.idProduto
	INNER JOIN tbStatusPedido sp (NOLOCK) ON sp.IDStatusPedido=p.IDStatusPedido
    LEFT JOIN tbCaixa ca (NOLOCK) ON ca.IDCaixa=p.IDCaixa
WHERE 
	pp.Cancelado=0
    AND p.IDStatusPedido != 50
	AND
	(
        (ca.IDFechamento=@idFechamento)
        OR
        (ca.IDFechamento IS NULL AND @idFechamento IS NULL)
    )
GROUP BY
     sp.Nome
	,pr.Nome
ORDER BY 
	pr.Nome
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idFechamento", (idFechamento == null ? DBNull.Value : (Object)idFechamento));

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }
        public static DataTable ProdutosCancelados(Int32? idFechamento)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
	 p.IDPedido
	,p.DtPedido
	,cl.NomeCompleto as Cliente
	,pr.Nome as Produto
	,pp.Quantidade
	,(pp.Quantidade*pp.ValorUnitario) as ValorTotal
	,u1.IDUsuario
	,u1.Nome as UsuarioCancelamento
	,pp.DtCancelamento
FROM 
	tbPedido p (NOLOCK) 
	INNER JOIN tbPedidoProduto pp (NOLOCK) ON pp.idPedido=p.idPedido
	INNER JOIN tbProduto pr (NOLOCK) ON pr.idProduto=pp.idProduto
	INNER JOIN tbUsuario u1 (NOLOCK) ON u1.IDUsuario=pp.IDUsuario_cancelamento
	LEFT JOIN tbCaixa ca (NOLOCK) ON ca.IDCaixa=p.IDCaixa
	LEFT JOIN tbCliente cl (NOLOCK) ON cl.idCliente=p.idCliente
WHERE 
	pp.Cancelado=1
	AND
	(
        (ca.IDFechamento=@idFechamento)
        OR
        (ca.IDFechamento IS NULL AND @idFechamento IS NULL)
    )
ORDER BY 
	cl.NomeCompleto
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idFechamento", (idFechamento == null ? DBNull.Value : (Object)idFechamento));

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable QuantidadePedidosPorSexo()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
	 sp.Nome as 'Status'
	,' ' + tp.Nome as 'Tipo'
	,COUNT(1) as 'Qtd.' 
FROM 
	tbPedido p
	LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
	LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
    LEFT JOIN tbTipoPedido tp ON tp.IDTipoPedido=p.IDTipoPedido
WHERE
	p.idTipoPedido<>20
	AND (ca.idFechamento IS NULL OR p.idCaixa IS NULL)
    AND p.IDStatusPedido <> 50
GROUP BY
	 sp.Nome
	,tp.Nome
	
UNION ALL

SELECT 
	 sp.Nome as 'Status'
	,case cl.Sexo 
		when 'm' then 'Homem' 
		when 'f' then 'Mulher' end as 'Tipo'
	,COUNT(1) as 'Qtd.' 
FROM 
	tbPedido p
	LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
	LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
	LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
	p.idTipoPedido=20
	AND (ca.idFechamento IS NULL OR p.idCaixa IS NULL)
    AND p.IDStatusPedido <> 50
GROUP BY
	 cl.Sexo
	,sp.Nome

UNION ALL

SELECT 
	 sp.Nome as 'Status'
	,'TOTAL' as 'Tipo'
	,COUNT(1) as 'qtd' 
FROM 
	tbPedido p
	LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
	LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
	LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
	(ca.IDFechamento IS NULL OR p.IDCaixa IS NULL)
    AND p.IDStatusPedido <> 50
GROUP BY
	sp.Nome

UNION ALL

SELECT 
	 'TODOS' as 'Status'
	,'TOTAL' as 'Tipo'
	,COUNT(1) as 'qtd' 
FROM 
	tbPedido p
	LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
	LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
	LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
	(ca.IDFechamento IS NULL OR p.IDCaixa IS NULL)
    AND p.IDStatusPedido <> 50
ORDER BY 'status', tipo
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable QuantidadePedidosPorDia()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
        sp.Nome as 'Status'
       ,' ' + tp.Nome as 'Tipo'
       ,COUNT(1) as 'Qtd.' 
FROM 
       tbPedido p
       LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
       LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
    LEFT JOIN tbTipoPedido tp ON tp.IDTipoPedido=p.IDTipoPedido
WHERE
    p.idTipoPedido<>20
    AND p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
    AND ca.IDFechamento is NULL
    AND p.IDStatusPedido <> 50
GROUP BY
       sp.Nome
       ,tp.Nome
       
UNION ALL

SELECT 
        sp.Nome as 'Status'
       ,case cl.Sexo 
             when 'm' then 'Homem' 
             when 'f' then 'Mulher' end as 'Tipo'
       ,COUNT(1) as 'Qtd.' 
FROM 
       tbPedido p
       LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
       LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
       LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
    p.idTipoPedido=20
    AND p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
	AND ca.IDFechamento is NULL
    AND p.IDStatusPedido <> 50
GROUP BY
       cl.Sexo
       ,sp.Nome

UNION ALL

SELECT 
        sp.Nome as 'Status'
       ,'TOTAL' as 'Tipo'
       ,COUNT(1) as 'Qtd' 
FROM 
       tbPedido p
       LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
       LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
       LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
    p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
    AND ca.IDFechamento is NULL
    AND p.IDStatusPedido <> 50
	     
GROUP BY
       sp.Nome

UNION ALL

SELECT 
        'TODOS' as 'Status'
       ,'TOTAL' as 'Tipo'
       ,COUNT(1) as 'Qtd' 
FROM 
       tbPedido p
       LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
       LEFT JOIN tbStatusPedido sp ON sp.idStatusPedido=p.idStatusPedido
       LEFT JOIN tbCaixa ca ON ca.IDCaixa=p.IDCaixa
WHERE
    p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
    AND ca.IDFechamento is NULL
    AND p.IDStatusPedido <> 50
ORDER BY 'status', tipo";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable QuantidadePedidosPorTipoEntrada()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
        ' MESA' as 'Tipo'
       ,COUNT(1) as 'Qtd.'
FROM 
       tbPedido p (NOLOCK)
       LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
WHERE
    p.idTipoPedido=10
    AND p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
    AND c.IDFechamento is NULL
    AND p.IDStatusPedido <> 50

UNION ALL

SELECT 
        ' DELIVERY' as 'Tipo'
       ,COUNT(1) as 'Qtd.'
FROM 
       tbPedido p (NOLOCK)
       LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
WHERE
    p.idTipoPedido=30
    AND p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
	AND c.IDFechamento is NULL
    AND p.IDStatusPedido <> 50

UNION ALL

SELECT 
        te.Nome as 'Tipo'
       ,COUNT(DISTINCT(p.idPedido)) as 'Qtd.'
FROM 
       tbPedido p (NOLOCK)
       LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
       LEFT JOIN tbTipoEntrada te (NOLOCK) ON te.idTipoEntrada=p.idTipoEntrada
WHERE
       p.idTipoPedido=20
       AND
       p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
	   AND
	   c.IDFechamento is NULL
GROUP BY
       te.Nome

UNION ALL

SELECT 
        'TOTAL'
       ,COUNT(DISTINCT(p.idPedido)) as 'Qtd.'
FROM 
       tbPedido p (NOLOCK)
       LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
WHERE
    p.DtPedido>(SELECT ISNull(MAX(DtFechamento), datefromparts(2000, 1, 1)) FROM tbFechamento)
    AND c.IDFechamento is NULL
    AND p.IDStatusPedido <> 50
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable ValorPorTipoPagamento(int? idFechamento = null)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
	 tp.Nome 'Tipo pagamento'
	,SUM(pp.Valor) 'Valor total (R$)'
FROM 
	tbCaixa c
	LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
	LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
	LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE
";
            if (idFechamento.HasValue)
                querySql += @"c.idFechamento = @idFechamento";
            else
                querySql += @"(p.idStatusPedido=40 AND c.idFechamento IS NULL OR p.idCaixa IS NULL)";

            querySql += $@"
    AND
	pp.idTipoPagamento IS NOT NULL
    AND pp.Excluido=0
    AND IsNull(pp.IDGateway, 0)<>{(int)EGateway.ContaCliente}
    AND p.IDStatusPedido <> 50
GROUP BY
	tp.Nome

UNION ALL

SELECT 
	 'TOTAL'
	,isnull(SUM(pp.Valor), 0) 'Valor total (R$)'
FROM 
	tbCaixa c
	LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
	LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
	LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE
";
            if (idFechamento.HasValue)
                querySql += @"c.idFechamento = @idFechamento";
            else
                querySql += @"(p.idStatusPedido=40 AND c.idFechamento IS NULL OR p.idCaixa IS NULL)";

            querySql += $@"
AND
	pp.idTipoPagamento IS NOT NULL
    AND pp.Excluido=0
    AND IsNull(pp.IDGateway, 0)<>{(int)EGateway.ContaCliente}
    AND p.IDStatusPedido <> 50";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            if (idFechamento.HasValue)
                da.SelectCommand.Parameters.AddWithValue("@idFechamento", idFechamento.Value);

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable ResumoCreditos(int? idFechamento = null)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
    'Pagamentos com crédito' Titulo
	,ISNULL(SUM(pp.Valor),0) Total
FROM 
	tbCaixa c
	LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
	LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
	LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE
";
            if (idFechamento.HasValue)
                querySql += @"c.idFechamento = @idFechamento";
            else
                querySql += @"(p.idStatusPedido=40 AND c.idFechamento IS NULL OR p.idCaixa IS NULL)";

            querySql += $@"

    AND pp.idTipoPagamento IS NOT NULL
    AND pp.Excluido=0
    AND IsNull(pp.IDGateway, 0)={(int)EGateway.ContaCliente}
    AND p.IDStatusPedido <> 50

UNION ALL

SELECT 
    'Compra de crédito' Titulo
	,ISNULL(SUM(pp.ValorUnitario),0) Total
FROM 
tbCaixa c
LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
INNER JOIN tbPedidoProduto pp ON p.IDPedido=pp.IDPedido
INNER JOIN tbProduto pd ON pd.IDProduto=pp.IDProduto
WHERE
";
if (idFechamento.HasValue)
                querySql += @"c.idFechamento = @idFechamento";
            else
                querySql += @"(p.idStatusPedido=40 AND c.idFechamento IS NULL OR p.idCaixa IS NULL)";

            querySql += $@"
    AND pd.IDTipoProduto=50 
    AND pp.Cancelado=0
    AND p.IDStatusPedido <> 50";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            if (idFechamento.HasValue)
                da.SelectCommand.Parameters.AddWithValue("@idFechamento", idFechamento.Value);

            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }

        #endregion

        public static DataTable CarregarRelatorio(String querySQL, Dictionary<object, object> parametros)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            da = new SqlDataAdapter(querySQL, DB.ConnectionString);
            if (parametros != null)
            {
                foreach (var item in parametros)
                {
                    da.SelectCommand.Parameters.AddWithValue(item.Key.ToString(), (item.Value != null ? item.Value : DBNull.Value));
                }
            }

            da.Fill(ds, "DataSet_DataTable");
            dt = ds.Tables[0];

            return dt;
        }
        
        public static DataTable TotalDesconto(int idFechamento)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string querySQL =
@"SELECT 
	 isnull(td.Nome, 'Motivo não informado') 'Motivo Desconto'
	,SUM(p.ValorDesconto) 'Valor total (R$)'
FROM 
	tbCaixa (nolock) c
	LEFT JOIN tbPedido (nolock) p ON p.idCaixa=c.idCaixa
	LEFT JOIN tbTipoDesconto (nolock) td on td.IDTipoDesconto = p.IDTipoDesconto
WHERE
	(c.idFechamento = @idFechamento) 
    AND td.IDTipoDesconto is not null
    AND p.IDStatusPedido <> 50
GROUP BY
	td.Nome

UNION ALL

SELECT 
	 'TOTAL' 'Motivo Desconto'
	,SUM(coalesce(p.ValorDesconto, 0)) 'Valor total (R$)'
FROM 
	tbCaixa (nolock) c
	LEFT JOIN tbPedido (nolock) p ON p.idCaixa=c.idCaixa
	LEFT JOIN tbTipoDesconto (nolock) td on td.IDTipoDesconto = p.IDTipoDesconto
WHERE
	(c.idFechamento = @idFechamento)
    AND p.IDStatusPedido <> 50";

            da = new SqlDataAdapter(querySQL, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idFechamento", idFechamento);

            da.Fill(ds);

            dt = ds.Tables[0];

            return dt;
        }
    }
}
