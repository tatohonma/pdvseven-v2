using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using System;
using System.Configuration;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos : ITerminalClient
    {
        public TerminalInformation Terminal { get; set; }

        public bool Valid => Terminal.pdvID >= 0;

        private TerminalEvent onTerminalEvent;
        private RegErro onErroEvent;
        private AutenticacaoAPI autenticacao;
        private PedidoAPI pedido;
        private ImpressaoAPI impressao;
        private int count = 0;

        private decimal TextoValor(string texto)
        {
            if (decimal.TryParse(texto, out decimal valor))
                return Math.Truncate(valor) / 100m;

            return 0;
        }

        public async void Create(TerminalEvent onEvent, RegErro onErro)
        {
            string url = ConfigurationManager.AppSettings["EnderecoAPI2"];
            var ws = new ClienteWS(url);
            autenticacao = new AutenticacaoAPI(ws);
            pedido = ws.Pedido();
            impressao = ws.Impressao();
            onTerminalEvent = onEvent;
            onErroEvent = onErro;
            count = 0;
            try
            {
                termDisplayMessageCenter(Terminal.terminalId, "Iniciando...", 10);
                var http = await autenticacao.AutenticarPDVAsync((int)ETipoPDV.POS_INTEGRADO_NTK, Terminal.terminalId);
                if (http.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var result = http.Content.ReadAsStringAsync().Result;
                    throw ExceptionAPI.Create(http, result);
                }

                var responseString = http.Content.ReadAsStringAsync().Result;
                var pdv = autenticacao.DeserializeObject<pdvResult>(responseString);

                if (pdv.idPDV > 0)
                {
                    Terminal.pdvUserID = 0;
                    Terminal.pdvID = pdv.idPDV;
                    Terminal.pdvName = pdv.nome;
                    onTerminalEvent(Terminal, "PDV " + Terminal.pdvID + " " + Terminal.pdvName);
                    termDisplayMessageCenter(Terminal.terminalId, pdv.nome, 10);
                    return;
                }

                // Invalida e prepara para desconectar
                Terminal.pdvID = -1;

                if (pdv.Codigo > 0 && pdv.Mensagem != null)
                    termDisplayMessageCenter(Terminal.terminalId,
                        pdv.Mensagem.Replace("] ", "]\r") + "\r\r" +
                        "ERRO " + pdv.Codigo.Value.ToString("X4"));

                else if (pdv.Mensagem != null)
                    termDisplayMessageCenter(Terminal.terminalId,
                        pdv.Mensagem.Replace("] ", "]\r"));

                else
                    termDisplayMessageCenter(Terminal.terminalId, "PDV POS invalido");

                onTerminalEvent(Terminal, pdv.Mensagem);

            }
            catch (Exception ex)
            {
                Terminal.pdvID = -1;
                onErroEvent(Terminal, new Exception("ERRO CREATE: " + ex.Message, ex));
            }
        }

        public void Loop()
        {
            if (Terminal.toScreen == null)
            {
                Terminal.toScreen = Reconect;
                Terminal.toAtual = Terminal.toAnterior = null;
                return;
            }

            var toScreen = Terminal.toScreen();

            if (toScreen == Tela1_MenuPrincipal)
            {
                Terminal.ret = 0;
                Terminal.toAtual = Terminal.toAnterior = null;
            }
            else if (toScreen != Terminal.toScreen && toScreen != Reconect) // Quando muda de tela
            {
                Terminal.toAnterior = Terminal.toAtual;
                Terminal.toAtual = Terminal.toScreen; // Salva a tela atual
            }

            Terminal.toScreen = toScreen; // proxima tela para o proximo ciclo de loop apos o equipmento estar em IDLE
        }

        private ScreenPOS Reconect()
        {
            if (Terminal.pdvID == 0)
            {
                if (Valid)
                {
                    count++;
                    termDisplayMessageCenter(Terminal.terminalId, "Iniciando... " + count, 1000);
                }
                return Reconect;
            }
            else if (Terminal.pdvUserID == 0)
                return TelaLogin;
            else if (Terminal.pagamento != null && Terminal.valorTEF > 0)
                return Tela5_ConfirmaPagamento;
            else
                return Tela1_MenuPrincipal;
        }

        private ScreenPOS TelaLogin()
        {
            Terminal.pdvUserChave = Terminal.pdvUserName = "";
            Terminal.pdvUserID = 0;

            var chave = termGetText(Terminal.terminalId, "\r\r\r" + Terminal.pdvName.PadBoth() +
                "\r\r  Chave de Acesso:", "@@@@@@@@", ref Terminal.ret, true);

            if (chave == null)
                return TelaLogin;

            if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return TelaLogin;
            }
            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(chave))
                return ErroValorVazio;

            var user = autenticacao.AutenticarUsuario(chave);
            if (user.idUsuario > 0)
            {
                Terminal.pdvUserID = user.idUsuario;
                Terminal.pdvUserName = user.nome;
                Terminal.pdvUserChave = chave;
                termDisplayMessageCenter(Terminal.terminalId, Terminal.pdvName + "\r\r" + Terminal.pdvUserName);
                return Tela1_MenuPrincipal;
            }
            else if (user.Codigo > 0 && user.Mensagem != null)
                termDisplayMessageCenter(Terminal.terminalId,
                    user.Mensagem.Replace("] ", "]\r") + "\r\r" +
                    "ERRO " + user.Codigo.Value.ToString("X4"));
            else if (user.Mensagem != null)
                termDisplayMessageCenter(Terminal.terminalId,
                    user.Mensagem.Replace("] ", "]\r"));
            else
                termDisplayMessageCenter(Terminal.terminalId, "Usuario invalido");

            return TelaLogin;
        }

        #region Erros

        private ScreenPOS ErroValorDigitado()
        {
            termDisplayMessageCenter(Terminal.terminalId,
                "Valor invalido\r" +
                ("ERRO\r" + termRetText(Terminal.ret)));
            return null; // volta a mesma pergunta
        }

        private ScreenPOS ErroValorVazio()
        {
            termDisplayMessageCenter(Terminal.terminalId, "Informe o valor", 3000);
            return null; // volta a mesma pergunta
        }

        private ScreenPOS ErroPagamento()
        {
            termDisplayMessageCenter(Terminal.terminalId,
                "Erro no pagamento\r" +
                "ERRO\r" + termRetText(Terminal.ret));
            return null;
        }

        #endregion
    }
}