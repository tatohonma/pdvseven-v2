using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.SaidaComanda.UI.Properties;
using System;
using System.Management;

namespace a7D.PDV.SaidaComanda.UI
{
    internal class PdvServices
    {
        internal int PDVID { get; private set; }
        internal string PDVNome { get; private set; }
        public string SenhaSaida { get; private set; }
        public string ChaveUsuario { get; private set; }
        internal int UsuarioID { get; private set; }

        private AutenticacaoAPI autenticacao;
        private ConfiguracaoAPI config;

        internal PdvServices(ClienteWS ws)
        {
            autenticacao = ws.Autenticacao();
            config = ws.Configuracao();
        }

        private string RetornarSerialHD()
        {
            var disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }

        internal string ValidarPDV()
        {
            var hardware = RetornarSerialHD();
            var result = autenticacao.AutenticarPDV(50, hardware);
            if (result.idPDV > 0)
            {
                PDVID = result.idPDV;
                PDVNome = result.nome;
                return "OK";
            }
            else if (result.Mensagem.StartsWith("download:"))
                return new Uri(new Uri(Settings.Default.EnderecoAPI2), result.Mensagem.Substring(9)).ToString();
            else
                throw new Exception(result.Mensagem);
        }

        internal void LerConfiguracoes()
        {
            ChaveUsuario = config.Chave("ChaveUsuario", PDVID, 50);
            SenhaSaida = config.Chave("SenhaSaida", PDVID, 50);

            if (string.IsNullOrEmpty(ChaveUsuario)
             || string.IsNullOrEmpty(SenhaSaida))
                throw new Exception("Não há configurações válidas para o Controle de Saída");

            var result = autenticacao.AutenticarUsuario(ChaveUsuario);
            if (result.idUsuario > 0)
                UsuarioID = result.idUsuario;
            else
                throw new Exception(result.Mensagem);

        }
    }
}
