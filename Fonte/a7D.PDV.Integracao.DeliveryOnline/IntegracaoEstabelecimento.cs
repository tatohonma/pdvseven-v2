using a7D.PDV.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    public partial class IntegraDeliveryOnline
    {
        public Boolean LojaAbertaSistema()
        {
            try
            {
                SqlDataAdapter da;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                String querySql = @"
				    SELECT
					    CASE
						    WHEN c.DtAbertura is null THEN  0
						    WHEN c.DtAbertura is not null THEN  1
					    END as 'Status Caixa'
				    FROM
					    tbCaixa c
				    WHERE
					    c.DtAbertura is not null
					    AND
					    c.DtFechamento is null
                        AND
                        c.IDPDV=@IDPDV
                ";

                da = new SqlDataAdapter(querySql, DB.ConnectionString);
                da.SelectCommand.Parameters.AddWithValue("@IDPDV", ConfigDO.CaixaPDV);

                da.Fill(ds);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    AddLog("Loja do Delivery Online fechada no sistema! (Verificar abertura de caixa)");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na verificação da loja aberta no sistema: " + ex.Message);
                return false;
            }
        }

        public Boolean LojaAbertaDeliveryOnline()
        {
            Boolean ret = false;
            Boolean lojaAberta = false;

            //Model.Merchant.MerchantSummary[] listMerchant = APIMerchant.List();

            //foreach (var merchant in listMerchant)
            //{
            //    AddLog("Verificando loja " + merchant.name);

            //    lojaAberta = LojaAbertaIfood(merchant.id);
            //    if (lojaAberta)
            //        ret = true;
            //}

            //return ret;

            return true;
        }

        //public Boolean LojaAbertaIfood(string merchantId)
        //{
        //    try
        //    {
        //        Model.Merchant.Status status = APIMerchant.Status(merchantId)[0];

        //        if (status.state == "ERROR")
        //        {
        //            APIOrder.EventsPolling();
        //            Sleep(10);

        //            status = APIMerchant.Status(merchantId)[0];
        //        }

        //        AddLog("Estado: " + status.state);
        //        if (status.state == "OK" || status.state == "WARNING")
        //        {
        //            return true;
        //        }
        //        else if (status.state == "CLOSED")
        //        {
        //            AddLog("Loja fechada no iFood!");
        //            AddLog(JsonConvert.SerializeObject(status));

        //            return false;
        //        }
        //        else
        //        {
        //            AddLog("Loja fechada no iFood e com algum erro!");
        //            AddLog(JsonConvert.SerializeObject(status));

        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AddLog("Erro na verificação da loja aberta no iFood: " + ex.Message);
        //        return false;
        //    }
        //}
    }
}
