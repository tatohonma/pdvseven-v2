using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth]
    public class GerarChaveAtivacaoController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        [HttpGet]
        // GET: GerarChaveAtivacao
        [ResponseType(typeof(ChaveAtivacaoHelper))]
        public IHttpActionResult Index(int idRevenda)
        {

            Random rnd = new Random();
            string chaveAtivacao;
            string codigoRevenda;
            string codigoAtivacao;
            decimal codigoVerificadorNumerico = 0;
            string codigoVerificador;
            string codigo1;

            var revenda = db.Revendas.Find(idRevenda);

            if (revenda == null)
                return NotFound();

            codigoRevenda = revenda.Codigo.ToString().PadLeft(3, '0');
            codigoAtivacao = rnd.Next(99999).ToString("00000");

            codigo1 = codigoRevenda + codigoAtivacao;

            for (int i = 0; i < codigo1.Length - 1; i++)
            {
                codigoVerificadorNumerico = codigoVerificadorNumerico + Convert.ToDecimal(codigo1[i].ToString()) + 1 / 2;
            }

            if (Convert.ToInt32(codigoVerificadorNumerico) <= 99)
                codigoVerificador = Convert.ToInt32(codigoVerificadorNumerico).ToString("00");
            else
                codigoVerificador = codigoVerificadorNumerico.ToString().Substring(0, 2);

            chaveAtivacao = codigoRevenda + "-" + codigoAtivacao + "-" + codigoVerificador;
            return Ok(new ChaveAtivacaoHelper { ChaveAtivacao = chaveAtivacao });
        }

        [DataContract]
        public class ChaveAtivacaoHelper
        {
            [DataMember]
            public string ChaveAtivacao { get; set; }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}