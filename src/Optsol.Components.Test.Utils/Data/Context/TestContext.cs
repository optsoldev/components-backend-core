using Microsoft.EntityFrameworkCore;

namespace Optsol.Components.Test.Utils.Data
{

    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<TestEntity> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new TestDeletableConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
