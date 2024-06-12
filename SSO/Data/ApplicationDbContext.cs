using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.Models;
using System.Reflection.Metadata;

namespace SSO.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            var appUser = builder.Entity<ApplicationUser>();
            appUser.ToTable("ApplicationUsers");
            appUser.HasKey(x => x.Id);

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
