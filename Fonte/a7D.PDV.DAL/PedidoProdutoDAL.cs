using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.DAL
{
    public class PedidoProdutoDAL
    {
        public static List<PedidoProdutoInformation> ListarPorPedidoAgrupado(Int32 idPedido)
        {
            List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>();
            PedidoProdutoInformation obj;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
                     p.IDProduto
	                ,p.Nome
	                ,SUM(pp.Quantidade) as 'Quantidade'
                FROM 
	                tbPedidoProduto pp (NOLOCK)
	                INNER JOIN tbProduto p (NOLOCK) ON p.idProduto=pp.idProduto
                WHERE
                    pp.IDPedidoProduto_pai IS NULL
					AND
	                pp.Cancelado <> 1
                    AND
                    pp.IDPedido=@idPedido
                GROUP BY
                     p.IDProduto
	                ,p.Nome
                ORDER BY
	                p.Nome
            "; // p.IDTipoProduto IN (10, 30)

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idPedido", idPedido);

            da.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                obj = new PedidoProdutoInformation();
                obj.Produto = new ProdutoInformation();
                obj.Produto.Nome = Convert.ToString(dr["Nome"]);
                obj.Produto.IDProduto = Convert.ToInt32(dr["IDProduto"]);
                obj.Quantidade = Convert.ToDecimal(dr["Quantidade"]);
                //obj.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);

                list.Add(obj);
            }

            return list;
        }

        public static List<PedidoProdutoInformation> ListarPorPedido(int idPedido)
        {
            List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>();

            String querySql = @"
                SELECT 
	                pp.*
	                ,pdv.Nome as pdv
	                ,p.Nome as produto
                    ,p.IDTipoProduto
                    ,p.CodigoERP
	                ,u.Nome as usuario
                FROM 
	                tbPedidoProduto pp (NOLOCK)
	                INNER JOIN tbPDV pdv (NOLOCK) ON pdv.IDPDV=pp.IDPDV
	                INNER JOIN tbProduto p (NOLOCK) ON p.IDProduto=pp.IDProduto
	                INNER JOIN tbUsuario u (NOLOCK) ON u.IDUsuario=pp.IDUsuario
                WHERE 
	                idPedido=@idPedido
                ORDER BY
                    pp.idPedidoProduto
            ";

            using (var da = new SqlDataAdapter(querySql, DB.ConnectionString))
            {
                da.SelectCommand.Parameters.AddWithValue("@idPedido", idPedido);
                using (var ds = new DataSet())
                {
                    da.Fill(ds);
                    var dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        var obj = new PedidoProdutoInformation
                        {
                            IDPedidoProduto = Convert.ToInt32(dr["idPedidoProduto"]),
                            Quantidade = Convert.ToDecimal(dr["quantidade"]),
                            ValorUnitario = Convert.ToDecimal(dr["valorUnitario"]),
                            ValorDesconto = dr["ValorDesconto"] == DBNull.Value ? default(decimal?) : Convert.ToDecimal(dr["ValorDesconto"]),
                            DtInclusao = Convert.ToDateTime(dr["dtInclusao"]),
                            Cancelado = Convert.ToBoolean(dr["cancelado"]),
                            CodigoAliquota = Convert.ToString(dr["codigoAliquota"]),
                            Notas = Convert.ToString(dr["notas"]),
                            Viagem = dr["viagem"] == DBNull.Value ? false : Convert.ToBoolean(dr["viagem"]),

                            Pedido = new PedidoInformation
                            {
                                IDPedido = Convert.ToInt32(dr["idPedido"])
                            },

                            Usuario = new UsuarioInformation
                            {
                                IDUsuario = Convert.ToInt32(dr["idUsuario"]),
                                Nome = Convert.ToString(dr["usuario"])
                            },

                            Produto = new ProdutoInformation
                            {
                                IDProduto = Convert.ToInt32(dr["idProduto"]),
                                Nome = Convert.ToString(dr["produto"]),
                                CodigoERP = Convert.ToString(dr["CodigoERP"]),
                                TipoProduto = new TipoProdutoInformation()
                                {
                                    IDTipoProduto = Convert.ToInt32(dr["idTipoProduto"])
                                }
                            },

                            PDV = new PDVInformation
                            {
                                IDPDV = Convert.ToInt32(dr["idPDV"]),
                                Nome = Convert.ToString(dr["pdv"])
                            }
                        };

                        if (dr["idPedidoProduto_pai"] != DBNull.Value)
                        {
                            obj.PedidoProdutoPai = new PedidoProdutoInformation();
                            obj.PedidoProdutoPai.IDPedidoProduto = Convert.ToInt32(dr["idPedidoProduto_pai"]);
                        }

                        list.Add(obj);
                    }

                    return list;
                }
            }
        }

        public static List<PedidoProdutoInformation> ListarModificacoes(int idPedidoProduto)
        {
            List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                 pp.*
	                ,pdv.Nome as pdv
	                ,p.Nome as produto
                    ,p.idTipoProduto
	                ,u.Nome as usuario
                FROM 
	                tbPedidoProduto pp (NOLOCK)
	                INNER JOIN tbPDV pdv (NOLOCK) ON pdv.IDPDV=pp.IDPDV
	                INNER JOIN tbProduto p (NOLOCK) ON p.IDProduto=pp.IDProduto
	                INNER JOIN tbUsuario u (NOLOCK) ON u.IDUsuario=pp.IDUsuario
                WHERE 
	                idPedidoProduto_pai=@idPedidoProduto
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idPedidoProduto", idPedidoProduto);

            da.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
                list.Add(BindItem(dr));

            return list;
        }

        private static PedidoProdutoInformation BindItem(DataRow dr)
        {
            return new PedidoProdutoInformation
            {
                IDPedidoProduto = Convert.ToInt32(dr["idPedidoProduto"]),
                Quantidade = Convert.ToDecimal(dr["quantidade"]),
                ValorUnitario = Convert.ToDecimal(dr["valorUnitario"]),
                DtInclusao = Convert.ToDateTime(dr["dtInclusao"]),
                Cancelado = Convert.ToBoolean(dr["cancelado"]),
                CodigoAliquota = Convert.ToString(dr["codigoAliquota"]),
                Pedido = new PedidoInformation()
                {
                    IDPedido = Convert.ToInt32(dr["idPedido"]),
                },
                Produto = new ProdutoInformation
                {
                    IDProduto = Convert.ToInt32(dr["idProduto"]),
                    Nome = Convert.ToString(dr["produto"]),
                    TipoProduto = new TipoProdutoInformation()
                    {
                        IDTipoProduto = Convert.ToInt32(dr["idTipoProduto"])
                    }
                },
                PDV = new PDVInformation
                {
                    IDPDV = Convert.ToInt32(dr["idPDV"]),
                    Nome = Convert.ToString(dr["pdv"])
                },
                Usuario = new UsuarioInformation
                {
                    IDUsuario = Convert.ToInt32(dr["idUsuario"]),
                    Nome = Convert.ToString(dr["usuario"])
                }
            };
        }

        public static bool PedidoDuplicado(string guidControleDuplicidade)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                IDPedidoProduto
                FROM 
	                tbPedidoProduto (NOLOCK)
                WHERE 
	                GUIDControleDuplicidade=@guidControleDuplicidade
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@guidControleDuplicidade", guidControleDuplicidade);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<PedidoProdutoInformation> ListarPorDataPedido(DateTime data)
        {
            List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>();

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
	                 pp.*
	                ,pdv.Nome as pdv
	                ,p.Nome as produto
                    ,p.idTipoProduto
	                ,u.Nome as usuario
                FROM 
	                tbPedidoProduto pp (NOLOCK)
                    INNER JOIN tbPedido pe (NOLOCK) ON pe.IDPedido=pp.IDPedido
	                INNER JOIN tbPDV pdv (NOLOCK) ON pdv.IDPDV=pp.IDPDV
	                INNER JOIN tbProduto p (NOLOCK) ON p.IDProduto=pp.IDProduto
	                INNER JOIN tbUsuario u (NOLOCK) ON u.IDUsuario=pp.IDUsuario
                WHERE 
	                pe.DtPedido BETWEEN @dt1 AND @dt2
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@dt1", data.Date);
            da.SelectCommand.Parameters.AddWithValue("@dt2", data.Date.AddDays(1).AddSeconds(-1));

            da.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
                list.Add(BindItem(dr));

            return list;
        }
    }
}
