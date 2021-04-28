using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class ResponseFactoryNullException: Exception
    {  
        public ResponseFactoryNullException()
            : base("O parametro IResponseFactory não foi resolvido pela injeção de dependência")
        {
            
        }

        protected ResponseFactoryNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
