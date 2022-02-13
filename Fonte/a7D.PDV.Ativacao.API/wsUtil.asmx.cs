using a7D.PDV.Ativacao.API.Services;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Services;

namespace a7D.PDV.Ativacao.API
{
    /*
    CREATE TABLE [dbo].[tbErro2](
        [idErro] [int] IDENTITY(1,1) NOT NULL,
        [Data] [datetime] NOT NULL,
        [ChaveAtivacao] [nvarchar](50) NOT NULL,
        [Aplicacao] [nvarchar](50) NULL,
        [Versao] [nvarchar](50) NULL,
        [idPDV] [int] NOT NULL,
        [Codigo] [nvarchar](10) NOT NULL,
        [Erro] [text] NULL,
        [StackTrace] [text] NULL,
        [Dados] [text] NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    */
    /// <summary>
    /// Summary description for CFeService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class wsUtil : WebService
    {
        private static readonly string ErrosDestinatarios = ConfigurationManager.AppSettings["ErrosDestinatarios"];
        private static readonly string ErrosVersaoMin = ConfigurationManager.AppSettings["ErrosVersaoMin"];
        private static readonly string[] ErrosCodigoIgnorar = ConfigurationManager.AppSettings["ErrosCodigoIgnorar"].Split(',');
        private static readonly string[] ErrosMensagemIgnorar = ConfigurationManager.AppSettings["ErrosMensagemIgnorar"].Split('|');
        private static readonly string[] ErrosNotificar = ConfigurationManager.AppSettings["ErrosNotificar"].Split(',');

        [WebMethod]
        public string Erro(string chaveAtivacao, string aplicacao, string versao, int idPDV, string codigo, string erro, string stackTrace, string dados)
        {
            try
            {
                string resultado = "Registrado";

                if (ErrosCodigoIgnorar.Any(e => codigo.StartsWith(e)))
                    return "Ignorado Código";

                else if (ErrosMensagemIgnorar.Any(e => erro.Contains(e)) || ErrosMensagemIgnorar.Any(e => stackTrace.Contains(e)))
                    return "Ignorado Mensagem";

                else if (new Version(versao) < new Version(ErrosVersaoMin))
                    return "Ignorado Versão";

                if (ErrosNotificar.Any(e => codigo.StartsWith(e)))
                {
                    resultado = EmailServices.EnviarErro(ErrosDestinatarios, chaveAtivacao, aplicacao, versao, idPDV, codigo, erro, stackTrace, dados);
                    if (resultado == "OK")
                        resultado = "Notificado";
                    else
                        dados = (dados ?? "") + "\r\nNotificação:" + resultado;
                }

                ClientesService.Registra(chaveAtivacao, versao, codigo, true);

                // Minimo uso de qualquer componente ou tecnologia externa para facilitar ou que possa depender ou gerar qualquer erro
                var cnString = ConfigurationManager.ConnectionStrings["AtivacaoContext"].ConnectionString;
                using (var cn = new SqlConnection(cnString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(
                        @"INSERT INTO tbErro2(Data,ChaveAtivacao,Aplicacao,Versao,idPDV,Codigo,Erro,StackTrace,Dados) 
                                      VALUES(@Data,@ChaveAtivacao,@Aplicacao,@Versao,@idPDV,@Codigo,@Erro,@StackTrace,@Dados)", cn))
                    {
                        cmd.Parameters.AddWithValue("@Data", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ChaveAtivacao", chaveAtivacao);
                        cmd.Parameters.AddWithValue("@Aplicacao", aplicacao ?? "-");
                        cmd.Parameters.AddWithValue("@Versao", versao ?? "-");
                        cmd.Parameters.AddWithValue("@idPDV", idPDV);
                        cmd.Parameters.AddWithValue("@Codigo", codigo);
                        cmd.Parameters.AddWithValue("@Erro", erro ?? "-");
                        cmd.Parameters.AddWithValue("@StackTrace", stackTrace ?? "-");
                        cmd.Parameters.AddWithValue("@Dados", dados ?? "-");
                        cmd.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        [WebMethod]
        public string CFE(string destinatario, string nomeFantasia, string cnpj, string dataDe, string dataAte, string nomeArquivo, Byte[] bytesArquivo)
        {
            try
            {
                var ldReplacements = new ListDictionary
                {
                    { "<%NOMEFANTASIA%>", nomeFantasia },
                    { "<%CNPJ%>", cnpj },
                    { "<%DATADE%>", dataDe },
                    { "<%DATAATE%>", dataAte }
                };

                using (var ms = new MemoryStream())
                {
                    ms.Write(bytesArquivo, 0, bytesArquivo.Length);
                    ms.Flush();
                    ms.Position = 0;

                    var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Zip);
                    var attach = new Attachment(ms, nomeArquivo);
                    attach.ContentDisposition.FileName = nomeArquivo;

                    var titulo = $"XML CF-e {nomeFantasia} {cnpj}";
                    var template = Server.MapPath(@"~\TemplatesHtml\emailCfe.html");
                    return EmailServices.EnviarTemplate(destinatario, titulo, template, ldReplacements, attach);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [WebMethod]
        public string EnviarNFEAoCliente(DadosNfeCliente dados)
        {
            try
            {
                var ldReplacements = new ListDictionary
                {
                    { "<%NOMEFANTASIA%>", dados.NomeFantasiaEmissor },
                    { "<%CPFCNPJ%>", dados.CPFCNPJCliente },
                    { "<%DATA%>", dados.DataEmissao },
                    { "<%CHAVEACESSO%>", dados.ChaveAcessoNF },
                    { "<%NUMERONOTA%>", dados.NumeroNota },
                    { "<%VALORTOTAL%>", dados.ValorTotal }

                };




                using (var ms = new MemoryStream())
                {
                    var nomeArquivo = $"xml{dados.ChaveAcessoNF}.xml";
                    var bytes = Convert.FromBase64String(dados.XMLBase64);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Flush();
                    ms.Position = 0;

                    var attach = new Attachment(ms, nomeArquivo);
                    attach.ContentDisposition.FileName = nomeArquivo;


                    var titulo = $"Nota Fiscal Eletrônica - {dados.NomeFantasiaEmissor}";
                    var template = Server.MapPath(@"~\TemplatesHtml\emailNfeCliente.html");
                    return EmailServices.EnviarTemplate(dados.Email, titulo, template, ldReplacements, attach);

                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class DadosNfeCliente
    {
        public string CPFCNPJCliente { get; set; }
        public string NomeFantasiaEmissor { get; set; }
        public string ChaveAcessoNF { get; set; }
        public string DataEmissao { get; set; }
        public string NumeroNota { get; set; }
        public string ValorTotal { get; set; }
        public string Email { get; set; }
        public string UF { get; set; }
        public string XMLBase64 { get; set; }
    }
}