using Microsoft.AspNetCore.Identity;

namespace AuthenticatorAppNew.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ProtectedPersonalData]
        public string? AuthenticatorKey { get; set; }

        public bool TwoFactorEnabled { get; set; }
    }
}
