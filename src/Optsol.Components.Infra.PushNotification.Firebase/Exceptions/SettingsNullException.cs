using System;
namespace Optsol.Components.Infra.PushNotification.Firebase.Exceptions
{
	public class SettingsNullException: Exception
    {
        public SettingsNullException(string objectName)
            : base($"A configuração FirebaseSettings:{objectName} está nula.")
        {

        }
    }
}

