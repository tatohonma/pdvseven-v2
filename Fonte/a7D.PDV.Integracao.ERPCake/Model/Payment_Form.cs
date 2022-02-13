using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#formas-de-pagamento
    public class Payment_Form : ModelCake
    {
        public string name; // Nome
        public string description; // Descrição
        public Decimal? minimum_parcel; // Valor mínimo de parcelas
        public bool? visible; // Flag que indica se é visivel no sistema
        public bool? change_parcel_prices; // Flag que indica se permite alterar valor das parcelas
        public bool? change_only_first_parcel; // Flag que indica se permite alterar a primeira parcela
        public bool? visible_pdv; // Flag que indica se permite visualizar no PDV
        public string name_pdv; // Nome da forma de pagamento para a impressora fiscal
        public int? bank_account; // Identificador do banco
        public bool? uses_tef; // Flag se usa TEF
        public bool? allow_change; // Flag se permite troco
        public string payment_type; // Nome do tipo base de pagamento
        public Decimal? delay_interest; // Juros por atraso
        public Decimal? penalty_for_delay; // Multa por atraso
        public string acknowledgement_of_a_debt; // Reconhecimento de dívida
        public bool? automatic_receipt; // Recebimento automático
        public bool? automatic_payment; // Pagamento automático

        public bool IsCardType() => payment_type == "cartao_credito" || payment_type == "cartao_debito";

        public override bool RequerAlteracaoPDV(DateTime dtSync, out int id)
        {
            id = 0;
            if (string.IsNullOrEmpty(description))
                return true; // Só cadastra
            else
                return false;
        }

        public override void SetCode(string code) => this.description = code;

        public override string ToString() => $"{id}: {name}";
    }
}