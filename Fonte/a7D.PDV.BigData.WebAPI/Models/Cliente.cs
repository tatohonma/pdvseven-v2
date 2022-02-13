
using a7D.PDV.BigData.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("Cliente")]
    public class Cliente : bdCliente
    {
        [Key, Column(Order = 1), Required, ForeignKey("Entidade")]
        public int IDEntidade { get; set; }

        public virtual Entidade Entidade { get; set; }

        internal void UpdateWith(bdCliente source)
        {
            NomeCompleto = source.NomeCompleto;
            DataNascimento = source.DataNascimento;
            Sexo = source.Sexo;
            dtAlteracao = source.dtAlteracao;
        }
    }
}