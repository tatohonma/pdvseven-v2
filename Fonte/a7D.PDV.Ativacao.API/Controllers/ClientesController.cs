using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
// using a7D.PDV.Ativacao.API.Filters; // Se você portou seu ApiAuth para Core
// using Microsoft.AspNetCore.Authorization;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [ApiAuth] // habilite se sua versão para ASP.NET Core existir
public class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ClientesController> _logger;

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

    /// <summary>
    /// GET: api/Clientes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetClientes(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] Filter? filter = null,
        CancellationToken ct = default)
    {
        try
        {
            IQueryable<Client> query = _db.Clients
                .AsNoTracking()
                .OrderBy(c => c.Name);

            if (filter is not null)
            {
                if (filter.Revenda.HasValue)
                    query = query.Where(c => c.ResellerId == filter.Revenda);

                if (!string.IsNullOrWhiteSpace(filter.Nome))
                    query = query.Where(c => c.Name.Contains(filter.Nome));

                if (!string.IsNullOrWhiteSpace(filter.RazaoSocial))
                    query = query.Where(c => c.CompanyName!.Contains(filter.RazaoSocial)); 

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

            var result = await query.ToListAsync(ct);

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
    /// GET: api/Clientes/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetCliente([FromRoute] int id, CancellationToken ct)
    {
        // Ajuste a chave primária conforme seu modelo (Id ou IDCliente)
        var cliente = await _db.Clients.FindAsync(new object[] { id }, ct);
        if (cliente is null)
            return NotFound();

        return Ok(cliente);
    }

    /// <summary>
    /// PUT: api/Clientes/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCliente([FromRoute] int id, [FromBody] Client cliente, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (id != cliente.Id)
            return BadRequest("ID do caminho não confere com o corpo da requisição.");

        _db.Entry(cliente).State = EntityState.Modified;

        if (cliente.Reseller is not null && cliente.ResellerId > 0)
        {
            var exists = await _db.Resellers.AnyAsync(r => r.Id == cliente.ResellerId, ct);
            if (!exists)
                return BadRequest("Revenda informada não existe.");
        }

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

        return NoContent();
    }

    /// <summary>
    /// POST: api/Clientes
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Client>> PostCliente([FromBody] Client cliente, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        _db.Clients.Add(cliente);
        await _db.SaveChangesAsync(ct);

        // Ajuste a rota/nome do método conforme seu GetCliente
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    /// <summary>
    /// DELETE: api/Clientes/{id}
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
