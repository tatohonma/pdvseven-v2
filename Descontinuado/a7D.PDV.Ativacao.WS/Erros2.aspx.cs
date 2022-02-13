using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace a7D.PDV.Ativacao.WS
{
    public partial class Erros2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var cnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (var adpt = new SqlDataAdapter(
@"SELECT e.Data, cc.Nome Cliente, e.Aplicacao, e.Versao, e.Erro FROM tbErro e 
INNER JOIN tbAtivacao aa ON e.ChaveAtivacao=aa.ChaveAtivacao 
INNER JOIN tbcliente cc ON aa.IDCliente=cc.IDCliente
WHERE DATEDIFF(day, e.Data, GETDATE())<30
ORDER BY e.Data DESC", cnString))
            {
                var tb = new DataTable();
                adpt.Fill(tb);
                gv.DataSource = tb;
                gv.DataBind();
            }
        }
    }
}