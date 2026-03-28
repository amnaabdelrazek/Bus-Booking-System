using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepo;

        public AdminBookingsController(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
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
        public IActionResult Edit(int id, Booking model)
        {
            if (id != model.Id)
                return NotFound();

            var existingBooking = _bookingRepo.GetById(id);
            if (existingBooking == null)
                return NotFound();

           
            existingBooking.Status = model.Status;

            _bookingRepo.Update(existingBooking);
            _bookingRepo.Save();

            TempData["SuccessMsg"] = "Booking status updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _bookingRepo.Delete(id);
            _bookingRepo.Save();
            TempData["SuccessMsg"] = "Booking cancelled successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}