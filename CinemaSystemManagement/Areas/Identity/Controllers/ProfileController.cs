using CinemaSystemManagement.Models;
using CinemaSystemManagement.Utility;
using CinemaSystemManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    [Authorize] 
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ProfileController> _logger;
        private readonly IWebHostEnvironment _env;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ProfileController> logger,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _env = env;
        }

        // ================= PROFILE =================
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                _logger.LogWarning("User not found in Profile Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var vm = new ApplicationUserVM
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ProfileImage = user.ProfileImage
            };

            return View(vm);
        }

        // ================= UPDATE PROFILE =================
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUserVM vm)
        {
            if (!ModelState.IsValid)
                return View("Index", vm);

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                _logger.LogError("User not found while updating profile");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                //  Upload Image
                if (vm.Image != null)
                {
                    var folder = Path.Combine(_env.WebRootPath, "images/profile");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(vm.Image.FileName);
                    var path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await vm.Image.CopyToAsync(stream);
                    }

                    user.ProfileImage = fileName;
                }

                // Update Data
                user.FirstName = vm.FirstName;
                user.LastName = vm.LastName;
                user.Address = vm.Address;
                user.PhoneNumber = vm.PhoneNumber;

                // Email Change
                if (user.Email != vm.Email)
                {
                    user.Email = vm.Email;
                    user.UserName = vm.Email;
                    user.EmailConfirmed = false;

                    _logger.LogInformation($"User {user.Id} changed email");
                }

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError("", e.Description);

                    _logger.LogWarning("Profile update failed");
                    return View("Index", vm);
                }

                TempData["success-notification"] = "Profile updated successfully ✅";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                ModelState.AddModelError("", "Something went wrong");
                return View("Index", vm);
            }
        }

        // ================= CHANGE PASSWORD =================
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ProfileChangePasswordVM());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                _logger.LogError("User not found in ChangePassword");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                vm.CurrentPassword,
                vm.NewPassword
            );

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);

                _logger.LogWarning("Password change failed");
                return View(vm);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["success-notification"] = "Password changed successfully";
            return RedirectToAction(nameof(Index));
        }

        // ================= LOGOUT =================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out");

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}