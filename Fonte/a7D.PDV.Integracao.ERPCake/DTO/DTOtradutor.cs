using a7D.PDV.EF.Enum;

namespace a7D.PDV.Integracao.ERPCake.DTO
{
    internal static class DTOtradutor
    {

        internal static string PaymentType(int? iDMeioPagamentoSAT)
        {
            switch ((EMetodoPagamento)(iDMeioPagamentoSAT ?? 1))
            {
                case EMetodoPagamento.Loja:
                    return "crediario";
                case EMetodoPagamento.Credito:
                    return "cartao_credito";
                case EMetodoPagamento.Debito:
                    return "cartao_debito";
                default:
                    return "dinheiro";
            }
        }
    }
}