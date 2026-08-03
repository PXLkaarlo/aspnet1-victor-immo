using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Data;

internal class IdentityInitializer
{
    public static async Task InitializeDefaultRolesAsync(IServiceProvider serviceProvider)
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
            //throw;
        }
    }


    public static async Task InitializeDefaultAdminAccountsAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var defaultAdmins = new List<AppUser>
        {
            AppUser.Create("admin@domain.local", null, null, null, true)
        };

        try
        {
            if (!await userManager.Users.AnyAsync())
            {
                string defaultPassword = "MegaMaggot123!";
                string defaultRoleName = "Admin";

                foreach (var admin in defaultAdmins)
                {
                    var created = await userManager.CreateAsync(admin, defaultPassword);

                    if (created.Succeeded)
                    {
                        if (await roleManager.RoleExistsAsync(defaultRoleName))
                            await userManager.AddToRoleAsync(admin, defaultRoleName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred while initializing default ADMIN accounts: " + ex);
            //throw;

            // By the Omnissiah.
        }
    }
}
