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
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AdminController(
            IBusRepository busRepo,
            ITripRepository tripRepo,
            IBookingRepository bookingRepo,
            UserManager<ApplicationUser> userManager)
        {
            _busRepo = busRepo;
            _tripRepo = tripRepo;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.TotalBuses = _busRepo.GetAll().Count();
            ViewBag.ActiveTrips = _tripRepo.GetAll()
                                           .Count(t => t.Status == TripStatus.OpenForBooking);
            ViewBag.TotalUsers = _userManager.Users.Count(u => !u.IsDeleted);
            ViewBag.TotalBookings = _bookingRepo.GetAll()
                                                .Count(b => b.Status != BookingStatus.Cancelled);

            return View();
        }

       
        [HttpGet]
        public IActionResult GetDashboardStats()
        {
            var totalBuses = _busRepo.GetAll().Count();
            var activeTrips = _tripRepo.GetAll().Count(t => t.Status == TripStatus.OpenForBooking);
            var totalUsers = _userManager.Users.Count(u => !u.IsDeleted);
            var totalBookings = _bookingRepo.GetAll()
                                            .Count(b => b.Status != BookingStatus.Cancelled);

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
