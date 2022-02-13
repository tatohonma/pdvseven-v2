namespace a7D.PDV.EF.Enum
{
    // CUIDADO: Não confundir com 'EMeioPagamento', esses códigos são os códigos do SAT
    public enum ECodigosPagamentoSAT
    {
        Dinheiro = 1,
        Cheque = 2,
        Credito = 3,
        Debito = 4,
        Loja = 5, // <= Conta Cliente
        Alimentacao = 10, 
        Refeicao = 11, // VR
        Presente = 12, // Fidelidade
        Combustível = 13,
        Outros = 99
    }
}