using a7D.PDV.EF;
using System;
using System.Configuration;

namespace a7D.PDV.BLL
{
    public static class Configuracao
    {
        public static void Init()
        {
            ConfigAttribute.TypeList = new Type[]
            {
                typeof(ConfiguracoesAtivacao),
                typeof(ConfiguracoesSistema),
                typeof(ConfiguracoesNFCe),
                typeof(ConfiguracoesSAT),
                typeof(ConfiguracoesCaixa),                 // 10
                typeof(ConfiguracoesComandaTerminal),       // 20, 40
                typeof(ConfiguracoesSaida),                 // 50
                typeof(ConfiguracoesTerminalWindows),       // 30
                typeof(ConfiguracoesCardapio),              // 60
                typeof(ConfiguracoesGerenciadorImpressao),  // 80
                typeof(ConfiguracoesAutoatendimento),       // 140
                typeof(ConfiguracoesIFood),                 // 150
                typeof(ConfiguracoesERP),                   // 160 
                typeof(ConfiguracoesLoggi),                 // 180
                typeof(ConfiguracoesEasyChopp),             // 190
                typeof(ConfiguracoesTorneira)               // 220
            };
        }

        public static void AlterarAppSettings(string chave, string valor)
        {
            // Open App.Config of executable
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Add an Application Setting.
            config.AppSettings.Settings.Remove(chave);
            config.AppSettings.Settings.Add(chave, valor);

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void AlterarConnectionStrings(string name, string connectionString, string providerName)
        {
            // Open App.Config of executable
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.ConnectionStrings.ConnectionStrings.Remove(name);
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString, providerName));

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }
    }
}
