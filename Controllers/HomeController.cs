using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bus_Booking_System.Controllers
{
	public class HomeController : Controller
	{
		private readonly ITripRepository tripRepository;
        private readonly ICityRepository cityRepository;
        //private readonly MyAppContext context;

		public HomeController(ITripRepository _tripRepository, MyAppContext _context, ICityRepository _cityRepository)
		{
			tripRepository = _tripRepository;
			//context = _context;
			cityRepository = _cityRepository;
		}

		public IActionResult Index()
		{
			var vm = new SearchViewModel
			{
                Cities = cityRepository.GetAll()
            };

			return View("Index",vm);
		}

		
		public IActionResult Search(SearchViewModel model)
		{
			if (!ModelState.IsValid)
			{
                model.Cities = cityRepository.GetAll();
                return View("Index", model);
			}

			var trips = tripRepository.SearchTrips(
				model.DepartureCityId,
				model.ArrivalCityId,
				model.TravelDate
			);

		
			var result = trips.Select(t => new TripResultViewModel
			{
				TripId = t.Id,
				From = t.BusRoute.OriginCity.Name,
				To = t.BusRoute.DestinationCity.Name,
				DepartureTime = t.DepartureTime,
				ArrivalTime = t.ArrivalTime,
				Price = t.BusRoute.Price,
				AvailableSeats = t.AvailableSeats,
				BusNumber = t.Bus.BusNum
			}).ToList();

            return RedirectToAction("Index", "Trip", new
            {
                travelDate = model.TravelDate,
                departureCityId = model.DepartureCityId,
                arrivalCityId = model.ArrivalCityId
            });
            //return View("Search", result);
		}
	}
}
