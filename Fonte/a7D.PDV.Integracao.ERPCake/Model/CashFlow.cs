using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#fluxo-de-caixa
    public class CashFlow : ModelCake
    {
        public int? cost_center;                // 	Identificador do centro de custo	Inteiro	11 dígitos	
        public int? payment_form;               // 	Identificador da forma de pagamento	Inteiro	11 dígitos	
        public int? registered_by;              // 	Identificador do usuário	Inteiro	11 dígitos	
        public DateTime registered_date;        //  *	Data do registro	Data	 	
        public bool incoming;                   //  *	Flag se é entrada	Booleano	 	
        public decimal amount;                  //  *	Total do lançamento	Decimal	20 dígitos e 2 decimais	
        public string description;              // 	Descrição do lançamento	Texto	255 posições	
        public int? category;                   // 	Identificador da categoria de fluxo de caixa	Inteiro	11 dígitos	
        public string due_date;                 //  *	Data de Vencimento	Data	 	
        public int? bank_account;               // 	Identificador do banco de origem	Inteiro	11 dígitos	
        public int? bank_account_to;            // 	Identificador do banco de destino	Inteiro	11 dígitos	
        public bool received;                   //  *	Flag de recebido	Booleano	 	
        public int? customer;                   // 	Identificador do cliente	Inteiro	11 dígitos	
        public string customer_cpf_cnpj;        // 	CPF ou CNPJ dependendo do tipo de pessoa que o cliente for.	Texto	14 posições	
        public int? ecf_printer;                // 	Identificador da impressora fiscal	Inteiro	11 posições	
        public int? ecf_number;                 // 	Número da impressora fiscal	Inteiro	11 posições	
        public int repeat;                      // 	Tipo de repetição	Inteiro	11 posições	
        public int repeat_n=1;                  // 	Quantidade de repetição	Inteiro	11 posições	
        public decimal amount_total;            // 	valor Total	Decimal	20 dígitos e decimais	
        public decimal penalty_for_delay;       // 	Multa por atraso	Decimal	20 dígitos e decimais	
        public decimal? delay_interest;         // 	Juros	Decimal	20 dígitos e decimais	
        public string date_received;            // 	Data do recebimento/pagamento	Data	 	
        public decimal? delay_interest_total;   // 	Juros totais	Decimal	20 dígitos e decimais	
        public decimal discount;                // 	Desconto	Decimal	20 dígitos e decimais	
        public int? transporter;                // 	Identificador da transportadora	Inteiro	11 dígitos	

        [JsonIgnore]
        public DateTime? DueDateConvert { get => GetDate(due_date); set => due_date = SetDate(value); }

        [JsonIgnore]
        public DateTime? DateReceivedConvert { get => GetDate(date_received); set => date_received = SetDate(value); }

        public override string ToString()
        {
            return $"{id}: {(incoming ? "E":"S")} {description} R$ {amount} - {discount} = {amount_total}";
        }
    }
}
