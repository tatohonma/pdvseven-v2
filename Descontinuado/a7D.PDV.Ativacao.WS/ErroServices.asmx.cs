using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

namespace a7D.PDV.Ativacao.WS
{
    /*
    CREATE TABLE [dbo].[tbErro](
        [idErro] [int] IDENTITY(1,1) NOT NULL,
        [Data] [datetime] NOT NULL,
        [ChaveAtivacao] [nvarchar](50) NOT NULL,
        [Aplicacao] [nvarchar](50) NULL,
        [Versao] [nvarchar](50) NULL,
        [idPDV] [int] NOT NULL,
        [Codigo] [int] NOT NULL,
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
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ErroServices : System.Web.Services.WebService
    {
        [WebMethod]
        public string Enviar(string chaveAtivacao, string aplicacao, string versao, int idPDV, int codigo, string erro, string stackTrace, string dados)
        {
            try
            {
                // Minimo uso de qualquer componente ou tecnologia externa para facilitar ou que possa depender ou gerar qualquer erro
                var cnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                using (var cn = new SqlConnection(cnString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(
                        @"INSERT INTO tbErro(Data,ChaveAtivacao,Aplicacao,Versao,idPDV,Codigo,Erro,StackTrace,Dados) 
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
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}