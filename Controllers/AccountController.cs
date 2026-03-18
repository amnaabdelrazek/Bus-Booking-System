using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // ================= Register =================
        [HttpGet]
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountRepository.Register(model);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // ================= Login =================
        [HttpGet]
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _accountRepository.Login(model);

            if (success)
            {
                // Role-based redirect
                var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

                if (role == "Admin")
                    return RedirectToAction("Dashboard", "Admin"); // صفحة الادمن
                return RedirectToAction("Index", "Home"); // صفحة المستخدم العادي
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        // ================= Logout =================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.Logout();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult test()
        {
            return View();
        }
    }
}
