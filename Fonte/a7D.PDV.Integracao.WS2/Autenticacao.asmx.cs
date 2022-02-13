using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for Autenticacao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Autenticacao : System.Web.Services.WebService
    {

        [WebMethod]
        public string ValidarPDV(Int32 idTipoPDV, String chaveHardware, String nome, DateTime horaAtual)
        {
            String xmlRetorno;
            try
            {
                PDVInformation pdv = new PDVInformation();

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                DateTime utcNow = DateTime.Now.ToUniversalTime();

                DateTime nowBrazil = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tzi);


                if (horaAtual >= nowBrazil.AddMinutes(2) || horaAtual <= nowBrazil.AddMinutes(-2))
                {
                    var ex = new ExceptionPDV(CodigoErro.C102);
                    ex.Data.Add("servidor", nowBrazil.ToString("dd/MM/yy HH:mm"));
                    ex.Data.Add("enviada", horaAtual.ToString("dd/MM/yy HH:mm"));
                    throw ex;
                }
                else if (idTipoPDV == 0)
                    throw new ExceptionPDV(CodigoErro.C101);

                var tipo = (ETipoPDV)idTipoPDV;
                using (var pdvServico = new Licencas())
                {
                    pdv = pdvServico.Carregar(tipo, chaveHardware, nome);
                }

                GA.PostEvento(pdv, $"Validar PDV: {tipo} {chaveHardware} {nome}");

                xmlRetorno = "<validarPDV>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <pdv>";
                xmlRetorno += "      <idPdv>" + pdv.IDPDV.ToString() + "</idPdv>";
                xmlRetorno += "      <nome>" + pdv.Nome + "</nome>";
                xmlRetorno += "    </pdv>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarPDV>";
            }
            catch (Exception ex)
            {
                xmlRetorno = "<validarPDV>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</validarPDV>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string Autenticar(string senha)
        {
            String xmlRetorno;

            try
            {
                var usuario = Usuario.Autenticar(senha);

                xmlRetorno = "<autenticacao>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
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
            catch (Exception ex)
            {
                xmlRetorno = "<autenticacao>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</autenticacao>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string ListarUsuarios(DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;

            try
            {
                List<UsuarioInformation> lista = BLL.Usuario.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                List<UsuarioInformation> listaExcluidos = BLL.Usuario.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();

                xmlRetorno = "<listarUsuarios>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
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
            catch (Exception ex)
            {
                xmlRetorno = "<listarUsuarios>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarUsuarios>";
            }

            return xmlRetorno;
        }
    }
}
