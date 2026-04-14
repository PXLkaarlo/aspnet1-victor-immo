namespace Application.Dtos.Identity;

public record AccountDetails
(
    string UserId,
    string? Email = null,
    string? FirstName = null,
    string? LastName = null,
    string? PhoneNumber = null,
    string? ImageUrl = null
);
