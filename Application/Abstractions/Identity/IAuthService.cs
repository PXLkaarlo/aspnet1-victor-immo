using Application.Dtos.Results;

namespace Application.Abstractions.Identity;

public interface IAuthService
{
    Task<AuthResult> CreateUserAsync(string email, string password, string? roleName = null);
    Task<AuthResult> UserExistsAsync(string email);
    Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false);
    Task SignOutUserAsync();
}
