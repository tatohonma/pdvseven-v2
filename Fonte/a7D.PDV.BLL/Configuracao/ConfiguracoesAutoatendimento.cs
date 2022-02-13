using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Runtime.Serialization;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesAutoatendimento : ConfiguracaoBD
    {
        public ConfiguracoesAutoatendimento()
            : base(-1, null)
        {
        }

        public ConfiguracoesAutoatendimento(int idPdv, string versapAPI)
            : base((int)ETipoPDV.AUTOATENDIMENTO, idPdv)
        {
            Versao = versapAPI;
        }

        public string Versao { get; set; }

        [Config("Gerar ordem de produção", ETipoPDV.AUTOATENDIMENTO, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool OrdemImpressao { get; set; }

        [Config("Nome da impressora local", ETipoPDV.AUTOATENDIMENTO, EConfig.ImpressaoLocal, Valor = "ELGIN i9(USB)", Obrigatorio = true)]
        public bool ImpressaoLocal { get; set; }

        [Config("Senha para sair do programa", ETipoPDV.AUTOATENDIMENTO, EConfig.SenhaSaida, Valor = "9999", Obrigatorio = true)]
        public string SenhaSaida { get; set; }

        [Config("Chave do usuário a ser usado no autoatendimento", ETipoPDV.AUTOATENDIMENTO, EConfig.ChaveUsuario, Valor = "9933", Obrigatorio = true)]
        public string ChaveUsuario { get; set; }

        [Config("Exibir ponteiro do Mouse", ETipoPDV.AUTOATENDIMENTO, EConfig.ExibirMouse, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public string ExibirMouse { get; set; }

        [Config("Tempo de desconexão por inatividade", ETipoPDV.AUTOATENDIMENTO, EConfig.TimeoutInativo, Valor = "120", Obrigatorio = true)]
        public string TimeoutInativo { get; set; }

        [Config("Tempo para aviso de desconexão", ETipoPDV.AUTOATENDIMENTO, EConfig.TimeoutAlerta, Valor = "100", Obrigatorio = true)]
        public string TimeoutAlerta { get; set; }

        [Config("Verificar disponibilidade dos produtos (minutos)", ETipoPDV.AUTOATENDIMENTO, EConfig.VerificarDisponibilidade, Valor = "10", Obrigatorio = true)]
        public string VerificarDisponibilidade { get; set; }

        [Config("Tipo de Ticket Pré-Pago", ETipoPDV.AUTOATENDIMENTO, EConfig.GerarTicketPrePago, Valor = "2", ValoresAceitos = "0:Nenhum|1:Padrão (antigo)|2:Últimos 2 dígitos do número do pedido|3:Ticket por Produto (Código de Barras)|4:Ticket por Produto (QRCode)", Obrigatorio = true)]
        public int GerarTicketPrePago { get; set; }

        [Config("Margem de Impressão Windows", ETipoPDV.AUTOATENDIMENTO, EConfig.MargemImpressaoWindows, Valor = "0", Obrigatorio = true)]
        public int MargemImpressaoWindows { get; protected set; }

        [Config("Largura de Impressão Windows", ETipoPDV.AUTOATENDIMENTO, EConfig.LarguraImpressaoWindows, Valor = "280", Obrigatorio = true)]
        public int LarguraImpressaoWindows { get; protected set; }

        [Config("Titulo do Ticket Pré-Pago", ETipoPDV.AUTOATENDIMENTO, EConfig.TituloTicketPrePago, Valor = "Autoatendimento", Obrigatorio = true)]
        public string TituloTicketPrePago { get; set; }

        [Config("Texto de validade do Ticket Pré-Pago", ETipoPDV.AUTOATENDIMENTO, EConfig.ValidadeTicketPrePago, Valor = null)]
        public string ValidadeTicketPrePago { get; set; }

        [Config("Rodape do Ticket Pré-Pago", ETipoPDV.AUTOATENDIMENTO, EConfig.RodapeTicketPrePago, Valor = null, Obrigatorio = true)]
        public string RodapeTicketPrePago { get; set; }

        [Config("Meio de pagamento", ETipoPDV.AUTOATENDIMENTO, EConfig.MeioPagamento, Valor = "STONE", ValoresAceitos = "STONE:Stone TEF|NTKPayGo:PayGo Web|NTKDLL:NTK DLL Dedicado|GranitoTEF:Granito TEF|nenhum:Nenhum", Obrigatorio = true)]
        public string MeioPagamento { get; set; }

        [Config("ID da Catégoria com os produtos referente aos valores de Crédito", ETipoPDV.AUTOATENDIMENTO, Valor = "0", Obrigatorio = true)]
        public int IDCategoriaProduto_Credito { get; set; }

        [Config("ID do Produto de Nova Comanda", ETipoPDV.AUTOATENDIMENTO, Valor = "0", Obrigatorio = true)]
        public int IDProduto_NovaComanda { get; set; }

        [Config("Chave de Configuração TEF Pago", ETipoPDV.AUTOATENDIMENTO, EConfig.PagoChave, Valor = "")]
        public string PagoChave { get; protected set; }
        [Config("ID PDV Granito (TEF)", ETipoPDV.AUTOATENDIMENTO)]
        public string GranitoIDPDV { get; protected set; }

    }
}
