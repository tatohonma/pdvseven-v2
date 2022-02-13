using a7D.PDV.Integracao.Pagamento.GranitoTEF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestPagoTef
    {
        [TestMethod]
        public void PagoTef_Login_Criptografar()
        {
            string chave = "001-95046-19";
            string cnpj = "26234916000108";
            string senha = "3558635370";
            int pdv = 1;
            string dado = GranitoLogin.Cript(chave, cnpj, senha, pdv);
            Console.WriteLine(dado);

            GranitoLogin.Decript(chave, dado);
            Assert.IsTrue(GranitoLogin.Is(cnpj, senha));

            Exception ex1 = null;
            try
            {
                chave = "1234";
                GranitoLogin.Decript(chave, dado);
            }
            catch (Exception ex)
            {
                ex1 = ex;
            }
            Assert.IsTrue(ex1 != null);
        }

        [TestMethod]
        public void PagoTef_String_Autorizada()
        {
            var retornoOK = "1123456TRANSACAO APROVADA                                         010000611028500TRANSACAO APROVADA            00700311000000000000005003003496831482             MASTERCARD|         Via Estabelecimento|| PAGO SA| Estab: 0000240 Pdv: 003| CNPJ: 22.177.858/0001-69| ALAMEDA RIO NEGRO, 500| BARUERI - SP|| Cred a vista | Valor:                        R$ 0,05| NSU Host: 400027902018   Auto: 030923| NSU: 003004       NL: 012005504624078| 537110XXXXXX8017       11/12/17 18:42| Codigo Transacao: 496831| 000000000033|| ARQC: 9995020678345912| AID: A0000000041010| Credit||  TRANSACAO APROVADA MEDIANTE SENHA|            SOUZA/FABIO F||360             MASTERCARD|             Via Cliente|| PAGO SA| Estab: 0000240 Pdv: 003| CNPJ: 22.177.858/0001-69| ALAMEDA RIO NEGRO, 500| BARUERI - SP|| Cred a vista | Valor:                        R$ 0,05| NSU Host: 400027902018   Auto: 030923| NSU: 003004       NL: 012005504624078| 537110XXXXXX8017       11/12/17 18:42| Codigo Transacao: 496831| 000000000033|";
            var aprovado = new StringTefAprovado(new StringBuilder(retornoOK));
            Assert.AreEqual(1, aprovado.Tipo);
            Assert.AreEqual(123456, aprovado.Identificador);
            Assert.AreEqual("TRANSACAO APROVADA             ", aprovado.Titulo);
            Assert.AreEqual(0006110285, aprovado.IDTransacao);
            Assert.AreEqual(0, aprovado.CodigoResposta);
            Assert.AreEqual("TRANSACAO APROVADA           ", aprovado.Descricao);
            Assert.AreEqual(PG_Rede.PAGO_GP, aprovado.Adiquirente);
            Assert.AreEqual(PG_Bandeiras.MASTERCARD, aprovado.Bandeira);
            Assert.AreEqual(PG_TipoTransacao.Credito, aprovado.Transacao);
            Assert.AreEqual(PG_TipoForma.AVista, aprovado.Forma);
            Assert.AreEqual(PG_TipoModalidade.AVista, aprovado.Modalidade);
            Assert.AreEqual(0, aprovado.Parcelas);
            Assert.AreEqual((decimal)0.05, aprovado.Valor);
            Assert.AreEqual("003003", aprovado.NSU);
            Assert.AreEqual("496831", aprovado.Aturorizacao);
        }
    }
}
