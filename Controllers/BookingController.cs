using Bus_Booking_System.Hubs;
using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bus_Booking_System.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IHubContext<BookingHub> hubContext;
        private readonly IHubContext<DashboardHub> dashboardHubContext;
        private readonly ITripRepository tripRepository;

        public BookingController(IBookingRepository _bookingRepository,
                                 IHubContext<BookingHub> _hubContext,
                                 IHubContext<DashboardHub> _dashboardHubContext,
                                 ITripRepository _tripRepository)
        {
            bookingRepository = _bookingRepository;
            hubContext = _hubContext;
            dashboardHubContext = _dashboardHubContext;
            tripRepository = _tripRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LockSeatsBeforeConfirm(int tripId, string seats)
        {
            if (string.IsNullOrEmpty(seats)) return BadRequest();

            var seatIds = seats.Split(',').Select(int.Parse).ToList();

            foreach (var seatId in seatIds)
            {
                var reservation = new SeatReservation
                {
                    TripId = tripId,
                    SeatId = seatId,
                    Status = SeatReservationStatus.Pending,
                    ExpireAt = DateTime.Now.AddMinutes(10)
                };
                await bookingRepository.AddSeatReservationAsync(reservation);
            }

            await bookingRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult Confirm(int tripId, string seats, decimal price)
        {
            if (string.IsNullOrEmpty(seats)) return RedirectToAction("Index", "Trip");

            var seatsIds = seats.Split(',').Select(int.Parse).ToList();
            var bookingVM = new ConfirmBookingVM
            {
                TripId = tripId,
                SeatIds = seatsIds,
                PricePerSeat = price,
            };
            return View(bookingVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(ConfirmBookingVM confirmBooking)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            // --- فحص الأمان النهائي لمنع الحجز المزدوج ---
            var alreadyBooked = await bookingRepository.CheckIfSeatsAlreadyBookedAsync(confirmBooking.TripId, confirmBooking.SeatIds);
            if (alreadyBooked)
            {
                TempData["Error"] = "عذراً، أحد المقاعد التي اخترتها تم حجزه بالفعل من قبل مستخدم آخر.";
                return RedirectToAction("ShowTrip", "Trip", new { id = confirmBooking.TripId });
            }

            var booking = new Booking
            {
                TripId = confirmBooking.TripId,
                UserId = userId,
                TotalPrice = confirmBooking.TotalPrice,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await bookingRepository.addAsync(booking);

            var pendingReservations = await bookingRepository.GetPendingReservationsAsync(confirmBooking.TripId, confirmBooking.SeatIds);

            foreach (var seatId in confirmBooking.SeatIds)
            {
                var res = pendingReservations.FirstOrDefault(p => p.SeatId == seatId);
                if (res != null)
                {
                    res.Status = SeatReservationStatus.Confirmed;
                    res.Booking = booking;
                    res.ExpireAt = null;
                }
                else
                {
                    await bookingRepository.AddSeatReservationAsync(new SeatReservation
                    {
                        TripId = confirmBooking.TripId,
                        SeatId = seatId,
                        Status = SeatReservationStatus.Confirmed,
                        Booking = booking
                    });
                }
            }

            if (await bookingRepository.SaveChangesAsync())
            {
                var trip = tripRepository.GetById(confirmBooking.TripId);
                bool isFull = false;
                if (trip != null)
                {
                    trip.AvailableSeats -= confirmBooking.SeatIds.Count;
                    if (trip.AvailableSeats <= 0)
                    {
                        trip.AvailableSeats = 0;
                        trip.Status = TripStatus.Completed;
                        isFull = true;
                    }
                    tripRepository.Update(trip);
                    tripRepository.Save();
                }

                // مسح القفل المؤقت من الذاكرة (SignalR Hub)
                foreach (var seatId in confirmBooking.SeatIds)
                {
                    BookingHub.ReleaseSeat(confirmBooking.TripId, seatId.ToString());
                }

                // تحديث الـ UI عند كل المستخدمين فوراً
                await hubContext.Clients.All.SendAsync("UpdateSeatStatus", confirmBooking.TripId, confirmBooking.SeatIds, "Booked", isFull);
                await dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");

                return RedirectToAction("MyBookings");
            }

            return View(confirmBooking);
        }

        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            var bookings = await bookingRepository.GetUserBookingsAsync(userId);
            var BookingVM = bookings.Select(b => new BookingDeatailsVM
            {
                BookingId = b.Id,
                Route = $"{b.Trip.BusRoute.OriginCity.Name} -> {b.Trip.BusRoute.DestinationCity.Name}",
                Date = b.Trip.TravelDate.ToShortDateString(),
                Time = b.Trip.DepartureTime.ToString("hh:mm tt"),
                TotalPrice = b.TotalPrice,
                Status = b.Status.ToString(),
                SeatNumbers = b.SeatReservations.Select(sr => sr.SeatId).ToList()
            }).ToList();

            return View(BookingVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int BookingId)
        {
            var booking = await bookingRepository.GetBookingWithDetailsAsync(BookingId);
            if (booking == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.UserId.ToString() != userId) return Forbid();

            booking.Status = BookingStatus.Cancelled;
            var trip = booking.Trip;
            var seatIds = booking.SeatReservations.Select(sr => sr.SeatId).ToList();

            trip.AvailableSeats += seatIds.Count;
            if (trip.Status == TripStatus.Completed) trip.Status = TripStatus.OpenForBooking;

            foreach (var res in booking.SeatReservations)
            {
                bookingRepository.DeleteReservation(res);
            }

            if (await bookingRepository.SaveChangesAsync())
            {
                await hubContext.Clients.All.SendAsync("UpdateSeatStatus", trip.Id, seatIds, "Available", false);
                await dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
                return Ok();
            }
            return BadRequest();
        }
    }
}