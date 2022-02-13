using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{

    public partial class InsercaoPagamento
    {

        public InsercaoPagamento(string GUIDISolicitacao = default(string), int? IDTIpoPedido = default(int?), int Numero = default(int), int? IDUsuario = default(int?), int? IDPDV = default(int?), List<Pagamento> Pagamentos = default(List<Pagamento>))
        {
            this.GUIDSolicitacao = GUIDISolicitacao;
            this.IDTipoPedido = IDTIpoPedido;
            this.Numero = Numero;
            this.IDUsuario = IDUsuario;
            this.IDPDV = IDPDV;
            this.Pagamentos = Pagamentos;
        }

        [DataMember(Name = "GUIDSolicitacao", EmitDefaultValue = false)]
        public string GUIDSolicitacao { get; set; }
        [DataMember(Name = "IDTipoPedido", EmitDefaultValue = false)]
        public int? IDTipoPedido { get; set; }
        [DataMember(Name = "Numero", EmitDefaultValue = false)]
        public int Numero { get; set; }
        [DataMember(Name = "IDUsuario", EmitDefaultValue = false)]
        public int? IDUsuario { get; set; }
        [DataMember(Name = "IDPDV", EmitDefaultValue = false)]
        public int? IDPDV { get; set; }
        [DataMember(Name = "Pagamentos", EmitDefaultValue = false)]
        public List<Pagamento> Pagamentos { get; set; }
    }
}
