using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesTerminalWindows : ConfiguracoesComumPedidos
    {
        // Singleton!
        public static ConfiguracoesTerminalWindows Valores { get; private set; }

        public ConfiguracoesTerminalWindows(int idPdv): 
            base((int)ETipoPDV.TERMINAL_WIN, idPdv)
        {
            Valores = this;
        }

        [Config("Sempre solicitar autenticação", ETipoPDV.TERMINAL_WIN, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AutenticarSempre { get; protected set; }

        [Config("Leitura de código de barras", ETipoPDV.TERMINAL_WIN, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool CodigoBarras { get; protected set; }

        [Config("Quantidade de Dígitos para o Código do produto", ETipoPDV.TERMINAL_WIN, Valor = "4", Obrigatorio = true)]
        public int DigitosCodigo { get; protected set; }

        [Config("Esconder Mesa", ETipoPDV.TERMINAL_WIN, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool EsconderMesa { get; protected set; }

        [Config("Esconder Comanda", ETipoPDV.TERMINAL_WIN, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool EsconderComanda { get; protected set; }

        [Config("Porta da Balança", ETipoPDV.TERMINAL_WIN, Valor = "COM1", Obrigatorio = true)]
        public string PortaBalanca { get; protected set; }

        [Config("Protocolo de comunicação da Balança", ETipoPDV.TERMINAL_WIN, Valor = "TOLEDO", ValoresAceitos = "TOLEDO|FILIZOLA", Obrigatorio = true)]
        public string ProtocoloBalanca { get; protected set; }

        [Config("O que está sendo impresso na etiqueta", ETipoPDV.TERMINAL_WIN, Valor = "peso", ValoresAceitos = "peso:Peso|preco:Preço", Obrigatorio = true)]
        public string PrecoPeso { get; protected set; }

        public bool BalancaPorPeso => string.Compare(PrecoPeso, "peso", true) == 0;

        [Config("Utiliza Balança Etiquetadora", ETipoPDV.TERMINAL_WIN, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool EtiquetaBalanca { get; protected set; }

        [Config("Touch Parametros (Fonte, Largura, Altura, Tempo de Cache)", ETipoPDV.TERMINAL_WIN, Valor = "12,120,60,2")]
        public string TouchParametros { get; protected set; }

    }
}
