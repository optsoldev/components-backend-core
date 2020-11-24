using Microsoft.EntityFrameworkCore;
using Optsol.Playground.Infra.Data.EntityConfig;

namespace Optsol.Playground.Infra.Data.Context
{
    public class PlaygroundContext : DbContext
    {
        public PlaygroundContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new CartaoCreditoConfiguration());

            // modelBuilder.Seed();
        }
    }
}
