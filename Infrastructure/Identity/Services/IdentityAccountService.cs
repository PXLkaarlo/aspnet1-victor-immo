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
            : AccountResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Unable to save changes");


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
}
