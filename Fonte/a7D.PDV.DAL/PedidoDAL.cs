using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace a7D.PDV.DAL
{
    public class PedidoDAL
    {
        public static PedidoInformation CarregarUltimoPedido(String guidIdentificacao)
        {
            PedidoInformation pedido = null;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String query = @"SELECT TOP 1 p.IDPedido, p.IDStatusPedido, tp.Nome, p.IDTipoPedido, p.IDCliente, p.GUIDIdentificacao, p.GUIDAgrupamentoPedido, p.IDCaixa
FROM tbPedido p (nolock)
    inner join tbTipoPedido tp (nolock) on tp.IDTipoPedido = p.IDTipoPedido
    WHERE p.GUIDIdentificacao=@guidIdentificacao
    AND p.IDStatusPedido IN (10, 20, 60, 70) 
    ORDER BY p.IDPedido DESC";

            da = new SqlDataAdapter(query, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@guidIdentificacao", guidIdentificacao);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                pedido = new PedidoInformation { IDPedido = Convert.ToInt32(dt.Rows[0]["IDPedido"]) };
                pedido.StatusPedido = new StatusPedidoInformation { IDStatusPedido = Convert.ToInt32(dt.Rows[0]["IDStatusPedido"]) };
                pedido.TipoPedido = new TipoPedidoInformation
                {
                    IDTipoPedido = Convert.ToInt32(dt.Rows[0]["IDTipoPedido"]),
                    Nome = Convert.ToString(dt.Rows[0]["Nome"])
                };

                if (dt.Rows[0]["GUIDIdentificacao"] != DBNull.Value)
                    pedido.GUIDIdentificacao = dt.Rows[0]["GUIDIdentificacao"].ToString();

                if (dt.Rows[0]["GUIDAgrupamentoPedido"] != DBNull.Value)
                    pedido.GUIDAgrupamentoPedido = dt.Rows[0]["GUIDAgrupamentoPedido"].ToString();

                if (dt.Rows[0]["IDCliente"] != DBNull.Value)
                    pedido.Cliente = new ClienteInformation { IDCliente = Convert.ToInt32(dt.Rows[0]["IDCliente"]) };

                if (dt.Rows[0]["IDCaixa"] != DBNull.Value)
                    pedido.Caixa = new CaixaInformation { IDCaixa = Convert.ToInt32(dt.Rows[0]["IDCaixa"]) };
            }

            return pedido;
        }

        public static DataTable ListarDeliveryPendentes()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
                SELECT 
					p.GUIDIdentificacao,
					p.IDPedido,
	                p.GUIDIdentificacao,
                    p.DtPedido,
                    p.DtEnvio,
	                sp.IDStatusPedido,
	                sp.Nome as StatusPedido,
	                cl.NomeCompleto,
                    cl.Telefone1Numero,
                    cl.Bloqueado,
					cl.Endereco,
					cl.EnderecoNumero,
					cl.Complemento,
					cl.Bairro,
					cl.Cidade,
					cl.EnderecoReferencia,
                    cl.Observacao
                FROM 
	                tbPedido p
					INNER JOIN tbStatusPedido sp (NOLOCK) ON sp.IDStatusPedido=p.IDStatusPedido					
	                LEFT JOIN tbCliente cl (NOLOCK) ON p.IDCliente=cl.IDCliente
                WHERE
					p.IDTipoPedido = 30
					AND
                    p.IDStatusPedido IN (10, 20, 60, 70)
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }

        public static PedidoInformation CarregarCompleto(Int32 idPedido)
        {
            PedidoInformation pedido = null;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            String querySql = @"
SELECT 
	 p.IDPedido
    ,p.IDTipoPedido
	,p.GUIDIdentificacao
    ,p.GUIDMovimentacao
	,p.NumeroCupom
	,p.DocumentoCliente
    ,p.NomeCliente
    ,p.EmailCliente
	,p.DtPedido
	,p.DtPedidoFechamento
	,p.SincERP
	,p.ValorConsumacaoMinima
	,p.ValorServico
	,p.ValorDesconto
	,p.TaxaServicoPadrao
	,p.ValorTotal
	,p.GUIDAgrupamentoPedido
	,p.Observacoes
    ,p.ObservacaoCupom
    ,p.ReferenciaLocalizacao
    ,p.NumeroPessoas
    ,p.ValorEntrega
    ,p.IDTaxaEntrega
    ,p.IDEntregador
    ,p.DtEnvio
    ,p.DtEntrega
    ,p.IDRetornoSAT_venda
    ,p.IDRetornoSAT_cancelamento
    ,p.AplicarDesconto
    ,p.AplicarServico
    ,p.IDUsuarioDesconto
    ,p.IDUsuarioTaxaServico
	,sp.IDStatusPedido
	,sp.Nome as 'StatusPedido_Nome'
	,c.IDCliente
	,c.NomeCompleto as 'Cliente_NomeCompleto'
	,c.Documento1 as 'Cliente_Documento1'
	,te.IDTipoEntrada
	,te.Nome as 'TipoEntrada_Nome'
	,te.ValorEntrada as 'TipoEntrada_ValorEntrada'
	,te.ValorConsumacaoMinima as 'TipoEntrada_ValorConsumacaoMinima'
    ,cx.IDCaixa
    ,tp.Nome as 'TipoPedido_Nome'
	,comanda.Numero as 'NumeroComanda'
	,mesa.Numero as 'NumeroMesa'
    
    ,p.IDTipoDesconto
    ,td.Nome as 'TipoDescontoNome'
FROM 
	tbPedido p (NOLOCK)
	INNER JOIN tbStatusPedido sp (NOLOCK) ON sp.IDStatusPedido=p.IDStatusPedido
    INNER JOIN tbTipoPedido tp (NOLOCK) ON tp.IDTipoPedido=p.IDTipoPedido
	LEFT JOIN tbComanda comanda (NOLOCK) ON p.GUIDIdentificacao=comanda.GUIDIdentificacao
	LEFT JOIN tbMesa mesa (NOLOCK) ON p.GUIDIdentificacao=mesa.GUIDIdentificacao
	LEFT JOIN tbCliente c (NOLOCK) ON c.idCliente=p.idCliente
	LEFT JOIN tbTipoEntrada te (NOLOCK) ON te.IDTipoEntrada=p.IDTipoEntrada
    LEFT JOIN tbCaixa cx (NOLOCK) on cx.IDCaixa = p.IDCaixa

    LEFT JOIN tbTipoDesconto td (NOLOCK) on td.IDTipoDesconto = p.IDTipoDesconto


WHERE
	p.IDPedido=@idPedido
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@idPedido", idPedido);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];

                pedido = new PedidoInformation();
                pedido.GUIDMovimentacao = Convert.ToString(dr["GUIDMovimentacao"]);
                pedido.IDPedido = Convert.ToInt32(dr["IDPedido"]);
                pedido.GUIDIdentificacao = Convert.ToString(dr["GUIDIdentificacao"]);
                pedido.NumeroCupom = Convert.ToString(dr["NumeroCupom"]);
                pedido.DocumentoCliente = Convert.ToString(dr["DocumentoCliente"]);
                pedido.NomeCliente = Convert.ToString(dr["NomeCliente"]);
                pedido.EmailCliente = Convert.ToString(dr["EmailCliente"]);
                pedido.DtPedido = (dr["DtPedido"] != DBNull.Value ? Convert.ToDateTime(dr["DtPedido"]) : (DateTime?)null);
                pedido.DtPedidoFechamento = (dr["DtPedidoFechamento"] != DBNull.Value ? Convert.ToDateTime(dr["DtPedidoFechamento"]) : (DateTime?)null);
                pedido.SincERP = (dr["SincERP"] != DBNull.Value ? Convert.ToBoolean(dr["SincERP"]) : (Boolean?)null);
                pedido.ValorConsumacaoMinima = (dr["ValorConsumacaoMinima"] != DBNull.Value ? Convert.ToDecimal(dr["ValorConsumacaoMinima"]) : (Decimal?)null);
                pedido.ValorServico = (dr["ValorServico"] != DBNull.Value ? Convert.ToDecimal(dr["ValorServico"]) : (Decimal?)null);
                pedido.ValorDesconto = (dr["ValorDesconto"] != DBNull.Value ? Convert.ToDecimal(dr["ValorDesconto"]) : (Decimal?)null);
                pedido.TaxaServicoPadrao = (dr["TaxaServicoPadrao"] != DBNull.Value ? Convert.ToDecimal(dr["TaxaServicoPadrao"]) : (Decimal?)null);
                pedido.ValorTotal = (dr["ValorTotal"] != DBNull.Value ? Convert.ToDecimal(dr["ValorTotal"]) : (Decimal?)null);
                pedido.GUIDAgrupamentoPedido = Convert.ToString(dr["GUIDAgrupamentoPedido"]);
                pedido.Observacoes = Convert.ToString(dr["Observacoes"]);
                pedido.ObservacaoCupom = Convert.ToString(dr["ObservacaoCupom"]);
                pedido.ReferenciaLocalizacao = Convert.ToString(dr["ReferenciaLocalizacao"]);
                pedido.NumeroPessoas = (dr["NumeroPessoas"] != DBNull.Value ? Convert.ToInt32(dr["NumeroPessoas"]) : (int?)null);

                pedido.TipoPedido = new TipoPedidoInformation();
                pedido.TipoPedido.IDTipoPedido = Convert.ToInt32(dr["IDTipoPedido"]);
                pedido.TipoPedido.Nome = Convert.ToString(dr["TipoPedido_Nome"]);

                pedido.StatusPedido = new StatusPedidoInformation();
                pedido.StatusPedido.IDStatusPedido = Convert.ToInt32(dr["IDStatusPedido"]);
                pedido.StatusPedido.Nome = Convert.ToString(dr["StatusPedido_Nome"]);
                pedido.ValorEntrega = (dr["ValorEntrega"] != DBNull.Value ? Convert.ToDecimal(dr["ValorEntrega"]) : default(decimal?));
                pedido.DtEntrega = (dr["DtEntrega"] != DBNull.Value ? Convert.ToDateTime(dr["DtEntrega"]) : default(DateTime?));
                pedido.DtEnvio = (dr["DtEnvio"] != DBNull.Value ? Convert.ToDateTime(dr["DtEnvio"]) : default(DateTime?));

                pedido.AplicarDesconto = (dr["AplicarDesconto"] != DBNull.Value ? Convert.ToBoolean(dr["AplicarDesconto"]) : default(bool?));
                pedido.AplicarServico = (dr["AplicarServico"] != DBNull.Value ? Convert.ToBoolean(dr["AplicarServico"]) : default(bool?));

                if (dr["IDTaxaEntrega"] != DBNull.Value)
                {
                    pedido.TaxaEntrega = new TaxaEntregaInformation { IDTaxaEntrega = Convert.ToInt32(dr["IDTaxaEntrega"]) };
                }

                if (dr["IDCaixa"] != DBNull.Value)
                {
                    pedido.Caixa = new CaixaInformation { IDCaixa = Convert.ToInt32(dr["IDCaixa"]) };
                }

                if (dr["IDCliente"] != DBNull.Value)
                {
                    pedido.Cliente = new ClienteInformation();
                    pedido.Cliente.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    pedido.Cliente.NomeCompleto = Convert.ToString(dr["Cliente_NomeCompleto"]);
                    pedido.Cliente.Documento1 = Convert.ToString(dr["Cliente_Documento1"]);
                }

                if (dr["IDTipoEntrada"] != DBNull.Value)
                {
                    pedido.TipoEntrada = new TipoEntradaInformation();
                    pedido.TipoEntrada.IDTipoEntrada = Convert.ToInt32(dr["IDTipoEntrada"]);
                    pedido.TipoEntrada.Nome = Convert.ToString(dr["TipoEntrada_Nome"]);
                    pedido.TipoEntrada.ValorEntrada = Convert.ToDecimal(dr["TipoEntrada_ValorEntrada"]);
                    pedido.TipoEntrada.ValorConsumacaoMinima = Convert.ToDecimal(dr["TipoEntrada_ValorConsumacaoMinima"]);
                }

                if (dr["IDEntregador"] != DBNull.Value)
                {
                    pedido.Entregador = new EntregadorInformation
                    {
                        IDEntregador = Convert.ToInt32(dr["IDEntregador"])
                    };
                }

                if (dr["IDRetornoSAT_venda"] != DBNull.Value)
                {
                    pedido.RetornoSAT_venda = new RetornoSATInformation
                    {
                        IDRetornoSAT = Convert.ToInt32(dr["IDRetornoSAT_venda"])
                    };
                }

                if (dr["IDRetornoSAT_cancelamento"] != DBNull.Value)
                {
                    pedido.RetornoSAT_cancelamento = new RetornoSATInformation
                    {
                        IDRetornoSAT = Convert.ToInt32(dr["IDRetornoSAT_cancelamento"])
                    };
                }

                if (dr["IDUsuarioDesconto"] != DBNull.Value)
                {
                    pedido.UsuarioDesconto = new UsuarioInformation
                    {
                        IDUsuario = Convert.ToInt32(dr["IDUsuarioDesconto"])
                    };
                }

                if (dr["IDUsuarioTaxaServico"] != DBNull.Value)
                {
                    pedido.UsuarioTaxaServico = new UsuarioInformation
                    {
                        IDUsuario = Convert.ToInt32(dr["IDUsuarioTaxaServico"])
                    };
                }
                if (dr["NumeroComanda"] != DBNull.Value)
                {
                    pedido.NumeroComanda = dr["NumeroComanda"].ToString();
                }
                if (dr["NumeroMesa"] != DBNull.Value)
                {
                    pedido.NumeroMesa = dr["NumeroMesa"].ToString();
                }
                
                if (dr["TipoDescontoNome"] != DBNull.Value)
                {
                    pedido.TipoDesconto = new TipoDescontoInformation
                    {
                        IDTipoDesconto = Convert.ToInt32(dr["IDTipoDesconto"]),
                        Nome = dr["TipoDescontoNome"].ToString()
                    };
                }
            }

            return pedido;
        }

        public static List<PedidoInformation> ListarFinalizadosUltimaHora()
        {
            return ListarFinalizadosAPartirDe(DateTime.Now.AddHours(-1)).ToList();
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosNoIntervalo(
            DateTime de, DateTime ate,
            int IDCliente, int skip, int limit)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            var lista = new List<PedidoInformation>();

            string querySql = @"SELECT * FROM tbPedido p (nolock) 
left join tbRetornoSAT rs on p.IDRetornoSAT_venda = rs.IDRetornoSAT
WHERE p.IDStatusPedido = 40";

            if (de != null)
            {
                querySql += " AND p.DtPedidoFechamento >= @dataMin";
            }

            if (ate != null)
            {
                querySql += " AND p.DtPedidoFechamento < @dataMax";
            }

            if (IDCliente != 0)
            {
                querySql += " AND p.IDCliente = @IDCliente";
            }

            querySql += " ORDER BY p.DtPedidoFechamento DESC";
            querySql += " OFFSET @skip ROWS";


            if (limit != 0)
            {
                querySql += " FETCH NEXT @limit ROWS ONLY";
            }

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = querySql;
                    cmd.Parameters.AddWithValue("@dataMin", de);
                    cmd.Parameters.AddWithValue("@dataMax", ate);
                    cmd.Parameters.AddWithValue("@IDCliente", IDCliente);
                    cmd.Parameters.AddWithValue("@skip", skip);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return ObterPedido(reader);
                        }
                    }
                }
            }
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosAPartirDe(DateTime de)
        {
            string querySql = @"SELECT * FROM tbPedido (nolock) WHERE IDStatusPedido=40 and DtPedidoFechamento >= @data";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = querySql;
                    cmd.Parameters.AddWithValue("@data", de);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return ObterPedido(reader);
                        }
                    }
                }
            }
        }

        public static IEnumerable<PedidoInformation> ListarPedidoSemEstoque()
        {
            string querySql = @"SELECT tbPedido.* FROM tbPedido   
LEFT JOIN tbEntradaSaida ON tbPedido.GUIDMovimentacao=tbEntradaSaida.GUID_Origem
WHERE tbPedido.IDStatusPedido=40
AND tbEntradaSaida.GUID_Origem IS NULL
AND tbPedido.DtPedido>=(SELECT MAX(Data) FROM tbInventario WHERE Processado=1 AND Excluido=0)";
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = querySql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return ObterPedido(reader);
                    }
                }
                conn.Close();
            }
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosPorDataAberturaAPartirDe(DateTime de)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            var lista = new List<PedidoInformation>();

            string querySql = @"SELECT * FROM tbPedido (nolock) WHERE DtPedido >= @data";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = querySql;
                    cmd.Parameters.AddWithValue("@data", de);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return ObterPedido(reader);
                        }
                    }
                }
            }
        }

        public static List<PedidoInformation> ListarEnviados()
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM tbPedido where IDStatusPedido = 20";
                    var reader = cmd.ExecuteReader();
                    var result = new List<PedidoInformation>();
                    while (reader.Read())
                    {
                        PedidoInformation pedido = ObterPedido(reader);

                        result.Add(pedido);
                    }
                    return result;
                }
            }
        }

        public static IEnumerable<PedidoInformation> ListarFinalizadosPorCliente(int idCliente)
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT p.* FROM tbPedido (nolock) p WHERE p.IDCliente = @idCliente and p.IDStatusPedido = 40 ORDER BY p.IDPedido ASC";
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return ObterPedido(reader);
                    }
                }
            }
        }
        public static IEnumerable<PedidoInformation> ListarPedidosSemSyncERP(DateTime dtInicio)
        {
            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.* 
FROM tbPedido (nolock) p 
WHERE 
    NOT p.DtPedidoFechamento IS NULL
    AND p.SincERP=0
    AND p.DtPedido>@dtInicio
ORDER BY p.IDPedido ASC";
                    cmd.Parameters.AddWithValue("@dtInicio", dtInicio);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return ObterPedido(reader);
                    }
                }
            }
        }

        private static PedidoInformation ObterPedido(SqlDataReader reader)
        {
            var pedido = new PedidoInformation
            {
                IDPedido = reader.Def<int>(reader.GetOrdinal("IDPedido")),
                GUIDIdentificacao = reader.Def<string>(reader.GetOrdinal("GUIDIdentificacao")),
                NumeroCupom = reader.Def<string>(reader.GetOrdinal("NumeroCupom")),
                DocumentoCliente = reader.Def<string>(reader.GetOrdinal("DocumentoCliente")),
                DtPedido = reader.Def<DateTime>(reader.GetOrdinal("DtPedido")),
                DtPedidoFechamento = reader.Def<DateTime>(reader.GetOrdinal("DtPedidoFechamento")),
                SincERP = reader.Def<bool>(reader.GetOrdinal("SincERP")),
                ValorConsumacaoMinima = reader.Def<decimal>(reader.GetOrdinal("ValorConsumacaoMinima")),
                ValorServico = reader.Def<decimal>(reader.GetOrdinal("ValorServico")),
                ValorDesconto = reader.Def<decimal>(reader.GetOrdinal("ValorDesconto")),
                TaxaServicoPadrao = reader.Def<decimal>(reader.GetOrdinal("TaxaServicoPadrao")),
                ValorTotal = reader.Def<decimal>(reader.GetOrdinal("ValorTotal")),
                GUIDAgrupamentoPedido = reader.Def<string>(reader.GetOrdinal("GUIDAgrupamentoPedido")),
                Observacoes = reader.Def<string>(reader.GetOrdinal("Observacoes")),
                ReferenciaLocalizacao = reader.Def<string>(reader.GetOrdinal("ReferenciaLocalizacao")),
                GUIDMovimentacao = reader.Def<string>(reader.GetOrdinal("GUIDMovimentacao")),
                NumeroPessoas = reader.Def<int>(reader.GetOrdinal("NumeroPessoas")),
                ValorEntrega = reader.Def<decimal>(reader.GetOrdinal("ValorEntrega")),
                DtEntrega = reader.Def<DateTime>(reader.GetOrdinal("DtEntrega")),
            };

            var IDCliente = reader.Val<int>("IDCliente");

            if (IDCliente.HasValue)
            {
                pedido.Cliente = new ClienteInformation { IDCliente = IDCliente };
            }

            var IDTipoPedido = reader.Val<int>("IDTipoPedido");

            if (IDTipoPedido.HasValue)
            {
                pedido.TipoPedido = new TipoPedidoInformation { IDTipoPedido = IDTipoPedido };
            }

            var IDStatusPedido = reader.Val<int>("IDStatusPedido");

            if (IDStatusPedido.HasValue)
            {
                pedido.StatusPedido = new StatusPedidoInformation { IDStatusPedido = IDStatusPedido };
            }

            var IDCaixa = reader.Val<int>("IDCaixa");

            if (IDCaixa.HasValue)
            {
                pedido.Caixa = new CaixaInformation { IDCaixa = IDCaixa };
            }

            var IDTipoEntrada = reader.Val<int>("IDTipoEntrada");

            if (IDTipoEntrada.HasValue)
            {
                pedido.TipoEntrada = new TipoEntradaInformation { IDTipoEntrada = IDTipoEntrada };
            }

            var IDRetornoSAT_venda = reader.Val<int>("IDRetornoSAT_venda");

            if (IDRetornoSAT_venda.HasValue)
            {
                pedido.RetornoSAT_venda = new RetornoSATInformation { IDRetornoSAT = IDRetornoSAT_venda };

                if (HasColumn(reader, "numeroSessao"))
                {
                    pedido.RetornoSAT_venda.numeroSessao = reader.Def<string>(reader.GetOrdinal("numeroSessao"));
                    pedido.RetornoSAT_venda.EEEEE = reader.Def<string>(reader.GetOrdinal("EEEEE"));
                    pedido.RetornoSAT_venda.CCCC = reader.Def<string>(reader.GetOrdinal("CCCC"));
                    pedido.RetornoSAT_venda.mensagem = reader.Def<string>(reader.GetOrdinal("mensagem"));
                    pedido.RetornoSAT_venda.cod = reader.Def<string>(reader.GetOrdinal("cod"));
                    pedido.RetornoSAT_venda.mensagemSEFAZ = reader.Def<string>(reader.GetOrdinal("mensagemSEFAZ"));
                    pedido.RetornoSAT_venda.arquivoCFeSAT = reader.Def<string>(reader.GetOrdinal("arquivoCFeSAT"));
                    pedido.RetornoSAT_venda.timeStamp = reader.Def<string>(reader.GetOrdinal("timeStamp"));
                    pedido.RetornoSAT_venda.chaveConsulta = reader.Def<string>(reader.GetOrdinal("chaveConsulta"));
                    pedido.RetornoSAT_venda.valorTotalCFe = reader.Def<string>(reader.GetOrdinal("valorTotalCFe"));
                    pedido.RetornoSAT_venda.CPFCNPJValue = reader.Def<string>(reader.GetOrdinal("CPFCNPJValue"));
                    pedido.RetornoSAT_venda.assinaturaQRCODE = reader.Def<string>(reader.GetOrdinal("assinaturaQRCODE"));
                    pedido.RetornoSAT_venda.TipoSolicitacaoSAT = new TipoSolicitacaoSATInformation
                    {
                        IDTipoSolicitacaoSAT = reader.Def<int>(reader.GetOrdinal("IDTipoSolicitacaoSAT"))
                    };
                }



            }

            var IDRetornoSAT_cancelamento = reader.Val<int>("IDRetornoSAT_cancelamento");

            if (IDRetornoSAT_cancelamento.HasValue)
            {
                pedido.RetornoSAT_cancelamento = new RetornoSATInformation { IDRetornoSAT = IDRetornoSAT_cancelamento };
            }

            var IDTipoDesconto = reader.Val<int>("IDTipoDesconto");

            if (IDTipoDesconto.HasValue)
            {
                pedido.TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = IDTipoDesconto };
            }

            var IDTaxaEntrega = reader.Val<int>("IDTaxaEntrega");

            if (IDTaxaEntrega.HasValue)
            {
                pedido.TaxaEntrega = new TaxaEntregaInformation { IDTaxaEntrega = IDTaxaEntrega };
            }

            var IDEntregador = reader.Def<int>("IDEntregador");

            if (IDEntregador > -1)
            {
                pedido.Entregador = new EntregadorInformation { IDEntregador = IDEntregador };
            }

            var AplicarDesconto = reader.Val<bool>("AplicarDesconto");

            if (AplicarDesconto != null)
                pedido.AplicarDesconto = AplicarDesconto;

            var AplicarServico = reader.Val<bool>("AplicarServico");

            if (AplicarServico != null)
                pedido.AplicarServico = AplicarServico;

            return pedido;
        }

        public static bool HasColumn(SqlDataReader r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public static PedidoInformation ObterPedidoAbertoPorMesa(int numeroMesa)
        {
            int idPedido = 0;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String query = @"SELECT p.IDPedido FROM tbPedido p
                inner join tbMesa m on p.GUIDIdentificacao = m.GUIDIdentificacao
                where IDStatusPedido = 10
                and IDTipoPedido = 10
                and m.Numero = @numero ";

            da = new SqlDataAdapter(query, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@numero", numeroMesa);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                idPedido = Convert.ToInt32(dt.Rows[0]["IDPedido"]);
                return CarregarCompleto(idPedido);
            }
            else
            {
                return null;
            }
        }

        public static PedidoInformation ObterPedidoAbertoPorComanda(int numeroComanda)
        {
            int idPedido = 0;

            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String query = @"
                                 SELECT  p.IDPedido  FROM tbPedido p
                                    inner join tbComanda m on p.GUIDIdentificacao = m.GUIDIdentificacao
                                      where IDStatusPedido = 10
                                      and IDTipoPedido = 20
                                      and m.Numero = @numero

            ";

            da = new SqlDataAdapter(query, DB.ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("@numero", numeroComanda);

            da.Fill(ds);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                idPedido = Convert.ToInt32(dt.Rows[0]["IDPedido"]);
                return CarregarCompleto(idPedido);
            }
            else
            {
                return null;
            }
        }
    }
}
