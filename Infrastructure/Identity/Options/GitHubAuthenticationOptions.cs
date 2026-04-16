namespace Infrastructure.Identity.Options;

public class GitHubAuthenticationOptions
{
    public const string SectionName = "Authentication:GitHub";

    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}
