using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // From Persistence/Extensions/PersistenceServiceCollectionExtensions.cs
        services.AddPersistence();

        // From Identity/Extensions/IdentityServiceCollectionExtensions.cs
        services.notyetAddIdentity();

        return services;
    }
}
