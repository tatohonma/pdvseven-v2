using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public static class DB
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"]?.ConnectionString;

        public static int Size()
        {
            // SELECT SUM((size * 8) / 1024) SizeMB FROM sys.master_files WHERE DB_NAME(database_id) = DB_NAME()
            string sql = "exec sp_spaceused";
            using (var cn = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(sql, cn);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                cn.Open();
                da.Fill(ds);
                cn.Close();
                var size = ((string)ds.Tables[0].Rows[0][1]).ToLower().Split(' ');
                size[0] = size[0].Replace(".", ",");
                var mb = (int)double.Parse(size[0]);
                if (size[1] == "kb")
                    return mb / 1024;
                else if (size[1] == "mb")
                    return mb;
                else if (size[1] == "gb")
                    return mb * 1024;
                else
                    return -1;
            }
        }

        public static void ExecutarQuery(string query)
        {
            SqlConnection conn = new SqlConnection(DB.ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteReader();

            conn.Close();
        }

        //public static int ExecuteNonQuery(string sql, params object[] prms)
        //{
        //    int result;
        //    int n = 1;
        //    var cn = new SqlConnection(DB.ConnectionString);
        //    var cmd = new SqlCommand(sql, cn);

        //    foreach (var prm in prms)
        //        cmd.Parameters.AddWithValue("@valor" + (n++), prm);

        //    try
        //    {
        //        cn.Open();
        //        result = cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Add("sql", sql);
        //        foreach (SqlParameter prm in cmd.Parameters)
        //            cmd.Parameters.AddWithValue(prm.ParameterName, prm.Value);

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (cn.State != System.Data.ConnectionState.Closed)
        //            cn.Close();

        //        cn.Dispose();
        //        cmd.Dispose();
        //    }
        //    return result;
        //}

    }
}
