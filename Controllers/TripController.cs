using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    public class TripController : Controller
    {
        ITripRepository tripRepository;
        public TripController(ITripRepository _tripRepository)
        {
            tripRepository = _tripRepository;
        }
        [HttpGet]
        public IActionResult Index(DateTime? travelDate, decimal? maxPrice, string? status)
        {
            var trips = tripRepository.GetTripsWithDetails();

            decimal highestPrice = trips.Any()
                                    ? trips.Max(t => t.BusRoute?.Price ?? 0m)
                                    : 500m;

            decimal lowestPrice = trips.Any()
                                    ? trips.Min(t => t.BusRoute?.Price ?? 0m)
                                    : 0m;
            var filteredQuery = trips.AsQueryable();
                if (travelDate.HasValue)
                {
                    filteredQuery = filteredQuery.Where(t=>t.TravelDate.Date == travelDate.Value);
                }

                if (maxPrice.HasValue)
                {
                    filteredQuery = filteredQuery.Where(t => t.BusRoute != null && t.BusRoute.Price <= maxPrice.Value);
                }

                if(!string.IsNullOrEmpty(status))
                {
                filteredQuery = filteredQuery.Where(t => t.Status.ToString() == status);
                }
            var TripsModel = new TripIndexVM
                {
                FilterDate = travelDate,
                FilterMaxPrice = maxPrice ?? highestPrice,
                MaxPrice = highestPrice,
                SelectedStatus = status,
                MinPrice = lowestPrice,
                Trips = filteredQuery.Select(t => new TripDetailsVM
                {
                    tripId = t.Id,
                    FromCity = t.BusRoute.OriginCity.Name,
                    ToCity = t.BusRoute.DestinationCity.Name,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,
                    Date = t.TravelDate,
                    Status = t.Status.ToString(),
                    AvailableSeats = t.AvailableSeats,
                    BusType = t.Bus.Type,
                    Price = t.BusRoute.Price
                }).ToList()
            };
            return View(TripsModel);
           
        }

        [HttpGet]
        public IActionResult ShowTrip(int id)
        {
            var trip = tripRepository.GetTripWithBooking(id);
            if(trip != null)
            {
                var TripVM = new TripSeatBookingVM
                {
                    TripId = id,
                    FromCity = trip.BusRoute.OriginCity.Name,
                    ToCity = trip.BusRoute.DestinationCity.Name,
                    Price = trip.BusRoute.Price,
                    Date = trip.TravelDate,
                    Time = trip.DepartureTime,
                    TotalSeats = trip.Bus.TotalSeats,
                    BookedSeats = trip.Bookings.SelectMany(b => b.SeatReservations)
                                               .Select(sr => sr.SeatId.ToString())
                                               .ToList()
                };
                return View(TripVM);
            }
            else
                return NotFound();
        }

        [HttpGet]
        public IActionResult AddTrip()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTrip(Trip trip)
        {
            if(ModelState.IsValid)
            {
                tripRepository.Add(trip);
                tripRepository.Save();
                return RedirectToAction("Index");
            }
            return View(trip);
        }

        [HttpGet]
        public IActionResult UpdateTrip(int id)
        {
            var trip = tripRepository.GetById(id);
            if(trip == null)
                return NotFound();
            return View(trip);
        }

        [HttpPost]
        public IActionResult UpdateTrip(Trip trip)
        {
            if(ModelState.IsValid)
            {
                tripRepository.Update(trip);
                tripRepository.Save();
                return RedirectToAction("Index");
            }
            return View(trip);
        }

        [HttpPost]
        public IActionResult DeleteTrip(int id)
        {
            var trip = tripRepository.GetById(id);
            if (trip == null) 
                return NotFound();
            tripRepository.Delete(id);
            tripRepository.Save();

            return RedirectToAction("Index");
        }
    }
}
