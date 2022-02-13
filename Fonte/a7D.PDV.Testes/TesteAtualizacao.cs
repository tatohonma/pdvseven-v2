using a7D.PDV.Componentes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Compression;
using System.Threading;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TesteAtualizacao
    {
        [TestMethod, TestCategory("Atualizacao")]
        public void Atualizacao_Etapa_Backup()
        {
            Atualizacao.BackupVersaoIfExist();
        }

        [TestMethod, TestCategory("Atualizacao")]
        public void Atualizacao_Copy()
        {
            Utils.CopyTo(@"N:\PDV Seven\PDVSeven\bin", @"N:\PDV Seven\PDVSeven\bin2", true);
        }

        [TestMethod, TestCategory("Atualizacao")]
        public void SplashScreen_ShowRun()
        {
            SplashScreen.ShowRun((s) =>
            {
                s.ShowInfo("teste 1");
                Thread.Sleep(1000);
                s.ShowInfo("teste 2");
                Thread.Sleep(1000);
                s.ShowInfo("teste 3");
                Thread.Sleep(1000);
            });
        }

        [TestMethod, TestCategory("Atualizacao")]
        public void Atualizacao_TestUnZip()
        {
            ZipFile.ExtractToDirectory(@"N:\PDV Seven\PDVSeven\Instalador\instalador_PDVSeven-2.18.0.21.exe", @"N:\PDV Seven\PDVSeven\Instalador");
        }
    }
}
