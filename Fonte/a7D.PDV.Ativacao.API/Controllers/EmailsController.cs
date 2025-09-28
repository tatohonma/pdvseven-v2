using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using a7D.PDV.Ativacao.API.Repository.User;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ApiAuth(requerAdm: true)] // Se você já portou esse atributo para Core, descomente.
// [Authorize(Policy = "AdminOnly")] // alternativa via policy/role.
public sealed class EmailsController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly ILogger<EmailsController> _logger;

    public EmailsController(IUserRepository users, ILogger<EmailsController> logger)
    {
        _users = users;
        _logger = logger;
    }

    /// <summary>
    /// Verifica se o e-mail já está em uso. Retorna 409 (Conflict) se existir; 204 (NoContent) se estiver livre.
    /// </summary>
    /// <param name="email">E-mail a validar</param>
    /// <param name="id">Opcional: ID inteiro do usuário a ignorar na verificação (edição)</param>
    [HttpGet("valid")]
    public async Task<IActionResult> EmailValido(
        [FromQuery][Required, EmailAddress] string email,
        [FromQuery] int? id,
        CancellationToken ct)
    {
        try
        {
            var exists = await _users.EmailExistsAsync(email, ignoreUserIntId: id, ct);
            if (exists)
                return Conflict(); // 409

            return NoContent();    // 204
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar e-mail {Email}", email);
            // 500 com Problem Details
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
