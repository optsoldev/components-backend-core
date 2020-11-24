using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class AutoMapperNullException: Exception
    {  
        public AutoMapperNullException()
            : base ("O parametro mapper não foi resolvido pela IoC")
        {
            
        }
    }
}
