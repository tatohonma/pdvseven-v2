
using a7D.PDV.BigData.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("Usuario")]
    public class Usuario : bdUsuario
    {
        [Key, Column(Order = 1), Required, ForeignKey("Entidade")]
        public int IDEntidade { get; set; }

        public virtual Entidade Entidade { get; set; }

        internal void UpdateWith(bdUsuario source)
        {
            Nome = source.Nome;
            Senha = source.Senha;
            Ativo = source.Ativo;
            dtAlteracao = source.dtAlteracao;
        }
    }
}