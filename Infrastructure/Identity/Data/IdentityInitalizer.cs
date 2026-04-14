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
        catch
        {
            // Handle exceptions if needed
            // By the Omnissiah, This catch block is just left empty in the original code.
            // Shouldn't the try catch only be used for testing? Or am I missing something?
            // This isn't even the only time this happens in this project.
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
        catch
        {
            throw new Exception("An error occurred while initializing default admin accounts.");
        }
    }
}
