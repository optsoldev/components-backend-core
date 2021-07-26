using Optsol.Components.Shared.Extensions;
using System;

namespace Optsol.Components.Shared.Settings
{
    public class FirebaseSettings : BaseSettings
    {
        public string FileKeyJson { get; set; }

        public override void Validate()
        {

            if(FileKeyJson.IsEmpty())
            {
                ShowingException(nameof(FileKeyJson));
            }
        }
    }
}
