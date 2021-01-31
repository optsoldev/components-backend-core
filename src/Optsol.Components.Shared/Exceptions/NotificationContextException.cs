using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class NotificationContextException : Exception
    {
        public NotificationContextException()
            : base("O parametro notificationContext não foi resolvido pela DI")
        {

        }
    }
}