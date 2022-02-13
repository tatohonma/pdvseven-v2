using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Balanca.WS.Controllers
{
    public class BalancaController : ApiController
    {
        [Route("toledo")]
        [HttpGet]
        public async Task<IHttpActionResult> GetToledo()
        {
            return Ok(await ObterDados(Tipo.TOLEDO));
        }

        [Route("filizola")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFilizola()
        {
            return Ok(await ObterDados(Tipo.FILIZOLA));
        }


        private async Task<Dados> ObterDados(Tipo tipo)
        {
            try
            {
                var balanca = BalancaFactory.ObterBalanca(tipo);
                var dados = await balanca.LerPesoAsync(ConfigurationManager.AppSettings["PortaBalanca"]);
                return dados;
            }
            catch
            {

            }
            return null;
        }
    }
}
