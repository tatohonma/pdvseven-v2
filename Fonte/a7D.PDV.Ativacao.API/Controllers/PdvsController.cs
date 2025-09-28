using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ApiAuth] // habilite se você já portou esse atributo para ASP.NET Core
public sealed class PdvsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PdvsController> _logger;

    public PdvsController(ApplicationDbContext db, ILogger<PdvsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET: api/pdvs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pdv>>> GetPdvs(CancellationToken ct)
    {
        var list = await _db.PdVs.AsNoTracking().ToListAsync(ct);
        return Ok(list);
    }

    // GET: api/pdvs/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Pdv>> GetPdv([FromRoute] int id, CancellationToken ct)
    {
        var pdv = await _db.PdVs.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pdv is null)
            return NotFound();

        return Ok(pdv);
    }

    // PUT: api/pdvs/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPdv([FromRoute] int id, [FromBody] Pdv pdv, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (id != pdv.Id)
            return BadRequest("Route id doesn't match body id.");

        // Anexa e marca como modificado (para entidade desconectada)
        _db.Entry(pdv).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _db.PdVs.AnyAsync(e => e.Id == id, ct))
                return NotFound();

            _logger.LogError(ex, "Concurrency error updating Pdv {Id}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    // POST: api/pdvs
    [HttpPost]
    public async Task<ActionResult<Pdv>> PostPdv([FromBody] Pdv pdv, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        _db.PdVs.Add(pdv);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetPdv), new { id = pdv.Id }, pdv);
    }

    // DELETE: api/pdvs/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePdv([FromRoute] int id, CancellationToken ct)
    {
        var pdv = await _db.PdVs.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pdv is null)
            return NotFound();

        _db.PdVs.Remove(pdv);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
