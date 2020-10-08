using System;

namespace Optsol.Sdk.Domain
{
    public interface IDeletable
    {
        bool IsDeleted { get; }
        DateTime? DeletedDate { get; }
        void Delete();
    }
}
