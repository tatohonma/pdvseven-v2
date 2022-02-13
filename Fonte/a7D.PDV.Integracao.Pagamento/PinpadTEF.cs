using a7D.PDV.Integracao.Pagamento.NTKTEF;
using a7D.PDV.Integracao.Pagamento.NTKTEFDLL;
using a7D.PDV.Integracao.Pagamento.GranitoTEF;
using System;

namespace a7D.PDV.Integracao.Pagamento
{
    public delegate void StatusUpdateCallBack(ITEF itef);

    public static class PinpadTEF
    {
        public static string Iniciar(TipoTEF tipo, out string viaCliente, out string viaEstabelecimento)
        {
            ITEF tef = null;
            viaCliente = null;
            viaEstabelecimento = null;

            switch (tipo)
            {
                case TipoTEF.NTKPayGo:

                    if (NTKPinpadPayGo.TransacaoPendente())
                    {
                        var ntk = NTKBuilder.CancelarTransacaoPendente();
                        tef = ntk.IniciaTransacao();

                        System.Threading.Thread.Sleep(5000);

                        while (tef.Processando()) // Usa o timeout padrão...
                            System.Threading.Thread.Sleep(1000);

                        viaCliente = tef.ViaCliente;
                        viaEstabelecimento = tef.ViaEstabelecimento;

                        return tef.Mensagem;
                    }

                    break;
            }

            return "";
        }

        public static PagamentoResultado Pagar(TipoTEF tipo, decimal valor, int pedido, string loja, string pdvNome, int idPDV, string celular, int parcelas)
        {
            FactoryWPF factory = null;

            ITEF tef = null;
            switch (tipo)
            {
                case TipoTEF.NTKPayGo:
                    var ntk = NTKBuilder.AutorizacaoVenda(pedido, loja, pdvNome, DateTime.Now, pedido, valor, 0);
                    tef = ntk.IniciaTransacao();

                    while (tef.Processando()) // Usa o timeout padrão...
                        System.Threading.Thread.Sleep(1000);

                    break;

                case TipoTEF.GRANITO:
                    factory = new FactoryWPF(false);
                    var pago = new GranitoPinpad(pedido, pedido, valor, celular);
                    tef = pago.IniciaVenda();

                    break;

                case TipoTEF.STONE:
                    factory = new FactoryWPF(false);
                    tef = new StoneTEF.PinpadStoneTEF(pedido, valor, null);

                    break;

            }

            factory?.WaitModal(tef, parcelas, true);

            return new PagamentoResultado()
            {
                Autorizacao = tef.Autorizacao,
                Bandeira = tef.Bandeira,
                Debito = tef.Debito,
                ContaRecebivel = tef.Adquirente,
                ViaEstabelecimento = tef.ViaEstabelecimento,
                ViaCliente = tef.ViaCliente,
                MensagemOperador = tef.Mensagem,
                Confirmado = tef.PagamentoConfirmado
            };
        }

        public static bool CancelarTransacao(TipoTEF tipo, string autorizacaoCancelamento)
        {
            FactoryWPF factory = null;
            ITEF tef = null;

            switch (tipo)
            {
                case TipoTEF.STONE:
                    factory = new FactoryWPF(false);
                    tef = new StoneTEF.PinpadStoneTEF(0, 0, autorizacaoCancelamento);

                    break;

                default:
                    throw new NotImplementedException();
            }

            //factory.WaitModal(tef, 0, true);

            return true;
        }

        public static string Administrar(TipoTEF tipo, out string viaCliente, out string viaEstabelecimento)
        {
            ITEF tef = null;
            viaCliente = null;
            viaEstabelecimento = null;

            switch (tipo)
            {
                case TipoTEF.NTKPayGo:
                    var ntk = NTKBuilder.Administar();
                    tef = ntk.IniciaTransacao();

                    System.Threading.Thread.Sleep(5000);

                    while (tef.Processando()) // Usa o timeout padrão...
                        System.Threading.Thread.Sleep(1000);

                    viaCliente = tef.ViaCliente;
                    viaEstabelecimento = tef.ViaEstabelecimento;

                    return tef.Mensagem;
            }

            return null;
        }
    }
}