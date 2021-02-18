using Microsoft.AspNetCore.Identity;
using System;

namespace Optsol.Components.Infra.Security.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
