using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class MongoContextNullException : Exception
    {
        public MongoContextNullException()
            : base("O parametro MongoContext não foi resolvido pela injeção de dependência")
        {

        }

        protected MongoContextNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
