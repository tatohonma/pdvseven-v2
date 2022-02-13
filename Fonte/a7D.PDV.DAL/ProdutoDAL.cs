using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace a7D.PDV.DAL
{
    public class ProdutoDAL
    {
        public static List<AreaImpressaoInformation> ListarAreaImpressao(Int32 idProduto)
        {
            List<AreaImpressaoInformation> list = new List<AreaImpressaoInformation>();
            AreaImpressaoInformation obj;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
SELECT 
	ai.* 
FROM 
	tbMapAreaImpressaoProduto map (NOLOCK) 
	INNER JOIN tbAreaImpressao ai (NOLOCK) ON ai.IDAreaImpressao=map.IDAreaImpressao
WHERE
    map.IDProduto=@idProduto
ORDER BY
	ai.Nome
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idProduto", idProduto);

            da.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                obj = new AreaImpressaoInformation();
                obj.IDAreaImpressao = Convert.ToInt32(dr["IDAreaImpressao"]);
                obj.Nome = Convert.ToString(dr["Nome"]);

                list.Add(obj);
            }

            return list;
        }

        public static List<ProdutoInformation> ListarPorData(DateTime? dataAlteracao = null, bool? ativos = null, bool? controlarEstoque = null, bool? alteradoOuDisponibilidade = null)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            var wheres = new List<string>();
            wheres.Add("Excluido=0");

            if (ativos != null && ativos == true)
                wheres.Add("Ativo=1");
            else if (ativos != null && ativos == false)
                wheres.Add("Ativo=0");

            if (controlarEstoque != null && controlarEstoque == true)
                wheres.Add("ControlarEstoque=1");
            else if (controlarEstoque != null && controlarEstoque == false)
                wheres.Add("ControlarEstoque=0");

            if (alteradoOuDisponibilidade == true)
                wheres.Add("(dtUltimaAlteracao>@data OR DtAlteracaoDisponibilidade>@data)");
            else if (dataAlteracao != null)
                wheres.Add("dtUltimaAlteracao>@data");

            string querySql = $"SELECT * FROM tbProduto WHERE " + string.Join(" AND ", wheres);

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            if (dataAlteracao != null)
                da.SelectCommand.Parameters.AddWithValue("@data", dataAlteracao);
            
            da.Fill(ds);
            dt = ds.Tables[0];
            List<ProdutoInformation> r = ConverterDTemLista(dt);

            return r;
        }

        public static List<ProdutoInformation> ListarExcluidos(DateTime data)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string querySql = @"SELECT * FROM tbProduto WHERE [Excluido] = 1 AND dtUltimaAlteracao > @data";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@data", data);

            da.Fill(ds);
            dt = ds.Tables[0];
            List<ProdutoInformation> r = ConverterDTemLista(dt);

            return r;
        }

        public static List<ProdutoInformation> ListarPorCategorias(int[] ids, bool listarCreditos)
        {
            if (ids == null || ids.Length == 0)
                return null;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string cIDs = "(";
            foreach (int id in ids)
                cIDs += id + ",";

            cIDs = cIDs.Substring(0, cIDs.Length - 1);

            cIDs += ")";

            string querySql = @"SELECT p.* FROM tbProduto p (NOLOCK)
INNER JOIN tbProdutoCategoriaProduto c (NOLOCK) ON p.IDProduto = c.IDProduto 
WHERE p.IDProduto>4 AND (p.IDTipoProduto=10" + (listarCreditos ? " OR p.IDTipoProduto=50" : "") + @")
AND p.Excluido=0 AND p.Ativo=1 
AND c.IDCategoriaProduto IN " + cIDs + " ORDER BY p.Nome";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];
            List<ProdutoInformation> r = ConverterDTemLista(dt);

            return r;
        }

        public static List<ProdutoInformation> ListarSemCategorias(bool listarCreditos)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string querySql = @"SELECT p.* FROM tbProduto p (NOLOCK)
