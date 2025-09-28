using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("pdvs_activations")] 
public class PdvActivation : BaseModel
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(128)]
    public string ActivationKey { get; set; } = null!;

    public int PdvId { get; set; }

    public int PdvTypeId { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(255)]
    public string? HardwareKey { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    [MaxLength(50)]
    public string? Version { get; set; }

    public bool IsActive { get; set; }
}
