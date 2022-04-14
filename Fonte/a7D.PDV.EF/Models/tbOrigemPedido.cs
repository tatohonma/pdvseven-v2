using a7D.PDV.EF.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    [Table("tbOrigemPedido")]
    public partial class tbOrigemPedido : IValueName
    {
        [Key]
        [Required]
        public int IDOrigemPedido { get; set; }
        [Required]
        public string Nome { get; set; }

        public EOrigemPedido Tipo => (EOrigemPedido)IDOrigemPedido;

        public ValueName GetValueName() => new ValueName(IDOrigemPedido, Nome);

        public void SetValueName(int value, string name)
        {
            IDOrigemPedido = value;
            Nome = name;
        }
    }
}
