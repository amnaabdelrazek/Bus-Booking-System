using Bus_Booking_System.Hubs;
using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTripsController : Controller
    {
        private readonly ITripRepository _tripRepo;
        private readonly IRouteRepository _routeRepo;
        private readonly IBusRepository _busRepo;

        private readonly IHubContext<DashboardHub> _hubContext;

        public AdminTripsController(ITripRepository tripRepo, IRouteRepository routeRepo, IBusRepository busRepo, IHubContext<DashboardHub> hubContext)
        {
            _tripRepo = tripRepo;
            _routeRepo = routeRepo;
            _busRepo = busRepo;
            _hubContext = hubContext; 
        }

        public IActionResult Index()
        {
            
            var trips = _tripRepo.GetTripsWithDetails().Where(t => !t.IsDeleted).ToList();
            return View(trips);
        }

        public IActionResult Create()
        {
            var viewModel = new TripVM
            {
                
                TravelDate = DateTime.Today,
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(2), 

                RoutesList = _routeRepo.GetAllWithCities().Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.OriginCity.Name} ➔ {r.DestinationCity.Name} (${r.Price})"
                }),
                BusesList = new SelectList(_busRepo.GetAll(), "Id", "BusNum")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(TripVM model)
        {
            if (ModelState.IsValid)
            {
               
                var selectedBus = _busRepo.GetById(model.BusId);

                var newTrip = new Trip
                {
                    BusRouteId = model.BusRouteId,
                    BusId = model.BusId,
                    TravelDate = model.TravelDate,
                    DepartureTime = model.DepartureTime,
                    ArrivalTime = model.ArrivalTime,
                    Status = model.Status,
                    
                    AvailableSeats = selectedBus?.TotalSeats ?? 0
                };

                _tripRepo.Add(newTrip);
                _tripRepo.Save();
                await _hubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
                TempData["SuccessMsg"] = "Trip scheduled successfully!";
                return RedirectToAction(nameof(Index));
            }

           
            model.RoutesList = _routeRepo.GetAllWithCities().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = $"{r.OriginCity.Name} ➔ {r.DestinationCity.Name}" });
            model.BusesList = new SelectList(_busRepo.GetAll(), "Id", "BusNum");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _tripRepo.Delete(id);
            _tripRepo.Save();
            TempData["SuccessMsg"] = "Trip deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var trip = _tripRepo.GetById(id);
            if (trip == null) return NotFound();

            var viewModel = new TripVM
            {
                Id = trip.Id,
                BusRouteId = trip.BusRouteId,
                BusId = trip.BusId,
                TravelDate = trip.TravelDate,
                DepartureTime = trip.DepartureTime,
                ArrivalTime = trip.ArrivalTime,
                Status = trip.Status,

                
                RoutesList = _routeRepo.GetAllWithCities().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = $"{r.OriginCity.Name} ➔ {r.DestinationCity.Name}" }),
                BusesList = new SelectList(_busRepo.GetAll(), "Id", "BusNum")
            };

            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TripVM model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingTrip = _tripRepo.GetById(id);
                if (existingTrip == null) return NotFound();

                
                existingTrip.BusRouteId = model.BusRouteId;
                existingTrip.BusId = model.BusId;
                existingTrip.TravelDate = model.TravelDate;
                existingTrip.DepartureTime = model.DepartureTime;
                existingTrip.ArrivalTime = model.ArrivalTime;
                existingTrip.Status = model.Status;

                _tripRepo.Update(existingTrip);
                _tripRepo.Save();
                TempData["SuccessMsg"] = "Trip updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            
            model.RoutesList = _routeRepo.GetAllWithCities().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = $"{r.OriginCity.Name} ➔ {r.DestinationCity.Name}" });
            model.BusesList = new SelectList(_busRepo.GetAll(), "Id", "BusNum");
            return View(model);
        }
    }
}