using System;
using System.IO;
using System.Threading.Tasks;

namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    internal static class Textos
    {
        internal static string FileChat => string.Format("chats-{0:yyyyMMdd-HH}.txt", DateTime.Now);

        internal static async Task Write(string info)
        {
            try
            {
                await File.AppendAllTextAsync(@".\wwwroot\" + FileChat, info + "\r\n");
            }
            catch
            {
            }
        }

        internal static string Bug = "Ops, deculpe tem um bug no meu código!\r\nJá informei o programador para me corrigir\r\nERRO: {0}\r\n{1}";
        internal static string ErroProposital = "Teste de erro proposital";
        internal static string Logout = "Até breve";
        internal static string Reload = "Solicitada recarregamento dos arquivos de intenções *.JSON";
        internal static string DialogoCancelado = "OK, vamos voltar ao início\r\nO que você deseja?";
        internal static string DialogoNaoExiste = "Não há nada a ser cancelado";
        internal static string BemVindoSimples = "Bem Vindo {0}";
    }
}
