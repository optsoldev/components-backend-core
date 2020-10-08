using System;

namespace Optsol.Sdk.Domain
{
    public class AggregateRoot : Entity<Guid>, IAggregateRoot<Guid>
    {
        public AggregateRoot()
        {
            this.Id = Guid.NewGuid();
            this.CreateDate = DateTime.Now;
        }

    }
}
