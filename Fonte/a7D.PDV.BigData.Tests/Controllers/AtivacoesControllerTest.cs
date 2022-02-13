using a7D.PDV.Ativacao.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace a7D.PDV.BigData.WebAPI.Tests.Controllers
{
    // TODO: Ter um banco de teste generico! 

    [TestClass]
    public class AtivacoesControllerTest
    {
        [TestMethod]
        public void Ativacoes_GetAtivacaoPorChave()
        {
            // Arrange
            var controller = new AtivacoesController();

            // Act
            dynamic result = controller.GetAtivacaoPorChave("001-06079-14");

            // Assert
            Assert.IsNotNull(result);

            var info = result.Content;
            Assert.IsNotNull(info.Mensagem);
        }
    }
}
