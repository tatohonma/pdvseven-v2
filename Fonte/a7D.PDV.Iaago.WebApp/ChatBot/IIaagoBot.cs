using a7D.PDV.Iaago.Dialogo;

namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    public interface IIaagoBot
    {
        string Ambiente { get; }

        IaagoVars UserIaago { get; }

        IaagoFluxo Fluxo { get; }
    }
}
