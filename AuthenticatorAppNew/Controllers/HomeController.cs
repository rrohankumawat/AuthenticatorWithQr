using AuthenticatorAppNew.Models;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var model = new AuthenticatorModel();

        // Generate or retrieve secret key
        if (HttpContext.Session.GetString("SecretKey") == null)
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(key);
            HttpContext.Session.SetString("SecretKey", base32Secret);
            model.SecretKey = base32Secret;
            model.QrCodeImage = GenerateQrCode($"otpauth://totp/AuthenticatorApp?secret={base32Secret}");
        }
        else
        {
            model.SecretKey = HttpContext.Session.GetString("SecretKey");
        }

        // Generate current code
        var totp = new Totp(Base32Encoding.ToBytes(model.SecretKey));
        model.CurrentCode = totp.ComputeTotp();
        model.RemainingSeconds = totp.RemainingSeconds();

        return View(model);
    }

    private string GenerateQrCode(string text)
    {
        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(5);
        return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
    }
}