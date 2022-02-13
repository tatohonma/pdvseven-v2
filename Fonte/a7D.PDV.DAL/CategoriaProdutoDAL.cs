using a7D.PDV.Model.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public static class CategoriaProdutoDAL
    {

        public static IEnumerable<CategoriaAtiva> CategoriasAtivas()
        {
            var query = @"SELECT
	tbCategoriaProduto.IDCategoriaProduto,
	tbCategoriaProduto.Nome,
	sum(case tbProduto.Disponibilidade when 1 then 1 else 0 end) as ativos,
	sum(case tbProduto.Disponibilidade when 1 then 0 else 1 end) as inativos
FROM tbCategoriaProduto (NOLOCK)
LEFT JOIN tbProdutoCategoriaProduto (NOLOCK) ON tbProdutoCategoriaProduto.IDCategoriaProduto = tbCategoriaProduto.IDCategoriaProduto
LEFT JOIN tbProduto (NOLOCK) ON tbProduto.IDProduto = tbProdutoCategoriaProduto.IDProduto 
WHERE
tbProduto.Ativo = 1 and tbProduto.Excluido = 0
GROUP BY tbCategoriaProduto.IDCategoriaProduto,
tbCategoriaProduto.Nome
ORDER BY 2 ASC";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new CategoriaAtiva
                            {
                                IDCategoriaProduto = reader.Def<int>("IDCategoriaProduto"),
                                Nome = reader.Def<string>("Nome"),
                                Ativos = reader.Def<int>("ativos"),
                                Inativos = reader.Def<int>("inativos")
                            };
                        }
                    }
                }
            }
        }

        public static void AlterarDisponibilidade(int idCategoria, bool disponivel)
        {
            var query = @"update tbproduto set Disponibilidade = @disponivel
from tbProdutoCategoriaProduto pcp
inner join tbProduto on tbProduto.IDProduto = pcp.IDProduto
where pcp.IDCategoriaProduto = @idCategoria
and tbProduto.Ativo = 1 and tbProduto.Excluido = 0";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
                    cmd.Parameters.AddWithValue("@disponivel", disponivel);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
