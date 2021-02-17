using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Security.Data
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public Guid ExternalId { get; set; }

        public bool IsEnabled { get; set; }

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    }
}
