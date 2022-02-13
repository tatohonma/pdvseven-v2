using System.Threading;

namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public delegate ScreenPOS ScreenPOS();

    public class TerminalInformation
    {
        public string terminalId;
        public string mac;
        public string model;
        public string serialNumber;
        //public bool transactionToConfirm;
        public int tipo; // Mesa/Comanda/Delivery
        public int numero; // Numero da mesa ou comanda
        public int pessoas; // Numero pessoas
        public int pedido; // numero do pedido
        public decimal valorTotal; // Valor total 
        public decimal valorServico; // Valor servicos (já adicionados ao valor total)
        public decimal valorPendente; // Valor (descontando o que já foi pago)
        public decimal valorTEF; // Valor a ser pago na transação atual do TEF
        public short ret;
        public ScreenPOS toScreen;
        public ScreenPOS toAtual; // foi a atual
        public ScreenPOS toAnterior; // foi a anterior que cancelou a aatual
        public Thread thread;
        public int pdvID;
        public string pdvName;
        public int pdvUserID;
        public string pdvUserChave;
        public string pdvUserName;
        public string documentoFidelidade;
        public string documentoFiscal;
        public Pagamento pagamento;
        public bool RequerDesconectar;

        // Armazena dos ultimos
        public string UltimaConta;
        public string UltimoComprovante;
    }

    public class Pagamento
    {
        public int cardType;
        public string autorizacao;
        public string cardName;
        public string adquirente;
        public string cardMask;
    }
}
