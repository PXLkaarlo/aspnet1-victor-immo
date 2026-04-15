using Application.Dtos.Identity;

namespace Application.Dtos.Results;

public sealed record AccountResult(bool Succeeded, AccountDetails? Details = null, string? ErrorMessage = null)
{
    public static AccountResult Success(AccountDetails? details = null) => new(true, details);
    public static AccountResult Failed(string errorMessage) => new(false, null, errorMessage);
    public static AccountResult NotFound(string errorMessage = "User not found") => new(false, null, errorMessage); 
}
