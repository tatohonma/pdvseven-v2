using System.Globalization;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Repository.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/activation")]
[Authorize]
public class ActivationsController : BaseSecureApiController
{
    readonly static CultureInfo CulturePtBr = new CultureInfo("pt-BR");
    readonly static TimeZoneInfo BrazilTz = GetBrazilTz();

    public ActivationsController(ApplicationDbContext db, IUserRepository usuarios)
        : base(db, usuarios)
    {
    }

    public class Filter
    {
        public string? Client { get; set; }
        public string? ActivationKey { get; set; }
        public string? IsActive { get; set; }
        public string? ReactivatedBySupport { get; set; }
        public string? IsDuplicate { get; set; }
        public string? Notes { get; set; }
    }

    [HttpGet]
    public async Task<IActionResult> GetActivations(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] Filter? filter = null,
        CancellationToken ct = default)
    {
        // Base query
        var query = Db.Activations
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.PDVs).ThenInclude(p => p.PdvType)
            .OrderBy(a => a.Client.Name)
            .AsQueryable();

        // Filtros (com null-safe em Notes)
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
                query = query.Where(a => a.Notes != null && a.Notes.Contains(filter.Notes));

            if (!string.IsNullOrWhiteSpace(filter.ReactivatedBySupport))
                query = query.Where(a => a.ReactivatedBySupport == (filter.ReactivatedBySupport == "1"));
        }

        var total = await query.CountAsync(ct);

        if (page > 0 && count > 0)
            query = query.Skip((page - 1) * count);
        if (count > 0)
            query = query.Take(count);

        // Projeção direta para DTO -> elimina ciclos
        var list = await query.Select(a => new ActivationResponseDto
        {
            Id = a.Id,
            ClientId = a.ClientId,
            Client = new ClientResponseDto
            {
                Id = a.Client.Id,
                ResellerId = a.Client.ResellerId,
                Reseller = null,
                Name = a.Client.Name,
                CompanyName = a.Client.CompanyName,
                CpfCnpj = a.Client.CpfCnpj,
                Street = a.Client.Street,
                Number = a.Client.Number,
                AdditionalInfo = a.Client.AdditionalInfo,
                City = a.Client.City,
                State = a.Client.State,
                Phone = a.Client.Phone,
                TinyId = a.Client.TinyId,
                CreatedAt = a.Client.CreatedAt,
                UpdatedAt = a.Client.UpdatedAt
            },
            ActivationKey = a.ActivationKey,
            LastCheckedAt = a.LastCheckedAt,
            ActivatedAt = a.ActivatedAt,
            ValidityDays = a.ValidityDays,
            IsActive = a.IsActive,
            Notes = a.Notes,
            ReactivatedBySupport = a.ReactivatedBySupport,
            SupportReactivatedAt = a.SupportReactivatedAt,
            ProvisionalValidityUntil = a.ProvisionalValidityUntil,
            IsDuplicate = a.IsDuplicate,
            SiteAdmin = a.SiteAdmin,
            PdVs = a.PDVs.Select(p => new PdvResponseDto
            {
                Id = p.Id,
                ActivationId = p.ActivationId,
                InstallationPdvId = p.InstallationPdvId,
                PdvTypeId = p.PdvTypeId,
                Name = p.Name,
                HardwareKey = p.HardwareKey,
                Version = p.Version,
                IsActive = p.IsActive,
                UpdatedAt = p.UpdatedAt
            }).ToList(),
            LicensesHtml = string.Join("",
                a.PDVs
                    .GroupBy(p => new { p.PdvTypeId, p.PdvType!.Name })
                    .OrderBy(g => g.Key.Name)
                    .Select(g => $"{g.Count()} {(string.IsNullOrWhiteSpace(g.Key.Name) ? "(type not loaded)" : g.Key.Name)}<br />")),
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        }).ToListAsync(ct);

        foreach (var a in list)
        {
            a.LastCheckedAt = ConvertUtcToTz(a.LastCheckedAt, BrazilTz);
            if (a.PdVs is not null)
                foreach (var p in a.PdVs)
                    p.UpdatedAt = ConvertUtcToTz(p.UpdatedAt, BrazilTz);
        }

        if (!IsAdminRequest())
        {
            foreach (var a in list)
            {
                a.ActivatedAt = null;
                a.ValidityDays = -1;
                a.Notes = null;
                a.PdVs = null;
                a.LicensesHtml = ""; // string vazia
            }
        }

        Response.Headers["X-Total-Count"] = total.ToString(CulturePtBr);
        return Ok(list);
    }


    // GET: api/activations/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetActivation([FromRoute] int id, CancellationToken ct = default)
    {
        var a = await Db.Activations
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.PDVs).ThenInclude(p => p.PdvType)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (a is null) return NotFound();

        var dto = ActivationEditMapping.ToEditDto(a);

        dto.LastCheckedAt = ConvertUtcToTz(dto.LastCheckedAt, BrazilTz);
        foreach (var p in dto.PdVs) p.UpdatedAt = ConvertUtcToTz(p.UpdatedAt, BrazilTz);

        if (!IsAdminRequest())
        {
            dto.ActivatedAt = null;
            dto.ValidityDays = -1;
            dto.Notes = null;
            dto.PdVs = new List<PdvEditDto>();
            dto.LicensesHtml = string.Empty;
        }

        return Ok(dto);
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
                .Replace(",", string.Empty)
        };

        return Ok(client);
    }

    // PUT: api/activations/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutActivation([FromRoute] int id, [FromBody] ActivationEditDto payload, CancellationToken ct = default)
    {
        if (payload is null || id != payload.Id)
            return BadRequest("Route id differs from body id.");

        
        var current = await Db.Activations
            .Include(a => a.Client)
            .Include(a => a.PDVs)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (current is null)
            return NotFound();

        
        var incoming = ActivationEditMapping.ToEntity(payload);

        
        if (!incoming.ReactivatedBySupport)
        {
            incoming.SupportReactivatedAt = null;
            incoming.ProvisionalValidityUntil = null;
        }
        else
        {
            var now = DateTime.UtcNow;
            incoming.IsActive = false;
            incoming.SupportReactivatedAt = now;
            incoming.ProvisionalValidityUntil = now.AddDays(3);
        }

        
        
        Db.Entry(current).CurrentValues.SetValues(incoming);
        Db.Entry(current).Property(x => x.CreatedAt).IsModified = false;

        
        var incomingPdvs = incoming.PDVs;
        var incomingById = incomingPdvs.Where(p => p.Id != 0).ToDictionary(p => p.Id);

        
        foreach (var pdvDb in current.PDVs.ToList())
        {
            if (incomingById.TryGetValue(pdvDb.Id, out var pdvIn))
            {
                Db.Entry(pdvDb).CurrentValues.SetValues(pdvIn);
                Db.Entry(pdvDb).Property(x => x.CreatedAt).IsModified = false;

                if (incoming.SiteAdmin == null)
                    pdvDb.LastUpdatedAt = DateTime.UtcNow;
            }
            
            // current.PDVs.Remove(pdvDb);
        }

        foreach (var pdvIn in incomingPdvs.Where(p => p.Id == 0))
        {
            pdvIn.Activation = null;
            pdvIn.PdvType = null;
            current.PDVs.Add(pdvIn);
        }

        await UpdateInstallationIdsAsync(current, ct);

        await Db.SaveChangesAsync(ct);

        var updated = await Db.Activations
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.PDVs).ThenInclude(p => p.PdvType)
            .FirstAsync(a => a.Id == id, ct);

        return Ok(ActivationEditMapping.ToEditDto(updated));
    }


    // POST: api/activations
    [HttpPost]
    public async Task<IActionResult> PostActivation([FromBody] ActivationEditDto payload, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || payload is null)
            return ValidationProblem(ModelState);

        // Força criação
        payload.Id = 0;

        // Se vier o objeto Client preenchido, use-o para definir o ClientId
        if (payload.ClientId == 0 && payload.Client?.Id > 0)
            payload.ClientId = payload.Client.Id;

        var activation = ActivationEditMapping.ToEntity(payload);

        // Carrega client (garante FK)
        activation.Client = await Db.Clients.FindAsync(new object?[] { activation.ClientId }, ct);
        if (activation.Client is null)
            return BadRequest("Cliente inválido.");

        // Regra de reativação pelo suporte (mesma do PUT)
        if (!activation.ReactivatedBySupport)
        {
            activation.SupportReactivatedAt = null;
            activation.ProvisionalValidityUntil = null;
        }
        else
        {
            var now = DateTime.UtcNow;
            activation.IsActive = false;
            activation.SupportReactivatedAt = now;
            activation.ProvisionalValidityUntil = now.AddDays(3);
        }

        // Garante consistência de PDVs novos (ActivationId = 0; entidades desconectadas)
        foreach (var p in activation.PDVs)
        {
            p.Id = 0;
            p.ActivationId = 0;
            p.PdvType = null;
            p.LastUpdatedAt ??= DateTime.UtcNow;
        }

        await Db.Activations.AddAsync(activation, ct);

        // Atribui InstallationPdvId
        await UpdateInstallationIdsAsync(activation, ct);

        await Db.SaveChangesAsync(ct);

        // Retorna já no formato de edição (o mesmo usado no GET/PUT)
        var created = await Db.Activations
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.PDVs).ThenInclude(p => p.PdvType)
            .FirstAsync(a => a.Id == activation.Id, ct);

        return CreatedAtAction(nameof(GetActivation), new { id = created.Id }, ActivationEditMapping.ToEditDto(created));
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


    async Task UpdateInstallationIdsAsync(Activation activation, CancellationToken ct)
    {
        var maxDb = await Db.PdVs
            .AsNoTracking()
            .Where(p => p.ActivationId == activation.Id)
            .MaxAsync(p => p.InstallationPdvId, ct) ?? 0;

        var received = activation.PDVs?
            .Where(p => p.InstallationPdvId.HasValue)
            .Select(p => p.InstallationPdvId!.Value)
            .ToList() ?? new List<int>();

        var maxReceived = received.Count > 0 ? received.Max() : 0;
        var nextId = Math.Max(maxDb, maxReceived) + 1;

        if (activation.PDVs is null) return;

        foreach (var pdv in activation.PDVs.Where(p => !p.InstallationPdvId.HasValue))
            pdv.InstallationPdvId = nextId++;
    }


    static TimeZoneInfo GetBrazilTz()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
    }

    static DateTime? ConvertUtcToTz(DateTime? dtUtc, TimeZoneInfo tz)
    {
        return dtUtc.HasValue
            ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dtUtc.Value, DateTimeKind.Utc), tz)
            : null;
    }
}
