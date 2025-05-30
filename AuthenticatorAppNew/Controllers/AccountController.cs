using DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using Services.Account;
using Shared.Entities;

namespace AuthenticatorAppNew.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AccountServices _accountServices;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AccountServices accountServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountServices = accountServices;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isValidOtp = await _accountServices.IsValidOtp(model.Email, model.InputCode);
                if (isValidOtp)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                        if (resetResult.Succeeded)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        foreach (var error in resetResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP Code.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("EnableAuthenticator", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    bool isValidOtp = await _accountServices.IsValidOtp(model.Email, model.InputCode);
                    if (isValidOtp)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid OTP Code.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                bool isValidOtp = await _accountServices.IsValidOtp(model.Email, model.InputCode);
                if (isValidOtp)
                {
                    return RedirectToAction("ResetPassword", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP Code.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
