using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class UnitOfWorkNullException: Exception
    {  
        public UnitOfWorkNullException()
            : base ("O parametro unitOfWork não foi resolvido pela DI")
        {
            
        }
    }
}
