using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Services;
using a7D.PDV.Ativacao.API.Services.EmailService;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/validation")]
public sealed class ValidationController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly IEmailService _email;
    readonly UserManager<AppUser> _userManager;
    readonly ILogger<ValidationController> _logger;

    public ValidationController(
        ApplicationDbContext db,
        IEmailService email,
        UserManager<AppUser> userManager,
        ILogger<ValidationController> logger)
    {
        _db = db;
        _email = email;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// GET: /api/validade?id={activationKey}&versao={optional}
    /// Retorna a validade (UTC) da licença.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetValidade([FromQuery] string id, CancellationToken ct)
    {
        string? versao = null;
        string status = "LIC";

        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Activation key is required.");

            versao = Request.Query.TryGetValue("versao", out var vv) ? vv.ToString() : null;

            var activation = await _db.Activations
                .AsNoTracking()
                .Include(a => a.PDVs)
                .FirstOrDefaultAsync(a => a.ActivationKey == id, ct);

            if (activation is null)
                throw new InvalidOperationException("Chave de ativação inválida");

            if (!activation.IsActive && !activation.ReactivatedBySupport)
                throw new InvalidOperationException("Chave de ativação desativada");

            if (activation.PDVs is null || activation.PDVs.Count == 0)
                throw new InvalidOperationException("Nenhum produto configurado pra essa licença");

            if (activation.ReactivatedBySupport)
            {
                if (activation.ProvisionalValidityUntil is null)
                    throw new InvalidOperationException("Chave de ativação desativada");

                return Ok(new { Validade = activation.ProvisionalValidityUntil.Value.ToUniversalTime() });
            }

            // válido por dias padrão
            var utc = DateTime.UtcNow.AddDays(activation.ValidityDays);
            return Ok(new { Validade = utc });
        }
        catch (DbUpdateException eex)
        {
            _logger.LogError(eex, "Erro de dados em GetValidade para {Key}", id);
            return NotFound(new { error = eex.Message });
        }
        catch (Exception ex)
        {
            status = ex.Message; // para telemetria
            _logger.LogError(ex, "Erro em GetValidade para {Key}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
        finally
        {
            if (!string.IsNullOrEmpty(id))
            {
                try { ClientesService.Registra(id, versao ?? string.Empty, status, erro: false); }
                catch { /* best-effort */ }
            }
        }
    }

    /// <summary>
    /// POST: /api/validade/liberacao/{id}
    /// Libera provisoriamente (3 dias úteis) uma ativação desativada.
    /// </summary>
    [HttpPost("liberacao/{id:int}")]
    public async Task<IActionResult> Liberacao([FromRoute] int id, CancellationToken ct)
    {
        try
        {
            var activation = await _db.Activations
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (activation is null)
                return NotFound();

            // Já ativa ou já possui liberação anterior → 202 Accepted com a validade atual
            if (activation.IsActive || activation.ReactivatedBySupport)
            {
                if (activation.ProvisionalValidityUntil is null)
                    return Accepted(new { Validade = (DateTime?)null });

                return Accepted(new { Validade = activation.ProvisionalValidityUntil.Value.ToUniversalTime() });
            }

            var now = DateTime.Now;

            activation.ReactivatedBySupport = true;
            activation.SupportReactivatedAt = now;
            activation.ProvisionalValidityUntil = AddBusinessDays(now, 3);

            await _db.SaveChangesAsync(ct);

            // Notifica por e-mail se NÃO for admin
            if (!User.IsInRole("Admin"))
            {
                var currentUser = await _userManager.GetUserAsync(User)
                                  ?? new AppUser { UserName = "system", Email = "noreply@localhost", Name = "System" };

                await _email.SendActivationAsync(
                    ETipoEmailAtivacao.LiberacaoTemporaria,
                    activation,
                    currentUser
                );
            }

            return Ok(new { Validade = activation.ProvisionalValidityUntil.Value.ToUniversalTime() });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao liberar validade temporária para {Id}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // --- Helpers ---

    // Adiciona dias úteis (ignora sábados e domingos).
    static DateTime AddBusinessDays(DateTime start, int businessDays)
    {
        if (businessDays <= 0) return start;
        var date = start;
        var added = 0;

        while (added < businessDays)
        {
            date = date.AddDays(1);
            if (date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
                added++;
        }

        return date;
    }
}
