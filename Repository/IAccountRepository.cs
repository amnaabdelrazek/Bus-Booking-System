using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Bus_Booking_System.Repository
{
    
        public interface IAccountRepository
        {
            Task<IdentityResult> Register(RegisterVM model);

            Task<bool> Login(LoginVM model);
            //Task<IList<string>> GetRoles(ApplicationUser user);

            Task Logout();
        }
    
}
