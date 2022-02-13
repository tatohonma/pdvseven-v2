using a7D.PDV.BigData.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("Entidade")]
    public class Entidade : bdEntidade
    {
        [Key]
        public int IDEntidade { get; set; }

        [Required]
        public string ChaveAtivacao { get; set; }
    }
}