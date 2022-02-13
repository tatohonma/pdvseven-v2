using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    public class Querys
    {
        [Key, Required]
        public int IDQuery { get; set; }

        [MaxLength(100), Required]
        public string Nome { get; set; }

        public string Title { get; set; }

        [Column(TypeName = "TEXT")]
        public string Query { get; set; }
    }
}