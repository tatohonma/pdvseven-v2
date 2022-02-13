using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EStatusMesa
    {
        Liberada = 10,
        [Description("Em Atendimento")] EmAtendimento = 20,
        [Description("Conta Solicitada")] ContaSolicitada = 30,
        Reservada = 40
    }
}
