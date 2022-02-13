using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class PDV2Test
    {
        //[TestMethod]
        //public void SaveTest()
        //{
        //    SqlDataAdapter da;
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    string querySql = @"
        //        SELECT * FROM tbPDV3 (nolock)
        //    ";

        //    da = new SqlDataAdapter(querySql, ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        //    da.Fill(ds);
        //    dt = ds.Tables[0];


        //    foreach (var pdv in dt.AsEnumerable())
        //    {
        //        var pdv2 = new PDVInformation
        //        {
        //            TipoPDV = new TipoPDVInformation { IDTipoPDV = pdv.Field<int>("IDTipoPDV") },
        //            ChaveHardware = pdv.Field<string>("ChaveHardware"),
        //            Nome = pdv.Field<string>("Nome"),
        //            UltimaAlteracao = pdv.Field<DateTime?>("UltimaAlteracao"),
        //            UltimoAcesso = pdv.Field<DateTime?>("UltimoAcesso"),
        //            Versao = pdv.Field<string>("Versao")
        //        };
        //        BLL.PDV.Salvar(pdv2);
        //    }

        //    ds.Dispose();
        //    da.Dispose();
        //}

        [TestMethod]
        public void LoadTest()
        {
            var now = DateTime.Now;
            const int idPDV = 92;
            var pdv2 = BLL.PDV.Carregar(idPDV);
            pdv2.UltimoAcesso = now;
            BLL.PDV.Salvar(pdv2);
            pdv2 = BLL.PDV.Carregar(idPDV);

            Assert.AreEqual(now.ToString("yyyyMMddHHmmssfff"), pdv2.UltimoAcesso.Value.ToString("yyyyMMddHHmmssfff"));
        }

        [TestMethod]
        public void ValidationTest()
        {
            BLL.PDV.Listar();
        }
    }
}
