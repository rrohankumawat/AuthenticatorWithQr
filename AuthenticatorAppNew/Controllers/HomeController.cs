using AuthenticatorAppNew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using QRCoder;
using Shared.Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public HomeController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !_signInManager.IsSignedIn(User))
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new AuthenticatorModel
        {
            Username = user.UserName
        };

        // Generate or retrieve secret key
        if (string.IsNullOrEmpty(user.AuthenticatorKey))
        {
            // Generate new key
            var key = KeyGeneration.GenerateRandomKey(20);
            model.SecretKey = Base32Encoding.ToString(key);

            // Store key in user record
            user.AuthenticatorKey = model.SecretKey;
            await _userManager.UpdateAsync(user);

            model.QrCodeImage = GenerateQrCode($"otpauth://totp/RoAuth:{user.Email}?secret={model.SecretKey}&issuer=RoAuth");
        }
        else
        {
            model.SecretKey = user.AuthenticatorKey;
            model.QrCodeImage = GenerateQrCode($"otpauth://totp/RoAuth:{user.Email}?secret={model.SecretKey}&issuer=RoAuth");
        }

        // Generate current code
        var totp = new Totp(Base32Encoding.ToBytes(model.SecretKey));
        model.CurrentCode = totp.ComputeTotp();
        model.RemainingSeconds = totp.RemainingSeconds();

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !_signInManager.IsSignedIn(User))
        {
            return RedirectToAction("Login", "Account");
        }

        // Same as Index but without requiring 2FA setup
        return RedirectToAction("Index");
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