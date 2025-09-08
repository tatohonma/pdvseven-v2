using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("PdvActivations")] // ajuste conforme o nome real da tabela
public class PdvActivation : BaseModel
{
    [Key]
    [Column("IDPDVAtivacao")]
    public int Id { get; set; }

    [Required, MaxLength(128)]
    public string ActivationKey { get; set; } = null!;

    [Column("IDPDV")]
    public int PdvId { get; set; }

    [Column("IDTipoPDV")]
    public int PdvTypeId { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(255)]
    public string? HardwareKey { get; set; }

    /// <summary>
    /// Last update timestamp for this PDV activation.
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }

    [MaxLength(50)]
    public string? Version { get; set; }

    public bool IsActive { get; set; }
}
