using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace a7D.PDV.Ativacao.API.Controllers
{

    public class AtivacoesController : BaseSecureApiController
    {
        private static readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private static readonly TimeZoneInfo _brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public class Filter
        {
            public string Cliente { get; set; }
            public string ChaveAtivacao { get; set; }
            public string Ativo { get; set; }
            public string ReativadoSuporte { get; set; }
            public string Duplicidade { get; set; }
            public string Observacao { get; set; }
        }

        // GET: api/Ativacoes
        [CustomHeaderFilter]
        [ApiAuth(requerAdm: false)]
        public IHttpActionResult GetAtivacoes([FromUri]int page = 0, [FromUri]int count = 0, [FromUri]Filter filter = null)
        {
            try
            {
                IQueryable<Entities.Ativacao> query;
                query = db.Ativacoes
                        .OrderBy(a => a.Cliente.Nome);

                if (filter != null)
                {
                    if (!string.IsNullOrWhiteSpace(filter.Cliente))
                        query = query.Where(a => a.Cliente.Nome.Contains(filter.Cliente));
                    if (!string.IsNullOrWhiteSpace(filter.ChaveAtivacao))
                        query = query.Where(a => a.ChaveAtivacao.Contains(filter.ChaveAtivacao));
                    if (!string.IsNullOrWhiteSpace(filter.Ativo))
                        query = query.Where(a => a.Ativo == (filter.Ativo == "1"));
                    if (!string.IsNullOrWhiteSpace(filter.Duplicidade))
                        query = query.Where(a => a.Duplicidade == (filter.Duplicidade == "1"));
                    if (!string.IsNullOrWhiteSpace(filter.Observacao))
                        query = query.Where(a => a.Observacao.Contains(filter.Observacao));
                    if (!string.IsNullOrWhiteSpace(filter.ReativadoSuporte))
                        query = query.Where(a => a.ReativadoSuporte == (filter.ReativadoSuporte == "1"));
                }

                var total = query.ToList().Count();
                Request.Properties["count"] = total.ToString();

                if (page > 0)
                    query = query.Skip((page - 1) * count);
                if (count > 0)
                    query = query.Take(count);
                var result = query.ToList();

                result.ForEach(a =>
                {
                    a.DtUltimaVerificacao = a.DtUltimaVerificacao.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(a.DtUltimaVerificacao.Value, _brasiliaTimeZone) : null as DateTime?;
                });

                var resultUnproxed = new List<Entities.Ativacao>();

                result.ForEach(a =>
                {
                    var unproxed = db.UnProxy(a);
                    unproxed.Cliente = db.UnProxy(db.Clientes.Find(unproxed.IDCliente));
                    unproxed.PDVs = db.UnProxy(a.PDVs.ToList()).ToList();
                    resultUnproxed.Add(unproxed);
                });

                resultUnproxed.ForEach(a =>
                {
                    a.Cliente = db.UnProxy(db.Clientes.Find(a.IDCliente));
                });

                if (!IsAdminRequest())
                {
                    resultUnproxed.ForEach(a =>
                    {
                        a.DataAtivacao = null;
                        a.PDVs = null;
                        a.Observacao = null;
                        a.Licencas = null;
                        a.DiasValidadeAtivacao = -1;
                    });
                }

                //return Json(new { status = HttpStatusCode.OK, ativacoes = result });
                var response = Request.CreateResponse(HttpStatusCode.OK, resultUnproxed);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
        }

        // GET: api/Ativacoes/5
        [ResponseType(typeof(Entities.Ativacao))]
        [ApiAuth]
        public IHttpActionResult GetAtivacao(int id)
        {
            Entities.Ativacao ativacao = db.Ativacoes.Find(id);
            if (ativacao == null)
            {
                return NotFound();
            }

            ativacao.DtUltimaVerificacao = ativacao.DtUltimaVerificacao.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(ativacao.DtUltimaVerificacao.Value, _brasiliaTimeZone) : null as DateTime?;

            db.Entry(ativacao).Collection(a => a.PDVs).Load();
            ativacao.PDVs = ativacao.PDVs
                    .OrderBy(pdv => pdv.IDTipoPDV)
                    //.OrderBy(pdv => pdv.IDPDV_instalacao)
                    //.OrderBy(pdv => pdv.Ativo)
                    .ToList();
            ativacao.PDVs.ForEach(pdv =>
            {
                pdv.DtUltimaAlteracao = pdv.DtUltimaAlteracao.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(pdv.DtUltimaAlteracao.Value, _brasiliaTimeZone) : null as DateTime?;
            });
            return Ok(ativacao);
        }

        // GET: api/ativacoes/cliente/001-06079-14
        [Route("api/ativacoes/cliente/{chave}")]
        public IHttpActionResult GetAtivacaoPorChave(string chave)
        {
            var ativacao = db.Ativacoes.FirstOrDefault(a => a.ChaveAtivacao == chave);
            if (ativacao == null)
            {
                return NotFound();
            }
            dynamic cliente = new
            {
                Estabelecimento = ativacao.Cliente.Nome,
                ativacao.Cliente.RazaoSocial,
                CNPJ = ativacao.Cliente.CNPJCPF
                        .ToString()
                        .Replace(".", string.Empty)
                        .Replace(",", string.Empty),
            };
            return Json(cliente);
        }

        // PUT: api/Ativacoes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [ApiAuth]
        public IHttpActionResult PutAtivacao(int id, Entities.Ativacao ativacaoRecebida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ativacaoRecebida.IDAtivacao)
            {
                return BadRequest();
            }

            if (ativacaoRecebida.ReativadoSuporte == false)
            {
                ativacaoRecebida.DataReativacaoSuporte = null;
                ativacaoRecebida.DataValidadeProvisoria = null;
            }
            else
            {
                var agora = DateTime.Now;
                ativacaoRecebida.Ativo = false;
                ativacaoRecebida.DataReativacaoSuporte = agora;
                ativacaoRecebida.DataValidadeProvisoria = agora.AdicionarDiasUteis(3);
            }

            //ativacaoRecebida.PDVs.Where(pdv => pdv.Deletado && pdv.IDPDV != 0).ToList().ForEach(pdv => db.Entry(pdv).State = EntityState.Deleted);
            ativacaoRecebida.PDVs.Where(pdv => pdv.IDPDV == 0).ToList().ForEach(pdv => db.Entry(pdv).State = EntityState.Added);
            ativacaoRecebida.PDVs.Where(pdv => pdv.IDPDV != 0).ToList().ForEach(pdv =>
            {
                if (ativacaoRecebida.SiteAdmin == null)
                    pdv.DtUltimaAlteracao = DateTime.UtcNow;

                db.Entry(pdv).State = EntityState.Modified;
            });

            db.Entry(ativacaoRecebida).State = EntityState.Modified;

            AtualizarIDInstalacao(ativacaoRecebida);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AtivacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    //var result = new StatusCodeResult(HttpStatusCode.InternalServerError, this);
                    //return StatusCode(HttpStatusCode.InternalServerError);
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));

                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
            return Ok(ativacaoRecebida);
        }

        private void AtualizarIDInstalacao(Entities.Ativacao ativacao)
        {
            var qdb = db.PDVs.Where(pdv => pdv.IDAtivacao == ativacao.IDAtivacao && pdv.IDPDV_instalacao.HasValue).ToList();
            var maxdb = qdb.Count > 0 ? qdb.Max(pdv => pdv.IDPDV_instalacao.Value) : 0;
            var qrec = ativacao.PDVs.Where(pdv => db.Entry(pdv).State != EntityState.Deleted && pdv.IDPDV_instalacao.HasValue).ToList();
            var maxrecieved = qrec.Count > 0 ? qrec.Max(pdv => pdv.IDPDV_instalacao.Value) : 0;

            var nextId = Math.Max(maxdb, maxrecieved) + 1;

            ativacao.PDVs.Where(pdv => !pdv.IDPDV_instalacao.HasValue).ToList().ForEach(pdv =>
            {
                pdv.IDPDV_instalacao = nextId++;
            });
        }

        // POST: api/Ativacoes
        [ResponseType(typeof(Entities.Ativacao))]
        [ApiAuth]
        public IHttpActionResult PostAtivacao(Entities.Ativacao ativacao)
        {
            if (!ModelState.IsValid || ativacao == null)
            {
                return BadRequest(ModelState);
            }
            ativacao.Cliente = db.Clientes.Find(ativacao.IDCliente);
            db.Ativacoes.Add(ativacao);
            AtualizarIDInstalacao(ativacao);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ativacao.IDAtivacao }, ativacao);
        }

        // DELETE: api/Ativacoes/5
        [ResponseType(typeof(Entities.Ativacao))]
        [ApiAuth]
        public IHttpActionResult DeleteAtivacao(int id)
        {
            Entities.Ativacao ativacao = db.Ativacoes.Find(id);
            if (ativacao == null)
            {
                return NotFound();
            }

            db.Ativacoes.Remove(ativacao);
            db.SaveChanges();

            return Ok(ativacao);
        }

        private bool AtivacaoExists(int id)
        {
            return db.Ativacoes.Count(e => e.IDAtivacao == id) > 0;
        }
    }
}