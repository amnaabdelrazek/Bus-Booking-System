using Bus_Booking_System.Data;
using Bus_Booking_System.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bus_Booking_System.Repository
{
    public class RouteRepository : IRouteRepository
    {
        private readonly MyAppContext _context;
        public RouteRepository(MyAppContext context) { _context = context; }

        public List<BusRoute> GetAll() => _context.BusRoutes.Where(r => !r.IsDeleted).ToList();

        
        public List<BusRoute> GetAllWithCities()
        {
            return _context.BusRoutes
                .Include(r => r.OriginCity)
                .Include(r => r.DestinationCity)
                .Where(r => !r.IsDeleted)
                .ToList();
        }

        public BusRoute? GetById(int id) => _context.BusRoutes.FirstOrDefault(r => r.Id == id && !r.IsDeleted);

        public void Add(BusRoute entity) => _context.BusRoutes.Add(entity);

        public void Update(BusRoute entity) => _context.BusRoutes.Update(entity);

        public void Delete(int id)
        {
            var route = GetById(id);
            if (route != null)
            {
                route.IsDeleted = true; 
                _context.BusRoutes.Update(route);
            }
        }

        public void Save() => _context.SaveChanges();

        public IQueryable<BusRoute> GetAllQueryable()
        {
            return _context.BusRoutes
               .Include(t => t.Trips)
               .AsNoTracking();
        }
    }
}