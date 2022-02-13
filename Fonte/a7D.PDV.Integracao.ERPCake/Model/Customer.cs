using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#clientes
    public class Customer : ModelCake
    {
        public int? group; // Identificador do grupo de clintes
        public string name; // Nome do cliente
        public string name_fantasy; // Nome Fantasia do cliente
        public string person_type = "F"; // Tipo da pessoa
        public string doc_cnpj; // Cnpj se for pessoa jurídica
        public string doc_ie; // Inscrição estadual se tiver
        public string doc_im; // Inscrição municipal
        public string doc_cpf; // Cpf se for pessoa física
        public string contact_name; // Nome para contato
        public string contact_email; // Email de contato
        public string contact_phone; // Telefone de contato
        public string contact_fax; // Numero do fax
        public string contact_cellphone; // Telefone Celular
        public string gender; // sexo, “F” para feminino e “M” para masculino
        public string birthday; // data de nascimento, formato “DD/MM/YYYY”
        public string address_street; // Descrição da rua do endereço
        public string address_number; // Número do endereço
        public string address_complement; // Complemento do endereço
        public string address_district; // Identificador do bairro ou descrição do bairro
        public int? address_city; // Identificador da cidade ou descrição da cidade
        public int? address_state; // Identificador do estado ou descrição do estado
        public int? address_country; // Identificador para o pais
        public string address_zip_code; // Número do CEP
        public string observation; // Observações do cliente
        public string sales_order_obs; // Observações para venda ao cliente
        public int? ind_ie_dest; // Indicador de inscrição estadual do destinatário
        public bool? ind_exterior; // Flag para pessoa do exterior
        public bool? active; // Flag se esta ativo
        public string code; // Código da pessoa
        public DateTime? register_date; // Data de cadastro

        [JsonIgnore]
        public DateTime? BirthdayConvert { get => GetDate(birthday); set => birthday = SetDate(value); }

        public override bool RequerAlteracaoPDV(DateTime dtSync, out int id)
            => (!int.TryParse(code, out id) || register_date > dtSync) && code != "PDV7"; // Não importa o cliente padrão

        public override void SetCode(string code) => this.code = code;

        public override string ToString() => $"{id}: {name}";
    }
}
