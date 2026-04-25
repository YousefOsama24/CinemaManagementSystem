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

            TempData["success-notification"] = "Account created, check your email ";
            return RedirectToAction(nameof(Login));
        }

        // ================= CONFIRM EMAIL =================
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return Content("Invalid link ❌");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Content("User not found ❌");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return Content("Email confirmed successfully ");

            return Content("Error confirming email ");
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

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email first ");
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
            return RedirectToAction("Movie", "Home", new { area = "Customer" });
        }


        // ================= RESEND EMAIL =================
        [HttpGet]
        public IActionResult ResendEmailConfirmation() => View();

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = _userManager.Users
                .FirstOrDefault(u =>
                    u.Email == vm.EmailOrUserName ||
                    u.UserName == vm.EmailOrUserName);

            if (user != null && !user.EmailConfirmed)
            {
                await SendConfirmationMailAsync(user);
            }

            await Task.Delay(500);

            TempData["success-notification"] = "check your email ";

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

                var resetLink = Url.Action(
                    "ChangePassword",
                    "Account",
                    new { userId = user.Id, token = token },
                    Request.Scheme
                );

                var message = $@"
                    <h2>Reset Password </h2>
                    <p>Click the link below:</p>
                    <a href='{resetLink}'>Reset Password</a>
                ";

                await _emailSender.SendEmailAsync(user.Email, "Reset Password", message);
            }

            TempData["success-notification"] = "Check your email ";
            return RedirectToAction(nameof(Login));
        }

       
    
        // ================= SEND CONFIRMATION =================
        private async Task SendConfirmationMailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token = token },
                Request.Scheme
            );

            var message = $@"
                <h2> Welcome</h2>
                <p>Confirm your email:</p>
                <a href='{link}'>Confirm Email</a>
            ";

            await _emailSender.SendEmailAsync(user.Email, "Confirm Email", message);
        }
    }
}