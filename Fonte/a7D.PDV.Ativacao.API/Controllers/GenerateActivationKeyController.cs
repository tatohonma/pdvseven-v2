using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a7D.PDV.Ativacao.API.Data;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/generate-activation-key")]
// [ApiAuth] // habilite se já tiver portado para ASP.NET Core
public sealed class GenerateActivationKeyController : ControllerBase
{
    readonly ApplicationDbContext _db;
    readonly ILogger<GenerateActivationKeyController> _logger;

    public GenerateActivationKeyController(ApplicationDbContext db, ILogger<GenerateActivationKeyController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public sealed class ActivationKeyDto
    {
        public string ActivationKey { get; init; } = null!;
    }

    [HttpGet]
    public async Task<ActionResult<ActivationKeyDto>> Generate([FromQuery] int resellerId, CancellationToken ct)
    {
        var reseller = await _db.Resellers.AsNoTracking().FirstOrDefaultAsync(r => r.Id == resellerId, ct);
        if (reseller is null)
            return NotFound();

        var rnd = new Random();

        var resellerCode = reseller.Code.ToString("000", CultureInfo.InvariantCulture);

        var activationCode = rnd.Next(0, 100_000).ToString("00000", CultureInfo.InvariantCulture);

        var baseCode = resellerCode + activationCode;

        decimal checkDigitSum = 0m;
        for (int i = 0; i < baseCode.Length - 1; i++)
        {
            checkDigitSum += Convert.ToDecimal(baseCode[i].ToString(), CultureInfo.InvariantCulture) + 0.5m;
        }

        var checkDigitInt = (int)checkDigitSum;
        string checkDigit = (checkDigitInt <= 99)
            ? checkDigitInt.ToString("00", CultureInfo.InvariantCulture)
            : checkDigitInt.ToString(CultureInfo.InvariantCulture).Substring(0, 2);

        var activationKey = $"{resellerCode}-{activationCode}-{checkDigit}";

        return Ok(new ActivationKeyDto { ActivationKey = activationKey });
    }
}
