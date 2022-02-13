using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

namespace a7D.PDV.DAL
{
    public static class EntradaSaidaDAL
    {
        public static DataTable EstoqueAtual(DateTime dataEstoque, out DataTable dtInventario, int? idProdutoFiltro = null, bool exibirProdutosInativos = false)
        {
            DataTable dtResultado = new DataTable();
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataSet dsInventario = new DataSet();
            dtInventario = new DataTable();
            var dataUltimoInventario = SqlDateTime.MinValue.Value;
            var temInventario = false;
            #region query
            string query = @"SELECT TOP 1 IDInventario, Data
FROM
	tbInventario (nolock)
WHERE
	Data = (
		SELECT ISNULL(MAX(Data), CAST(0 AS DATETIME))
		FROM
			tbInventario (nolock)
		WHERE
			Processado = 1
			AND Excluido = 0
			AND Data <= @dataEstoque
	)";
            #endregion
            using (da = new SqlDataAdapter(query, DB.ConnectionString))
            {
                var p = da.SelectCommand.Parameters.Add("@dataEstoque", SqlDbType.DateTime2);
                p.Value = dataEstoque;
                //da.SelectCommand.Parameters.AddWithValue("@dataEstoque", dataEstoque);
                da.Fill(ds);
            }

            if (ds.Tables.Count < 1)
                throw new Exception();
            int? idInventario = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                idInventario = ds.Tables[0].Rows[0].Field<int?>("IDInventario");
                dataUltimoInventario = ds.Tables[0].Rows[0].Field<DateTime>("Data");
            }

            temInventario = idInventario.HasValue;
            if (temInventario)
            {
                #region query
                query = @"SELECT 
	  tbProduto.IDProduto
	, tbProduto.Nome
	, tbUnidade.IDUnidade
	, tbUnidade.Nome as 'Unidade'
	, tbUnidade.Simbolo
	, tbInventarioProdutos.Quantidade
FROM tbInventarioProdutos (nolock)
INNER JOIN tbProduto (nolock) on tbProduto.IDProduto = tbInventarioProdutos.IDProduto
INNER JOIN tbUnidade (nolock) on tbUnidade.IDUnidade = tbInventarioProdutos.IDUnidade
WHERE 
	IDInventario = @idInventario
    AND (tbProduto.IDProduto = @idProduto OR @idProduto is null)";
                if (!exibirProdutosInativos)
                {
                    query += " AND tbProduto.Ativo = 1 AND tbProduto.Excluido = 0";
                }

                #endregion
                using (da = new SqlDataAdapter(query, DB.ConnectionString))
                {
                    da.SelectCommand.Parameters.AddWithValue("@idInventario", idInventario);
                    if (idProdutoFiltro.HasValue)
                        da.SelectCommand.Parameters.AddWithValue("@idProduto", idProdutoFiltro);
                    else
                        da.SelectCommand.Parameters.AddWithValue("@idProduto", DBNull.Value);

                    da.Fill(dsInventario);
                }

                dtInventario = dsInventario.Tables[0];
            }
            else
                dtInventario = null;

            #region query
            query = @"SELECT
	  tbproduto.IDProduto
    , tbProduto.Nome 
	, tbProduto.IDUnidade
    , tbUnidade.Nome as 'Unidade'
    , tbUnidade.Simbolo
	, sum(tbEntradaSaida.Quantidade) as Quantidade
FROM tbEntradaSaida (nolock)
INNER JOIN tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
INNER JOIN tbUnidade (nolock) on tbUnidade.IDUnidade = tbProduto.IDUnidade
LEFT JOIN tbMovimentacao (nolock) on tbMovimentacao.GUID = tbEntradaSaida.GUID_Origem
WHERE 
    tbEntradaSaida.Data > @dataMovimentacao
    AND (tbMovimentacao.IDMovimentacao is null or (tbMovimentacao.Processado = 1 and tbMovimentacao.Excluido = 0))
    AND tbEntradaSaida.Data <= @dataEstoque
    AND (tbProduto.IDProduto = @idProduto OR @idProduto is null)";

            if (!exibirProdutosInativos)
            {
                query += " AND tbProduto.Ativo = 1 AND tbProduto.Excluido = 0";
            }

