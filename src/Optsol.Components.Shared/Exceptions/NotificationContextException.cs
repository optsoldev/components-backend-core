using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class NotificationContextException : Exception
    {
        public NotificationContextException()
            : base("O parametro NotificationContext não foi resolvido pela injeção de dependência")
        {

        }
    }
}