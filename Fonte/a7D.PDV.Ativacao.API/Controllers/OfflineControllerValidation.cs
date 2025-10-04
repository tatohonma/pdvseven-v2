using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Services.EmailService;
using a7D.PDV.Ativacao.Shared.Services;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/offline-validation")]
// [ApiAuth(requerAdm: false)] // habilite se você já portou esse atributo para ASP.NET Core
public sealed class OfflineControllerValidation : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _email;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<OfflineControllerValidation> _logger;

    public OfflineControllerValidation(
        ApplicationDbContext db,
        IEmailService email,
        UserManager<AppUser> userManager,
        ILogger<OfflineControllerValidation> logger)
    {
        _db = db;
        _email = email;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Gera o código de validação offline para a chave informada.
    /// Uso: GET /api/validacaooffline?id=001-06079-14
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Generate([FromQuery] string id, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Activation key (id) is required.");

            var activation = await _db.Activations
                .AsNoTracking()
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.ActivationKey == id, ct);

            if (activation is null)
                return NotFound();

            var payload = new { ativacaoOffline = PDVSecurity.GerarAtivacaoOffline(id) };

            if (!User.IsInRole("Admin"))
            {
                var currentUser = await _userManager.GetUserAsync(User)
                                  ?? new AppUser { UserName = "system", Email = "noreply@localhost" };

                await _email.SendActivationAsync(
                    ETipoEmailAtivacao.AtivacaoOffline,
                    activation,
                    currentUser
                );
            }

            return Ok(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar validação offline para {Key}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
