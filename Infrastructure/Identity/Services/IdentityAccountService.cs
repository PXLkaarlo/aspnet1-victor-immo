using Application.Abstractions.Identity;
using Application.Dtos.Identity;
using Application.Dtos.Results;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class IdentityAccountService(UserManager<AppUser> userManager) : IAccountService
{
    public async Task<AccountResult> GetUserAccountAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));


        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return AccountResult.NotFound();


        var details = new AccountDetails(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.ImageUrl
        );

        return AccountResult.Success(details);
    }


    public async Task<AccountResult> UpdateUserAccountDetailsAsync(UpdateAccountDetails details)
    {
        ArgumentNullException.ThrowIfNull(details);


        var user = await userManager.FindByIdAsync(details.UserId);

        if (user is null)
            return AccountResult.NotFound();


        user.FirstName = details.FirstName;
        user.LastName = details.LastName;
        user.PhoneNumber = details.PhoneNumber;
        user.ImageUrl = details.ImageUrl;


        var result = await userManager.UpdateAsync(user);

        return result.Succeeded
            ? AccountResult.Success()
            : AccountResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Failed to save changes");


        // This makes it more readable, but is not needed in this case.
        //if (result.Succeeded)
        //    return AccountResult.Success();

        //var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Failed to save changes";
        //return AccountResult.Failed(errorMessage);
    }


    public async Task<AccountResult> DeleteUserAccountAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));


        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return AccountResult.NotFound();


        var deleted = await userManager.DeleteAsync(user);

        return deleted.Succeeded
            ? AccountResult.Success()
            : AccountResult.Failed(deleted.Errors.FirstOrDefault()?.Description ?? "Unable to delete user account");
    }


    public async Task<AccountResult> UpdateUserRoleAsync(string userId, string? roleName = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentNullException(nameof(roleName));


        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return AccountResult.NotFound();


        var currentRoles = await userManager.GetRolesAsync(user);

        if (currentRoles.Contains(roleName))
            return AccountResult.Failed("User already has the specified role.");


        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded
            ? AccountResult.Success()
            : AccountResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Failed to update user role");
    }
    // Ended up not using this method, but it can be useful in the future if we want to allow users to have multiple roles.
}
