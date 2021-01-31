using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class ServiceResultNullException: Exception
    {  
        public ServiceResultNullException()
            : base ("O parametro IServiceResult não foi resolvido pela DI")
        {
            
        }
    }
}
