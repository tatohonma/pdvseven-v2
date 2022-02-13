using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.PainelMesaComanda.UI.Properties;
using System.Windows;

namespace a7D.PDV.PainelMesaComanda.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static PainelMesasAPI wsAPI;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            wsAPI = new ClienteWS(Settings.Default.EnderecoAPI2).PainelMesas();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        internal static MesaComandasTotal[] LerMesas() 
            => wsAPI.LerMesas();

        internal static NumeroQuantidadeTotalPrimeiro[] LerComandasPorMesa(int mesa) 
            => wsAPI.LerComandasPorMesa(mesa);
    }
}
