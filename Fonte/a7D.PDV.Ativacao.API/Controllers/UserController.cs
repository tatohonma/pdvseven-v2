using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Repository.User;
using Microsoft.AspNetCore.Identity;

namespace a7D.PDV.Ativacao.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserRepository users,
    ILogger<UserController> logger, 
    UserManager<AppUser> userManager)
    : ControllerBase
{
    
    [HttpPost("generate-reset-token")]
    public async Task<IActionResult> GenerateResetToken([FromBody] string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound("Usuário não encontrado");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return Ok(new { UserId = user.Id, Token = token });
    }

    
    
    [HttpPost("renovar")]
    public async Task<IActionResult> RenewPassword([FromBody] string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("E-mail é obrigatório.");

        await users.SendResetPasswordEmailAsync(email, ct);
        return NoContent();
    }

    
    
    [HttpPost("reenviar")]
    public async Task<IActionResult> ResendRegistration([FromBody] string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("E-mail é obrigatório.");

        await users.SendRegistrationEmailAsync(email, ct);
        return NoContent();
    }

    
    
    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO? dto, CancellationToken ct)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.UserId) ||
          string.IsNullOrWhiteSpace(dto.NewPassword))
            return BadRequest("UserId, Token e NewPassword são obrigatórios.");

        await users.ResetPasswordAsync(dto.UserId, dto.Token, dto.NewPassword, dto.NewName, ct);
        return NoContent();
    }

    
    [HttpPost]
    public async Task<ActionResult<AppUser>> AddUser([FromBody] UserDTO user, CancellationToken ct)
    {
        if (user is null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Name))
            return BadRequest("E-mail e Nome são obrigatórios.");

        var created = await users.AddUserAsync(user.Email, user.Name, tempPassword: null, ct);
        await users.SendRegistrationEmailAsync(created.Email!, ct);

        
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser([FromRoute] string id, CancellationToken ct)
    {
        var user = await users.GetByIdSafeAsync(id, ct);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUser([FromRoute] string id, CancellationToken ct)
    {
        await users.SoftDeleteUserAsync(id, ct);
        return NoContent();
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDTO user, CancellationToken ct)
    {
        if (user is null || string.IsNullOrWhiteSpace(id) || (user.Id is not null && user.Id != id))
            return BadRequest("ID do caminho não confere com o ID do corpo.");

        await users.UpdateProfileAsync(id, user.Name, user.IsActive, user.IsAdmin, ct);
        return NoContent();
    }

    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> QueryUsers(
        [FromQuery] int page = 0,
        [FromQuery] int count = 0,
        [FromQuery] UserFilter filter = null!,
        CancellationToken ct = default)
    {
        filter ??= new UserFilter();

        var list = new List<AppUser>();
        await foreach (var u in users.QueryUsersAsync(
            page,
            count,
            filter.Name,
            filter.Email,
            filter.Admin,
            filter.PendingRegistration,
            filter.Active ?? "1",
            filter.Deleted ?? "0",
            ct))
        {
            list.Add(u);
        }

        Response.Headers["X-Total-Count"] = list.Count.ToString();
        return Ok(list);
    }

    
    public sealed class ResetPasswordDTO
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string? NewName { get; set; }
    }

    public sealed class UserDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public sealed class UpdateUserDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
    }

    public sealed class UserFilter
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Active { get; set; }                
        public string? PendingRegistration { get; set; }   
        public string? Admin { get; set; }                 
        public string? Deleted { get; set; }               
    }
}