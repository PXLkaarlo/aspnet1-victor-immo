using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;


//public class DemoPersistanceContext : IdentityDbContext<AppUser>
//{
//    CTRL + . on IdentityDbContext to add the constructor.
//}

//public class DemoPersistanceContext : IdentityDbContext<AppUser>
//{
//    public DemoPersistanceContext(DbContextOptions options) : base(options)
//    {
//        CTRL + . on constructor to put as primary constructor. Remove the unused one beforehand.
//    }
//}
//result this:

public class PersistenceContext(DbContextOptions<PersistenceContext> options) : IdentityDbContext<AppUser, IdentityRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
    }
}
