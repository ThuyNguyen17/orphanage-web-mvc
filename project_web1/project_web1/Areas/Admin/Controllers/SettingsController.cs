using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using project_web1.Models;

namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ cho phép admin truy cập
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SettingsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> AccountInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountInfo(string id, string email, string fullName, string address, int age)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Return error if the user is not found
            }

            // Update the user details
            user.Email = email;
            user.FullName = fullName; // Assuming you have a FullName property in your ApplicationUser model
            user.Address = address; // Assuming you have an Address property in your ApplicationUser model
            user.Age = age.ToString(); // Assuming you have an Age property in your ApplicationUser model

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Account information updated successfully!";
                return RedirectToAction("AccountInfo", new { id = user.Id });
            }

            // If there are errors, add them to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Return the view with the updated data if validation fails
            return View(user);
        }

        public IActionResult Notifications()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Notification(IFormCollection form)
        {
            // You can process or save settings here
            // For now, just return the view with a confirmation message or reload
            TempData["Message"] = "Settings saved!";
            return View();
        }
        public IActionResult ChangePasswords(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound(); // Handle if user is not found
            }

            return View(user); // Pass the user to the view to allow them to change password
        }
        // Handle the change password POST request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "New Password and Confirm Password must match.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Verify the current password
            var result = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View();
            }

            // Change the password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (changePasswordResult.Succeeded)
            {
                // If password change is successful, refresh the sign-in and show a success message
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("AccountInfo", new { id = user.Id });
            }

            // If there are errors, add them to the ModelState
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }


        public IActionResult SecurityPrivacy(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound(); // Handle if user is not found
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotificationSettings(IFormCollection form)
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Map the form data to user notification settings
            //user.EnableDesktopNotification = form["EnableDesktopNotification"].ToString() == "on";
            //user.EnableUnreadBadge = form["EnableUnreadBadge"].ToString() == "on";
            //user.PushTimeout = form["PushTimeout"].ToString();
            //user.EmailCommunication = form["EmailCommunication"].ToString() == "on";
            //user.EmailUpdates = form["EmailUpdates"].ToString() == "on";
            //user.MuteSounds = form["MuteSounds"].ToString() == "on";

            // Update the user with the new settings
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Notification settings updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update notification settings.";
            }

            return RedirectToAction("Notifications");
        }

    }
}
