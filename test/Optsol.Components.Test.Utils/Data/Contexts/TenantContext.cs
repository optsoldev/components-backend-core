using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Test.Utils.Data.Configurations;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Contexts
{

    public class TenantDbContext : CoreContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }

        public DbSet<TenantEntity> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
                        
            base.OnModelCreating(modelBuilder);
        }
    }

    public class MultiTenantContext : CoreContext
    {
        private readonly ITenantProvider _tenantProvider;

        public MultiTenantContext(DbContextOptions options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<TestTenantEntity> Test { get; set; }

        public DbSet<TestTenantDeletableEntity> TestDeletable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestTenantConfiguration(_tenantProvider));
            modelBuilder.ApplyConfiguration(new TestTenantDeletableConfiguration(_tenantProvider));

            base.OnModelCreating(modelBuilder);
        }
    }
}
