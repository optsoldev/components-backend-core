using System;

namespace Optsol.Components.Domain.Entities
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
