using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.Ativacao.WS
{
    public partial class Erros : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var cnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (var adpt = new SqlDataAdapter(
@"SELECT e.Data, c.Nome, p.Nome NomePDV, Aplicacao, e.Versao, e.Erro, e.StackTrace, e.Dados
FROM tbErro e
INNER JOIN tbAtivacao a ON e.ChaveAtivacao=a.ChaveAtivacao
INNER JOIN tbPDV p ON a.IDAtivacao=p.IDAtivacao AND e.idPDV=p.IDPDV_Instalacao
INNER JOIN tbcliente c ON a.IDCliente=c.IDCliente
ORDER BY e.Data DESC", cnString))
            {
                var tb = new DataTable();
                adpt.Fill(tb);
                gv.DataSource = tb;
                gv.DataBind();
            }
            /* Power BI
             * https://app.powerbi.com/view?r=eyJrIjoiYjk0NDM5NjQtYTcyZi00NTgyLTlkMTEtZTNjNzYxNTAxNWEwIiwidCI6ImJmODExZDUyLWViNTItNDcwYy04ZjU3LTAzMDA2OWU3YmJmOCJ9
             * http://wsativacaopdvseven.azurewebsites.net/erros.aspx
             * 
            SELECT Year(Data) Ano, Month(Data) Mes, Day(Data) Dia, Count(*) QTD
            FROM tbErro
            GROUP BY Year(Data) , Month(Data) , Day(Data)
            ORDER BY Ano DESC, Mes DESC, Dia DESC
            */

        }
    }
}