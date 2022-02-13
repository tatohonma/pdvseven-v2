using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static a7D.PDV.BLL.Mesa;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class PainelMesasController : ApiController
    {
        [HttpGet]
        [Route("api/painelmesas")]
        public IHttpActionResult MesasComandas()
        {
            try
            {
                string sql = @"SELECT m.Numero Mesa, mc.Comandas, mc.Total
FROM tbMesa m
LEFT JOIN (
	SELECT 
		p.ReferenciaLocalizacao Mesa,
		COUNT(DISTINCT co.Numero) Comandas,
		SUM(pp.ValorUnitario) Total
	FROM
		tbPedido p(NOLOCK)
		INNER JOIN tbComanda co(NOLOCK) ON p.GUIDIdentificacao = co.GUIDIdentificacao
		INNER JOIN tbPedidoProduto pp(NOLOCK) ON p.IDPedido = pp.IDPedido
	WHERE
		p.IDStatusPedido = 10
	GROUP BY
		p.ReferenciaLocalizacao) mc ON CAST(m.Numero as nvarchar)=mc.Mesa
ORDER BY m.Numero";

                using (var pdv = new pdv7Context())
                    return Ok<MesaComandasTotal[]>(pdv.Database.SqlQuery<MesaComandasTotal>(sql).ToArray());
            }
            catch (BLL.ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpGet]
        [Route("api/painelmesas/{mesa}")]
        public IHttpActionResult MesasLocalizacao(string mesa)
        {
            try
            {
                string sql = @"SELECT 
	co.Numero,
	SUM(pp.Quantidade) Quantidade,
	SUM(pp.ValorUnitario) Total,
	MIN(p.DtPedido) Primeiro
FROM 
	tbPedido p (NOLOCK) 
	INNER JOIN tbComanda co (NOLOCK) ON p.GUIDIdentificacao=co.GUIDIdentificacao
	INNER JOIN tbPedidoProduto pp (NOLOCK) ON p.IDPedido=pp.IDPedido
WHERE
    p.IDStatusPedido=10 AND
	p.ReferenciaLocalizacao=@p0
GROUP BY 
	co.Numero";

                using (var pdv = new pdv7Context())
                    return Ok<NumeroQuantidadeTotalPrimeiro[]>(pdv.Database.SqlQuery<NumeroQuantidadeTotalPrimeiro>(sql, mesa).ToArray());
            }
            catch (BLL.ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }
    }
}