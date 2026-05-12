using Application.Dtos.Identity;
using Application.Dtos.Results;

namespace Application.Abstractions.Identity;

public interface IAccountService
{
    Task<AccountResult> GetUserAccountAsync(string userId);
    Task<AccountResult> DeleteUserAccountAsync(string userId);
    Task<AccountResult> UpdateUserAccountDetailsAsync(UpdateAccountDetails details);
    Task<AccountResult> UpdateUserRoleAsync(string userId, string? roleName = null);
}
