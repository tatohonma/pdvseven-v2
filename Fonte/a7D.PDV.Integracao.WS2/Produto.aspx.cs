using a7D.PDV.Integracao.API2.Model;
using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace a7D.PDV.Integracao.WS2.Iaago
{
    public partial class Produto : Page
    {
        public static ProdutoItem[] Itens = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.HttpMethod == "POST")
            {
                var jsonSerializer = new JavaScriptSerializer();
                var result = new ResultadoOuErro("OK");
                try
                {
                    using (var sr = new StreamReader(Request.InputStream))
                    {
                        var jsonProdutos = sr.ReadToEnd();
                        Itens = jsonSerializer.Deserialize<ProdutoItem[]>(jsonProdutos);
                    }
                }
                catch (Exception ex)
                {
                    result.Mensagem = ex.Message;
                    result.Codigo = 500;
                }

                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(jsonSerializer.Serialize(result));
                Response.End();
                return;
            }
            else
            {
                gv.DataSource = Itens;
                gv.DataBind();
            }
        }
    }

    public class ProdutoItem
    {
        public int IDProduto { get; set; }
        public string Nome { get; set; }
        public int IDProdutoGlobal { get; set; }
        public string EAN { get; set; }
        public decimal QTDatual { get; set; }
        public decimal Valor { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public decimal EstoqueIdeal { get; set; }
        public int EstoqueFrequenciaValor { get; set; } // Numero de dias, semenas ou meses
        public string EstoqueFrequenciaTipo { get; set; } // Semanal, Mensal, Anual, ou dia da semana: SEG, TER, QUA, QUI, SEX, SAB, DOM
    }
}