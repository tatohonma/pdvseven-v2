using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    /// <summary>
    /// Não confundir com 'ECodigosPagamentoSAT', até o código 5 é igual!!!
    /// </summary>
    public enum EMetodoPagamento
    {
        Dinheiro = 1,
        Cheque = 2,
        [Description("Cartão de Crédito")] Credito = 3,
        [Description("Cartão de Débito")] Debito = 4,
        [Description("Crédito Loja")] Loja = 5,
        [Description("Vale Alimentação")] Alimentacao = 6,
        [Description("Vale Refeição")] Refeicao = 7,
        [Description("Vale Presente")] Presente = 8,
        [Description("Vale Combustível")] Combustivel = 9,
        Outros = 10
    }
}
