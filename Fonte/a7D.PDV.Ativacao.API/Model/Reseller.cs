using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("Resellers")] // ajuste se quiser manter "tbRevenda"
public class Reseller : BaseModel
{
    [Key]
    [Column("IDRevenda")]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Business code or identifier for the reseller.
    /// </summary>
    [Required]
    public int Code { get; set; }
}
