using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    public class AuthenticatorModel
    {
        public string? SecretKey { get; set; }
        public string? CurrentCode { get; set; }
        public int RemainingSeconds { get; set; }
        public string? QrCodeImage { get; set; }
        public string? Username { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        [Required]
        public string InputCode { get; set; }
    }
}
