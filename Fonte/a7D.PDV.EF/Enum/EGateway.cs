using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EGateway : int
    {
        // Meios de pagamento: 1-99
        [Description("NTK TEF Pay&Go")] NTKTEF = 1,
        //[Description("NTK TEF DLL (Dedicado)")] NTKTEFDLL = 2,
        [Description("NTK POS Integrado")] NTKPOS = 3,

        [Description("iFood")] iFood = 4,

        [Description("Stone TEF")] StoneTEF = 9,
        [Description("Stone POS Integrado")] StonePOS = 10,

        //[Description("Pago TEF")] PagoTEF = 11,
        //[Description("Pago POS Integrado")] PagoPOS = 12,

        [Description("Conta de Cliente")] ContaCliente = 15,

        //[Description("Todo Cartões")] TodoCartoes = 21,

        [Description("Granito POS Integrado")] GranitoPOS = 22,
        [Description("Granito TEF")] GranitoTEF = 23,

        // Meios de Entrega: 101-199
        [Description("Loggi")] Loggi = 101
    }
}