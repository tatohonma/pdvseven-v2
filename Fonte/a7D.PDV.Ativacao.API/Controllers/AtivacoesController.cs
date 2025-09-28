using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Repository;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Repository.User;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/activation")]
[Authorize]
public class ActivationsController : BaseSecureApiController
{
    readonly static CultureInfo CulturePtBr = new("pt-BR");
    readonly static TimeZoneInfo BrazilTz = GetBrazilTz();

    public ActivationsController(ApplicationDbContext db, IUserRepository usuarios)
        : base(db, usuarios) { }

    public class Filter
    {
        public string? Client { get; set; }
        public string? ActivationKey { get; set; }
        public string? IsActive { get; set; }
        public string? ReactivatedBySupport { get; set; }
        public string? IsDuplicate { get; set; }
        public string? Notes { get; set; }
    }

    // GET: api/activations
    [HttpGet]
    public async Task<IActionResult> GetActivations(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] Filter? filter = null,
        CancellationToken ct = default)
    {
        IQueryable<Activation> query = Db.Activations
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.PDVs)
            .OrderBy(a => a.Client.Name);

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Client))
                query = query.Where(a => a.Client.Name.Contains(filter.Client));

            if (!string.IsNullOrWhiteSpace(filter.ActivationKey))
                query = query.Where(a => a.ActivationKey.Contains(filter.ActivationKey));

            if (!string.IsNullOrWhiteSpace(filter.IsActive))
                query = query.Where(a => a.IsActive == (filter.IsActive == "1"));

            if (!string.IsNullOrWhiteSpace(filter.IsDuplicate))
                query = query.Where(a => a.IsDuplicate == (filter.IsDuplicate == "1"));

            if (!string.IsNullOrWhiteSpace(filter.Notes))
                query = query.Where(a => a.Notes!.Contains(filter.Notes));

            if (!string.IsNullOrWhiteSpace(filter.ReactivatedBySupport))
                query = query.Where(a => a.ReactivatedBySupport == (filter.ReactivatedBySupport == "1"));
        }

        var total = await query.CountAsync(ct);

        if (page > 0 && count > 0)
            query = query.Skip((page - 1) * count);
        if (count > 0)
            query = query.Take(count);

        var result = await query.ToListAsync(ct);

        // Ajuste de fuso horário
        foreach (var a in result)
        {
            a.LastCheckedAt = ConvertUtcToTz(a.LastCheckedAt, BrazilTz);

            if (a.PDVs is not null)
            {
                foreach (var pdv in a.PDVs)
                    pdv.UpdatedAt = ConvertUtcToTz(pdv.UpdatedAt, BrazilTz);
            }
        }

        if (!IsAdminRequest())
        {
            foreach (var a in result)
            {
                a.ActivatedAt = null;
                a.PDVs = null;
                a.Notes = null;
                a.LicensesHtml = null!;
                a.ValidityDays = -1;
            }
        }

        Response.Headers["X-Total-Count"] = total.ToString(CulturePtBr);
        return Ok(result);
    }

    // GET: api/activations/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetActivation([FromRoute] int id, CancellationToken ct = default)
    {
        var activation = await Db.Activations
            .Include(a => a.PDVs)
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (activation is null)
            return NotFound();

        activation.LastCheckedAt = ConvertUtcToTz(activation.LastCheckedAt, BrazilTz);

        if (activation.PDVs is not null)
        {
            activation.PDVs = activation.PDVs
                .OrderBy(p => p.PdvTypeId)
                .ToList();

            foreach (var pdv in activation.PDVs)
                pdv.UpdatedAt = ConvertUtcToTz(pdv.UpdatedAt, BrazilTz);
        }

        return Ok(activation);
    }

    // GET: api/activations/client/{key}
    [HttpGet("client/{key}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivationByKey([FromRoute] string key, CancellationToken ct = default)
    {
        var activation = await Db.Activations
            .AsNoTracking()
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.ActivationKey == key, ct);

        if (activation is null)
            return NotFound();

        var client = new
        {
            Establishment = activation.Client.Name,
            activation.Client.CompanyName,
            TaxId = (activation.Client.CpfCnpj ?? string.Empty)
                .Replace(".", string.Empty)
                .Replace(",", string.Empty),
        };

        return Ok(client);
    }

    // PUT: api/activations/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutActivation([FromRoute] int id, [FromBody] Activation payload, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || payload is null)
            return ValidationProblem(ModelState);

        if (id != payload.Id)
            return BadRequest("Route id differs from body id.");

        Db.Attach(payload);

        if (!payload.ReactivatedBySupport)
        {
            payload.SupportReactivatedAt = null;
            payload.ProvisionalValidityUntil = null;
        }
        else
        {
            var now = DateTime.UtcNow;
            payload.IsActive = false;
            payload.SupportReactivatedAt = now;
            payload.ProvisionalValidityUntil = now.AddDays(3);
        }

        if (payload.PDVs is not null)
        {
            foreach (var pdv in payload.PDVs.Where(p => p.Id == 0))
                Db.Entry(pdv).State = EntityState.Added;

            foreach (var pdv in payload.PDVs.Where(p => p.Id != 0))
            {
                if (payload.SiteAdmin == null)
                    pdv.UpdatedAt = DateTime.UtcNow;

                Db.Entry(pdv).State = EntityState.Modified;
            }
        }

        Db.Entry(payload).State = EntityState.Modified;

        UpdateInstallationIds(payload);

        try
        {
            await Db.SaveChangesAsync(ct);
            return Ok(payload);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var exists = await Db.Activations.AnyAsync(a => a.Id == id, ct);
            if (!exists)
                return NotFound();

            return Problem(
                title: "Concurrency conflict while saving activation",
                detail: ex.ToString(),
                statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error while updating activation",
                detail: ex.ToString(),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // POST: api/activations
    [HttpPost]
    public async Task<IActionResult> PostActivation([FromBody] Activation activation, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || activation is null)
            return ValidationProblem(ModelState);

        activation.Client = await Db.Clients.FindAsync(new object?[] { activation.ClientId }, ct);

        await Db.Activations.AddAsync(activation, ct);
        UpdateInstallationIds(activation);

        await Db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetActivation), new { id = activation.Id }, activation);
    }

    // DELETE: api/activations/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteActivation([FromRoute] int id, CancellationToken ct = default)
    {
        var activation = await Db.Activations.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (activation is null)
            return NotFound();

        Db.Activations.Remove(activation);
        await Db.SaveChangesAsync(ct);

        return Ok(activation);
    }

    // ===== Helpers =====

    void UpdateInstallationIds(Activation activation)
    {
        var maxDb = Db.PdVs
            .Where(p => p.ActivationId == activation.Id && p.InstallationPdvId.HasValue)
            .Select(p => p.InstallationPdvId!.Value)
            .DefaultIfEmpty(0)
            .Max();

        var received = activation.PDVs?
            .Where(p => Db.Entry(p).State != EntityState.Deleted && p.InstallationPdvId.HasValue)
            .Select(p => p.InstallationPdvId!.Value)
            .DefaultIfEmpty(0)
            .ToList() ?? new List<int>();

        var maxReceived = received.Count > 0 ? received.Max() : 0;
        var nextId = Math.Max(maxDb, maxReceived) + 1;

        if (activation.PDVs is null) return;

        foreach (var pdv in activation.PDVs.Where(p => !p.InstallationPdvId.HasValue))
            pdv.InstallationPdvId = nextId++;
    }

    private static TimeZoneInfo GetBrazilTz()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); }
        catch { return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"); }
    }

    private static DateTime? ConvertUtcToTz(DateTime? dtUtc, TimeZoneInfo tz)
        => dtUtc.HasValue
            ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dtUtc.Value, DateTimeKind.Utc), tz)
            : null;
}
