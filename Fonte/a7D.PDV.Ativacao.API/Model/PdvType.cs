using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("PdvTypes")] // ajuste se quiser manter "tbTipoPDV"
public class PdvType : BaseModel
{
    [Key]
    [Column("IDTipoPDV")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;
}
