namespace Presentation.WebApp.Areas.Authentication.Models;

public class SetEmailViewModel
{
    public SetEmailForm Form { get; set; } = new SetEmailForm();

    public string? ErrorMessage { get; set; }
}
