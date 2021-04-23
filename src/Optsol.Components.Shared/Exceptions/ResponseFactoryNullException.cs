using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class ResponseFactoryNullException: Exception
    {  
        public ResponseFactoryNullException()
            : base("O parametro IResponseFactory não foi resolvido pela injeção de dependência")
        {
            
        }
    }
}
