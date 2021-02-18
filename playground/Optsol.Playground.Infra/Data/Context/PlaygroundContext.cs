using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Infra.Data.EntityConfig;

namespace Optsol.Playground.Infra.Data.Context
{
    public class PlaygroundContext : CoreContext
    {
        public PlaygroundContext(DbContextOptions<PlaygroundContext> options) 
            : base(options)
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
