using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    // http://api.docs.dev.loggi.com/recursos/address-orcamentos.html
    public enum ETamanhoPacote
    {
        [Description("Loggi Pequeno (15x15x15 cm)")] box = 10,
        [Description("Loggi Médio (20x20x20 cm)")] medium_box = 20,
        [Description("Loggi Grande (42x4432 cm)")] large_box = 30,
    }
}