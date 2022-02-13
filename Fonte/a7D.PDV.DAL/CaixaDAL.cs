using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public class CaixaDAL
    {
        public static Decimal TotalMovimentacao(Int32 idCaixa, Int32 idTipoPagamento)
        {
            Decimal result = 0;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                ISNULL(SUM(Valor), 0) total
                FROM 
	                tbPedido (nolock) p
	                LEFT JOIN tbPedidoPagamento (nolock) pp ON pp.IDPedido=p.IDPedido
                WHERE 
	                p.IDCaixa=@IDCaixa 
	                AND 
	                IDTipoPagamento=@IDTipoPagamento
                    AND
                    pp.Excluido=0
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@IDCaixa", idCaixa);
            da.SelectCommand.Parameters.AddWithValue("@IDTipoPagamento", idTipoPagamento);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
                result = Convert.ToDecimal(dt.Rows[0]["total"]);

            return result;
        }

        public static IEnumerable<CaixaInformation> ListarAbertos()
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM tbCaixa (nolock) where DtFechamento IS NULL"))
                {
                    cmd.Connection = conn;
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.HasRows)
                            yield break;
                        while (dr.Read())
                        {
                            yield return ObterCaixa(dr);
                        }
                    }
                }
            }
        }

        private static CaixaInformation ObterCaixa(SqlDataReader dr)
        {
            var caixa = new CaixaInformation
            {
                IDCaixa = dr.Def<int>("IDCaixa"),
                DtAbertura = dr.Val<DateTime>("DtAbertura"),
                DtFechamento = dr.Val<DateTime>("DtFechamento"),
                SincERP = dr.Val<bool>("SincERP")
            };

            var idPdv = dr.Val<int>("IDPDV");
            var idUsuario = dr.Val<int>("IDUsuario");

            if (idPdv.HasValue)
                caixa.PDV = new PDVInformation { IDPDV = idPdv.Value };

            if (idUsuario.HasValue)
                caixa.Usuario = new UsuarioInformation { IDUsuario = idUsuario.Value };
            return caixa;
        }

        public static IEnumerable<CaixaInformation> CaixasSemFechamento()
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM tbCaixa(nolock) WHERE IDFechamento is null"))
                {
                    cmd.Connection = conn;
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.HasRows)
                            yield break;
                        while (dr.Read())
                        {
                            yield return ObterCaixa(dr);
                        }
                    }
                }
            }
        }
    }
}
