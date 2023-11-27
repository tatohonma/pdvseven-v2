using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EGateway : int
    {
        // Meios de pagamento: 1-99
        [Description("NTK TEF Pay&Go")] NTKTEF = 1,
        [Description("NTK POS Integrado")] NTKPOS = 3,
        [Description("iFood")] iFood = 4,
        [Description("PagSplit")] PagSplit = 5,
        [Description("Stone TEF")] StoneTEF = 9,
        [Description("Stone POS Integrado")] StonePOS = 10,
        [Description("Conta de Cliente")] ContaCliente = 15,
        [Description("Granito POS Integrado")] GranitoPOS = 22,
        [Description("Granito TEF")] GranitoTEF = 23,

        // Meios de Entrega: 101-199
        [Description("Loggi")] Loggi = 101
    }
}