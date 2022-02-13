using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class AutenticacaoController : ApiController
    {
        String Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public class DadosValidacao
        {
            public int IDTipoPDV { get; set; }
            public string ChaveHardware { get; set; }
            public string Nome { get; set; }
            public DateTime HoraAtual { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage ValidarPDV([FromBody]DadosValidacao dados)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            PDVInformation pdv = new PDVInformation();

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            DateTime utcNow = DateTime.Now.ToUniversalTime();

            DateTime nowBrazil = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tzi);

            try
            {
                if (dados.HoraAtual >= nowBrazil.AddMinutes(2) ||dados.HoraAtual <= nowBrazil.AddMinutes(-2))
                    throw new Exception("Favor verificar a hora do PDV! No servidor são " + nowBrazil.ToString("dd/MM/yy HH:mm") + " hora enviada: " + dados.HoraAtual.ToString
                        ("dd/MM/yy HH:mm"));

                pdv = new Licencas().Carregar((ETipoPDV)dados.IDTipoPDV, dados.ChaveHardware, dados.Nome);

                xmlRetorno = "<validarPDV>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <pdv>";
                xmlRetorno += "      <idPdv>" + pdv.IDPDV.ToString() + "</idPdv>";
                xmlRetorno += "      <nome>" + pdv.Nome + "</nome>";
                xmlRetorno += "    </pdv>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarPDV>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.Unauthorized;
                xmlRetorno = "<validarPDV>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarPDV>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpPost]
        public HttpResponseMessage Autenticar(string senha)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            UsuarioInformation usuario = new UsuarioInformation();

            try
            {
                usuario = Usuario.Autenticar(senha);

                xmlRetorno = "<autenticacao>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <usuario>";
                xmlRetorno += "      <idUsuario>" + usuario.IDUsuario.ToString() + "</idUsuario>";
                xmlRetorno += "      <nome>" + usuario.Nome + "</nome>";
                xmlRetorno += "      <permissaoAdm>" + Convert.ToInt32(usuario.PermissaoAdm.Value) + "</permissaoAdm>";
                xmlRetorno += "      <permissaoCaixa>" + Convert.ToInt32(usuario.PermissaoCaixa.Value) + "</permissaoCaixa>";
                xmlRetorno += "      <permissaoGarcom>" + Convert.ToInt32(usuario.PermissaoGarcom.Value) + "</permissaoGarcom>";
                xmlRetorno += "      <permissaoGerente>" + Convert.ToInt32(usuario.PermissaoGerente.Value) + "</permissaoGerente>";
                xmlRetorno += "    </usuario>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</autenticacao>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.Unauthorized;
                xmlRetorno = "<autenticacao>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</autenticacao>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        public HttpResponseMessage ListarUsuarios([FromUri]DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;

            try
            {
                List<UsuarioInformation> lista = BLL.Usuario.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                List<UsuarioInformation> listaExcluidos = BLL.Usuario.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();

                xmlRetorno = "<listarUsuarios>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaUsuario>";
                foreach (var usuario in lista)
                {
                    xmlRetorno += "      <usuario>";
                    xmlRetorno += "        <idUsuario>" + usuario.IDUsuario + "</idUsuario>";
                    xmlRetorno += "        <nome>" + usuario.Nome + "</nome>";
                    xmlRetorno += "        <ativo>" + Convert.ToInt16(usuario.Ativo.Value) + "</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + usuario.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";

                    xmlRetorno += "      </usuario>";
                }

                foreach (var usuario in listaExcluidos)
                {
                    xmlRetorno += "      <usuario>";
                    xmlRetorno += "        <idUsuario>" + usuario.IDUsuario + "</idUsuario>";
                    xmlRetorno += "        <nome>" + usuario.Nome + "</nome>";
                    xmlRetorno += "        <ativo>0</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + usuario.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";

                    xmlRetorno += "      </usuario>";
                }

                xmlRetorno += "    </listaUsuario>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarUsuarios>";


            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarUsuarios>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarUsuarios>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

    }
}
