using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCitiesController : Controller
    {
        private readonly ICityRepository _cityRepo;

        public AdminCitiesController(ICityRepository cityRepo)
        {
            _cityRepo = cityRepo;
        }
        
        
        public IActionResult Index()
        {
            return View(_cityRepo.GetAll());
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepo.Add(city);
                _cityRepo.Save();
                TempData["SuccessMsg"] = "City added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        
        public IActionResult Edit(int id)
        {
            var city = _cityRepo.GetById(id);
            if (city == null) return NotFound();
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, City city)
        {
            if (id != city.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _cityRepo.Update(city);
                _cityRepo.Save();
                TempData["SuccessMsg"] = "City updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _cityRepo.Delete(id);
            _cityRepo.Save();
            TempData["SuccessMsg"] = "City deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}