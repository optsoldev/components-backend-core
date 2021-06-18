using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this object settings, string settingsRoot)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            foreach (var property in settings.GetType().GetProperties())
            {
                yield return new KeyValuePair<string, string>($"{settingsRoot}:{property.Name}", property.GetValue(settings).ToString());
            }
        }
    }
}
