using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Runtime.Serialization;

namespace a7D.PDV.BLL
{
    [Serializable]
    [DataContract]
    public class ConfiguracoesSaida : ConfiguracaoBD
    {
        public ConfiguracoesSaida()
            : base(-1, null)
        {
        }

        public ConfiguracoesSaida(int idPdv, string versapAPI)
            : base((int)ETipoPDV.SAIDA, idPdv)
        {
            Versao = versapAPI;
        }

        [DataMember]
        public string Versao { get; set; }

        [Config("Senha para sair do programa", ETipoPDV.SAIDA, Valor = "9999", Obrigatorio = true)]
        [DataMember]
        public string SenhaSaida { get; set; }

        [Config("Chave do usuário a ser usado no autoatendimento", ETipoPDV.SAIDA, Valor = "9933", Obrigatorio = true)]
        [DataMember]
        public string ChaveUsuario { get; set; }
    }
}
