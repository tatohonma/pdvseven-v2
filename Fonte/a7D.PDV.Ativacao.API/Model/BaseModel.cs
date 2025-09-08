using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.API.Model;

public abstract class BaseModel
{
    [Column(TypeName = "datetime2")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    [Column(TypeName = "datetime2")]
    public DateTime? UpdatedAt { get; set; }
}
