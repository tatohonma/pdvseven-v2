using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Services;
using a7D.PDV.BLL;
using a7D.PDV.Integracao.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestMensagem
    {
        private const string chave = "001-06079-14";

        [TestMethod, TestCategory("Ativador")]
        public void Adivador_Decript()
        {
            var valor = "1QcPvtxKeNlC4GHqpCV8qCP0S6GtGMGV";
            var result = CryptMD5.Descriptografar(valor);
            Console.WriteLine(result);
            // 05/11/2018 01:41:27
        }

        [TestMethod, TestCategory("Ativador")]
        public void Adivador_Criptografar()
        {
            var valor = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); // "30/01/2019 02:00:00";
            var result = CryptMD5.Criptografar(valor);
            Console.WriteLine(result);
            // tXPUNdCBUlotMtFcyIx3/ToKLDGusZxv
        }

        [TestMethod, TestCategory("Ativador")]
        public void Mensagem_Ler()
        {
            var mensagens = Ativador.API.SyncMensage(chave, EOrigemDestinoMensagem.Integrador, "1.2.3", "TDD").Result;
            //var mensagens = Ativador.API.Mensagens(chave).Result;
            if (mensagens.Length == 0)
                Assert.Inconclusive("Sem mensagens");

            foreach (var msg in mensagens)
                Console.WriteLine(msg);
        }

        [TestMethod, TestCategory("Ativador")]
        public void Mensagem_Enviar()
        {
            var msg = new MensagemNova()
            {
                Chave = chave,
                Tipo = ETipoMensagem.Pergunta_SIMNAO,
                Origem = EOrigemDestinoMensagem.Integrador,
                Destino = EOrigemDestinoMensagem.Ativador,
                Texto = "to perguntado algo...",
                IdMensagemOrigem = 123
            };
            var result = Ativador.API.Enviar(msg).Result;
        }

    }
}