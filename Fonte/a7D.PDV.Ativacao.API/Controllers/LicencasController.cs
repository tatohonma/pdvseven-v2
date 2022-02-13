using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Extensions;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth(requerAdm: false)]
    public class LicencasController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();
        private readonly TimeSpan _fromSeconds = TimeSpan.FromSeconds(1);

        [HttpPost]
        public IHttpActionResult Enviar([FromBody]PdvAtivacao[] pdvs)
        {
            try
            {
                Entities.Ativacao ativacao = null;
                foreach (var pdv in pdvs)
                {
                    ativacao = db.Ativacoes
                                        .Where(a => a.ChaveAtivacao == pdv.ChaveAtivacao)
                                        .FirstOrDefault();

                    if (ativacao != null)
                    {
                        ativacao.DtUltimaVerificacao = DateTime.Now.ToUniversalTime();
                        var pdvExistente = ativacao.PDVs.Where(p => p.IDPDV_instalacao == pdv.IdPdv).FirstOrDefault();
                        if (pdvExistente != null)
                        {
                            pdvExistente.Nome = pdv.Nome;
                            if (pdv.DtUltimaAlteracao?.Truncate(_fromSeconds) < pdvExistente.DtUltimaAlteracao?.Truncate(_fromSeconds) && ativacao.Ativo)
                            {
                                if (!ativacao.Duplicidade)
                                {
                                    ativacao.Duplicidade = true;
                                    var ativacaoUnproxed = db.UnProxy(ativacao);
                                    ativacao.Observacao += "\nDuplicidade detectada em " + DateTime.Now.ToString("dd/MM/yyyy");
                                    EmailServices.EnviarAtivacao(ETipoEmailAtivacao.AtivacaoOffline, ativacaoUnproxed, null);
                                }
                            }
                            if (pdvExistente.DtUltimaAlteracao != null)
                            {
                                if (pdvExistente.DtUltimaAlteracao < pdv.DtUltimaAlteracao?.Truncate(_fromSeconds))
                                    pdvExistente.DtUltimaAlteracao = pdv.DtUltimaAlteracao?.Truncate(_fromSeconds);
                            }
                            else
                            {
                                pdvExistente.DtUltimaAlteracao = pdv.DtUltimaAlteracao?.Truncate(_fromSeconds);
                            }
                            pdvExistente.ChaveHardware = pdv.ChaveHardware;
                            pdvExistente.IDTipoPDV = pdv.IdTipoPdv;
                            pdvExistente.Nome = pdv.Nome;
                            pdvExistente.Versao = pdv.Versao;
                            pdvExistente.Ativo = pdv.Ativo;
                        }
                        else
                        {
                            var novoPdv = new Entities.PDV { IDAtivacao = ativacao.IDAtivacao };
                            novoPdv.IDPDV_instalacao = pdv.IdPdv;
                            novoPdv.ChaveHardware = pdv.ChaveHardware;
                            novoPdv.Nome = pdv.Nome;
                            novoPdv.DtUltimaAlteracao = pdv.DtUltimaAlteracao;
                            novoPdv.IDTipoPDV = pdv.IdTipoPdv;
                            novoPdv.Versao = pdv.Versao;
                            novoPdv.Ativo = pdv.Ativo;

                            db.PDVs.Add(novoPdv);
                        }
                    }
                    else
                        throw new Exception("Chave de ativação inválida");
                }

                db.SaveChanges();

                if (ativacao?.Ativo == false)
                {
                    if (ativacao?.DataValidadeProvisoria < DateTime.Now)
                        throw new Exception("Chave de ativação desativada");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
        }

        [HttpGet]
        public IHttpActionResult Receber(string id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ativacao = db.Ativacoes.Include("Cliente").Include("PDVs").AsNoTracking().Where(a => a.ChaveAtivacao == id).FirstOrDefault();

                if (ativacao == null)
                    throw new Exception("Chave de ativação inválida");

                if (!ativacao.Ativo)
                {
                    if (ativacao.DataValidadeProvisoria < DateTime.Now)
                        throw new Exception("Chave de ativação desativada");
                }
                if (ativacao.PDVs == null || ativacao.PDVs.Count == 0)
                    throw new Exception("Nenhum produto configurado pra essa licença");

                //var cliente = db.Clientes.Where(c => c.IDCliente == ativacao.IDCliente).FirstOrDefault();

                return Json(new
                {
                    Status = (int)HttpStatusCode.OK,
                    id = ativacao.IDAtivacao,
                    cliente = ativacao.Cliente?.Nome,
                    pdvs = ativacao.PDVs.ToList()
                });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
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

        [DataContract]
        class MensagemRetorno
        {
            [DataMember]
            public string Mensagem { get; set; }
        }
    }
}
