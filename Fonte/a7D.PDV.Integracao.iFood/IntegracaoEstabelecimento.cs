using a7D.PDV.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.iFood
{
    public partial class IntegraIFood
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
					cbd.Valor as 'ID PDV iFood',
					CASE
						WHEN c.DtAbertura is null THEN  0
						WHEN c.DtAbertura is not null THEN  1
					END as 'Status Caixa iFood'
				FROM
					tbConfiguracaoBD cbd
					INNER JOIN tbCaixa c ON c.IDPDV = cbd.Valor
				WHERE
					cbd.Chave = 'CaixaPDV'
					AND
					c.DtAbertura is not null
					AND
					c.DtFechamento is null
            ";

                da = new SqlDataAdapter(querySql, DB.ConnectionString);

                da.Fill(ds);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    AddLog("Loja do iFood fechada no sistema!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na verificação da loja aberta no sistema: " + ex.Message);
                return false;
            }
        }

        public Boolean LojaAbertaIfood()
        {
            Model.Merchant.Status status = APIMerchant.Status(ConfigIFood.merchantId)[0];

            if (status.state == "ERROR")
            {
                APIOrder.EventsPolling();
                Sleep(10);

                status = APIMerchant.Status(ConfigIFood.merchantId)[0];
            }

            AddLog("Estado: " + status.state);
            if (status.state == "OK" || status.state == "WARNING")
            {
                return true;
            }
            else if (status.state == "CLOSED")
            {
                AddLog("Loja fechada no iFood!");
                AddLog(JsonConvert.SerializeObject(status));

                return false;
            }
            else
            {
                AddLog("Loja fechada no iFood e com algum erro!");
                AddLog(JsonConvert.SerializeObject(status));

                return false;
            }
        }
    }
}
