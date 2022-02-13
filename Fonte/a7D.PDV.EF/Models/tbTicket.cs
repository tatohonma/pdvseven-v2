using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    [Table("tbTicket")]
    public class tbTicket
    {
        public tbTicket()
        {
            PedidoProduto = new List<tbPedidoProduto>();
        }

        [Key()]
        [Required]
        public int IDTicket { get; set; }

        [Required]
        public int IDPedidoProduto { get; set; }

        public DateTime? dtUtilizacao { get; set; }

        public int? IDUtilizacaoPDV { get; set; }

        public int? IDUtilizacaoUsuario { get; set; }

        public virtual ICollection<tbPedidoProduto> PedidoProduto { get; set; }

        public override string ToString() => $"{IDTicket}: {IDPedidoProduto} {dtUtilizacao} {IDUtilizacaoPDV} {IDUtilizacaoUsuario}";
    }
}

