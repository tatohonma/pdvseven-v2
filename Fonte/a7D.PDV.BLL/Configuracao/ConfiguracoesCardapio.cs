using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Runtime.Serialization;

namespace a7D.PDV.BLL
{
    [Serializable]
    [DataContract]
    public class ConfiguracoesCardapio : ConfiguracoesComandaTerminal
    {
        public ConfiguracoesCardapio()
            : base(-1, ETipoPDV.CARDAPIO_DIGITAL, "")
        {
        }

        public ConfiguracoesCardapio(int idPdv, string versapAPI) 
            : base(idPdv, ETipoPDV.CARDAPIO_DIGITAL, versapAPI)
        {
        }

        [Config("Verificar por novas imagens", ETipoPDV.CARDAPIO_DIGITAL, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [DataMember]
        public bool VerifImagens { get; set; }

        [Config("Senha para definir mesa", ETipoPDV.CARDAPIO_DIGITAL, Valor = "555")]
        [DataMember]
        public string SenhaMesa { get; set; }

        [Config("Chave do usuário a ser usado no Cardápio", ETipoPDV.CARDAPIO_DIGITAL, Valor = "9933", Obrigatorio = true)]
        [DataMember]
        public string ChaveUsuario { get; set; }

        [Config("Tipo de pedido", ETipoPDV.CARDAPIO_DIGITAL, Valor = "NAO", ValoresAceitos = "MESA:Mesa|COMANDADIGITACAO:Comanda por digitação|COMANDALEITORA:Comanda por leitora", Obrigatorio = true)]
        [DataMember]
        public string UsarComanda { get; set; }
    }

}
