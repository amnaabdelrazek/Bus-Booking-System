using Bus_Booking_System.Data;
using Bus_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_System.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyAppContext _context;
        public BookingRepository(MyAppContext context) { _context = context; }

        public List<Booking> GetAll() => _context.Bookings.Where(b => !b.IsDeleted).ToList();


        public List<Booking> GetAllWithDetails()
        {
            return _context.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                .Include(b => b.User)
                .Include(b => b.SeatReservations)
                    .ThenInclude(sr => sr.Seat) 
                .Where(b => !b.IsDeleted)
                .ToList();
        }

        public Booking GetById(int id) => _context.Bookings.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
        public void Add(Booking entity) => _context.Bookings.Add(entity);
        public void Update(Booking entity) => _context.Bookings.Update(entity);

        public void Delete(int id)
        {
            var booking = GetById(id);
            if (booking != null)
            {
                booking.IsDeleted = true;
                _context.Bookings.Update(booking);
            }
        }
        public void Save() => _context.SaveChanges();
    }
}