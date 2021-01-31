using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class ResponseFactoryNullException: Exception
    {  
        public ResponseFactoryNullException()
            : base ("O parametro IResponseFactory não foi resolvido pela DI")
        {
            
        }
    }
}
