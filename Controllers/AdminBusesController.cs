using Bus_Booking_System.Hubs;
using Bus_Booking_System.Models;
using Bus_Booking_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bus_Booking_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBusesController : Controller
    {
        private readonly IBusRepository _busRepo;
        private readonly IHubContext<DashboardHub> _dashboardHubContext;

        public AdminBusesController(IBusRepository busRepo, IHubContext<DashboardHub> dashboardHubContext)
        {
            _busRepo = busRepo;
            _dashboardHubContext = dashboardHubContext;
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
        public async Task<IActionResult> Create(Bus bus)
        {
            if (ModelState.IsValid)
            {
                _busRepo.Add(bus);
                _busRepo.Save();
                await _dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
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
        public async Task<IActionResult> Edit(int id, Bus bus)
        {
            if (id != bus.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _busRepo.Update(bus);
                _busRepo.Save();
                await _dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
                TempData["SuccessMsg"] = "Bus updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _busRepo.Delete(id);
            _busRepo.Save();
            await _dashboardHubContext.Clients.All.SendAsync("ReceiveStatsUpdate");
            TempData["SuccessMsg"] = "Bus deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
