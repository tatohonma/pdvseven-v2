using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ApiAuth] // habilite se você já portou o atributo para ASP.NET Core
public sealed class ResellersController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ResellersController> _logger;

    public ResellersController(ApplicationDbContext db, ILogger<ResellersController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET: api/resellers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reseller>>> GetResellers(CancellationToken ct)
    {
        var list = await _db.Resellers.AsNoTracking().ToListAsync(ct);
        return Ok(list);
    }

    // GET: api/resellers/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reseller>> GetReseller([FromRoute] int id, CancellationToken ct)
    {
        var reseller = await _db.Resellers.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        if (reseller is null)
            return NotFound();

        return Ok(reseller);
    }

    // PUT: api/resellers/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutReseller([FromRoute] int id, [FromBody] Reseller reseller, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (id != reseller.Id)
            return BadRequest("Route id doesn't match body id.");

        _db.Entry(reseller).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var exists = await _db.Resellers.AnyAsync(r => r.Id == id, ct);
            if (!exists) return NotFound();

            _logger.LogError(ex, "Concurrency error updating Reseller {Id}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    // POST: api/resellers
    [HttpPost]
    public async Task<ActionResult<Reseller>> PostReseller([FromBody] Reseller reseller, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        _db.Resellers.Add(reseller);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetReseller), new { id = reseller.Id }, reseller);
    }

    // DELETE: api/resellers/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReseller([FromRoute] int id, CancellationToken ct)
    {
        var reseller = await _db.Resellers.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (reseller is null)
            return NotFound();

        _db.Resellers.Remove(reseller);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
