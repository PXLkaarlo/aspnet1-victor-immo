using Infrastructure.Identity.Data;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Data;

public class InfrastructureInitializer
{
    // Initialize the database and seed it with data. We will call this method in Program.cs after app.Run().
    public static async Task InitializeAsync(IServiceProvider service)
    {
        // initialize database
        await PersistenceInitializer.InitializeDatabaseAsync(service);

        // initialize default identity roles
        await IdentityInitalizer.InitilizeDefaultRolesAsync(service);

        // initialize default user accounts
        await IdentityInitalizer.InitilizeDefaultAdminAccountsAsync(service);
    }
}
