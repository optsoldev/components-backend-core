using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Configurations;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Contexts
{

    public class TenantContext : CoreContext
    {
        public TenantContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TestTenantEntity> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestTenantConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
