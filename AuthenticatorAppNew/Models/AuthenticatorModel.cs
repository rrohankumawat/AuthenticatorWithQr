namespace AuthenticatorAppNew.Models
{
    public class AuthenticatorModel
    {
        public string? SecretKey { get; set; }
        public string? CurrentCode { get; set; }
        public int RemainingSeconds { get; set; }
        public string? QrCodeImage { get; set; }
    }
}
