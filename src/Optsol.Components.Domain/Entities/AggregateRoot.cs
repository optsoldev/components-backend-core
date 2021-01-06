using System;

namespace Optsol.Components.Domain.Entities
{
    public class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        public AggregateRoot()
        {
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }

    public class AggregateRoot : AggregateRoot<Guid>
    {
        public AggregateRoot()
            : base()
        {
            Id = Guid.NewGuid();
        }

    }
}
