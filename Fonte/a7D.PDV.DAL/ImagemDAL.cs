using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public class ImagemDAL
    {
        public static byte[] CarregarDados(int idImagem)
        {
            SqlDataAdapter da;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string querySql;

            querySql = @"
                SELECT 
	                 Dados
                FROM 
	                tbImagem (nolock)
                WHERE
	                IDImagem = @IDImagem
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@IDImagem", idImagem);

            da.Fill(ds);
            dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0].Field<byte[]>("Dados");
            }
            return null;
        }
    }
}
