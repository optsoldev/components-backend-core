using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException()
            : base("O repositório foi configurado incorretamente")
        {

        }

        protected InvalidRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
