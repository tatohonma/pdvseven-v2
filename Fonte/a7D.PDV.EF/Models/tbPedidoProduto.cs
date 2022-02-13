using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPedidoProduto
    {
        public tbPedidoProduto()
        {
            this.tbPedidoProduto1 = new List<tbPedidoProduto>();
        }

        public int IDPedidoProduto { get; set; }
        public int IDPedido { get; set; }
        public int IDProduto { get; set; }
        public int? IDPedidoProduto_pai { get; set; }
        public int? IDTipoEntrada { get; set; }
        public int? IDPDV { get; set; }
        public int? IDUsuario { get; set; }
        public int? IDPDV_cancelamento { get; set; }
        public int? IDUsuario_cancelamento { get; set; }
        public int? IDMotivoCancelamento { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? ValorAliquota { get; set; }
        public string CodigoAliquota { get; set; }
        public string Notas { get; set; }
        public bool Cancelado { get; set; }
        public bool? Viagem { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
        public string ObservacoesCancelamento { get; set; }
        public DateTime? DtCancelamento { get; set; }
        public string GUIDControleDuplicidade { get; set; }
        public bool RetornarAoEstoque { get; set; }
        public int? IDPainelModificacao { get; set; }
        public decimal? ValorDesconto { get; set; }
        public int? IDUsuarioDesconto { get; set; }
        public int? IDTipoDesconto { get; set; }
        public virtual tbMotivoCancelamento tbMotivoCancelamento { get; set; }
        public virtual tbPainelModificacao tbPainelModificacao { get; set; }
        public virtual PDVInformation tbPDV { get; set; }
        public virtual PDVInformation tbPDV1 { get; set; }
        public virtual tbPedido tbPedido { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProduto1 { get; set; }
        public virtual tbPedidoProduto tbPedidoProduto2 { get; set; }
        public virtual tbProduto tbProduto { get; set; }
        public virtual tbTipoDesconto tbTipoDesconto { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbUsuario tbUsuario2 { get; set; }
    }
}
