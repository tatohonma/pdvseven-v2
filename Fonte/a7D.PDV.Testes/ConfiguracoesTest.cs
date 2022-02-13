using a7D.PDV.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static a7D.PDV.BLL.ConfiguracaoBD;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class ConfiguracoesTest
    {

        [TestMethod]
        public void DeveObeterConfiguracoesDoSistemaCorretamente()
        {
            var type = typeof(ConfiguracoesSistema);
            foreach (var md in type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                Debug.WriteLine($"Configuração {md.Name}: {md.Invoke(null, new object[] { })}");
            }
        }

        [TestMethod]
        public void DeveObterConfiguracoesCaixaCorretamente()
        {
            int idPdv = 2;
            var type = typeof(ConfiguracoesCaixa);
            foreach (var md in type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                Debug.WriteLine($"Configuração {md.Name}: {md.Invoke(null, new object[] { idPdv })}");
            }
        }
    }
}
