using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class NotificationContextException : Exception
    {
        public NotificationContextException()
            : base("O parametro NotificationContext não foi resolvido pela injeção de dependência")
        {

        }

        protected NotificationContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}