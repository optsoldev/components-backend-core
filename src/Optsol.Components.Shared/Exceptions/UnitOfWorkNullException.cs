using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class UnitOfWorkNullException: Exception
    {  
        public UnitOfWorkNullException()
            : base("O parametro UnitOfWork não foi resolvido pela injeção de dependência")
        {
            
        }

        protected UnitOfWorkNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
