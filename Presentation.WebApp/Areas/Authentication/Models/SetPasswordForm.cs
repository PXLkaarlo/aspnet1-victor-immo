using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SetPasswordForm
{
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "Password must be confirmed.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm  password")]
    public string ConfirmPassword { get; set; } = null!;


    public string? ErrorMessage { get; set; }
}
