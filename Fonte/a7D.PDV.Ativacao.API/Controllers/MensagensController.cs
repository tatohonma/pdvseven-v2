using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Services;
using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public class MensagensController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext(true);

        protected override ExceptionResult InternalServerError(Exception ex)
        {
            return base.InternalServerError(this.ObterDadosErro(ex));
        }

        /*
{
    "Chave": "001-06079-14",
    "IdMensagemOrigem": 0,
    "Tipo": "Update",
    "Origem": "Ativador",
    "OrigemKey": "",
    "Destino": "Integrador",
    "DestinoKey": "",
    "Texto": "1" ,
    "Parametros": ""
}
{
    "Chave": "001-06079-14",
    "Tipo": "Update",
    "Origem": "Ativador",
    "Destino": "Integrador",
    "Texto": "333" 
}
        */
        [HttpPost]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> Enviar([FromBody] MensagemNova mensagem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var json = await this.GetRawPostData();
                    ModelState.AddModelError("body_request", json);
                    return BadRequest(ModelState);
                }

                var ativacao = db.Ativacoes.FirstOrDefault(a => a.ChaveAtivacao == mensagem.Chave);
                if (ativacao == null)
                {
                    ModelState.AddModelError(nameof(mensagem.Chave), "Chave de ativação não existe");
                    return BadRequest(ModelState);
                }

                var msg = new Mensagem(mensagem);
                msg.IDAtivacao = ativacao.IDAtivacao;
                db.Mensagens.Add(msg);
                db.SaveChanges();
                return Ok(msg.IDMensagem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomHeaderFilter]
        [ApiAuth(requerAdm: false)]
        public IHttpActionResult GetMensagens([FromUri]int page = 0, [FromUri]int count = 0, [FromUri]Filter filter = null)
        {
            var queryValue = Request.RequestUri.ParseQueryString();
            if (!string.IsNullOrEmpty(queryValue["chave"])) // Protege contra a API antiga
                return NotFound();

            var query2 = from m in db.Mensagens.AsNoTracking()
                         where (filter.Cliente == null || m.Ativacao.Cliente.Nome.Contains(filter.Cliente))
                            && (filter.Tipo == null || m.IDTipo.ToString() == filter.Tipo)
                         orderby m.DataCriada descending
                         select m;

            var lista = query2.ToList().Select(m => new
            {
                m.IDMensagem,
                m.DataCriada,
                m.Tipo,
                m.DataRecebida,
                Cliente = m.Ativacao?.Cliente?.Nome ?? "???",
                m.Texto
            });

            var total = lista.Count();
            Request.Properties["count"] = total.ToString();

            return Ok(lista);
        }

        [HttpGet()]
        [ResponseType(typeof(Mensagem[]))]
        [Route("api/mensagens/receber")]
        public IHttpActionResult Receber([FromUri] string chave) //  Ferreira: 001-06079-14
        {
            return SyncMessage(chave, EOrigemDestinoMensagem.Integrador, "", "");
        }

        [HttpGet()]
        [ResponseType(typeof(Mensagem[]))]
        [Route("api/mensagens/syncmsg")]
        public IHttpActionResult SyncMessage(
            [FromUri] string chave, 
            [FromUri, TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))]EOrigemDestinoMensagem to, 
            [FromUri] string versao, 
            [FromUri] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(chave))
                    return NotFound();

                ClientesService.Registra(chave, versao, status, false);

                //int? nto = string.IsNullOrEmpty(to) ? (int)EOrigemDestinoMensagem.Integrador : int.Parse(to);

                int nto = (int)to;

                var query = from m in db.Mensagens.AsNoTracking()
                             where !m.DataRecebida.HasValue 
                             && m.Ativacao.ChaveAtivacao == chave
                             && nto == m.IDDestino
                             orderby m.DataCriada
                             select m;

                //string sql = query.ToString();

                var result = query.ToList();
                //var finalLog = db.LogSQL;

                if (result.Count > 10)
                    throw new Exception($"Mais de {result.Count} mensagens pendentes! O limite de 10 mensagens");

                var result2 = result.Take(3).Select(m => m.Lida());
                return Ok(result2);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


    public class Filter
    {
        public string Cliente { get; set; }
        public string Tipo { get; set; }
    }

}
