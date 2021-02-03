using Microsoft.EntityFrameworkCore;

namespace Optsol.Components.Infra.Data
{
    public abstract class CoreContext : DbContext
    {
        protected CoreContext() 
            : base()
        {
        }

        protected CoreContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
