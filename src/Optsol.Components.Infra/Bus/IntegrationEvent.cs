using System;

namespace Optsol.Components.Infra.Bus
{
    public class IntegrationEvent
    {
        public Guid Id { get; private set; }
        public DateTime CreateDate { get; set; }

        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreateDate = createDate;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}