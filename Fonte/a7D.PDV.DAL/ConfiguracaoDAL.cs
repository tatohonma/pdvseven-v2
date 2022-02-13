using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using a7D.PDV.Model;

namespace a7D.PDV.DAL
{
    public class ConfiguracaoDAL
    {
        public static void Alterar(string chave, string valor)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string querySql = "SELECT * FROM tbConfiguracao WHERE chave = @chave";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@chave", chave);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                querySql = "UPDATE tbConfiguracao SET valor = @valor WHERE chave = @chave";
                da = new SqlDataAdapter();
                using (var connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    da.UpdateCommand = new SqlCommand(querySql, connection);
                    da.UpdateCommand.Parameters.AddWithValue("@chave", chave);
                    da.UpdateCommand.Parameters.AddWithValue("@valor", valor);
                    da.UpdateCommand.ExecuteNonQuery();
                }
            }
            else
            {
                querySql = "INSERT INTO tbConfiguracao (chave, valor) VALUES (@chave, @valor)";
                da = new SqlDataAdapter();
                using (var connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    da.InsertCommand = new SqlCommand(querySql, connection);
                    da.InsertCommand.Parameters.AddWithValue("@chave", chave);
                    da.InsertCommand.Parameters.AddWithValue("@valor", valor);
                    da.InsertCommand.ExecuteNonQuery();
                }
            }
        }

        public static List<ConfiguracaoInformation> Listar()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<ConfiguracaoInformation> configuracoes = new List<ConfiguracaoInformation>();

            string querySql = "SELECT * FROM tbConfiguracao (nolock)";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var chave = row["Chave"];
                var valor = row["Valor"];

                var configuracao = new ConfiguracaoInformation();
                configuracao.Chave = chave != DBNull.Value ? chave.ToString() : string.Empty;
                configuracao.Valor = valor != DBNull.Value ? valor.ToString() : string.Empty;

                configuracoes.Add(configuracao);
            }

            return configuracoes;
        }

        public static bool Existe(string chave)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string querySql = "SELECT * FROM tbConfiguracao (nolock) where Chave = @Chave";
            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@Chave", chave.Trim());
            da.Fill(ds);
            return ds.Tables[0].Rows.Count != 0;

        }

        public static bool Remover(string chave)
        {
            SqlDataAdapter da;
            da = new SqlDataAdapter();
            using (var conexao = new SqlConnection(DB.ConnectionString))
            {
                conexao.Open();
                da.DeleteCommand = new SqlCommand("DELETE FROM tbConfiguracao where Chave = @chave", conexao);
                da.DeleteCommand.Parameters.AddWithValue("@chave", chave);
                return da.DeleteCommand.ExecuteNonQuery() == 1;
            }
        }
    }
}
