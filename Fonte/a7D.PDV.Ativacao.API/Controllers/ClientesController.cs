using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.DTO.ClientDto;
using a7D.PDV.Ativacao.API.Model;

// using a7D.PDV.Ativacao.API.Filters;
// using Microsoft.AspNetCore.Authorization;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/client")]
public class ClientesController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly ILogger<ClientesController> _logger;

    public ClientesController(ApplicationDbContext db, ILogger<ClientesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public sealed class Filter
    {
        public int? Revenda { get; set; }
        public string? Nome { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Documento { get; set; }
        public string? Telefone { get; set; }
    }

    static ClientResponseDto ToResponseDto(Client c) => new()
    {
        Id = c.Id,
        ResellerId = c.ResellerId,
        Reseller = c.Reseller?.Name,
        Name = c.Name,
        CompanyName = c.CompanyName,
        CpfCnpj = c.CpfCnpj,
        Street = c.Street,
        Number = c.Number,
        AdditionalInfo = c.AdditionalInfo,
        City = c.City,
        State = c.State,
        Phone = c.Phone,
        TinyId = c.TinyId,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };

    static void ApplyUpdate(Client entity, ClientUpdateDto dto)
    {
        entity.Name = dto.Name;
        entity.CompanyName = dto.CompanyName;
        entity.CpfCnpj = dto.CpfCnpj;
        entity.Street = dto.Street;
        entity.Number = dto.Number;
        entity.AdditionalInfo = dto.AdditionalInfo;
        entity.City = dto.City;
        entity.State = dto.State;
        entity.Phone = dto.Phone;
        entity.TinyId = dto.TinyId;
    }

    static Client FromCreateDto(ClientCreateDto dto) => new()
    {
        ResellerId = dto.ResellerId,
        Name = dto.Name,
        CompanyName = dto.CompanyName,
        CpfCnpj = dto.CpfCnpj,
        Street = dto.Street,
        Number = dto.Number,
        AdditionalInfo = dto.AdditionalInfo,
        City = dto.City,
        State = dto.State,
        Phone = dto.Phone,
        TinyId = dto.TinyId
    };

    /// <summary>
    /// GET: api/client
    /// Lista paginada de clientes usando DTO de resposta.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetClientes(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] Filter? filter = null,
        CancellationToken ct = default)
    {
        try
        {
            IQueryable<Client> query = _db.Clients
                .AsNoTracking()
                .Include(c => c.Reseller) // para ResellerName
                .OrderBy(c => c.Name);

            if (filter is not null)
            {
                if (filter.Revenda.HasValue)
                    query = query.Where(c => c.ResellerId == filter.Revenda.Value);

                if (!string.IsNullOrWhiteSpace(filter.Nome))
                    query = query.Where(c => c.Name.Contains(filter.Nome));

                if (!string.IsNullOrWhiteSpace(filter.RazaoSocial))
                    query = query.Where(c => c.CompanyName != null && c.CompanyName.Contains(filter.RazaoSocial));

                if (!string.IsNullOrWhiteSpace(filter.Documento))
                    query = query.Where(c => c.CpfCnpj != null && c.CpfCnpj.Contains(filter.Documento));

                if (!string.IsNullOrWhiteSpace(filter.Telefone))
                    query = query.Where(c => c.Phone != null && c.Phone.Contains(filter.Telefone));
            }

            var total = await query.CountAsync(ct);

            if (page > 0 && count > 0)
                query = query.Skip((page - 1) * count).Take(count);
            else if (count > 0)
                query = query.Take(count);

            var result = await query
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    ResellerId = c.ResellerId,
                    Reseller = c.Reseller != null ? c.Reseller.Name : null,
                    Name = c.Name,
                    CompanyName = c.CompanyName,
                    CpfCnpj = c.CpfCnpj,
                    Street = c.Street,
                    Number = c.Number,
                    AdditionalInfo = c.AdditionalInfo,
                    City = c.City,
                    State = c.State,
                    Phone = c.Phone,
                    TinyId = c.TinyId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync(ct);

            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar clientes");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// GET: api/client/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientResponseDto>> GetCliente([FromRoute] int id, CancellationToken ct)
    {
        var cliente = await _db.Clients
            .AsNoTracking()
            .Include(c => c.Reseller)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (cliente is null)
            return NotFound();

        return Ok(ToResponseDto(cliente));
    }

    /// <summary>
    /// PUT: api/client/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCliente([FromRoute] int id, [FromBody] ClientUpdateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var entity = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return NotFound();

        ApplyUpdate(entity, dto);

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await ClienteExistsAsync(id, ct))
                return NotFound();

            _logger.LogError(ex, "Concorrência ao atualizar cliente {Id}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        // opcional: retornar o DTO atualizado
        var updated = await _db.Clients.AsNoTracking().Include(c => c.Reseller).FirstAsync(c => c.Id == id, ct);
        return Ok(ToResponseDto(updated));
    }

    /// <summary>
    /// POST: api/client
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClientResponseDto>> PostCliente([FromBody] ClientCreateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        // valida revenda
        var exists = await _db.Resellers.AnyAsync(r => r.Id == dto.ResellerId, ct);
        if (!exists)
            return BadRequest("Revenda informada não existe.");

        var entity = FromCreateDto(dto);
        _db.Clients.Add(entity);
        await _db.SaveChangesAsync(ct);

        // carrega com reseller para popular ResellerName
        var created = await _db.Clients
            .AsNoTracking()
            .Include(c => c.Reseller)
            .FirstAsync(c => c.Id == entity.Id, ct);

        var response = ToResponseDto(created);
        return CreatedAtAction(nameof(GetCliente), new { id = response.Id }, response);
    }

    /// <summary>
    /// DELETE: api/client/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCliente([FromRoute] int id, CancellationToken ct)
    {
        var cliente = await _db.Clients.FindAsync(new object[] { id }, ct);
        if (cliente is null)
            return NotFound();

        _db.Clients.Remove(cliente);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }

    private Task<bool> ClienteExistsAsync(int id, CancellationToken ct)
        => _db.Clients.AnyAsync(c => c.Id == id, ct);
}
