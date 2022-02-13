using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EStatusComanda
    {
        Liberada = 10,
        EmUso = 20,
        Cancelada = 30,
        Perdida = 40,
        [Description("Conta Solicitada")] ContaSolicitada = 50
    }
}
