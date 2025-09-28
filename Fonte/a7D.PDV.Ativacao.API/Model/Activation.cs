using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace a7D.PDV.Ativacao.API.Model;

[Table("activation")]
public class Activation : BaseModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual Client Client { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string ActivationKey { get; set; } = null!;

    public DateTime? LastCheckedAt { get; set; }

    public DateTime? ActivatedAt { get; set; }

    [Required]
    public int ValidityDays { get; set; }

    public bool IsActive { get; set; }

    public string? Notes { get; set; }

    public bool ReactivatedBySupport { get; set; }

    public DateTime? SupportReactivatedAt { get; set; }

    public DateTime? ProvisionalValidityUntil { get; set; }

    public bool IsDuplicate { get; set; }

    [NotMapped]
    public bool? SiteAdmin { get; set; }

    public virtual List<Pdv> PDVs { get; set; } = new();

    [NotMapped]
    public string LicensesHtml
    {
        get
        {
            if (PDVs == null || PDVs.Count == 0)
                return string.Empty;

            var groups = PDVs
                .GroupBy(p => new { p.PdvTypeId, Name = p.PdvType?.Name })
                .Select(g => new { g.Key.Name, Count = g.Count() })
                .OrderBy(g => g.Name);

            var sb = new StringBuilder();
            foreach (var g in groups)
            {
                var name = string.IsNullOrWhiteSpace(g.Name) ? "(type not loaded)" : g.Name;
                sb.AppendFormat("{0} {1}<br />", g.Count, name);
            }
            return sb.ToString();
        }
        set{}
    }
}