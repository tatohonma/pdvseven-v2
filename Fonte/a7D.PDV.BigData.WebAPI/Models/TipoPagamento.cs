
using a7D.PDV.BigData.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("TipoPagamento")]
    public class TipoPagamento : bdTipoPagamento
    {
        [Key, Column(Order = 1), Required, ForeignKey("Entidade")]
        public int IDEntidade { get; set; }

        public virtual Entidade Entidade { get; set; }

        internal void UpdateWith(bdTipoPagamento source)
        {
            Nome = source.Nome;
            dtAlteracao = source.dtAlteracao;
        }
    }
}