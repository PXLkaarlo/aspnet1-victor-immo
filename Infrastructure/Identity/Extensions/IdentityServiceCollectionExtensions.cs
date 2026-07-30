using Application.Abstractions.Identity;
using Infrastructure.Identity.Options;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure.Identity.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure regular identity services
        services.AddIdentity<AppUser, IdentityRole>(x =>
        {
            x.SignIn.RequireConfirmedAccount = false;
            x.User.RequireUniqueEmail = true;
            x.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<PersistenceContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(x =>
        {
            x.LoginPath = "/sign-in";
            x.LogoutPath = "/";
            x.AccessDeniedPath = "/denied";

            x.Cookie.IsEssential = true;
            x.Cookie.Name = "corefitness_auth_cookie";
            x.ExpireTimeSpan = TimeSpan.FromDays(31);
            x.SlidingExpiration = true;
        });


        // Authentication builder configuration for GitHub
        var authenticationBuilder = services.AddAuthentication();

        var gitHubOptions = configuration
            .GetSection(GitHubAuthenticationOptions.SectionName)
            .Get<GitHubAuthenticationOptions>();

        if (gitHubOptions is not null && !string.IsNullOrWhiteSpace(gitHubOptions.ClientId) && !string.IsNullOrWhiteSpace(gitHubOptions.ClientSecret))
        {
            authenticationBuilder.AddGitHub("GitHub", options =>
            {
                options.ClientId = gitHubOptions.ClientId;
                options.ClientSecret = gitHubOptions.ClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-github";

                options.Scope.Add("user:email");

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:github:picture", "avatar_url");
                options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                options.ClaimActions.MapJsonKey("urn:github:login", "login");

                options.SaveTokens = true;
            });
        }


        services.AddScoped<IAuthService, IdentityAuthService>();

        services.AddScoped<IAccountService, IdentityAccountService>();

        return services;
    }
}