WHERE p.IDProduto>4 AND (p.IDTipoProduto=10" + (listarCreditos ? " OR p.IDTipoProduto=50" : "") + @")
AND p.Excluido=0 AND p.Ativo=1 
AND p.IDProduto not in (SELECT DISTINCT IDProduto FROM tbProdutoCategoriaProduto (NOLOCK))";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];
            List<ProdutoInformation> r = ConverterDTemLista(dt);

            return r;
        }


        private static List<ProdutoInformation> ConverterDTemLista(DataTable dt)
        {
            List<ProdutoInformation> r = new List<ProdutoInformation>();


            foreach (DataRow dr in dt.Rows)
            {
                var produto = new ProdutoInformation();
                produto.IDProduto = Convert.ToInt32(dr["IDProduto"]);
                produto.TipoProduto = new TipoProdutoInformation { IDTipoProduto = Convert.ToInt32(dr["IDTipoProduto"]) };
                produto.Codigo = Convert.ToString(dr["Codigo"]);
                produto.CodigoERP = Convert.ToString(dr["CodigoERP"]);
                produto.Nome = Convert.ToString(dr["Nome"]);
                produto.Descricao = Convert.ToString(dr["Descricao"]);
                produto.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
                produto.ValorUnitario2 = dr["ValorUnitario2"] != DBNull.Value ? Convert.ToDecimal(dr["ValorUnitario2"]) : (decimal?)null;
                produto.ValorUnitario3 = dr["ValorUnitario3"] != DBNull.Value ? Convert.ToDecimal(dr["ValorUnitario3"]) : (decimal?)null;
                produto.CodigoAliquota = Convert.ToString(dr["CodigoAliquota"]);
                produto.Ativo = Convert.ToBoolean(dr["Ativo"]);
                produto.AssistenteModificacoes = dr["AssistenteModificacoes"] != DBNull.Value ? Convert.ToBoolean(dr["AssistenteModificacoes"]) : (bool?)false;
                produto.Disponibilidade = Convert.ToBoolean(dr["Disponibilidade"]);
                produto.DtAlteracaoDisponibilidade = Convert.ToDateTime(dr["DtAlteracaoDisponibilidade"]);
                produto.DtUltimaAlteracao = Convert.ToDateTime(dr["DtUltimaAlteracao"]);
                produto.Excluido = Convert.ToBoolean(dr["Excluido"]);
                produto.ClassificacaoFiscal = dr["IDClassificacaoFiscal"] != DBNull.Value ? new ClassificacaoFiscalInformation { IDClassificacaoFiscal = Convert.ToInt32(dr["IDClassificacaoFiscal"]) } : null;
                produto.Unidade = dr["IDUnidade"] != DBNull.Value ? new UnidadeInformation { IDUnidade = Convert.ToInt32(dr["IDUnidade"]) } : null;
                produto.cEAN = Convert.ToString(dr["cEAN"]);
                produto.ControlarEstoque = Convert.ToBoolean(dr["ControlarEstoque"]);
                produto.UtilizarBalanca = Convert.ToBoolean(dr["UtilizarBalanca"]);
                r.Add(produto);
            }

            return r;
        }

        public static int? BuscarPorEAN(string ean)
        {
            #region query
            string querySql = @"
                SELECT TOP 1 IDProduto FROM tbProduto where cEAN is not null and cEAN != '' and cEAN = @ean and excluido = 0 and ativo = 1
            ";
            #endregion

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(querySql, conn))
                {
                    cmd.Parameters.AddWithValue("@ean", ean);
                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (r.Read())
                        {
                            return r.Def<int>(r.GetOrdinal("IDProduto"));
                        }
                    }
                }
            }

            return null;
        }

        public static List<int> ListarSemCategoria()
        {
            SqlDataAdapter da;

            string querySql = @"select IDProduto
from tbProduto (NOLOCK)
where IDProduto not in (select distinct IDProduto from tbProdutoCategoriaProduto (NOLOCK))";

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (var ds = new DataSet())
            {

                da.Fill(ds);
                return ds.Tables[0].AsEnumerable().Select(dr => dr.Field<int>("IDProduto")).ToList();
            }
        }

        public static int ContarSemCategoria(bool listarCreditos)
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select count(1)
FROM tbProduto (NOLOCK)
WHERE IDProduto>4 
and (tbProduto.IDTipoProduto=10" + (listarCreditos ? " OR tbProduto.IDTipoProduto=50" : "") + @")
AND Excluido=0 AND Ativo=1 
AND IDProduto not in (select distinct IDProduto from tbProdutoCategoriaProduto (NOLOCK))";
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public static bool UtilizaBalanca(int idProduto)
        {
            SqlDataAdapter da;

            string querySql = @"select UtilizarBalanca
from tbProduto (NOLOCK)
where IDProduto = @idProduto";

            using (da = new SqlDataAdapter(querySql, DB.ConnectionString))
            using (var ds = new DataSet())
            {
                da.SelectCommand.Parameters.AddWithValue("@idProduto", idProduto);
                da.Fill(ds);
                if (ds.Tables.Count == 0)
                    return false;
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                return ds.Tables[0].Rows[0].Field<bool>("UtilizarBalanca");
            }
        }

        public static List<ProdutoInformation> ListarNaoExcluidos()
        {
            string querySql = $"SELECT * FROM tbProduto (NOLOCK) WHERE Excluido = 0";

            using (var da = new SqlDataAdapter(querySql, DB.ConnectionString))
            {
                using (var ds = new DataSet())
                {
                    da.Fill(ds);
                    List<ProdutoInformation> r = ConverterDTemLista(ds.Tables[0]);
                    return r;
                }
            }
        }

        public static bool ExistemProdutosComEstoqueControlado()
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select 1 where exists(SELECT 1 FROM tbProduto (NOLOCK) WHERE ControlarEstoque = 1)";
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }

        public static int? BuscarPorCodigoIntegracao(string codigo) // int tipo, 
        {
            var campo = "Codigo";
            var querySql = string.Format("SELECT TOP 1 IDProduto FROM TbProduto (nolock) where excluido = 0 and ativo = 1 and {0} is not null and {0} != '' and {0} = @codigo", campo);

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(querySql, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (r.Read())
                        {
                            return r.Def<int>(r.GetOrdinal("IDProduto"));
                        }
                    }
                }
            }

            return null;
        }

        public static bool ProdutosSemAreaProducao()
        {
            var query = @"SELECT count(1) FROM tbProduto (NOLOCK)
LEFT JOIN tbMapAreaImpressaoProduto (NOLOCK) on tbMapAreaImpressaoProduto.idproduto = tbproduto.idproduto
where tbMapAreaImpressaoProduto.IDMapAreaImpressaoProduto is null
and tbProduto.IDTipoProduto = 10
and tbProduto.IDProduto > 4";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (r.Read())
                    {
                        return r.Val<int>(0) > 0;
                    }
                    return false;
                }
            }
        }
    }
}
