using a7D.PDV.Model;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    /*
    public static class AreaImpressaoDAL
    {
        public static int QuantidadeAreaSAT(int? idExcluido)
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    if (idExcluido.HasValue)
                    {
                        cmd.CommandText = "SELECT count(*) as c FROM tbAreaImpressao (nolock) WHERE IDTipoAreaImpressao = 3 AND IDAreaImpressao != @idExcluido";
                        cmd.Parameters.AddWithValue("@idExcluido", idExcluido);
                    }
                    else
                        cmd.CommandText = "SELECT count(*) as c FROM tbAreaImpressao (nolock) WHERE IDTipoAreaImpressao = 3";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return 0;
                        return reader.Def<int>("c");
                    }
                }
            }
        }

        public static AreaImpressaoInformation AreaImpressaoSAT()
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT TOP 1 tbAreaImpressao.*, tbTipoAreaImpressao.Nome as NomeTipo FROM tbAreaImpressao (nolock) 
                                            INNER JOIN tbTipoAreaImpressao (nolock) ON tbTipoAreaImpressao.IDTipoAreaImpressao = tbAreaImpressao.IDTipoAreaImpressao
											WHERE tbTipoAreaImpressao.IDTipoAreaImpressao = 3";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AreaImpressaoInformation
                            {
                                IDAreaImpressao = reader.Val<int>("IDAreaImpressao"),
                                Nome = reader.Ref<string>("Nome"),
                                NomeImpressora = reader.Ref<string>("NomeImpressora"),
                                TipoAreaImpressao = new TipoAreaImpressaoInformation
                                {
                                    IDTipoAreaImpressao = reader.Val<int>("IDTipoAreaImpressao"),
                                    Nome = reader.Ref<string>("NomeTipo")
                                }
                            };
                        }
                        else
                            return null;
                    }
                }
            }
        }
    }
    */
}
