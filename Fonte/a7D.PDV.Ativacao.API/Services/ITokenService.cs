using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace a7D.PDV.Ativacao.API.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateToken(IEnumerable<Claim> claims, IConfiguration configuration);
    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
}
