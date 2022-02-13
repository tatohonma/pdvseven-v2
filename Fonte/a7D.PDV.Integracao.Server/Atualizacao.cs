using a7D.PDV.Integracao.Servico.Core;
using System;
using System.IO;
using System.Reflection;

namespace a7D.PDV.Integracao.Server
{
    internal class Atualizacao
    {
        private const string updateZip= @"WebServices\www2\Release\update.zip";
        public static bool TemUpdate { get; private set; } = false;

        internal static void ApagaUpdateZip(OnMensagemListener AddInfo)
        {
            try
            {
                var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
                var fileZip = Path.Combine(fi.Directory.FullName, updateZip);
                AddInfo("Atualização Limpa");
                File.Delete(fileZip);
            }
            catch (Exception ex)
            {
                AddInfo("Erro ao apagar 'Update.zip': " + ex.Message);
            }
        }

        static bool notificouAtualizacaoValida = false;
        internal static void VerificaUpdateZip(OnMensagemListener AddInfo)
        {
            try
            {
                var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
                var fileZip = Path.Combine(fi.Directory.FullName, updateZip);
                if (File.Exists(fileZip))
                {
                    TemUpdate = true;
                    if(!notificouAtualizacaoValida)
                        AddInfo("Atualização válida");

                    notificouAtualizacaoValida = true;
                    return;
                }

                AddInfo("Criando atualização: " + fileZip);

                Componentes.Utils.CreateZipFromDirectory(fi.Directory.FullName, fileZip,
                    f => !f.StartsWith("WebServices", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("Backup", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("Update", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("Relatorios", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("a7D.PDV.Integracao.Servico.UI", StringComparison.InvariantCultureIgnoreCase)
                      && !f.StartsWith("a7D.PDV.Configurador.UI", StringComparison.InvariantCultureIgnoreCase));

                TemUpdate = true;
            }
            catch (Exception ex)
            {
                AddInfo("Erro ao gerar 'Update.zip': " + ex.Message);
            }
        }
    }
}
