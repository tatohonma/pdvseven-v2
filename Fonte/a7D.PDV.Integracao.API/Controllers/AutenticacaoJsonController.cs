using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Integracao.API.ExtensionMethods;
using a7D.PDV.Integracao.API.Responses;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class AutenticacaoJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public class DadosValidacao
        {
            public int IDTipoPDV { get; set; }
            public string ChaveHardware { get; set; }
            public string Nome { get; set; }
            public DateTime HoraAtual { get; set; }
        }

        [HttpPost]
        public IHttpActionResult ValidarPDV([FromBody]DadosValidacao dados)
        {
            try
            {
                var response = new ValidarPdvResponse();

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                DateTime utcNow = DateTime.Now.ToUniversalTime();

                DateTime nowBrazil = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tzi);

                if (dados.HoraAtual >= nowBrazil.AddMinutes(2) || dados.HoraAtual <= nowBrazil.AddMinutes(-2))
                    throw new Exception("Favor verificar a hora do PDV! No servidor são " + nowBrazil.ToString("dd/MM/yy HH:mm") + " hora enviada: " + dados.HoraAtual.ToString
                        ("dd/MM/yy HH:mm"));

                var pdv = new Licencas().Carregar((ETipoPDV)dados.IDTipoPDV, dados.ChaveHardware, dados.Nome);

                response.idPdv = pdv.IDPDV.Value;
                response.nome = pdv.Nome;

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    versao = Versao,
                    mensagemErro = e.Message,
                    detalhes = e.ToString()
                };

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost]
        public IHttpActionResult Autenticar([FromBody]string senha)
        {
            try
            {
                var response = new AutenticarResponse();

                var usuario = Usuario.Autenticar(senha);

                response.usuario = new AutenticarResponse.Usuario
                {
                    idUsuario = usuario.IDUsuario.Value,
                    nome = usuario.Nome,
                    permissoes = new AutenticarResponse.Permissoes
                    {
                        adm = usuario.PermissaoAdm == true,
                        caixa = usuario.PermissaoCaixa == true,
                        garcom = usuario.PermissaoGarcom == true,
                        gerente = usuario.PermissaoGerente == true,
                    }
                };

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.Unauthorized
                });
            }
        }

        [HttpGet]
        public IHttpActionResult ListarUsuarios([FromUri]string ultimaVerificacao)
        {
            DateTime dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            var response = new ListarUsuariosResponse();
            var listaUsuarios = new List<ListarUsuariosResponse.Usuario>();

            Usuario
                .Listar()
                .Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao)
                .ToList()
                .ForEach(usuario =>
                {
                    listaUsuarios.Add(new ListarUsuariosResponse.Usuario
                    {
                        idUsuario = usuario.IDUsuario.Value,
                        nome = usuario.Nome,
                        ativo = true,
                        dtUltimaAlteracao = usuario.DtUltimaAlteracao.Value
                    });
                });

            Usuario
                .ListarExcluidos()
                .Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao)
                .ToList()
                .ForEach(usuario =>
                {
                    listaUsuarios.Add(new ListarUsuariosResponse.Usuario
                    {
                        idUsuario = usuario.IDUsuario.Value,
                        nome = usuario.Nome,
                        ativo = false,
                        dtUltimaAlteracao = usuario.DtUltimaAlteracao.Value
                    });
                });

            response.usuarios = listaUsuarios.ToArray();

            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
