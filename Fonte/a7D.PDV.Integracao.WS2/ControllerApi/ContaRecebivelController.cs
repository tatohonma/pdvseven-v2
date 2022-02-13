using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ContaRecebivelController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetContas()
        {
            try
            {
                var result = new List<ContaRecebivel>();
                var contasRecebiveis = BLL.ContaRecebivel.Listar();
                foreach (var conta in contasRecebiveis)
                {
                   result.Add(DTO.Converter(conta));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(new Erro(20, ex)));
            }
        }

        [HttpPut]
        [Route("api/conta-recebivel/codigo-integracao")]
        public IHttpActionResult AtualizarCodigoIntegracao([FromBody] ContaRecebivel conta)
        {
            try
            {
                if (string.IsNullOrEmpty(conta.CodigoIntegracao))
                    conta.CodigoIntegracao = null;
                    //return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(25, new Exception("Codigo de Integração não pode estar vazio"))));

                if (conta.IDContaRecebivel == 0)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(26, new Exception("ID da conta de recebível não pode estar vazio"))));

                var contaRecebivel = BLL.ContaRecebivel.Carregar(conta.IDContaRecebivel);
                
                contaRecebivel.CodigoIntegracao = conta.CodigoIntegracao;
                EF.Repositorio.Atualizar(contaRecebivel);
                return Ok(contaRecebivel);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(new Erro(30, ex)));
            }
        }
      
    }
}
