using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Data;

internal class IdentityInitalizer
{
    public static async Task InitilizeDefaultRolesAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new List<IdentityRole>()
        {
            new("Admin"),
            new("Member")
        };

        try
        {
            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role.Name) && !await roleManager.RoleExistsAsync(role.Name))
                    await roleManager.CreateAsync(role);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred while initializing default roles: " + ex);
            throw;
        }
    }


    public static async Task InitilizeDefaultAdminAccountsAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var defaultAdmins = new List<string>()
        {
            "admin@domain.local",
        };

        try
        {
            if (!await userManager.Users.AnyAsync())
            {
                var defaultPassword = "MegaMaggot123!";
                var defaultRoleName = "Admin";

                foreach (var admin in defaultAdmins)
                {
                    var user = AppUser.Create(admin);
                    user.EmailConfirmed = true;

                    var created = await userManager.CreateAsync(user, defaultPassword);

                    if (created.Succeeded && await roleManager.RoleExistsAsync(defaultRoleName))
                        await userManager.AddToRoleAsync(user, defaultRoleName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred while initializing default ADMIN accounts: " + ex);
            throw;

            // By the Omnissiah. I made it so heretical before.
            // I pray to the Machine God that it will be pleased with this new way of handling errors.
        }
    }
}
