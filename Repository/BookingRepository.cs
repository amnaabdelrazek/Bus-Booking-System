using Bus_Booking_System.Data;
using Bus_Booking_System.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bus_Booking_System.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyAppContext _context;
        public BookingRepository(MyAppContext context) { _context = context; }

        public async Task addAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task AddSeatReservationAsync(SeatReservation reservation)
        {
            await _context.SeatReservations.AddAsync(reservation);
        }

        public void DeleteReservation(SeatReservation seatReservation)
        {
            _context.SeatReservations.Remove(seatReservation);
        }

        public async Task<Booking> GetBookingWithDetailsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.SeatReservations)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                .Include(b => b.SeatReservations)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                .Include(b => b.SeatReservations)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }
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

        public IQueryable<Booking> GetAllQueryable()
        {
            return _context.Bookings
                .Include(t => t.SeatReservations)
                .AsNoTracking();
        }
    }
}
