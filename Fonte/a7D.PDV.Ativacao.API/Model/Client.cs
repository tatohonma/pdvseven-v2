using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

[Table("clients")] // ajuste conforme o nome real da tabela
public class Client : BaseModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ResellerId { get; set; }

    [ForeignKey(nameof(ResellerId))]
    public virtual Reseller Reseller { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string? CompanyName { get; set; }

    /// <summary>CNPJ or CPF (Brazilian tax id)</summary>
    [MaxLength(20)]
    public string? CpfCnpj { get; set; }

    [MaxLength(255)]
    public string? Street { get; set; }

    [MaxLength(20)]
    public string? Number { get; set; }

    [MaxLength(100)]
    public string? AdditionalInfo { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(2)]
    public string? State { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    /// <summary>ID used to sync with Tiny ERP</summary>
    [MaxLength(50)]
    public string? TinyId { get; set; }
}
