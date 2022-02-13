using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using a7D.PDV.BLL;
using a7D.PDV.Model;

namespace a7D.PDV.Integracao.eMenu.UI
{
   public class Produto
    {
       public Produto()
       {
       }

       public String SincItens()
       {
           Int32 qtdTotal = 0;
           Int32 qtdSucesso = 0;
           Int32 qtdErro = 0;

           String strDataUltimaSinc = Configuracao.RetornarValor("sincEMENU_DtAtualizacaoItens");
           DateTime dtUltimaSinc = new DateTime();
           DateTime dtSinc = DateTime.Now;

           if(strDataUltimaSinc != null && strDataUltimaSinc != "")
               dtUltimaSinc = Convert.ToDateTime(strDataUltimaSinc);

           List<ProdutoInformation> listaProduto = BLL.Produto.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaSinc && l.TipoProduto != null && l.TipoProduto.IDTipoProduto == 10).ToList();
           String xml;
           qtdTotal = listaProduto.Count();

           if (qtdTotal > 0)
           {
               xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
               xml += "<POS_ITEM_LIST>\r\n";
               foreach (var item in listaProduto)
               {
                   xml += " <ITEM>\r\n";
                   xml += "  <ITEM_ID>" + item.IDProduto + "</ITEM_ID>\r\n";
                   xml += "  <ITEM_NAME>" + item.Nome + "</ITEM_NAME>\r\n";
                   xml += "  <DESCRIPTION>" + item.Descricao + "</DESCRIPTION>\r\n";
                   xml += "  <RECIPE>0</RECIPE>\r\n";
                   xml += "  <IMAGE_PATH></IMAGE_PATH>\r\n";
                   xml += "  <PREPARATION_TIME>0</PREPARATION_TIME>\r\n";
                   xml += "  <CALORIE>0</CALORIE>\r\n";
                   xml += "  <SERVINGS>0</SERVINGS>\r\n";
                   xml += "  <ISACTIVE>" + (item.Quantidade > 0 ? "true" : "false") + "</ISACTIVE>\r\n";
                   xml += "  <ISDELETED>" + (!item.Ativo).ToString() + "</ISDELETED>\r\n";
                   xml += "  <PRICE1>" + item.ValorUnitario + "</PRICE1>\r\n";
                   xml += "  <PRICE2></PRICE2>\r\n";
                   xml += "  <PRICE3></PRICE3>\r\n";
                   xml += "  <PRICE4></PRICE4>\r\n";
                   xml += "  <PRICE5></PRICE5>\r\n";
                   xml += "  <DEFAULT_PRICE>1</DEFAULT_PRICE>\r\n";
                   xml += "  <TAX1></TAX1>\r\n";
                   xml += "  <TAX2></TAX2>\r\n";
                   xml += "  <TAX3></TAX3>\r\n";
                   xml += "  <CALCULATE_TAX2_AFTER_TAX1>false</CALCULATE_TAX2_AFTER_TAX1>\r\n";
                   xml += "  <CALCULATE_TAX3_AFTER_TAX1>false</CALCULATE_TAX3_AFTER_TAX1>\r\n";
                   xml += "  <CALCULATE_TAX3_AFTER_TAX2>false</CALCULATE_TAX3_AFTER_TAX2>\r\n";
                   xml += " </ITEM>\r\n";

                   qtdSucesso++;
               }

               xml += "</POS_ITEM_LIST>";

               String caminhoXMLItens = Configuracao.RetornarValor("sincEMENU_CaminhoXMLItens") + "\\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xml";
               StreamWriter arquivoXML = new StreamWriter(caminhoXMLItens);
               arquivoXML.Write(xml);
               arquivoXML.Close();
           }

           Configuracao.Alterar("sincEMENU_DtAtualizacaoItens", dtSinc.ToString());

           return qtdTotal + "|" + qtdSucesso + "|" + qtdErro;
       }
       public String SincModificacoes()
       {
           Int32 qtdTotal = 0;
           Int32 qtdSucesso = 0;
           Int32 qtdErro = 0;

           String strDataUltimaSinc = Configuracao.RetornarValor("sincEMENU_DtAtualizacaoModificacoes");
           DateTime dtUltimaSinc = new DateTime();
           DateTime dtSinc = DateTime.Now;

           if (strDataUltimaSinc != null && strDataUltimaSinc != "")
               dtUltimaSinc = Convert.ToDateTime(strDataUltimaSinc);

           List<ProdutoInformation> listaProduto = BLL.Produto.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaSinc && l.TipoProduto != null && l.TipoProduto.IDTipoProduto == 20).ToList();
           String xml;
           qtdTotal = listaProduto.Count();

           if (qtdTotal > 0)
           {
               xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
               xml += "<POS_MODIFIER_LIST>\r\n";
               foreach (var item in listaProduto)
               {
                   xml += " <MODIFIER>\r\n";
                   xml += "   <MODIFIER_ID>" + item.IDProduto + "</MODIFIER_ID>\r\n";
                   xml += "   <MODIFIER_NAME>" + item.Nome + "</MODIFIER_NAME>\r\n";
                   xml += "   <DESCRIPTION>" + item.Descricao + "</DESCRIPTION>\r\n";
                   xml += "   <ISACTIVE>" + (item.Quantidade > 0 ? "true" : "false") + "</ISACTIVE>\r\n";
                   xml += "   <PRICE1>" + item.ValorUnitario + "</PRICE1>\r\n";
                   xml += "   <PRICE2></PRICE2>\r\n";
                   xml += "   <PRICE3></PRICE3>\r\n";
                   xml += "   <PRICE4></PRICE4>\r\n";
                   xml += "   <PRICE5></PRICE5>\r\n";
                   xml += "   <DEFAULT_PRICE>1</DEFAULT_PRICE>\r\n";
                   xml += "   <IGNORE_ITEM_PRICE>false</IGNORE_ITEM_PRICE>\r\n";
                   xml += "   <TAX1></TAX1>\r\n";
                   xml += "   <TAX2></TAX2>\r\n";
                   xml += "   <TAX3></TAX3>\r\n";
                   xml += "   <CALCULATE_TAX2_AFTER_TAX1>false</CALCULATE_TAX2_AFTER_TAX1>\r\n";
                   xml += "   <CALCULATE_TAX3_AFTER_TAX1>false</CALCULATE_TAX3_AFTER_TAX1>\r\n";
                   xml += "   <CALCULATE_TAX3_AFTER_TAX2>false</CALCULATE_TAX3_AFTER_TAX2>\r\n";
                   xml += " </MODIFIER>\r\n";

                   qtdSucesso++;
               }

               xml += "</POS_MODIFIER_LIST>";

               String caminhoXMLItens = Configuracao.RetornarValor("sincEMENU_CaminhoXMLModificacoes") + "\\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xml";
               StreamWriter arquivoXML = new StreamWriter(caminhoXMLItens);
               arquivoXML.Write(xml);
               arquivoXML.Close();
           }

           Configuracao.Alterar("sincEMENU_DtAtualizacaoModificacoes", dtSinc.ToString());

           return qtdTotal + "|" + qtdSucesso + "|" + qtdErro;
       }
    }
}
