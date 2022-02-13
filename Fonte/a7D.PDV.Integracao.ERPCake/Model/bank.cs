namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#bancos
    public class Bank : ModelCake
    {
        public int? base_bank;                  //  *	Idenficador do Banco base	Inteiro	11 dígito(s)	
        public string name;                     //  *	Nome do banco	Texto	255 posições	
        public int agency;                      //  *	Agência	Inteiro	11 dígito(s)	
        public int account;                     //  *	Conta	Inteiro	11 dígito(s)	
        public bool? boleto;                    // 	Flag que indica se emite boleto	Booleano	1 dígito(s)	
        public int? transferor_account;         // 	Conta do cedente	Inteiro	11 dígito(s)	
        public int? wallet;                     // 	Carteira padrão	Inteiro	11 dígito(s)	
        public string demonstrative;            // 	Mensagem impressa	Texto	4000 posições	
        public string instructions;             // 	Instruções	Texto	4000 posições	
        public int? payment_days;               // 	Dias para o vencimento	Inteiro	11 dígito(s)	
        public decimal? tax;                    // 	Taxa do boleto	Decimal	20 dígito(s) e 2 decimais	
        public bool? accept_after_expiration;   // 	Aceita depois de vencido	Booleano	1 dígito(s)	
        public string document_species;         // 	Espécie de documento	Texto	3 posições	
        public bool? agreement;		            // 	Convênio	Booleano	1 dígito(s)	

        public override string ToString()
        {
            return $"{id}: {name}";
        }
    }
}
