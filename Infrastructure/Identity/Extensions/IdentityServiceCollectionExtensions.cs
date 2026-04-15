using Application.Abstractions.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            x.ExpireTimeSpan = TimeSpan.FromDays(30);
            x.SlidingExpiration = true;
        });


        // Authentication builder configuration for GitHub
        var authenticationBuilder = services.AddAuthentication();

        // Will work on this later. Need to ensure regular identity services are working first before adding external providers.


        services.AddScoped<IAccountService, IdentityAccountService>();

        return services;
    }
}
