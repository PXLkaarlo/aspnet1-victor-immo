using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Data;

internal static class PersistenceInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<PersistenceContext>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed.
            Console.Error.WriteLine("An error occurred while initializing the database: " + ex);

            throw; // Keep the original stack trace intact by rethrowing the exception without specifying it again.

            // This way of using a catch block is not efficient
            // because it just rethrows the exception without doing anything about it.
        }
    }
}
