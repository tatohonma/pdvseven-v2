using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace a7D.PDV.DAL
{
    public class Geral
    {
        public static void ExecutarQuery(string query)
        {
            SqlConnection conn = new SqlConnection(DB.ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteReader();

            conn.Close();
        }
    }
}
