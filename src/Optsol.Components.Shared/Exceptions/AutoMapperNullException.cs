using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class AutoMapperNullException: Exception
    {  
        public AutoMapperNullException()
            : base ("O parametro mapper não foi resolvido pela injeção de dependência")
        {
            
        }

        protected AutoMapperNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
