using Application.Dtos.Results;

namespace Application.Abstractions.Identity;

public interface IAuthService
{
    Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false);
    Task SignOutUserAsync();
}
