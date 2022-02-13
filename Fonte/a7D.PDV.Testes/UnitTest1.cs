using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a7D.PDV.BLL;
using a7D.PDV.Model;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = CryptMD5.Criptografar(DateTime.Now.AddDays(-1).ToString());
            var dt = Convert.ToDateTime(CryptMD5.Descriptografar("vvJlZseANMQSjusZPkaQ949tnQqQAviX"));
        }

        [TestMethod]
        public void TestarDAL()
        {
            Produto.ListarAtivosDAL(DateTime.Now);
        }

        [TestMethod]
        public void LerLicenca()
        {

        }

        [TestMethod]
        public void SalvarPedido()
        {
            try
            {
                PedidoInformation pedido = new PedidoInformation();
                pedido.TipoPedido = new TipoPedidoInformation { IDTipoPedido = 10 };
                pedido.StatusPedido = new StatusPedidoInformation { IDStatusPedido = 10 };
                pedido.GUIDIdentificacao = "49cd4c0d-075a-457c-b944-b9f0e55d9839";
                pedido.DtPedido = DateTime.Now;
                pedido.TaxaServicoPadrao = TipoPedido.RetornarTaxaServico(10);

                Pedido.Salvar(pedido);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public void EstoqueAtual()
        {
            var estoqueAtual = EntradaSaida.EstoqueAtual();

            foreach (var estoque in estoqueAtual)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("{0} | {1}{2}", estoque.Produto.Nome, estoque.Quantidade.ToString(), estoque.Unidade.Simbolo));
            }
        }
    }
}
