using Optsol.Components.Shared.Exceptions;

namespace Optsol.Components.Shared.Settings
{
    public abstract class BaseSettings
    {
        public abstract void Validate();

        public static void ShowingException(string objectName)
        {
            throw new SettingsNullException(objectName);
        }
    }
}
