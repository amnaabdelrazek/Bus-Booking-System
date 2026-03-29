using Bus_Booking_System.Hubs;
using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Bus_Booking_System.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IHubContext<BookingHub> hubContext;
        private readonly IHubContext<DashboardHub> dashboardHubContext;
        private readonly ITripRepository tripRepository;

        public BookingController(IBookingRepository _bookingRepository, IHubContext<BookingHub> _hubContext, IHubContext<DashboardHub> _dashboardHubContext, ITripRepository _tripRepository)
        {
            bookingRepository = _bookingRepository;
            hubContext = _hubContext;
            dashboardHubContext = _dashboardHubContext;
            tripRepository = _tripRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Confirm(int tripId, string seats, decimal price)
        {
            var seatsIds = seats.Split(',').Select(int.Parse).ToList();
            var bookingVM = new ConfirmBookingVM
            {
                TripId = tripId,
                SeatIds = seatsIds,
                PricePerSeat = price
            };
            return View(bookingVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Confirm(ConfirmBookingVM confirmBooking)
        {
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                 ?? User.FindFirstValue("Id");

            if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out int userId) || userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var booking = new Booking
            {
                TripId = confirmBooking.TripId,
                TotalPrice = confirmBooking.TotalPrice,
                Status = BookingStatus.Confirmed,
                UserId = userId
            };

            await bookingRepository.addAsync(booking);

            foreach (var seatId in confirmBooking.SeatIds)
            {
                var reservation = new SeatReservation
                {
                    Booking = booking,
                    SeatId = seatId,
                    TripId = confirmBooking.TripId,
                    Status = SeatReservationStatus.Confirmed,
                    ExpireAt = DateTime.Now.AddDays(1)
                };
                await bookingRepository.AddSeatReservationAsync(reservation);
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

                await hubContext.Clients.All.SendAsync("UpdateSeatStatus", confirmBooking.TripId, confirmBooking.SeatIds, "Booked", isFull);
                await hubContext.Clients.All.SendAsync("ReceiveBookingsUpdate");
                await dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
                return RedirectToAction("MyBookings");
            }
            return BadRequest();
        }
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            int userId;
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(claimValue, out userId))
            {
                var backupClaim = User.FindFirstValue("Id");
                int.TryParse(backupClaim, out userId);
            }
            if (string.IsNullOrEmpty(claimValue))
            {
                return RedirectToAction("Login", "Account");
            }
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

        public async Task<IActionResult> CancelBooking(int BookingId)
        {
            var booking = await bookingRepository.GetBookingWithDetailsAsync(BookingId);
            if (booking == null)
                return NotFound();
            booking.Status = BookingStatus.Cancelled;

            var trip = booking.Trip;
            var seatIds = booking.SeatReservations.Select(sr => sr.SeatId).ToList();
            trip.AvailableSeats += seatIds.Count();

            if (trip.Status == TripStatus.Completed && trip.AvailableSeats > 0)
            {
                trip.Status = TripStatus.OpenForBooking;
            }

            foreach (var res in booking.SeatReservations)
            {
                bookingRepository.DeleteReservation(res);
            }

            if (await bookingRepository.SaveChangesAsync())
            {
                await hubContext.Clients.All.SendAsync("UpdateSeatStatus", trip.Id, seatIds, "Available", false);
                await hubContext.Clients.All.SendAsync("BookingCancelled", BookingId);
                await hubContext.Clients.All.SendAsync("ReceiveBookingsUpdate");
                await dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
                return Ok();
            }
            return BadRequest();
        }
    }
}
