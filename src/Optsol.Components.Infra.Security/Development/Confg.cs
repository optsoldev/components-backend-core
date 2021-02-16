using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Optsol.Components.Infra.Security.Development
{
    public static class Confg
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "optsol",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("sub", "1"),
                    },
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "test",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("sub", "2"),
                    },
                },
            };
        }
    }
}
