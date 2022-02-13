using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    /// <summary>
    /// Automação Comercial - Variáveis da isntancia do programa atual
    /// </summary>
    public static class AC
    {
        public static string Chave { get; private set; }
        public static int Ativacao { get; private set; }
        public static string Cliente { get; private set; }
        public static string Aplicacao { get; private set; }
        public static string NomeAplicacao { get; private set; }
        public static string Versao { get; private set; }
        public static string OS { get; private set; }
        public static PDVInformation PDV { get; private set; }
        public static UsuarioInformation Usuario { get; private set; }
        public static int idPDV => PDV?.IDPDV ?? 0;
        public static int idUsuario => Usuario?.IDUsuario ?? 0;
        public static ETipoUsuario TipoUsuario { get; private set; }

        public static void RegitraLicenca(string chaveAtivacao)
        {
            if (Chave == null)
            {
                Chave = chaveAtivacao;
                Cliente = chaveAtivacao;
            }
        }

        public static void RegitraAtivacao(int id, string cliente)
        {
            if (Ativacao == 0)
            {
                Ativacao = id;
                Cliente = cliente;
            }
        }

        public static void RegitraPDV(string aplicacao, string versaoAplicacao, PDVInformation pdv = null)
        {
            if (Aplicacao != null)
                return;

            Aplicacao = aplicacao;
            Versao = versaoAplicacao;
            PDV = pdv;
            OS = GetOSVersion();
            var partesNome = aplicacao.Split('.');
            if (partesNome[partesNome.Length - 1] == "UI")
                NomeAplicacao = partesNome[partesNome.Length - 2];
            else
                NomeAplicacao = partesNome[partesNome.Length - 1];

            try
            {
                Logs.VerificarExistenciaFonte();
            }
            catch (Exception)
            {
            }
        }

        public static void RegitraUsuario(UsuarioInformation usuario)
        {
            Usuario = usuario;
            TipoUsuario = ETipoUsuario.desconhecido;

            if (usuario != null)
            {
                if (Usuario.PermissaoAdm == true)
                    TipoUsuario = ETipoUsuario.Administrador;
                else if (Usuario.PermissaoGerente == true)
                    TipoUsuario = ETipoUsuario.Gerente;
                else if (Usuario.PermissaoCaixa == true)
                    TipoUsuario = ETipoUsuario.Caixa;
                else if (Usuario.PermissaoGarcom == true)
                    TipoUsuario = ETipoUsuario.Garcon;
            }
        }

        // https://stackoverflow.com/questions/2819934/detect-windows-version-in-net
        private static string GetOSVersion()
        {
            //Environment.Is64BitOperatingSystem 
            switch (Environment.OSVersion.Version.Major)
            {
                case 5:
                    return "Windows XP";
                case 6:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "Windows Vista";
                        case 1:
                            return "Windows 7";
                        case 2:
                            return "Windows 10";
                        default:
                            return "Windows Unknown " + Environment.OSVersion.Version.Minor;
                    }
                case 10:
                    return "Windows 10";
                default:
                    return "Unknown";
            }
        }
    }
}
