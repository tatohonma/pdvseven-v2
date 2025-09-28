using System.Runtime.CompilerServices;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace a7D.PDV.Ativacao.API.Repository.User;

public class UserRepository : IUserRepository
{
    readonly UserManager<AppUser> _userManager;
    readonly IEmailService _emailService;

    public UserRepository(UserManager<AppUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    static AppUser ToSafeUser(AppUser u) => new AppUser
    {
        Id = u.Id,
        UserName = u.UserName,
        Email = u.Email,
        EmailConfirmed = u.EmailConfirmed,
        PhoneNumber = u.PhoneNumber,
        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
        Name = u.Name,
        IsActive = u.IsActive,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt,
        LastPasswordChangeAt = u.LastPasswordChangeAt,
    };

    static IQueryable<AppUser> ApplyFilters(
        IQueryable<AppUser> q,
        string? name, string? email, string? admin, string? pendingRegistration,
        string active, string deleted)
    {
        if (!string.IsNullOrWhiteSpace(name))
            q = q.Where(u => u.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(email))
            q = q.Where(u => u.Email!.Contains(email));
        if (!string.IsNullOrWhiteSpace(pendingRegistration))
        {
            bool pending = pendingRegistration == "1";
            q = q.Where(u => u.EmailConfirmed != pending); 
        }
        if (!string.IsNullOrWhiteSpace(active))
        {
            bool isActive = active == "1";
            q = q.Where(u => u.IsActive == isActive);
        }

        if (!string.IsNullOrWhiteSpace(deleted) && deleted == "1")
        {
            q = q.Where(u => !u.IsActive);
        }
        else
        {
            q = q.Where(u => u.IsActive);
        }


        return q;
    }

    public async Task<AppUser?> GetByEmailAsync(string email, bool asNoTracking = true, CancellationToken ct = default)
    {
        var q = _userManager.Users.Where(u => u.Email == email);
        if (asNoTracking) q = q.AsNoTracking();
        return await q.FirstOrDefaultAsync(ct);
    }

    public async Task<AppUser?> GetByIdSafeAsync(string userId, CancellationToken ct = default)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, ct);
        return user is null ? null : ToSafeUser(user);
    }

    public async Task<bool> EmailExistsAsync(string email, int? ignoreUserIntId = null, CancellationToken ct = default)
    {
        var exists = await _userManager.Users.AnyAsync(u => u.Email == email, ct);
        return exists;
    }

    public async IAsyncEnumerable<AppUser> QueryUsersAsync(
        int page, int count,
        string? name, string? email,
        string? admin, string? pendingRegistration,
        string active = "1", string deleted = "0",
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var q = _userManager.Users.AsNoTracking().OrderBy(u => u.Name).AsQueryable();
        q = ApplyFilters(q, name, email, admin, pendingRegistration, active, deleted);

        if (page > 0 && count > 0)
            q = q.Skip((page - 1) * count).Take(count);
        else if (count > 0)
            q = q.Take(count);

        await foreach (var u in q.AsAsyncEnumerable().WithCancellation(ct))
            yield return ToSafeUser(u);
    }

    public async Task<AppUser> AuthenticateAsync(string email, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, ct)
                   ?? throw new InvalidOperationException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is inactive.");

        if (!user.EmailConfirmed)
            throw new InvalidOperationException("Email not confirmed.");

        var valid = await _userManager.CheckPasswordAsync(user, password);
        if (!valid)
            throw new InvalidOperationException("Invalid credentials.");

        return ToSafeUser(user);
    }

    public async Task<AppUser> AddUserAsync(string email, string? name = null, string? tempPassword = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        var existing = await GetByEmailAsync(email, asNoTracking: false, ct);
        if (existing is not null)
            throw new InvalidOperationException("E-mail already exists.");

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            Name = name ?? email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = false
        };

        IdentityResult result;
        if (string.IsNullOrWhiteSpace(tempPassword))
        {
            result = await _userManager.CreateAsync(user);
        }
        else
        {
            result = await _userManager.CreateAsync(user, tempPassword);
        }

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return ToSafeUser(user);
    }

    public async Task<string> SendRegistrationEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
            throw new InvalidOperationException("User not found.");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailService.SendUserAsync(
            ETipoEmailUsuario.NovoCadastro,        
            ToSafeUser(user),
            new Dictionary<string, string?>
            {
                ["UserId"] = user.Id,
                ["Token"]  = token
            }
        );

        return token;
    }



    public async Task<string> RequestPasswordResetAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive, ct);
        if (user is null)
            return string.Empty; // silencioso, como prática comum

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task ResetPasswordAsync(string userId, string resetToken, string newPassword, string? newName = null, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new InvalidOperationException("User not found.");

        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrWhiteSpace(newName))
            user.Name = newName;

        user.EmailConfirmed = true; 
        user.LastPasswordChangeAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));
    }

    public async Task UpdateProfileAsync(string userId, string? name = null, bool? isActive = null, bool? isAdmin = null, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return;

        if (!string.IsNullOrWhiteSpace(name))
            user.Name = name;

        if (isActive.HasValue)
            user.IsActive = isActive.Value;

        if (isAdmin.HasValue)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin.Value && !isInRole)
                await _userManager.AddToRoleAsync(user, "Admin");
            else if (!isAdmin.Value && isInRole)
                await _userManager.RemoveFromRoleAsync(user, "Admin");
        }

        user.UpdatedAt = DateTime.UtcNow;

        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }

    public async Task SoftDeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new ArgumentOutOfRangeException(nameof(userId));

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }

    public async Task SendResetPasswordEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _emailService.SendUserAsync(
            ETipoEmailUsuario.EsqueciASenha,
            ToSafeUser(user),
            new Dictionary<string, string?>
            {
                ["UserId"] = user.Id,
                ["Token"]  = token
            }
        );
    }

}