            query += @" GROUP BY 
	  tbProduto.IDProduto
	, tbProduto.IDUnidade
	, tbProduto.Nome
	, tbUnidade.Nome
	, tbUnidade.Simbolo
HAVING 
    SUM(tbentradasaida.quantidade) != 0";
            #endregion
            ds = new DataSet();

            using (da = new SqlDataAdapter(query, DB.ConnectionString))
            {
                da.SelectCommand.Parameters.AddWithValue("@dataMovimentacao", dataUltimoInventario);
                da.SelectCommand.Parameters.AddWithValue("@dataEstoque", dataEstoque);
                if (idProdutoFiltro.HasValue)
                    da.SelectCommand.Parameters.AddWithValue("@idProduto", idProdutoFiltro);
                else
                    da.SelectCommand.Parameters.AddWithValue("@idProduto", DBNull.Value);
                da.Fill(ds);
            }

            dtResultado.Columns.Add("IDProduto", typeof(int));
            dtResultado.Columns.Add("Nome", typeof(string));
            dtResultado.Columns.Add("IDUnidade", typeof(int));
            dtResultado.Columns.Add("Unidade", typeof(string));
            dtResultado.Columns.Add("Simbolo", typeof(string));
            dtResultado.Columns.Add("Quantidade", typeof(decimal));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var newRow = dtResultado.NewRow();
                var idProduto = row.Field<int>("IDProduto");
                var idUnidade = row.Field<int>("IDUnidade");
                var nome = row.Field<string>("Nome");
                var unidade = row.Field<string>("Unidade");
                var simbolo = row.Field<string>("Simbolo");

                newRow.SetField("IDProduto", idProduto);
                newRow.SetField("IDUnidade", idUnidade);

                var quantidade = row.Field<decimal>("Quantidade");

                newRow.SetField("Quantidade", quantidade);
                newRow.SetField("Nome", nome);
                newRow.SetField("Unidade", unidade);
                newRow.SetField("Simbolo", simbolo);
                dtResultado.Rows.Add(newRow);
            }

            ds.Dispose();
            dsInventario.Dispose();

