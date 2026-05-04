using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }


    // This is a factory method to create an AppUser with just an email.
    // Works on, example, external logins where we might only have the email address to create a user.
    public static AppUser Create(string email)
    {
        return new AppUser
        {
            UserName = email.Trim().ToLowerInvariant(),
            Email = email.Trim().ToLowerInvariant()
            // Is this supposed to be interchangeable?
            // It's literally the same thing, just on different properties.
        };
    }

    // This is an overloaded factory method to create an AppUser with more details.
    public static AppUser Create(
        string email, string? firstName = null, string? lastName = null, string? imageUrl = null, bool emailConfirmed = false)
    {
        return new AppUser
        {
            UserName = email.Trim().ToLowerInvariant(),
            Email = email.Trim().ToLowerInvariant(),
            FirstName = firstName?.Trim(),
            LastName = lastName?.Trim(),
            ImageUrl = imageUrl ?? "profile-image-avatar.png",
            EmailConfirmed = emailConfirmed
        };
    }

    public static AppUser UpdateDetails(AppUser user, string? firstName, string? lastName, string? imageUrl, string? phoneNumber)
    {
        if (user.FirstName != firstName)
            user.FirstName = firstName;

        if (user.LastName != lastName)
            user.LastName = lastName;

        if (user.ImageUrl != imageUrl)
            user.ImageUrl = imageUrl;

        if (user.PhoneNumber != phoneNumber)
            user.PhoneNumber = phoneNumber;


        return user;
    }
}
