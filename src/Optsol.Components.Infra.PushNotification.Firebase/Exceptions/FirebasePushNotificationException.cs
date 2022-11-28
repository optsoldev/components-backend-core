using System;
namespace Optsol.Components.Infra.PushNotification.Firebase.Exceptions
{
	public class FirebasePushNotificationException: Exception
	{
        public FirebasePushNotificationException() {}

        public FirebasePushNotificationException(string message) : base(message) { }

        public FirebasePushNotificationException(string message, Exception innerException) : base(message, innerException) { }
    }
}

