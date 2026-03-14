using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bus_Booking_System.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountRepository(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // ================= Register =================
        public async Task<IdentityResult> Register(RegisterVM model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                // تسجيل الدخول مباشرة بعد التسجيل
                await LoginInternal(user, isPersistent: false);
            }

            return result;
        }

        // ================= Login =================
        public async Task<bool> Login(LoginVM model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) return false;

            var user = await _userManager.FindByNameAsync(model.UserName);

            // تسجيل الدخول مع Claims و RememberMe
            await LoginInternal(user, model.RememberMe);

            return true;
        }

        // ================= Logout =================
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        // ================= Helper: Login with Claims =================
        private async Task LoginInternal(ApplicationUser user, bool isPersistent)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = isPersistent
                        ? DateTimeOffset.UtcNow.AddDays(7)
                        : DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }
    }
}
