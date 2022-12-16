using System.Security.Claims;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Models
{
    public class UserInfo
    {
        public List<CustomClaim> Claims { get; set; }

        public UserInfo()
        {
            this.Claims = new();
        }

        public void AddClaim(string claim, string autoridade, string claimType = ClaimTypes.Role)
            => this.Claims.Add(new CustomClaim(claimType, claim, valueType: "", autoridade, ""));

        public bool HasAccess(string access)
            => this.Claims.Any(x => x.Value == access);
    }

    public class CustomClaim : Claim
    {
        public CustomClaim(string type, string value, string valueType, string issuer, string originalIssuer) :
            base(type, value, valueType, issuer, originalIssuer)
        { }
    }
}
