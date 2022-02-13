using a7D.PDV.BigData.WebAPI.Models;
using a7D.PDV.BigData.WebAPI.Services;
using a7D.PDV.BigData.WebAPI.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace a7D.PDV.BigData.WebAPI.Controllers
{
    public class IaagoController : ApiController
    {
        BDContext context;

        public IaagoController()
        {
            context = new BDContext();
        }

        protected override ExceptionResult InternalServerError(Exception ex)
        {
            return base.InternalServerError(this.ObterDadosErro(ex));
        }

        public static string GeraAuth(Entidade e, Usuario u)
        {
            return $"AUTH_{e.IDEntidade}_{u.IDUsuario}";
        }

        [Route("api/iaago/generic")]
        [AcceptVerbs("POST")]
        // [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> Generic()
        {
            var v = new
            {
                texto = "query " + HttpContext.Current.Request.QueryString["prm"],
                usuario = "form " + HttpContext.Current.Request.Form["user"],
                numero = 123,
                data = DateTime.Now,
                preco = 5.67
            };
            return Json(v);
        }

        [Route("api/iaago/login")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Autenticar([FromBody, Required] bdLoginAuthRequest req)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var result = new bdLoginAuthResult();

                if (req.CNPJ == "auto" && req.Senha == "auto")
                {
                    var cnUsr = context.ChannelUsers.FirstOrDefault(
                        c => c.ChannelId == req.ChannelId
                          && c.FromId == req.FromId);

                    if (cnUsr == null)
                    {
                        result.Mensagem = "NOT FOUND";
                        return Json(result);
                    }

                    var ent = context.Entidades.FirstOrDefault(e => e.IDEntidade == cnUsr.IDEntidade);
                    var usr = context.Usuarios.FirstOrDefault(u => u.IDEntidade == ent.IDEntidade && u.IDUsuario == cnUsr.IDUsuario && u.Ativo);
                    if (usr == null)
                    {
                        result.Mensagem = "Usuário não encontrado ou inativo";
                        return Json(result);
                    }

                    result.Mensagem = "OK";
                    result.Nome = usr.Nome;
                    result.Entidade = ent.Estabelecimento;
                    result.Auth = GeraAuth(ent, usr);
                    return Json(result);
                }

                var entidade = context.Entidades.FirstOrDefault(e => e.CNPJ == req.CNPJ);
                if (entidade == null)
                    result.Mensagem = "Estabelecimento não encontrada";
                else if (req.Senha == "BoT!ValidaCNPJ")
                {
                    result.Entidade = entidade.Estabelecimento;
                    result.Mensagem = "OK";
                }
                else
                {
                    var usuario = context.Usuarios.FirstOrDefault(u => u.IDEntidade == entidade.IDEntidade && u.Senha == req.Senha);
                    if (usuario == null)
                        result.Mensagem = "Usuário não encontrado";
                    else if (!usuario.Ativo)
                        result.Mensagem = "Usuário inativo";
                    else if (usuario.Senha != req.Senha)
                        result.Mensagem = "Senha inválida";
                    else
                    {
                        result.Mensagem = "OK";
                        result.Nome = usuario.Nome;
                        result.Entidade = entidade.Estabelecimento;
                        result.Auth = GeraAuth(entidade, usuario);

                        var cnUsr = context.ChannelUsers.FirstOrDefault(
                            c => c.IDEntidade == entidade.IDEntidade
                              && c.IDUsuario == usuario.IDUsuario
                              && c.ChannelId == req.ChannelId);

                        if (cnUsr == null)
                        {
                            cnUsr = new ChannelUser()
                            {
                                IDEntidade = entidade.IDEntidade,
                                IDUsuario = usuario.IDUsuario,
                                ChannelId = req.ChannelId,
                                FromId = req.FromId,
                                LastLogin = DateTime.Now
                            };
                            context.ChannelUsers.Add(cnUsr);
                        }
                        else
                        {
                            cnUsr.FromId = req.FromId;
                            cnUsr.LastLogin = DateTime.Now;
                            context.Entry(cnUsr).State = EntityState.Modified;
                        }

                        context.SaveChanges();

                        // Remove o usuário anterior caso exista
                        var cnUsrOld = context.ChannelUsers.FirstOrDefault(
                            c => c.ChannelId == req.ChannelId
                              && c.FromId == req.FromId
                              && c.IDChannelUser != cnUsr.IDChannelUser);

                        if (cnUsrOld != null)
                        {
                            context.Entry(cnUsrOld).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                    }
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/iaago/query/{nome}")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DynamicQuery(string nome, [FromUri, Required]string auth)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var q = context.Querys.FirstOrDefault(i => i.Nome == nome);
                if (q == null) return NotFound();

                var prms = new List<KeyValuePair<string, object>>();
                if (this.Request.Content.IsFormData())
                {
                    var formData = await this.Request.Content.ReadAsFormDataAsync();
                    foreach (string key in formData.Keys)
                    {
                        prms.Add(new KeyValuePair<string, object>(key, formData[key])); //String tudo!
                    }
                }

                var authprms = auth.Split('_');
                int entidade = int.Parse(authprms[1]);
                prms.Add(new KeyValuePair<string, object>("entidade", entidade));

                var rows = context.DynamicListFromSql(q.Query, prms);

                return Json(rows);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /*
        [Route("api/iaago/info")]
        [AcceptVerbs("POST")]
        public IHttpActionResult Info(bdInfoRequest req)
        {
            try
            {
                var result = new bdInfoResult();

                if (string.IsNullOrEmpty(req.Auth))
                {
                    result.Resposta = "Autenticação inesistente";
                    result.Tipo = ETipoInfoResult.Resposta;
                }
                else if (req.Pergunta == "#echo")
                {
                    result.Resposta = "Você disse: " + req.Pergunta;
                    result.Tipo = ETipoInfoResult.Resposta;
                }
                else if (req.Pergunta == "#pergunta")
                {
                    result.Resposta = "Qual sua idade";
                    result.Tipo = ETipoInfoResult.PerguntaNumero;
                }
                else if (req.Pergunta == "#hora")
                {
                    result.Resposta = "Server BigData: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    result.Tipo = ETipoInfoResult.PerguntaNumero;
                }
                else if (req.Pergunta == "#opcoes")
                {
                    result.Resposta = "Qual a forma de pagamento";
                    result.Tipo = ETipoInfoResult.ListaOpcoes;
                    result.Opcoes = new Dictionary<string, string>() {
                        { "1", "Debito" },
                        { "2", "Credito" },
                        { "3", "Boleto" }};
                }
                else if (req.Pergunta.StartsWith("#produto ") && req.Pergunta.Length > 10)
                {
                    req.Entidade = req.Pergunta.Substring(9);
                    result = IntencaoProduto(req);
                }
                else if (req.Intencao == "verificarEstoque")
                {
                    result = IntencaoProduto(req);
                }
                else if (string.IsNullOrEmpty(req.Intencao))
                {
                    result.Tipo = ETipoInfoResult.Resposta;
                    result.Resposta = "Ainda não sei responder sobre *" + req.Intencao + "* da entidade *" + req.Entidade + "*";
                }
                else
                {
                    // Não sabe responder (nulo)
                    return Ok();
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private bdInfoResult IntencaoProduto(bdInfoRequest req)
        {
            var result = new bdInfoResult()
            {
                Tipo = ETipoInfoResult.Resposta
            };

            // Se tiver errado deixa dar erro!!!
            var auth = req.Auth.Split(':');
            int entidade = int.Parse(auth[1]);

            if (string.IsNullOrEmpty(req.Entidade))
            {
                result.Resposta = "Não consegui entender o nome do produto, vou listar os produtos que você controla estoque:\r\n";
                var produtos = context.Produtos.Where(p => p.IDEntidade == entidade && p.ControlarEstoque).ToArray();

                foreach (var prod in produtos)
                    result.Resposta += $"* {prod.EstoqueAtual} *{prod.Nome}* R$ {prod.Valor.ToString("0.00")}\r\n";
            }
            else
            {
                var produtos = context.Produtos.Where(
                    p => p.IDEntidade == entidade
                      && p.Nome.Contains(req.Entidade)).ToArray();

                if (produtos.Length == 0)
                    result.Resposta = $"Não achei nenhum produto com *{req.Entidade}*";
                else if (produtos.Length > 10)
                    result.Resposta = $"Há mais de {produtos.Length} resultados com *{req.Entidade}* encontrados, refine a busca e tente novamente";
                else
                {
                    string lista = "";
                    foreach (var prod in produtos)
                        lista += $"* {prod.EstoqueAtual} *{prod.Nome}* R$ {prod.Valor.ToString("0.00")}\r\n";

                    if (produtos.Length == 1)
                        result.Resposta = lista.Substring(2);
                    else
                        result.Resposta = $"Encontrei {produtos.Length} produtos:\r\n" + lista;
                }
            }
            return result;
        }

    */
    }
}
