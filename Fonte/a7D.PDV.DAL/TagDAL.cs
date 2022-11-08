using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.DAL
{
    public static class TagDAL
    {
        public static List<String> ListaChaves()
        {
            List<String> list = new List<String>();

            SqlDataAdapter da;
            DataTable dt = new DataTable();

            string querySql = "SELECT Chave, COUNT(Chave) qtd FROM tbTag GROUP BY Chave ORDER BY qtd DESC";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {                
                list.Add(dr.Field<string>("Chave"));
            }

            return list;
        }
    }
}
