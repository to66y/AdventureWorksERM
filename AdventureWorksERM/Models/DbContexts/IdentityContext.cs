using Microsoft.EntityFrameworkCore;
using AdventureWorksERM.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AdventureWorksERM.Models.DbContexts
{
    public class IdentityContext : IdentityDbContext<awUser>
    {
        public IdentityContext()
        {

        }
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<awUser> AwUsers { get; set; }
    }
}
