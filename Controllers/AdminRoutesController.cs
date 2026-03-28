using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Bus_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminRoutesController : Controller
    {
        private readonly IRouteRepository _routeRepo;
        private readonly ICityRepository _cityRepo;

        public AdminRoutesController(IRouteRepository routeRepo, ICityRepository cityRepo)
        {
            _routeRepo = routeRepo;
            _cityRepo = cityRepo;
        }

        public IActionResult Index()
        {
            
            var routes = _routeRepo.GetAllWithCities();
            return View(routes);
        }

        public IActionResult Create()
        {
           
            var viewModel = new RouteVM
            {
                CitiesList = new SelectList(_cityRepo.GetAll(), "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RouteVM model)
        {
            
            if (model.OriginCityId == model.DestinationCityId)
            {
                ModelState.AddModelError("DestinationCityId", "Origin and Destination cannot be the same city.");
            }

            if (ModelState.IsValid)
            {
                
                var newRoute = new BusRoute
                {
                    OriginCityId = model.OriginCityId,
                    DestinationCityId = model.DestinationCityId,
                    Distance = model.Distance,
                    TimeNeeded = model.TimeNeeded,
                    Price = model.Price
                };

                _routeRepo.Add(newRoute);
                _routeRepo.Save();
                TempData["SuccessMsg"] = "Route added successfully!";
                return RedirectToAction(nameof(Index));
            }

            
            model.CitiesList = new SelectList(_cityRepo.GetAll(), "Id", "Name");
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var route = _routeRepo.GetById(id);
            if (route == null) return NotFound();

           
            var viewModel = new RouteVM
            {
                Id = route.Id,
                OriginCityId = route.OriginCityId,
                DestinationCityId = route.DestinationCityId,
                Distance = route.Distance,
                TimeNeeded = route.TimeNeeded,
                Price = route.Price,
                CitiesList = new SelectList(_cityRepo.GetAll(), "Id", "Name")
            };

            return View(viewModel);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RouteVM model)
        {
            if (id != model.Id) return NotFound();

            
            if (model.OriginCityId == model.DestinationCityId)
            {
                ModelState.AddModelError("DestinationCityId", "Origin and Destination cannot be the same city.");
            }

            if (ModelState.IsValid)
            {
                var existingRoute = _routeRepo.GetById(id);
                if (existingRoute == null) return NotFound();

                
                existingRoute.OriginCityId = model.OriginCityId;
                existingRoute.DestinationCityId = model.DestinationCityId;
                existingRoute.Distance = model.Distance;
                existingRoute.TimeNeeded = model.TimeNeeded;
                existingRoute.Price = model.Price;

                _routeRepo.Update(existingRoute);
                _routeRepo.Save();
                TempData["SuccessMsg"] = "Route updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            model.CitiesList = new SelectList(_cityRepo.GetAll(), "Id", "Name");
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _routeRepo.Delete(id);
            _routeRepo.Save();
            TempData["SuccessMsg"] = "Route deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}