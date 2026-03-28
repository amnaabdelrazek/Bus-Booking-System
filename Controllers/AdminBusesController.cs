using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBusesController : Controller
    {
        private readonly IBusRepository _busRepo;

        public AdminBusesController(IBusRepository busRepo)
        {
            _busRepo = busRepo;
        }

        public IActionResult Index()
        {
            return View(_busRepo.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bus bus)
        {
            if (ModelState.IsValid)
            {
                _busRepo.Add(bus);
                _busRepo.Save();
                TempData["SuccessMsg"] = "Bus added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        public IActionResult Edit(int id)
        {
            var bus = _busRepo.GetById(id);
            if (bus == null) return NotFound();
            return View(bus);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Bus bus)
        {
            if (id != bus.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _busRepo.Update(bus);
                _busRepo.Save();
                TempData["SuccessMsg"] = "Bus updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _busRepo.Delete(id);
            _busRepo.Save();
            TempData["SuccessMsg"] = "Bus deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}