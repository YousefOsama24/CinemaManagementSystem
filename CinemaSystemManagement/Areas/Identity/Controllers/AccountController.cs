using CinemaSystemManagement.ViewModels;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // ================= LOGOUT =================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = new ApplicationUser
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                UserName = vm.UserName,
                Address = vm.Address
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(vm);
            }

            await SendConfirmationMailAsync(user);

            TempData["success-notification"] = "Account created, check your email";

            return RedirectToAction(nameof(Login));
        }

        // ================= CONFIRM EMAIL =================

        public async Task<IActionResult> Confirm(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                TempData["error-notification"] =
                    string.Join(", ", result.Errors.Select(e => e.Description));

                return RedirectToAction(nameof(Login));
            }

            TempData["success-notification"] = "Account Activated";
            return RedirectToAction(nameof(Login));
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail)
                       ?? await _userManager.FindByNameAsync(vm.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, vm.Password, vm.RememberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login");

                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Account locked");

                return View(vm);
            }

            TempData["success-notification"] = $"Welcome {user.FirstName}";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        // ================= RESEND EMAIL =================

        [HttpGet]
        public IActionResult ResendEmailConfirmation() => View();

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.EmailOrUserName)
                       ?? await _userManager.FindByNameAsync(vm.EmailOrUserName);

            if (user != null)
                await SendConfirmationMailAsync(user);

            TempData["success-notification"] = "Email sent";
            return RedirectToAction(nameof(Login));
        }

        // ================= FORGET PASSWORD =================

        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.EmailOrUserName)
                       ?? await _userManager.FindByNameAsync(vm.EmailOrUserName);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                TempData["token"] = token;
                TempData["userId"] = user.Id;
            }

            return RedirectToAction(nameof(ChangePassword));
        }

        // ================= CHANGE PASSWORD =================

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (TempData.Peek("userId") == null)
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = TempData["userId"]?.ToString();
            var token = TempData["token"]?.ToString();

            if (userId == null || token == null)
                return RedirectToAction(nameof(Login));

            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ResetPasswordAsync(user, token, vm.Password);

            if (!result.Succeeded)
            {
                TempData["error-notification"] =
                    string.Join(", ", result.Errors.Select(e => e.Description));

                TempData["userId"] = userId;
                TempData["token"] = token;

                return View(vm);
            }

            TempData["success-notification"] = "Password changed";
            return RedirectToAction(nameof(Login));
        }

        // ================= PRIVATE =================

        private async Task SendConfirmationMailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action("Confirm", "Account",
                new { area = "Identity", token, userId = user.Id },
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your account",
                $"<h3>Click <a href='{link}'>here</a></h3>");
        }
    }
}