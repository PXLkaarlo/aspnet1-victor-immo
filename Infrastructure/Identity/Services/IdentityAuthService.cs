using Application.Abstractions.Identity;
using Application.Dtos.Results;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class IdentityAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.Failed("Incorrect email address or password");


        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        if (result.IsLockedOut)
            return AuthResult.Failed("This account is locked out.");

        if ( result.IsNotAllowed)
            return AuthResult.Failed("This user is not allowed to sign in.");

        if (result.RequiresTwoFactor)
            return AuthResult.Failed("This user requires two-factor authentication.");

        if (!result.Succeeded)
            return AuthResult.Failed("Incorrect email address or password.");


        return AuthResult.Success();
    }


    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}
