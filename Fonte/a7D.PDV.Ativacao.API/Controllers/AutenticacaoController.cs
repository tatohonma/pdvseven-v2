using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using a7D.PDV.Ativacao.API.Model; 
using a7D.PDV.Ativacao.API.Repository.User;
using a7D.PDV.Ativacao.API.Services; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AutenticacaoController(
    ITokenService tokenService,
    IConfiguration configuration,
    UserManager<AppUser> userManager,
    IUserRepository usuarios) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await userManager.FindByNameAsync(loginRequest.Email);
        if (user == null) return Unauthorized(new {message = "E-mail não encontrado."});
        
        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!isPasswordValid) return Unauthorized(new {message = "Senha incorreta."});
        
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        foreach (var role in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, role));
        
        var token = tokenService.GenerateToken(authClaims, configuration);
        
        var refreshToken = tokenService.GenerateRefreshToken();
        
        _ = int.TryParse(configuration["Jwt:RefreshTokenLength"], out var refreshTokenValidityInMinutes);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(refreshTokenValidityInMinutes);
        
        await userManager.UpdateAsync(user);
        
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            ExpiresAt = user.RefreshTokenExpiresAt,
        });
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenCreateDto? tokenDto)
    {
        if (tokenDto == null) return BadRequest("Request invalida");

        var accessToken = tokenDto.AccessToken ?? throw new ArgumentNullException(nameof(tokenDto.AccessToken));

        var refreshToken = tokenDto.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDto.RefreshToken));

        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken, configuration);

        // if (principal == null) return BadRequest("Invalid access token/refresh token");

        var userName = principal.Identity?.Name;

        var user = await userManager.FindByNameAsync(userName!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiresAt < DateTime.Now)
            return BadRequest("Invalid refresh token");

        var newAccessToken = tokenService.GenerateToken(principal.Claims.ToList(), configuration);

        var newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Usuário não encontrado");

        user.RefreshToken = null;
        await userManager.UpdateAsync(user);

        return Ok(new { message = "Logout realizado com sucesso" });
    }
}

// ===== DTOs enxutos =====
public sealed class LoginDto
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
}
public sealed class TokenCreateDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
