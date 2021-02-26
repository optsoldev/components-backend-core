using System;

namespace Optsol.Components.Shared.Settings
{
    public class ConnectionStrings : BaseSettings
    {
        public string DefaultConnection { get; set; }

        public string IdentityConnection { get; set; }

        public override void Validate() { }
    }
}
