using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum ETipoAreaImpressao
    {
        Conta = 0,
        [Description("Conta Padrão")] ContaPadrao = 1,
        [Description("Produção")] Producao = 2,
        [Description("S@T")] SAT = 3
    }
}
