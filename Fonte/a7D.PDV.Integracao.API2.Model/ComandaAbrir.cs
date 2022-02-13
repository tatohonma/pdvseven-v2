using System.Runtime.Serialization;
using System.Text;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public partial class ComandaAbrir
    {
        [DataMember()]
        public object Comanda { get; set; }

        [DataMember()]
        public int ClienteID { get; set; }

        [DataMember()]
        public int PDVID { get; set; }

        [DataMember()]
        public int UsuarioID { get; set; }

        [DataMember()]
        public int? IDTipoEntrada { get; set; }

        [DataMember()]
        public bool Validar { get; set; }

    }
}