            return dtResultado;
        }

        public static List<EntradaSaidaInformation> ListarValidas(DateTime? dataDe, DateTime? dataAte, bool exibirProdutosInativos = false)
        {
            SqlDataAdapter da;
            DataSet ds;
            #region query
            var querySql = @"select tbEntradaSaida.*
from tbEntradaSaida (nolock)
left join tbMovimentacao (nolock) on tbMovimentacao.GUID = tbEntradaSaida.GUID_Origem
left join tbProduto (nolock) on tbEntradaSaida.IDProduto = tbProduto.IDProduto
where
tbMovimentacao.IDMovimentacao IS NULL or (tbMovimentacao.Processado = 1 and tbMovimentacao.Excluido = 0)
and (@dataDe is null or tbEntradaSaida.Data > @dataDe)
and (@dataAte is null or tbEntradaSaida.Data <= @dataAte)";
            #endregion

            if (!exibirProdutosInativos)
                querySql += " and tbProduto.Ativo = 1 and tbProduto.Excluido = 0";

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            {
                da.SelectCommand.Parameters.AddWithValue("@dataDe", dataDe);
                da.SelectCommand.Parameters.AddWithValue("@dataAte", dataAte);
                using (ds = new DataSet())
                {
                    var resultado = new List<EntradaSaidaInformation>();
                    da.Fill(ds);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var entradaSaida = new EntradaSaidaInformation
                        {
                            IDEntradaSaida = row.Field<int?>("IDEntradaSaida"),
                            Data = row.Field<DateTime?>("Data"),
                            Entrada = row.Field<bool?>("Entrada"),
                            GUID_Origem = row.Field<string>("GUID_Origem"),
                            Produto = new ProdutoInformation
                            {
                                IDProduto = row.Field<int?>("IDProduto")
                            },
                            Quantidade = row.Field<decimal?>("Quantidade")
                        };

                        resultado.Add(entradaSaida);
                    }
                    return resultado;
                }
            }
        }

        public static bool ExisteMovimentacao(int idProduto)
        {
            SqlDataAdapter da;
            DataSet ds;

            #region querySql
            var querySql = @"select 1 as 'existe'
where exists (select 1 from tbEntradaSaida (nolock) where IDProduto = @idProduto)
or exists (select 1 from tbInventarioProdutos (nolock) where IDProduto = @idProduto)";
            #endregion

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (ds = new DataSet())
            {
                da.SelectCommand.Parameters.AddWithValue("@idProduto", idProduto);
                da.Fill(ds);
                return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            }
        }

        public static DataSet Historico(DateTime dataHistorico, int idProduto)
        {
            DataSet dsResultado = new DataSet();
            DataTable dtMovimentacoes;
            DataTable dtInventarioTmp;
            SqlDataAdapter da;
            DataSet ds;

            #region QuerySql
            var querySql = @"SELECT
	  CASE WHEN tbMovimentacao.IDMovimentacao IS NOT NULL
	  THEN 'Movimentação'
	  ELSE CASE WHEN tbPedido.IDPedido IS NOT NULL
            THEN 'Venda'
            ELSE 'Saída Avulsa'
            END
	  END as 'Tipo'
	, tbEntradaSaida.Data
	, tbProduto.IDProduto
	, tbProduto.Nome AS 'Produto'
    , CASE WHEN tbMovimentacao.IDMovimentacao IS NOT NULL
	  THEN 'Movimentação ' + tbMovimentacao.Descricao
	  ELSE CASE WHEN tbPedido.IDPedido IS NOT NULL
            THEN 'Pedido ' + cast(tbpedido.idpedido as varchar)
            ELSE '-'
            END
	  END as 'Detalhes'
	, tbUnidade.IDUnidade
	, tbUnidade.Simbolo
	, tbUnidade.Nome AS 'Unidade'
	, tbEntradaSaida.Quantidade
FROM
	tbEntradaSaida        (NOLOCK)
INNER JOIN tbProduto      (NOLOCK) on tbProduto.IDProduto        = tbEntradaSaida.IDProduto
INNER JOIN tbUnidade      (NOLOCK) on tbProduto.IDUnidade        = tbUnidade.IDUnidade
 LEFT JOIN tbPedido       (NOLOCK) on tbPedido.GUIDMovimentacao  = tbEntradaSaida.GUID_Origem
 LEFT JOIN tbMovimentacao (NOLOCK) on tbMovimentacao.GUID        = tbEntradaSaida.GUID_Origem
WHERE
	(tbMovimentacao.IDMovimentacao IS NULL OR (tbMovimentacao.Processado = 1 AND tbMovimentacao.Excluido = 0))
    AND tbEntradaSaida.IDProduto = @idProduto
    AND cast(tbEntradaSaida.Data as date) = cast(@data as date)
ORDER BY tbEntradaSaida.Data";
            #endregion

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (ds = new DataSet())
            {
                da.SelectCommand.Parameters.AddWithValue("@idProduto", idProduto);
                da.SelectCommand.Parameters.AddWithValue("@data", dataHistorico.Date);
                da.Fill(ds);
                dtMovimentacoes = ds.Tables[0];
            }

            dsResultado = new DataSet();

            var dtMovCopy = dtMovimentacoes.Copy();
            dtMovCopy.TableName = "Movimentacoes";
            dtMovimentacoes.Dispose();

            dsResultado.Tables.Add(dtMovCopy);

            var idInventario = InventarioDAL.UltimoInventarioProcessado(dataHistorico.Date);

            if (idInventario.HasValue)
            {

                #region QuerySql
                querySql = @"SELECT 'Ajuste de Inventário'    AS 'Tipo'
       , tbinventario.Data
       , tbinventarioprodutos.IDProduto
       , tbproduto.Nome AS 'Produto'
       , tbInventario.Descricao AS 'Detalhes'
       , tbunidade.IDunidade
       , tbunidade.Simbolo
       , tbunidade.Nome AS 'Unidade'
       , tbinventarioprodutos.Quantidade
FROM   tbinventarioprodutos (nolock)
       INNER JOIN tbinventario (nolock)
               ON tbinventario.Idinventario = tbinventarioprodutos.Idinventario
                  AND tbinventario.Excluido = 0
                  AND tbinventario.Processado = 1
       INNER JOIN tbproduto (nolock)
               ON tbproduto.Idproduto = tbinventarioprodutos.Idproduto
       INNER JOIN tbunidade (nolock)
               ON tbunidade.Idunidade = tbinventarioprodutos.Idunidade
WHERE  
    tbproduto.IDproduto = @idProduto
    AND tbInventario.IDInventario = @idInventario
ORDER  BY tbinventario.Data";
                #endregion

                using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
                using (ds = new DataSet())
                {
                    da.SelectCommand.Parameters.AddWithValue("@idProduto", idProduto);
                    da.SelectCommand.Parameters.AddWithValue("@idInventario", idInventario.Value);

                    da.Fill(ds);
                    dtInventarioTmp = ds.Tables[0];
                }
                var dtInvCopy = dtInventarioTmp.Copy();
                dtInvCopy.TableName = "Inventario";
                dtInventarioTmp.Dispose();

                dsResultado.Tables.Add(dtInvCopy);
            }

            return dsResultado;
        }

        public static DataTable RelatorioInventario(int idInventario, DateTime dataInicio)
        {
            SqlDataAdapter da;
            DataSet ds;

            #region querySql
            var querySql = @"select * from tbInventario (nolock) where IDInventario = @idInventario";
            #endregion

            InventarioInformation inventario = null;

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (ds = new DataSet())
            {
                da.SelectCommand.Parameters.AddWithValue("@idInventario", idInventario);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                    inventario = PreencherInventario(ds.Tables[0].Rows[0]);
            }

            var dataFim = inventario.Data.Value;

            #region querySql
            querySql = @"select 
	  tbProduto.IDProduto
	, sum(tbEntradaSaida.Quantidade) as quantidade
from 
tbEntradaSaida (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
left join tbPedido (nolock) on tbPedido.GUIDMovimentacao = tbEntradaSaida.GUID_Origem
left join tbMovimentacao (nolock) on tbMovimentacao.GUID = tbEntradaSaida.GUID_Origem
where
tbPedido.IDPedido is null
and tbMovimentacao.IDMovimentacao is null
and tbEntradaSaida.Data between @dataInicio and @dataFim

group by tbProduto.IDProduto";
            #endregion

            var saidasAvulsas = BuscarValores(querySql, dataInicio, dataFim);

            #region querySql
            querySql = @"select
	  tbProduto.IDProduto
	, sum(tbEntradaSaida.Quantidade) as quantidade
from 
tbEntradaSaida (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
left join tbPedido (nolock) on tbPedido.GUIDMovimentacao = tbEntradaSaida.GUID_Origem
where
tbPedido.idpedido is not null
and tbpedido.IDStatusPedido = 40
and tbEntradaSaida.Data between @dataInicio and @dataFim

group by tbProduto.IDProduto";
            #endregion

            var vendas = BuscarValores(querySql, dataInicio, dataFim);

            #region querySql
            querySql = @"select 
	  tbProduto.IDProduto
	, sum(tbEntradaSaida.Quantidade) as quantidade
from 
tbEntradaSaida (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
left join tbMovimentacao (nolock) on tbMovimentacao.GUID = tbEntradaSaida.GUID_Origem
left join tbTipoMovimentacao (nolock) on tbTipoMovimentacao.IDTipoMovimentacao = tbMovimentacao.IDTipoMovimentacao
where
tbTipoMovimentacao.Tipo = '+'
and tbMovimentacao.Processado = 1 and tbMovimentacao.Excluido = 0
and tbMovimentacao.DataMovimentacao between @dataInicio and @dataFim

group by tbProduto.IDProduto";
            #endregion

            var movimentacoesEntrada = BuscarValores(querySql, dataInicio, dataFim);

            #region querySql
            querySql = @"select 
	  tbProduto.IDProduto
	, sum(tbEntradaSaida.Quantidade) as quantidade
from 
tbEntradaSaida (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
left join tbMovimentacao (nolock) on tbMovimentacao.GUID = tbEntradaSaida.GUID_Origem
left join tbTipoMovimentacao (nolock) on tbTipoMovimentacao.IDTipoMovimentacao = tbMovimentacao.IDTipoMovimentacao
where
tbTipoMovimentacao.Tipo = '-'
and tbMovimentacao.Processado = 1 and tbMovimentacao.Excluido = 0
and tbMovimentacao.DataMovimentacao between @dataInicio and @dataFim

group by tbProduto.IDProduto";
            #endregion

            var movimentacoesSaida = BuscarValores(querySql, dataInicio, dataFim);

            var produtos = ProdutoDAL.ListarPorData(SqlDateTime.MinValue.Value, true, true);

            DataTable dtResultado = new DataTable("RelatorioInventario");
            dtResultado.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("IDProduto", typeof(int)),
                new DataColumn("Produto", typeof(string)),
                new DataColumn("QuantidadeAnterior", typeof(decimal)),
                new DataColumn("QuantidadeMovimentacaoEntrada", typeof(decimal)),
                new DataColumn("QuantidadeVenda", typeof(decimal)),
                new DataColumn("QuantidadeMovimentacaoSaida", typeof(decimal)),
                new DataColumn("QuantidadeSaidaAvulsa", typeof(decimal)),
                new DataColumn("QuantidadeInventarioAtual", typeof(decimal)),
            });

            foreach (var produto in produtos)
            {
                var newRow = dtResultado.NewRow();

                var idProduto = produto.IDProduto.Value;

                newRow.SetField("IDProduto", idProduto);
                newRow.SetField("Produto", produto.Nome);
                newRow.SetField("QuantidadeAnterior", 0m);

                newRow.SetField("QuantidadeVenda", vendas.Quantidade(idProduto));
                newRow.SetField("QuantidadeMovimentacaoEntrada", movimentacoesEntrada.Quantidade(idProduto));
                newRow.SetField("QuantidadeMovimentacaoSaida", movimentacoesSaida.Quantidade(idProduto));
                newRow.SetField("QuantidadeSaidaAvulsa", saidasAvulsas.Quantidade(idProduto));

                newRow.SetField("QuantidadeInventarioAtual", 0m);

                dtResultado.Rows.Add(newRow);
            }

            return dtResultado;
        }

        private static InventarioInformation PreencherInventario(DataRow row)
        {
            var inventario = new InventarioInformation
            {
                IDInventario = Convert.ToInt32(row["IDInventario"]),
                Data = Convert.ToDateTime(row["Data"]),
                Descricao = Convert.ToString(row["Descricao"]),
                Excluido = Convert.ToBoolean(row["Excluido"]),
                GUID = Convert.ToString(row["GUID"]),
                InventarioProdutos = new List<InventarioProdutosInformation>(),
                Processado = Convert.ToBoolean(row["Processado"])
            };

            return inventario;
        }

        private static List<Valor> BuscarValores(string querySql, DateTime dataInicio, DateTime dataFim)
        {
            SqlDataAdapter da;
            DataSet ds;
            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (ds = new DataSet())
            {
                var paramInicio = da.SelectCommand.Parameters.Add("@dataInicio", SqlDbType.DateTime2);
                var paramFim = da.SelectCommand.Parameters.Add("@dataFim", SqlDbType.DateTime2);

                paramInicio.Value = dataInicio;
                paramFim.Value = dataFim;

                da.Fill(ds);

                return Valor.PreencherValores(ds.Tables[0]);
            }
        }

        private static decimal Quantidade(this List<Valor> lista, int idProduto)
        {
            var valor = lista.Where(v => v.IDProduto == idProduto).FirstOrDefault();
            if (valor != null)
                return valor.Quantidade;
            return 0;
        }

        public static void CopiarCampos<T>(this DataRow linhaPara, DataRow linhaDe, params string[] campos)
        {
            foreach (var campo in campos)
                linhaPara.SetField(campo, linhaDe.Field<T>(campo));
        }

        internal class Valor
        {
            internal static List<Valor> PreencherValores(DataTable dt)
            {
                var resultado = new List<Valor>();
                foreach (DataRow dr in dt.AsEnumerable())
                {
                    resultado.Add(new Valor
                    {
                        IDProduto = Convert.ToInt32(dr["IDProduto"]),
                        Quantidade = Convert.ToDecimal(dr["Quantidade"])
                    });
                }
                return resultado;
            }

            private Valor()
            {

            }

            public int IDProduto { get; set; }
            public decimal Quantidade { get; set; }
        }
    }
}
