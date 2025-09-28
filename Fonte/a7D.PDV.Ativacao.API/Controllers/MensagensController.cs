using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Services;
using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Services;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/mensagens")]
// [ApiAuth(requerAdm: false)] // habilite sua versão Core se desejar
public sealed class MensagensController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly ILogger<MensagensController> _logger;

    public MensagensController(ApplicationDbContext db, ILogger<MensagensController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // ===== DTOs de resposta (projeções seguras) =====
    public sealed class MensagemListItemDto
    {
        public int Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Tipo { get; init; } = null!;
        public DateTime? ReceivedAt { get; init; }
        public string Cliente { get; init; } = "???";
        public string? Texto { get; init; }
    }

    public sealed class Filtro
    {
        public string? Cliente { get; set; }
        public string? Tipo { get; set; }
    }

    /// <summary>
    /// POST: api/mensagens
    /// Corpo: MensagemNova
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Enviar([FromBody] MensagemNova mensagem, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var ativacao = await _db.Activations
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ActivationKey == mensagem.Chave, ct);

            if (ativacao is null)
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(mensagem.Chave)] = new[] { "Chave de ativação não existe" }
                }));

            var msg = new Mensagem(mensagem)
            {
                IDAtivacao = ativacao.Id
            };

            _db.Mensagems.Add(msg);
            await _db.SaveChangesAsync(ct);

            return Ok(msg.IDMensagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar mensagem");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// GET: api/mensagens
    /// Lista paginada com filtros.
    /// </summary>
    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetMensagens(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] Filtro? filter = null,
        CancellationToken ct = default)
    {
        if (Request.Query.ContainsKey("chave"))
            return NotFound();

        filter ??= new Filtro();

        // 1) Parse do filtro de tipo para evitar ToString() no predicado
        int? tipoId = null;
        if (!string.IsNullOrWhiteSpace(filter.Tipo) && int.TryParse(filter.Tipo, out var parsed))
            tipoId = parsed;

        // 2) Base query ainda como IQueryable<Mensagem>
        IQueryable<Mensagem> baseQuery = _db.Mensagems
            .AsNoTracking()
            .Include(m => m.Ativacao).ThenInclude(a => a.Client)
            .Where(m =>
                (filter.Cliente == null || (m.Ativacao.Client.Name != null && m.Ativacao.Client.Name.Contains(filter.Cliente))) &&
                (tipoId == null || m.IDTipo == tipoId));

        var total = await baseQuery.CountAsync(ct);
        IQueryable<Mensagem> pagedQuery = baseQuery.OrderByDescending(m => m.DataCriada);

        if (page > 0 && count > 0)
            pagedQuery = pagedQuery.Skip((page - 1) * count).Take(count);
        else if (count > 0)
            pagedQuery = pagedQuery.Take(count);

        var list = await pagedQuery
            .Select(m => new MensagemListItemDto
            {
                Id = m.IDMensagem,
                CreatedAt = m.DataCriada,
                Tipo = m.Tipo.ToString(),
                ReceivedAt = m.DataRecebida,
                Cliente = m.Ativacao.Client.Name ?? "???",
                Texto = m.Texto
            })
            .ToListAsync(ct);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(list);
    }

    /// <summary>
    /// GET: api/mensagens/receber?chave=...
    /// (atalho para integrador)
    /// </summary>
    [HttpGet("receber")]
    public Task<IActionResult> Receber([FromQuery] string chave, CancellationToken ct)
        => SyncMessage(chave, EOrigemDestinoMensagem.Integrador, versao: "", status: "", ct);

    /// <summary>
    /// GET: api/mensagens/syncmsg?chave=...&to=Integrador&versao=...&status=...
    /// Devolve até 3 mensagens pendentes para o destino informado e marca como lidas.
    /// </summary>
    [HttpGet("syncmsg")]
    public async Task<IActionResult> SyncMessage(
        [FromQuery] string chave,
        [FromQuery, TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))] EOrigemDestinoMensagem to,
        [FromQuery] string versao,
        [FromQuery] string status,
        CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(chave))
                return NotFound();

            ClientesService.Registra(chave, versao, status, erro: false);

            var destino = (int)to;

            var pendingQuery = _db.Mensagems
                .Include(m => m.Ativacao)
                .Where(m =>
                    !m.DataRecebida.HasValue &&
                    m.Ativacao.ActivationKey == chave &&
                    m.IDDestino == destino)
                .OrderBy(m => m.DataCriada);

            var all = await pendingQuery.ToListAsync(ct);

            if (all.Count > 10)
                return Problem(detail: $"Mais de {all.Count} mensagens pendentes! O limite de 10 mensagens", statusCode: StatusCodes.Status500InternalServerError);

            var now = DateTime.Now;
            var toSend = all.Take(3).ToList();

            foreach (var m in toSend)
                m.DataRecebida = now;

            await _db.SaveChangesAsync(ct);

            var payload = toSend.Select(m => new
            {
                m.IDMensagem,
                m.DataCriada,
                m.Tipo,
                m.DataRecebida,
                m.Origem,
                m.Destino,
                m.Texto,
                m.Parametros,
                m.IDMensagemOrigem
            });

            return Ok(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no syncmsg (chave={Chave})", chave);
            try { ClientesService.Registra(chave, versao, status, erro: true); } catch { /* best-effort */ }
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
