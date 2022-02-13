using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class Epsilon
    {
        [TestMethod]
        public void TestEpsilon()
        {
            var epsilon = 1.0;
            while (1.0 + 0.5 * epsilon != 1.0)
            {
                epsilon = 0.5 * epsilon;
            }
            Debug.WriteLine(epsilon.ToString("0.0##############################################"));

            var dEpsilon = new decimal(epsilon);
            while (1.0m + 0.5m * dEpsilon != 1.0m)
            {
                dEpsilon = 0.5m * dEpsilon;
            }
            Debug.WriteLine(dEpsilon.ToString("0.0#######################################################################################################"));
        }
    }
}
