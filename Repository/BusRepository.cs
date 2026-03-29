using Bus_Booking_System.Data;
using Bus_Booking_System.Models;

namespace Bus_Booking_System.Repository
{
    public class BusRepository : IBusRepository
    {
        private readonly MyAppContext _context;
        public BusRepository(MyAppContext context) { _context = context; }

        public List<Bus> GetAll() => _context.Buses.Where(b => !b.IsDeleted).ToList();

        public Bus? GetById(int id) => _context.Buses.FirstOrDefault(b => b.Id == id && !b.IsDeleted);

        public void Add(Bus entity) => _context.Buses.Add(entity);

        public void Update(Bus entity) => _context.Buses.Update(entity);

        public void Delete(int id)
        {
            var bus = GetById(id);
            if (bus != null)
            {
                bus.IsDeleted = true; 
                _context.Buses.Update(bus);
            }
        }

        public void Save() => _context.SaveChanges();

        public IQueryable<Bus> GetAllQueryable()
        {
            throw new NotImplementedException();
        }
    }
}
