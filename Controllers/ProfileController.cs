using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ProfileController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ProfileDetailsVM profileDetails = new ProfileDetailsVM();

            if (user.UserName == null)
            {
                profileDetails.UserName = "";
            }
            else
            {
                profileDetails.UserName = user.UserName;
            }

            if (user.Email == null)
            {
                profileDetails.Email = "";
            }
            else
            {
                profileDetails.Email = user.Email;
            }

            if (user.PhoneNumber == null)
            {
                profileDetails.Phone = "";
            }
            else
            {
                profileDetails.Phone = user.PhoneNumber;
            }

            profileDetails.FullName = user.FullName;

            return View(profileDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ApplicationUser? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            EditProfileVM editProfile = new EditProfileVM();

            if (user.UserName == null)
            {
                editProfile.UserName = "";
            }
            else
            {
                editProfile.UserName = user.UserName;
            }

            if (user.Email == null)
            {
                editProfile.Email = "";
            }
            else
            {
                editProfile.Email = user.Email;
            }

            if (user.PhoneNumber == null)
            {
                editProfile.Phone = "";
            }
            else
            {
                editProfile.Phone = user.PhoneNumber;
            }

            editProfile.FullName = user.FullName;

            return View(editProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.FullName = model.FullName;

            IdentityResult result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Profile updated successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ChangePasswordVM changePassword = new ChangePasswordVM();
            return View(changePassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser? user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            IdentityResult result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Password changed successfully.";

            return RedirectToAction("Index");
        }
    }
}
