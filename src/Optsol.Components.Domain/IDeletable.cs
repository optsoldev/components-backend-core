using System;

namespace Optsol.Components.Domain
{
    public interface IDeletable
    {
        bool IsDeleted { get; }
        DateTime? DeletedDate { get; }
        void Delete();
    }
}
