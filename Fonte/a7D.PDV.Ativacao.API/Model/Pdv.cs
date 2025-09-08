using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("Pdvs")] 
public class Pdv : BaseModel
{
    [Key]
    [Column("IDPDV")]
    public int Id { get; set; }

    [Required]
    [Column("IDAtivacao")]
    public int ActivationId { get; set; }

    [ForeignKey(nameof(ActivationId))]
    public virtual Activation Activation { get; set; } = null!;

    /// <summary>
    /// Optional link to installation PDV (self-reference).
    /// </summary>
    [Column("IDPDV_instalacao")]
    public int? InstallationPdvId { get; set; }

    [Required]
    [Column("IDTipoPDV")]
    public int PdvTypeId { get; set; }

    [ForeignKey(nameof(PdvTypeId))]
    public virtual PdvType PdvType { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? HardwareKey { get; set; }

    /// <summary>
    /// Last update timestamp for this PDV.
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }

    public bool IsActive { get; set; }

    [MaxLength(50)]
    public string? Version { get; set; }

    // --- Legacy helper preserved (se ainda precisar) ---
    /// <summary>
    /// Generates a raw SQL insert (legacy use only).
    /// </summary>
    public string ToInsertScript(int nextId)
    {
        return $"INSERT [dbo].[tbPDV] ([IDPDV], [IDTipoPDV], [ChaveHardware], [Nome], [UltimoAcesso], [Ativo]) " +
               $"VALUES ({nextId}, {PdvTypeId}, NULL, '{Name}', NULL, 1)";
    }
}
