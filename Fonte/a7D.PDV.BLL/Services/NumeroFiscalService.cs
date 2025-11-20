using System.Data;
using System.Data.SqlClient;
using a7D.PDV.DAL;

namespace a7D.PDV.BLL.Services
{
    public class NumeroFiscalService
    {
        public static int ObterProximoNumero(string tipoDocumento)
        {
            using (var cn = new SqlConnection(DB.ConnectionString))
            using (var cmd = new SqlCommand("sp_ObterProximoNumeroFiscal", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TipoDocumento", tipoDocumento);
                var pOut = new SqlParameter("@ProximoNumero", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pOut);

                cn.Open();
                cmd.ExecuteNonQuery();

                return (int)pOut.Value;
            }
        }
    }
}
