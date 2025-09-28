using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Repository.User;

public interface IUserRepository
{
    Task<AppUser> AuthenticateAsync(string email, string password, CancellationToken ct = default);
    Task<string> RequestPasswordResetAsync(string email, CancellationToken ct = default);
    Task ResetPasswordAsync(string userId, string resetToken, string newPassword, string? newName = null, CancellationToken ct = default);

    Task<AppUser> AddUserAsync(string email, string? name = null, string? tempPassword = null, CancellationToken ct = default);
    Task<string> SendRegistrationEmailAsync(string email, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, int? ignoreUserIntId = null, CancellationToken ct = default);
    Task<AppUser?> GetByEmailAsync(string email, bool asNoTracking = true, CancellationToken ct = default);
    Task<AppUser?> GetByIdSafeAsync(string userId, CancellationToken ct = default);

    IAsyncEnumerable<AppUser> QueryUsersAsync(
        int page, int count,
        string? name, string? email,
        string? admin, string? pendingRegistration,
        string active = "1", string deleted = "0",
        CancellationToken ct = default);

    Task UpdateProfileAsync(string userId, string? name = null, bool? isActive = null, bool? isAdmin = null, CancellationToken ct = default);
    Task SoftDeleteUserAsync(string userId, CancellationToken ct = default);

    Task SendResetPasswordEmailAsync(string email, CancellationToken ct = default);
}
