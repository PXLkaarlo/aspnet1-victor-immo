using Application.Abstractions.Identity;
using Application.Dtos.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services;

public class IdentityAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<AuthResult> CreateUserAsync(string email, string password, string? roleName = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        // It's supposed to never be null at this stage. Therefore these null checks are here.


        var exists = await UserExistsAsync(email);

        // if (exists.Succeeded == true) => "User already exists."
        // if (exists.Succeeded == false) => Not Found
        if (exists.Succeeded)
            return AuthResult.Failed("User with this email address does already exist.");


        var user = AppUser.Create(email);

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    await userManager.AddToRoleAsync(user, roleName);
            }
            else
                throw new NotImplementedException("Role is required.");
        }

        return result.Succeeded
            ? AuthResult.Success()
            : AuthResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Unable to create user.");
    }


    public async Task<AuthResult> UserExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));


        var result = await userManager.Users.AnyAsync(x => x.Email == email);

        return result
            ? AuthResult.Success()
            : AuthResult.Failed("User with this email address does already exist.");
    }


    public async Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));


        var exists = await UserExistsAsync(email);
        if (!exists.Succeeded)
            return AuthResult.Failed("Incorrect email address or password.");


        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        if (result.IsLockedOut)
            return AuthResult.Failed("This account is locked out.");

        if (result.IsNotAllowed)
            return AuthResult.Failed("This user is not allowed to sign in.");

        if (result.RequiresTwoFactor)
            return AuthResult.Failed("This user requires two-factor authentication.");

        if (!result.Succeeded)
            return AuthResult.Failed("Incorrect email address or password.");




        return AuthResult.Success();
    }


    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}
