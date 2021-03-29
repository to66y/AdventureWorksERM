using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        //}
    }
}
