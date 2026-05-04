using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SignInForm
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;


    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;


    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;


    public string? ErrorMessage { get; set; }
}
