using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class DbContextNullException : Exception
    {  
        public DbContextNullException()
            : base ("O parametro DBContext não foi resolvido pela injeção de dependência")
        {
            
        }

        protected DbContextNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
