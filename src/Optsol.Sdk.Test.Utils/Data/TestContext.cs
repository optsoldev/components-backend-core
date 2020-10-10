using Microsoft.EntityFrameworkCore;

namespace Optsol.Sdk.Test.Shared.Data
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
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
