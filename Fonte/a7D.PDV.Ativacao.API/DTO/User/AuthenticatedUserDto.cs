using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.DTO.User;

public class AuthenticatedUserDto
{
    public AppUser User { get; set; } = default!;
    public List<string> Roles { get; set; } = new();

    
    // public bool HasRole(string role) => Roles.Contains(role);
    public bool HasAnyRole(params string[] roles)
    {
        return Roles.Any(r => roles.Contains(r));
    }
}
