using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Runtime.Serialization;

namespace a7D.PDV.BLL
{
    [Serializable]
    [DataContract]
    public class ConfiguracoesComandaTerminal : ConfiguracoesComumPedidos
    {
        public ConfiguracoesComandaTerminal() :
            base(-1, null)
        {
        }

        public ConfiguracoesComandaTerminal(int idPdv, ETipoPDV tipo, string versapAPI)
            : base((int)tipo, idPdv)
        {
            Versao = versapAPI;
        }

        [DataMember]
        public string Versao { get; set; }

        [Config("Sempre solicitar autenticação", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Sempre solicitar autenticação", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool AutenticarSempre { get; set; }

        [Config("Solicitar Referência", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Solicitar Referência", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool SolicitarRef { get; set; }

        [Config("Exibir resumo do pedido", ETipoPDV.TERMINAL_TAB, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Exibir resumo do pedido", ETipoPDV.COMANDA_ELETRONICA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool MostrarResumo { get; set; }

        [Config("Exibir fechamento de Conta", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Exibir fechamento de Conta", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool MostrarFecharConta { get; set; }

        [Config("Exibir lista de produtos", ETipoPDV.TERMINAL_TAB, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Exibir lista de produtos", ETipoPDV.COMANDA_ELETRONICA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool MostrarLista { get; set; }

        [Config("Solicitar info de fechamento", ETipoPDV.TERMINAL_TAB, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Solicitar info de fechamento", ETipoPDV.COMANDA_ELETRONICA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Solicitar info de fechamento", ETipoPDV.CARDAPIO_DIGITAL, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool SolicitarPessoas { get; set; }

        [Config("Solicitar preço quando preço é zero", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Solicitar preço quando preço é zero", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool AskPrice { get; set; }

        [Config("Local de impressão de conta (ID) se diferente de 'Conta Padrão'", ETipoPDV.TERMINAL_TAB, Valor = "-1", Obrigatorio = true)]
        [Config("Local de impressão de conta (ID) se diferente de 'Conta Padrão'", ETipoPDV.COMANDA_ELETRONICA, Valor = "-1", Obrigatorio = true)]
        [Config("Local de impressão de conta (ID) se diferente de 'Conta Padrão'", ETipoPDV.CARDAPIO_DIGITAL, Valor = "-1", Obrigatorio = true)]
        [DataMember]
        public int ImpressaoConta { get; set; }

        [Config("Tipo de Cliente", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0:CPF/CNPJ|1:E-mail|2:Telefone", Obrigatorio = true)]
        [Config("Tipo de Cliente", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0:CPF/CNPJ|1:E-mail|2:Telefone", Obrigatorio = true)]
        [Config("Tipo de Cliente", ETipoPDV.CARDAPIO_DIGITAL, Valor = "0", ValoresAceitos = "0:CPF/CNPJ|1:E-mail|2:Telefone", Obrigatorio = true)]
        [DataMember]
        public string TipoCliente { get; set; }

        [Config("Permitir CheckIn por Tipo de Cliente", ETipoPDV.TERMINAL_TAB, Valor = "NAO", ValoresAceitos = "NAO:Não|SIM:Sim, somente de cadastros existentes|NOVO:Sim, cadastros existentese ou novos", Obrigatorio = true)]
        [Config("Permitir CheckIn por Tipo de Cliente", ETipoPDV.COMANDA_ELETRONICA, Valor = "NAO", ValoresAceitos = "NAO:Não|SIM:Sim, somente de cadastros existentes|NOVO:Sim, cadastros existentese ou novos", Obrigatorio = true)]
        [DataMember]
        public string AbrirComanda { get; set; }

        [Config("Senha para sair do programa", ETipoPDV.TERMINAL_TAB, Valor = "9999", Obrigatorio = true)]
        [Config("Senha para sair do programa", ETipoPDV.COMANDA_ELETRONICA, Valor = "9999", Obrigatorio = true)]
        [Config("Senha para sair do programa", ETipoPDV.CARDAPIO_DIGITAL, Valor = "9999", Obrigatorio = true)]
        [DataMember]
        public string Senha { get; set; }

        [Config("Senha para configurar o programa", ETipoPDV.TERMINAL_TAB, Valor = "2606", Obrigatorio = true)]
        [Config("Senha para configurar o programa", ETipoPDV.COMANDA_ELETRONICA, Valor = "2606", Obrigatorio = true)]
        [Config("Senha para configurar o programa", ETipoPDV.CARDAPIO_DIGITAL, Valor = "2606", Obrigatorio = true)]
        [DataMember]
        public string Config { get; set; }
    }
}
