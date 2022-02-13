using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public static class OrdemImpressaoDAL
    {
        public static void ExcluirAntigos(int manterPorDias)
        {
            var command = @"delete from tbOrdemImpressao
where DtOrdem < DATEADD(day, @dias, GETDATE())
and EnviadoFilaImpressao = 1";

            using (var da = new SqlDataAdapter())
            {
                using (var conn = new SqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@dias", manterPorDias * -1);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
