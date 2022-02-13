using System;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public static class InventarioDAL
    {
        public static DataTable RelatorioInventario(int idInventario)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt;

            var querySql = @"SELECT
      tbProduto.IDProduto
	, tbProduto.Nome as 'Produto'
    , tbUnidade.IDUnidade
	, tbUnidade.Nome as 'Unidade'
	, tbInventarioProdutos.QuantidadeAnterior as 'Quantidade Anterior'
	, tbInventarioProdutos.Quantidade as 'Quantidade no Inventário'
	, tbInventarioProdutos.Quantidade - tbInventarioProdutos.QuantidadeAnterior as 'Diferença'
FROM
	tbInventario (nolock)
	INNER JOIN tbInventarioProdutos (nolock) on tbInventarioProdutos.IDInventario = tbInventario.IDInventario
	INNER JOIN tbProduto (nolock) on tbProduto.IDProduto = tbInventarioProdutos.IDProduto
	INNER JOIN tbUnidade (nolock) on tbUnidade.IDUnidade = tbInventarioProdutos.IDUnidade
WHERE
	tbInventario.IDInventario = @idInventario";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idInventario", idInventario);

            da.Fill(ds);

            dt = ds.Tables[0];

            return dt;
        }

        public static int? UltimoInventarioProcessado(DateTime data)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            #region querySql

            var querySql = @"SELECT TOP 1 
	IDInventario 
FROM tbInventario (nolock)
WHERE processado = 1
AND Excluido = 0
AND CONVERT(DATE, Data) = CONVERT(DATE, @data)
ORDER BY Data DESC";

            #endregion

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            var param = da.SelectCommand.Parameters.Add("@data", SqlDbType.DateTime2);
            param.Value = data.Date;

            da.Fill(ds);

            using (var dt = ds.Tables[0])
            {
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0].Field<int>("IDInventario");
            }
            return null;
        }
    }
}
