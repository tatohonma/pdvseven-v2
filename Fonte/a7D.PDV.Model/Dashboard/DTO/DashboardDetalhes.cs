using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard.DTO
{
    [DataContract]
    public class DashboardDetalhes
    {
        [DataMember(Name = "categoria")]
        public _Data Categoria { get; set; }

        [DataMember(Name = "tipoPagamento")]
        public _Data TipoPagamento { get; set; }

        [DataMember(Name = "tipoPedido")]
        public _Data TipoPedido { get; set; }

        [DataMember(Name = "volumeGarcom")]
        public _Data VolumeGarcom { get; set; }

        [DataMember(Name = "diaDaSemana")]
        public _Data DiaDaSemana { get; set; }

        [DataMember(Name = "motivosCancelamento")]
        public _Data MotivosCancelamento { get; set; }
    }
}
