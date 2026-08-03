using Infrastructure.Identity.Data;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Data;

public class InfrastructureInitializer
{
    // Initialize the database and seed it with data. We will call this method in Program.cs before app.Run().
    public static async Task InitializeAsync(IServiceProvider service)
    {
        // This is the method that will automatically create the database if it does not exist,
        // and will apply any pending migrations.
        await PersistenceInitializer.InitializeDatabaseAsync(service);


        await IdentityInitializer.InitializeDefaultRolesAsync(service);

        // initialize default admin accounts
        await IdentityInitializer.InitializeDefaultAdminAccountsAsync(service);
    }
}
