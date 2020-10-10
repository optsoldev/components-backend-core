using System;

namespace Optsol.Sdk.Domain
{
    public class AggregateRoot : Entity<Guid>, IAggregateRoot<Guid>
    {
        public AggregateRoot()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
