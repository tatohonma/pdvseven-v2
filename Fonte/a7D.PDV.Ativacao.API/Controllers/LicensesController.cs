using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
// using a7D.PDV.Ativacao.API.Services.EmailService; // se for notificar por e-mail

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/licenses")]
public sealed class LicensesController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly ILogger<LicensesController> _logger;
    readonly static TimeSpan TruncateStep = TimeSpan.FromSeconds(1);

    public LicensesController(ApplicationDbContext db, ILogger<LicensesController> logger /*, IEmailService email */)
    {
        _db = db;
        _logger = logger;
        // _email = email;
    }

    // ===== DTO de entrada (equivalente ao antigo PdvAtivacao) =====
    public sealed class PdvActivationDto
    {
        public string ActivationKey { get; set; } = null!;
        public int PdvId { get; set; } 
        public int PdvTypeId { get; set; }
        public string? Name { get; set; }
        public string? HardwareKey { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool IsActive { get; set; }
    }

    // ===== POST: api/licenses/send  (antigo Enviar) =====
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] PdvActivationDto[]? items, CancellationToken ct)
    {
        if (items is null || items.Length == 0)
            return BadRequest("Payload vazio.");

        try
        {
            Activation? lastActivation = null;

            foreach (var dto in items)
            {
                var activation = await _db.Activations
                    .Include(a => a.PDVs)
                    .FirstOrDefaultAsync(a => a.ActivationKey == dto.ActivationKey, ct);

                if (activation is null)
                    throw new InvalidOperationException("Invalid activation key.");

                activation.LastCheckedAt = DateTime.UtcNow;

                var existing = activation.PDVs.FirstOrDefault(p => p.InstallationPdvId == dto.PdvId);

                var dtoTs = Truncate(dto.LastUpdatedAt, TruncateStep);
                var existingTs = Truncate(existing?.LastUpdatedAt, TruncateStep);

                if (existing is not null)
                {
                    if (dtoTs is not null && existingTs is not null &&
                        dtoTs < existingTs && activation.IsActive)
                    {
                        if (!activation.IsDuplicate)
                        {
                            activation.IsDuplicate = true;

                            var stamp = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));
                            activation.Notes = (activation.Notes ?? string.Empty) + $"\nDuplicidade detectada em {stamp}";

                            // Se precisar notificar por e-mail, adapte aqui:
                            // var unproxied = activation; // EF Core já não cria proxy por padrão com AsNoTracking/POCO
                            // await _email.SendActivationAsync(ETipoEmailAtivacao.AtivacaoOffline, activation, /* user */ null);
                        }
                    }

                    // 3b) Atualiza o LastUpdatedAt para o mais novo
                    if (existing.LastUpdatedAt is not null)
                    {
                        if (dtoTs is not null && existing.LastUpdatedAt < dtoTs)
                            existing.LastUpdatedAt = dtoTs;
                    }
                    else
                    {
                        existing.LastUpdatedAt = dtoTs;
                    }

                    // 3c) Atualiza demais campos
                    existing.HardwareKey = dto.HardwareKey;
                    existing.PdvTypeId = dto.PdvTypeId;
                    existing.Name = dto.Name;
                    existing.Version = dto.Version;
                    existing.IsActive = dto.IsActive;
                }
                else
                {
                    // 3d) Cria novo PDV
                    var newPdv = new Pdv
                    {
                        ActivationId = activation.Id,
                        InstallationPdvId = dto.PdvId,
                        HardwareKey = dto.HardwareKey,
                        Name = dto.Name,
                        LastUpdatedAt = dtoTs,
                        PdvTypeId = dto.PdvTypeId,
                        Version = dto.Version,
                        IsActive = dto.IsActive
                    };

                    _db.PdVs.Add(newPdv);
                }

                lastActivation = activation;
            }

            await _db.SaveChangesAsync(ct);

            if (lastActivation is not null && !lastActivation.IsActive)
            {
                if (lastActivation.ProvisionalValidityUntil < DateTime.Now)
                    throw new InvalidOperationException("Activation key disabled.");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no envio de licenças");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("receive")]
    public async Task<IActionResult> Receive([FromQuery] string id, CancellationToken ct)
    {
        try
        {
            var activation = await _db.Activations
                .AsNoTracking()
                .Include(a => a.Client)
                .Include(a => a.PDVs)
                .FirstOrDefaultAsync(a => a.ActivationKey == id, ct);

            if (activation is null)
                throw new InvalidOperationException("Invalid activation key.");

            if (!activation.IsActive && activation.ProvisionalValidityUntil < DateTime.Now)
                throw new InvalidOperationException("Activation key disabled.");

            if (activation.PDVs is null || activation.PDVs.Count == 0)
                throw new InvalidOperationException("No product licensed for this activation.");

            var dto = new
            {
                Status = (int)HttpStatusCode.OK,
                id = activation.Id,
                client = activation.Client?.Name,
                pdvs = activation.PDVs.Select(p => new
                {
                    id = p.Id,
                    installationPdvId = p.InstallationPdvId,
                    pdvTypeId = p.PdvTypeId,
                    name = p.Name,
                    hardwareKey = p.HardwareKey,
                    lastUpdatedAt = p.LastUpdatedAt,
                    version = p.Version,
                    isActive = p.IsActive
                }).ToList()
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao receber licença {Key}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    // ===== Helper =====
    static DateTime? Truncate(DateTime? dt, TimeSpan step)
    {
        if (dt is null) return null;
        var ticks = (dt.Value.Ticks / step.Ticks) * step.Ticks;
        return new DateTime(ticks, dt.Value.Kind);
    }
}
