namespace Application.Dtos.Results;

public sealed record AuthResult(bool Succeeded, string? ErrorMessage = null)
{
    public static AuthResult Success() => new(true);
    public static AuthResult Failed(string errorMessage) => new(false, errorMessage);
}
