namespace Application.Dtos.Identity;

public record UpdateAccountDetails
(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber = null,
    string? ImageUrl = null
);
