using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#formas-de-pagamento
    public class Payment_Form_Condition : ModelCake
    {
        public int payment_form; // Identificador da forma de pagamento
        public int parcel; // Número de parcelas
        public Decimal percentage; // Percentual de desconto
        public bool percentage_discount; // Flag que indica se permite desconto
        public int days_parcel; // Dias para a parcela
    }
}