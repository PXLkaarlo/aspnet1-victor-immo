using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SetEmailForm
{
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;


    [Range(typeof(bool), "true", "true", ErrorMessage = "Accepting the terms and conditions is required.")]
    public bool TermsAndConditions { get; set; }
}
