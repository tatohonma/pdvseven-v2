using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace a7D.PDV.Ativacao.API.Model;

public class AppUser : IdentityUser 
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime2")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? LastPasswordChangeAt { get; set; }

    [MaxLength(255)]
    public string? RefreshToken { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? RefreshTokenExpiresAt { get; set; }
}
