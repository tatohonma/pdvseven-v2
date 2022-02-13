using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesTorneira : ConfiguracaoBD
    {
        public ConfiguracoesTorneira()
            : base((int)ETipoPDV.TORNEIRA, null)
        {
        }

        [Config("Serial ESP Toneira", ETipoPDV.TORNEIRA, Valor = "0", Obrigatorio = true)]
        public int SerialESP { get; set; }

        [Config("ID Produto", ETipoPDV.TORNEIRA, Valor = "0", Obrigatorio = true)]
        public int idProduto { get; set; }

        [Config("Senha para sair do programa", ETipoPDV.TORNEIRA, Valor = "9999", Obrigatorio = true)]
        public string SenhaSaida { get; set; }

        [Config("Chave do usuário a ser usado na torneira", ETipoPDV.TORNEIRA, Valor = "9933", Obrigatorio = true)]
        public string ChaveUsuario { get; set; }
    }
}
