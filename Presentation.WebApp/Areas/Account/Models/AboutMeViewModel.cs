namespace Presentation.WebApp.Areas.Account.Models;

public class AboutMeViewModel
{
    public AboutMeForm Form { get; set; } = new AboutMeForm();
    public string? Message { get; set; }
    public string? ProfileImageUrl { get; set; }
}
