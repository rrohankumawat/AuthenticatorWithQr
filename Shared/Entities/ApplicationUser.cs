using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Shared.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [ProtectedPersonalData]
        public string? AuthenticatorKey { get; set; }

        public bool TwoFactorEnabled { get; set; }
    }
}
