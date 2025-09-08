using System.Security.Claims;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace a7D.PDV.Ativacao.API.Controllers;

// Em APIs novas, a rota costuma ser definida nos controllers concretos.
// Se preferir, você pode manter um Route aqui também.
[ApiController]
[Authorize] // Garante que apenas requisições autenticadas acessem controladores que herdam desta base
public abstract class BaseSecureApiController : ControllerBase
{
    protected readonly ApplicationDbContext Db;
    protected readonly UsuariosRepository Usuarios;

    protected BaseSecureApiController(ApplicationDbContext db, UsuariosRepository usuarios)
    {
        Db = db;
        Usuarios = usuarios;
    }

    /// <summary>
    /// Verifica se a requisição possui a claim de "role" com valor "adm".
    /// Também considera ClaimTypes.Role (caso a lib de emissão mude o type).
    /// </summary>
    protected bool IsAdminRequest()
    {
        var user = HttpContext?.User;
        if (user is null || !user.Identity?.IsAuthenticated == true)
            return false;

        return user.IsInRole("adm")
               || HasClaim(user, "role", "adm")
               || HasClaim(user, ClaimTypes.Role, "adm");
    }

    static bool HasClaim(ClaimsPrincipal user, string type, string value)
        => user.HasClaim(c => c.Type == type && c.Value == value);

    /// <summary>
    /// Retorna o usuário associado ao token da requisição, usando o SID (Id) da claim.
    /// </summary>
    protected async Task<Usuario?> UsuarioRequisicaoAsync(CancellationToken ct = default)
    {
        var id = ObterUsuarioIdDasClaims();
        if (id is null)
            return null;

        return await Usuarios.GetAsync(id.Value, ct);
    }

    /// <summary>
    /// Tenta ler o Id do usuário a partir de ClaimTypes.Sid (ou "sid").
    /// </summary>
    protected int? ObterUsuarioIdDasClaims()
    {
        var user = HttpContext?.User;
        if (user is null || !user.Identity?.IsAuthenticated == true)
            return null;

        var sid = user.FindFirstValue(ClaimTypes.Sid) ?? user.FindFirstValue("sid");
        if (int.TryParse(sid, out var id))
            return id;

        return null;
    }
}