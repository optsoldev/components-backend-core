using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Configurations;
using Optsol.Components.Test.Utils.Entity.Entities;

namespace Optsol.Components.Test.Utils.Data.Contexts
{

    public class Context : CoreContext
    {
        public Context(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TestEntity> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
