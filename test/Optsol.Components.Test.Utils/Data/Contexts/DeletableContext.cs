using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Configurations;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Contexts
{

    public class DeletableContext : CoreContext
    {
        public DeletableContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TestDeletableEntity> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestDeletableConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
