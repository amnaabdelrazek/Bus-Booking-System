using Bus_Booking_System.Hubs;
using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IHubContext<DashboardHub> _dashboardHubContext;
        private readonly IHubContext<BookingHub> _bookingHubContext;

        public AdminBookingsController(IBookingRepository bookingRepo, IHubContext<DashboardHub> dashboardHubContext, IHubContext<BookingHub> bookingHubContext)
        {
            _bookingRepo = bookingRepo;
            _dashboardHubContext = dashboardHubContext;
            _bookingHubContext = bookingHubContext;
        }

       
        public IActionResult Index()
        {
            var bookings = _bookingRepo.GetAllWithDetails();
            return View(bookings);
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            var booking = _bookingRepo.GetAllWithDetails().FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking model)
        {
            if (id != model.Id)
                return NotFound();

            var existingBooking = _bookingRepo.GetById(id);
            if (existingBooking == null)
                return NotFound();

           
            existingBooking.Status = model.Status;

            _bookingRepo.Update(existingBooking);
            _bookingRepo.Save();
            await _dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
            await _bookingHubContext.Clients.All.SendAsync("ReceiveBookingsUpdate");

            TempData["SuccessMsg"] = "Booking status updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _bookingRepo.Delete(id);
            _bookingRepo.Save();
            await _dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
            await _bookingHubContext.Clients.All.SendAsync("ReceiveBookingsUpdate");
            TempData["SuccessMsg"] = "Booking cancelled successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
