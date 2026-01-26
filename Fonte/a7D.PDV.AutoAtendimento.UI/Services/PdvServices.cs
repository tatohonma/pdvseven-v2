using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.Pagamento.GranitoTEF;
using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using a7D.PDV.BLL;
using a7D.PDV.Integracao.Pagamento.StoneTEF;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal class PdvServices
    {
        const int idTipoPDV = 140;
        internal static int PDVID { get; private set; }
        internal static string PDVNome { get; private set; }
        internal static string ImpressaoLocal { get; private set; }
        internal static int SenhaSaida { get; private set; }
        internal static bool ExibirMouse { get; private set; }
        internal static string ChaveUsuario { get; private set; }
        internal static int UsuarioID { get; private set; }
        internal static int TimeoutInativo { get; private set; }
        internal static int TimeoutAlerta { get; private set; }
        internal static int VerificarDisponibilidade { get; private set; }
        internal static string MeioPagamento { get; private set; }
        internal static bool Fiscal { get; private set; }
        internal static bool OrdemImpressao { get; private set; }
        internal static bool ComandaComCredito { get; private set; }
        internal static int IDCategoriaProduto_Credito { get; private set; }
        internal static int IDProduto_NovaComanda { get; private set; }
        
        
        internal static string AutoTefBaseUrl { get; private set; }
        internal static string StoneCode { get; private set; }
        internal static string AutoTefConnectionName { get; private set; }
        
        static AutoTefClient _autoTefClient;
        static bool _autoTefActivated;

        AutenticacaoAPI autenticacao;
        ConfiguracaoAPI config;

        static readonly SemaphoreSlim _activateLock = new SemaphoreSlim(1, 1);
        
        internal PdvServices(ClienteWS ws)
        {
            autenticacao = ws.Autenticacao();
            config = ws.Configuracao();
        }

        string RetornarSerialHD()
        {
            var disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }


        internal static AutoTefClient GetAutoTefClient()
        {
            if (_autoTefClient == null)
                _autoTefClient = new AutoTefClient(AutoTefBaseUrl ?? "http://localhost:8000/");
            return _autoTefClient;
        }


        internal static async Task EnsureAutoTefActivatedAsync()
        {
            if (_autoTefActivated) return;

            await _activateLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_autoTefActivated) return;

                var client = GetAutoTefClient();
                var resp = await client.ActivateAsync(StoneCode, AutoTefConnectionName).ConfigureAwait(false);

                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception($"Falha ao ativar AutoTEF ({(int)resp.StatusCode}): {body}");
                }

                _autoTefActivated = true;
            }
            finally
            {
                _activateLock.Release();
            }
        }


        internal string ValidarPDV()
        {
            var hardware = RetornarSerialHD();
            var result = autenticacao.AutenticarPDV(140, hardware);
            if (result.idPDV > 0)
            {
                PDVID = result.idPDV;
                PDVNome = result.nome;
                return "OK";
            }
            else
                throw new Exception(result.Mensagem);
        }

        internal async void LerConfiguracoes()
        {
            ChaveUsuario = config.Chave("ChaveUsuario", PDVID, idTipoPDV);
            ImpressaoLocal = config.Chave("ImpressaoLocal", PDVID, idTipoPDV);
            string cSenhaSaida = config.Chave("SenhaSaida", PDVID, idTipoPDV);

            if (string.IsNullOrEmpty(ChaveUsuario)
             || string.IsNullOrEmpty(ImpressaoLocal)
             || string.IsNullOrEmpty(cSenhaSaida))
                throw new Exception("Não há configurações válidas para o Autoatendimento");

            var result = autenticacao.AutenticarUsuario(ChaveUsuario);
            if (result.idUsuario > 0)
                UsuarioID = result.idUsuario;
            else
                throw new Exception(result.Mensagem);

            SenhaSaida = int.Parse(cSenhaSaida);
            ExibirMouse = config.Chave("ExibirMouse", PDVID, idTipoPDV) == "1";
            OrdemImpressao = config.Chave("OrdemImpressao", PDVID, idTipoPDV) == "1";
            Fiscal = config.Chave("Fiscal") == "NFCe";
            TimeoutInativo = int.Parse(config.Chave("TimeoutInativo", PDVID, idTipoPDV) ?? "120");
            TimeoutAlerta = int.Parse(config.Chave("TimeoutAlerta", PDVID, idTipoPDV) ?? "60");
            VerificarDisponibilidade = int.Parse(config.Chave("VerificarDisponibilidade", PDVID, idTipoPDV) ?? "10");
            MeioPagamento = config.Chave("MeioPagamento", PDVID, idTipoPDV); // NTKDLL, PAGODLL, nenhum
            ComandaComCredito = config.Chave("ComandaComCredito") == "1";
            AutoTefBaseUrl = config.Chave("AutoTefBaseUrl", PDVID, idTipoPDV);

            AutoTefBaseUrl = "http://localhost:8000/";
            StoneCode = config.Chave("StoneCode");
            AutoTefConnectionName = "PDVSeven";
            
            
            if (!string.IsNullOrEmpty(StoneCode))
            {
                AutoTefBridge.Register(
                    getClient: () => GetAutoTefClient(),
                    ensureActivatedAsync: () => EnsureAutoTefActivatedAsync()
                );

                try
                {
                    await EnsureAutoTefActivatedAsync();
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.A310, ex);
                }
            }

            if (int.TryParse(config.Chave("IDCategoriaProduto_Credito", PDVID, idTipoPDV), out int catCredito))
                IDCategoriaProduto_Credito = catCredito;

            if (int.TryParse(config.Chave("IDProduto_NovaComanda", PDVID, idTipoPDV), out int novoCredito))
                IDProduto_NovaComanda = novoCredito;

            ImpressoraServices.Left = int.Parse(config.Chave("MargemImpressaoWindows", PDVID, idTipoPDV) ?? "0");
            ImpressoraServices.Width = int.Parse(config.Chave("LarguraImpressaoWindows", PDVID, idTipoPDV) ?? "280");
            
            
            //TODO SDK antigo 
            // Integracao.Pagamento.StoneTEF.PinpadStoneTEF.StoneCode = config.Chave("StoneCode");

            string granitoIdPdvString = config.Chave("GranitoIDPDV", PdvServices.PDVID);

            if (!int.TryParse(granitoIdPdvString, out int granitoIdPdv))
            {
                throw new Exception("ID PDV Granito TEF não configurado");
            }

            GranitoPinpad.GranitoCNPJ = config.Chave("GranitoCNPJ");
            GranitoPinpad.GranitoCode = config.Chave("GranitoCode");
            GranitoPinpad.GranitoIdPDV = granitoIdPdv.ToString("000");


            var chavePago = config.Chave("PagoChave", PDVID, idTipoPDV);
            if (!string.IsNullOrEmpty(chavePago))
            {
                try
                {
                    var chaveAtivacao = config.Chave("ChaveAtivacao");
                    GranitoLogin.Decript(chaveAtivacao, chavePago);
                }
                catch (Exception ex)
                {
#if !TESTE
                    throw new Exception("Chave PAGO inválida", ex);
#endif
                }
            }

        }
    }
}
