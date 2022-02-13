using a7D.PDV.Integracao.Server.BigData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BigData.WebAPI.Tests
{
    [TestClass]
    class IntegradorServices
    {
        [TestMethod]
        public void IntegracaoServices_BigData_Test()
        {
            // Arrange
            var server = new BigDataServices();
            server.AddLog += (info) => Console.WriteLine(info);

            // Act
            server.Sync();

            // Assert

        }
    }
}
