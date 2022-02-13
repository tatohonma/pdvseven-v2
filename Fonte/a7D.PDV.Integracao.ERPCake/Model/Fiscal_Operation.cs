using System;
namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#operacoes-fiscais
    public class Fiscal_Operation : ModelCake
    {
        public string name; // Nome da operação fiscal
        public int? operation; // Identificador do tipo da operação
        public string type; // Tipo da operação
        public string sender_receiver; // Tipo de destinatário
        public int? financial_account; // Centro de custo
        public string change_stock; // Indicador de altera estoque
    }
}
