using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EBandeira : int
    {
        Desconhecida = 0,
        Visa = 1,
        MasterCard = 2,
        [Description("American Express")] Amex = 3,
        Aura = 4,
        Avista = 5,
        Banese = 6,
        BrasilCard = 7,
        Cabal = 8,
        CardBan = 9,
        [Description("Diners Club")] DinersClub = 10,
        Discover = 11,
        Elo = 12,
        FortBrasil = 13,
        GBarbosa = 14,
        Hipercard = 15,
        JCB = 16,
        [Description("Personal Card")] PersonalCard = 17,
        [Description("Pleno Card")] PlenoCard = 18,
        Santander = 19,
        Sicredi = 20,
        Sorocred = 21,
        ValeCard = 22,
        [Description("Mastercard Débito")]
        MastercardD = 23, // MAESTROCP
        VisaElectro = 24,
        [Description("Elo Crédito")]
        EloCredito = 25,
        VR = 26,
        SODEXO = 27,
        TICKET = 28,
        OUTROS = 29, // Migrar???
        Alelo = 30,
        Presente = 90,
        Fidelidade = 91
    }
}
