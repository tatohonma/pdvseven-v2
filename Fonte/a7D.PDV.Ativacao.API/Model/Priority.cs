using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("Priorities")] // ajuste se quiser manter "tbPrioridade"
public class Priority : BaseModel
{
    [Key]
    [Column("IDPrioridade")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;
}
