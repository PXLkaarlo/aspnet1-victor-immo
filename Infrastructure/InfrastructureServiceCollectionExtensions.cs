using Infrastructure.Identity.Extensions;
using Infrastructure.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // From Persistence/Extensions/PersistenceServiceCollectionExtensions.cs
        services.AddPersistence(configuration, environment);

        // From Identity/Extensions/IdentityServiceCollectionExtensions.cs
        services.AddIdentity(configuration);

        return services;
    }
}
