using System;

namespace Optsol.Components.Domain.Entities
{
    public interface IDeletable
    {
        bool IsDeleted { get; }

        DateTime? DeletedDate { get; }

        void Delete();
    }
}
