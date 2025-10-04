using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/pdv-type")]
// [ApiAuth] // habilite se já tiver portado seu atributo para ASP.NET Core
public sealed class PdvTypesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PdvTypesController> _logger;

    public PdvTypesController(ApplicationDbContext db, ILogger<PdvTypesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET: api/pdvtypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PdvType>>> GetPdvTypes(CancellationToken ct)
    {
        var list = await _db.PdvTypes.AsNoTracking().ToListAsync(ct);
        return Ok(list);
    }

    // GET: api/pdvtypes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PdvType>> GetPdvType([FromRoute] int id, CancellationToken ct)
    {
        var item = await _db.PdvTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    // PUT: api/pdvtypes/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPdvType([FromRoute] int id, [FromBody] PdvType pdvType, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (id != pdvType.Id)
            return BadRequest("Route id doesn't match body id.");

        _db.Entry(pdvType).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var exists = await _db.PdvTypes.AnyAsync(t => t.Id == id, ct);
            if (!exists)
                return NotFound();

            _logger.LogError(ex, "Concurrency error updating PdvType {Id}", id);
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    // POST: api/pdvtypes
    [HttpPost]
    public async Task<ActionResult<PdvType>> PostPdvType([FromBody] PdvType pdvType, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        _db.PdvTypes.Add(pdvType);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetPdvType), new { id = pdvType.Id }, pdvType);
    }

    // DELETE: api/pdvtypes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePdvType([FromRoute] int id, CancellationToken ct)
    {
        var item = await _db.PdvTypes.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (item is null)
            return NotFound();

        _db.PdvTypes.Remove(item);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
