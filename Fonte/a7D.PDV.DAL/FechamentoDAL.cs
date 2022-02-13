using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public static class FechamentoDAL
    {
        public static int UltimoFechamento()
        {
            var query = @"select top 1 IDFechamento as id From tbfechamento order by IDFechamento desc";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.Def<int>(reader.GetOrdinal("id"));
                    }
                }
            }
            return -1;
        }
    }
}
