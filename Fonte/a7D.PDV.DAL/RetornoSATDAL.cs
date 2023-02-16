using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.DAL
{
    public class RetornoSATDAL
    {
        public static DataTable ListarPedidosParaCancelamento()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                rs.IDRetornoSAT, 
	                p.DtPedidoFechamento, 
	                p.DocumentoCliente, 
	                p.ValorTotal
                FROM 
	                tbRetornoSAT rs (NOLOCK)
	                INNER JOIN tbPedido p (NOLOCK) ON p.IDRetornoSAT_venda=rs.IDRetornoSAT
                WHERE
	                rs.EEEEE = '06000'
	                AND
	                rs.IDRetornoSAT_cancelamento IS NULL
	                AND
	                p.DtPedidoFechamento>DATEADD(minute, -27, GETDATE())

            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }
    }
}
