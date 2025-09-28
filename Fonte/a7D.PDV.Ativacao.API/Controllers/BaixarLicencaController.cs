using System.Text;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// CryptMD5
// AtivacaoContext (ajuste o namespace real)
// ApiAuth (se existir versão ASP.NET Core)
// Suas entidades

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaixarLicencaController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly ILogger<BaixarLicencaController> _logger;

    public BaixarLicencaController(ApplicationDbContext db, ILogger<BaixarLicencaController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Index([FromRoute] int id, CancellationToken ct)
    {
        // Carrega ativação e PDVs (e Cliente para nome do arquivo)
        var ativacao = await _db.Activations
            .Include(a => a.PDVs)
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (ativacao is null)
            return NotFound();

        if (ativacao.PDVs.Count < 1)
            return NotFound();

        var sb = new StringBuilder();

        sb.AppendLine(string.Format(
            @"DECLARE @chaveAtivacao AS VARCHAR(50)
                SET @chaveAtivacao = '{0}'

                DELETE FROM tbConfiguracao WHERE chave='chaveAtivacao'
                INSERT [dbo].[tbConfiguracao] ([Chave], [Valor]) VALUES (N'chaveAtivacao', @chaveAtivacao)

                DELETE FROM [tbPDV]",
                            ativacao.ActivationKey));

        var proximoId = 0;
        foreach (var pdv in ativacao.PDVs)
        {
            // Pressupondo que PDV possui método Insert(int idSequencial) que retorna o SQL
            sb.AppendLine(pdv.ToInsertScript(++proximoId));
        }

        // Criptografa e converte para Base64 (mantendo a lógica original)
        var licCripto = CryptMD5.Criptografar(sb.ToString());
        var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(licCripto));

        var nomeCliente = ativacao.Client?.Name ?? "cliente";
        var safeName = string.Concat(nomeCliente.Where(c => !Path.GetInvalidFileNameChars().Contains(c)))
            .Replace(" ", string.Empty);
        if (string.IsNullOrWhiteSpace(safeName))
            safeName = "licenca";

        var fileName = $"{safeName}.lic";

        // Retorna como arquivo (mesmo conteúdo que você retornava: string Base64 como bytes)
        var bytes = Encoding.UTF8.GetBytes(base64String);
        const string contentType = "application/octet-stream";

        return File(bytes, contentType, fileName);
    }
}
