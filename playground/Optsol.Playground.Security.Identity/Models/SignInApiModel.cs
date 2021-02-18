using System.ComponentModel.DataAnnotations;

namespace Optsol.Playground.Security.Identity.Models
{
    public class SignInApiModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}
