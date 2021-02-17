using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Optsol.Components.Infra.Security.Data
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> 
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.ExternalId);

                entity.Property(u => u.IsEnabled);

                entity.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
            });
        }
    }
}
