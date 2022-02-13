using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using a7D.PDV.BLL.Entity;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class LicencasTest
    {
        [TestMethod]
        public void SincronizarLicencas()
        {
            using (var pdvServico = new PdvServico())
            {
                try
                {
                    pdvServico.SincronizarLicencas(BLL.PDV.TipoApp.SERVER);
                    pdvServico.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void InvalidarLicenca()
        {
            BLL.PDV.AlterarDataValidade(DateTime.Now.AddDays(-1));
        }
    }
}
