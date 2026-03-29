using Bus_Booking_System.Data;
using Bus_Booking_System.Models;
using System;

namespace Bus_Booking_System.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly MyAppContext _context;
        public CityRepository(MyAppContext context) { _context = context; }

        public List<City> GetAll() => _context.Cities.Where(c => !c.IsDeleted).ToList();

        public City? GetById(int id) => _context.Cities.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

        public void Add(City entity) => _context.Cities.Add(entity);

        public void Update(City entity) => _context.Cities.Update(entity);

        public void Delete(int id)
        {
            var city = GetById(id);
            if (city != null)
            {
                city.IsDeleted = true; 
                _context.Cities.Update(city);
            }
        }

        public void Save() => _context.SaveChanges();

        public IQueryable<City> GetAllQueryable()
        {
            return _context.Cities
                   .Include(t => t.RoutesAsOrigin)
                   .Include(t => t.RoutesAsDestination)
                   .AsNoTracking();
        }
    }
}