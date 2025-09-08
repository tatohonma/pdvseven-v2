using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace a7D.PDV.Ativacao.API.Services;

public class TokenService:ITokenService
{
   public JwtSecurityToken GenerateToken(IEnumerable<Claim> claims, IConfiguration configuration)
    {
        var key = configuration["JWT:Key"] ?? throw new InvalidOperationException("Invalid secret key");

        var privateKey = Encoding.UTF8.GetBytes(key);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),

            Audience = configuration.GetSection("JWT").GetValue<string>("Audience"),
            Issuer = configuration.GetSection("JWT").GetValue<string>("Issuer"),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }
    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randonNumberGenerator = RandomNumberGenerator.Create();

        randonNumberGenerator.GetBytes(secureRandomBytes);
        return Convert.ToBase64String(secureRandomBytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
    {
        var key = configuration["JWT:Key"] ?? throw new InvalidOperationException("Invalid secret key");


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
