using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum ETipoPDV
    {
        [Description("BackOffice")] BACKOFFICE = 1,
        [Description("Caixa")] CAIXA = 10,
        [Description("Terminal Android")] TERMINAL_TAB = 20,
        [Description("Terminal Windows")] TERMINAL_WIN = 30,
        [Description("Comanda Eletrônica")] COMANDA_ELETRONICA = 40,
        [Description("Controle Saída")] SAIDA = 50,
        [Description("Cardápio Digital")] CARDAPIO_DIGITAL = 60,
        [Description("Controle de Estoque")] CONTROLE_ESTOQUE = 70,
        [Description("Gerenciador de Impressao")] GERENCIADOR_IMPRESSAO = 80,
        [Description("Integração Tiny")] INTEGRACAO_TINY = 90,
        [Description("WebService")] WEB_SERVICE = 100,
        [Description("POS Integrado NTK")] POS_INTEGRADO_NTK = 110,
        [Description("POS Integrado Stone")] POS_INTEGRADO_STONE = 120,
        [Description("PagSeven")] PAGSEVEN = 130,
        [Description("Autoatendimento")] AUTOATENDIMENTO = 140,
        [Description("iFood")] IFOOD = 150,
        [Description("ERP")] ERP = 160,
        [Description("Pesquisa de Satisfação")] PESQUISA = 170,
        [Description("Loggi")] LOGGI = 180,
        [Description("EasyChopp")] EASYCHOPP = 190, // ademir@easychopp.com.br
        [Description("BigData")] BigData = 200,
        [Description("Iaago")] IAAGO = 210,
        [Description("Torneira")] TORNEIRA = 220,
        [Description("Validador de Ticket")] TICKET = 230,
        [Description("POS Integrado Granito")] POS_INTEGRADO_GRANITO = 240,
        [Description("Delivery Online")] DELIVERY_ONLINE = 250,
        [Description("Pix-Conta")] PIX_CONTA = 260,
        [Description("Anota-Ai")] ANOTA_AI = 270,
    }
}
