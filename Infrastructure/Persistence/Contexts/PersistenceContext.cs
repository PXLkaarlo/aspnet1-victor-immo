using Infrastructure.Identity;
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
//        CTRL + . on constructor to put as primary constructor and remove the unused one.
//    }
//}
//result this:

public class PersistenceContext(DbContextOptions<PersistenceContext> options) : IdentityDbContext<AppUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
    }
}

// delete this comment later, for secrets.json 
//{
//  "Authentication": {
//    "GitHub": {
//      "ClientId": "",
//      "ClientSecret": ""
//    }
//  }
//}
