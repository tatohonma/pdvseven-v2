using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.WS2.Properties;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class AutenticacaoController : ApiController
    {
        private const string pathDownload = "release";

        [HttpGet]
        [Route("api/autenticacao/pdv/{tipoPDV}/{hardware}")]
        public IHttpActionResult ValidarPDV(int tipoPDV, string hardware)
        {
            return ValidarPDV(new pdvRequest { tipoPDV = tipoPDV, hardware = hardware });
        }

        [HttpGet]
        [Route("api/download")]
        public IHttpActionResult Download([FromUri]string prefixo, [FromUri]string sufixo)
        {
            return Ok(UltimaVersao(prefixo, sufixo));
        }

        [HttpGet]
        [Route("api/versao")]
        public IHttpActionResult Versao()
        {
            return Ok(AC.Versao);
        }

        [HttpPost]
        [Route("api/autenticacao/pdv")]
        public IHttpActionResult ValidarPDV(pdvRequest request)
        {
            try
            {
                if (request == null)
                    throw new ExceptionPDV(CodigoErro.E08E);
                else if (request.tipoPDV == 0)
                    throw new ExceptionPDV(CodigoErro.C101);
                else if (request.hardware == null)
                    throw new ExceptionPDV(CodigoErro.F07F);

                using (var pdvServico = new Licencas())
                {
                    ETipoPDV tipo = (ETipoPDV)request.tipoPDV;
                    if (tipo == ETipoPDV.POS_INTEGRADO_STONE)
                    {
                        var gw = EF.Repositorio.Carregar<tbTipoPagamento>(t => t.IDGateway == (int)EGateway.StonePOS);
                        if (gw == null)
                            throw new ExceptionPDV(CodigoErro.EESB);
                    }
                    else if (tipo == ETipoPDV.POS_INTEGRADO_NTK)
                    {
                        var gw = EF.Repositorio.Carregar<tbTipoPagamento>(t => t.IDGateway == (int)EGateway.NTKPOS);
                        if (gw == null)
                            throw new ExceptionPDV(CodigoErro.EESC);
                    }
                    else if (tipo == ETipoPDV.SAIDA)
                    {
                        if (request.versaoAPP != null)
                        {
                            if(request.versaoAPP.Split('.').Length==3)
                            {
                                // APP Android Versão com 3 digitos
                                if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoSaida))
                                {
                                    return Ok(new pdvResult()
                                    {
                                        Mensagem = "download:" + UltimaVersao("saida-", ".apk") + "?",
                                        versaoWS = AC.Versao
                                    });
                                }
                            }
                            else
                            {
                                // APP Windows Versão com 4 digitos!
                                if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoSaida))
                                {
                                    return Ok(new pdvResult()
                                    {
                                        Mensagem = "download:release/update.zip",
                                        versaoWS = AC.Versao
                                    });
                                }
                            }
                            
                        }
                        else
                            throw new ExceptionPDV(CodigoErro.C000);
                    }
                    else if (tipo == ETipoPDV.COMANDA_ELETRONICA || tipo == ETipoPDV.TERMINAL_TAB)
                    {
                        if (request.versaoAPP != null)
                        {
                            if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoComanda))
                            {
                                return Ok(new pdvResult()
                                {
                                    //Mensagem = "download:release/comanda-" + VersaoComanda + ".apk?",
                                    Mensagem = "download:" + UltimaVersao("comanda-", ".apk") + "?",
                                    versaoWS = AC.Versao
                                });
                            }
                        }
                        else
                            throw new ExceptionPDV(CodigoErro.C000);
                    }
                    else if (tipo == ETipoPDV.CARDAPIO_DIGITAL)
                    {
                        if (request.versaoAPP != null)
                        {
                            if (request.versaoAPP == Settings.Default.VersaoCardapio)
#pragma warning disable CS0642 // Possible mistaken empty statement
                                ; // nada
#pragma warning restore CS0642 // Possible mistaken empty statement
                            else if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoCardapio))
                            {
                                return Ok(new pdvResult()
                                {
                                    Mensagem = "download:" + UltimaVersao("cardapio-", ".apk") + "?",
                                    versaoWS = AC.Versao
                                });
                            }
                        }
                        else
                            throw new ExceptionPDV(CodigoErro.C000);
                    }
                    else if (tipo == ETipoPDV.TORNEIRA )
                    {
                        if (request.versaoAPP != null)
                        {
                            if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoTorneira))
                            {
                                return Ok(new pdvResult()
                                {
                                    Mensagem = "download:" + UltimaVersao("torneira-", ".apk") + "?",
                                    versaoWS = AC.Versao
                                });
                            }
                        }
                        else
                            throw new ExceptionPDV(CodigoErro.C000);
                    }
                    else if (tipo == ETipoPDV.AUTOATENDIMENTO)
                    {
                        if (request.versaoAPP == null)
                            throw new ExceptionPDV(CodigoErro.C000);
                        else if (new Version(request.versaoAPP) < new Version(Settings.Default.VersaoAutoatendimento))
                            throw new ExceptionPDV(CodigoErro.C103);
                    }

                    var pdv = pdvServico.Carregar((ETipoPDV)request.tipoPDV, request.hardware);

                    GA.PostEvento(pdv, $"Validar PDV: {tipo} {request.hardware} {request.versaoAPP}");

                    return Ok(new pdvResult()
                    {
                        Mensagem = "OK",
                        idPDV = pdv.IDPDV.Value,
                        nome = pdv.Nome,
                        versaoWS = AC.Versao
                    });
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound,
                    new Erro(CodigoErro.F081, ex, $"{request?.tipoPDV}: {request?.hardware} {request?.versaoAPP}")));
            }
        }

        internal static string UltimaVersao(string prefixo, string sufixo)
        {
            var pathRelease = System.Web.Hosting.HostingEnvironment.MapPath("~/" + pathDownload);
            var di = new DirectoryInfo(pathRelease);
            if (!di.Exists)
                throw new Exception("Pasta 'release' não existe");

            var files = di.GetFiles(prefixo + "*" + sufixo);
            string result = null;
            Version maxVersion = null;
            foreach (var file in files)
            {
                try
                {
                    string cVersao = file.Name.Substring(prefixo.Length);
                    cVersao = cVersao.Substring(0, cVersao.Length - sufixo.Length);
                    var versao = new Version(cVersao);
                    if (maxVersion == null || versao > maxVersion)
                    {
                        maxVersion = versao;
                        result = pathDownload + "/" + file.Name;
                    }
                }
                catch (Exception)
                {
                }
            }
            return result;
        }

        [HttpGet]
        [Route("api/autenticacao/senha/{chave}")]
        public IHttpActionResult Autenticar(string chave)
        {
            return Autenticar(new usuarioRequest(chave));
        }

        [HttpPost]
        [Route("api/autenticacao/usuario")]
        public IHttpActionResult Autenticar([FromBody]usuarioRequest senha)
        {
            try
            {
                if (senha == null || senha.senha == null)
                    throw new ExceptionPDV(CodigoErro.A005);

                var usuario = Usuario.Autenticar(senha.senha);

                GA.PostEvento($"Usuario", usuario: usuario);

                return Ok(new usuarioResult()
                {
                    idUsuario = usuario.IDUsuario.Value,
                    nome = usuario.Nome
                });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound,
                    new Erro(CodigoErro.E055, ex)));
            }
        }
    }
}
