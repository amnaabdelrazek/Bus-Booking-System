using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBusRepository _busRepo;
        private readonly ITripRepository _tripRepo;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public AdminController(
            IBusRepository busRepo,
            ITripRepository tripRepo,
            UserManager<ApplicationUser> userManager)
        {
            _busRepo = busRepo;
            _tripRepo = tripRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            
            var totalBuses = _busRepo.GetAll().Count();

           
            var activeTrips = _tripRepo.GetAll()
                                       .Count(t => t.Status == TripStatus.OpenForBooking);

           
            var totalUsers = _userManager.Users.Count();

          
            var totalBookings = 0;

           
            ViewBag.TotalBuses = totalBuses;
            ViewBag.ActiveTrips = activeTrips;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalBookings = totalBookings;

            return View();
        }

       
        [HttpGet]
        public IActionResult GetDashboardStats()
        {
            var totalBuses = _busRepo.GetAll().Count();
            var activeTrips = _tripRepo.GetAll().Count(t => t.Status == TripStatus.OpenForBooking);
            var totalUsers = _userManager.Users.Count();
            var totalBookings = 0; 

            return Json(new
            {
                buses = totalBuses,
                trips = activeTrips,
                users = totalUsers,
                bookings = totalBookings
            });
        }
    }
}