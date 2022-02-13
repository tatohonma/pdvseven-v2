using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public class ComandaDAL
    {
      

        public static DataTable ListarAbertas(string texto, float codigo)
        {
           var querySql = @"
                SELECT 
	                p.GUIDIdentificacao,
	                co.IDStatusComanda,
	                co.Numero,
	                cl.NomeCompleto,
                    cl.Telefone1Numero,
                    cl.Bloqueado
                FROM 
	                tbPedido p (NOLOCK) 
	                INNER JOIN tbComanda co (NOLOCK) ON p.GUIDIdentificacao=co.GUIDIdentificacao
	                INNER JOIN tbCliente cl (NOLOCK) ON p.IDCliente=cl.IDCliente
                WHERE
                    (p.IDStatusPedido=10) AND 
                    ((@codigo=0 AND (@texto='' OR CAST(cl.Telefone1Numero as VARCHAR) LIKE @texto OR cl.NomeCompleto LIKE @texto))
                  OR (@codigo>0 AND (co.Numero=@codigo OR co.Codigo=@codigo)))
                ORDER BY co.Numero";

            DataSet ds = new DataSet();
            var da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@texto", "%" + texto + "%");
            da.SelectCommand.Parameters.AddWithValue("@codigo", codigo);
            da.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable ListarTudoOuCodigo(float codigo)
        {
            var querySql = @"
                SELECT *
                FROM tbComanda (NOLOCK)
                WHERE
                  @codigo=0 OR 
                  (Numero=@codigo OR Codigo=@codigo)
                ORDER BY Numero";

            DataSet ds = new DataSet();
            var da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@codigo", codigo);
            da.Fill(ds);
            return ds.Tables[0];
        }

        public static void DefinirCaixaEmPedidosComandaFechados(int idCaixa)
        {
            using (var cn = new SqlConnection(DB.ConnectionString))
            {
                cn.Open();
                string querySql = $"UPDATE tbPedido SET idCaixa={idCaixa} WHERE IDTipoPedido=20 AND IDStatusPedido=40 AND idCaixa IS NULL";
                var cmd = new SqlCommand(querySql, cn);
                var result = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
