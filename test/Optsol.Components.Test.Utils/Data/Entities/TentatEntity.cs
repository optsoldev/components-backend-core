using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Test.Utils.Data.Entities
{
    public class TenantEntity : AggregateRoot
    {
        public string Name { get; private set; }
        public string Host { get; private set; }

        public TenantEntity(string host, string name)
        {
            Host = host;
            Name = name;
        }

        public TenantEntity()
        {
        }
    }
}
