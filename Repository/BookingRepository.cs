using Bus_Booking_System.Data;
using Bus_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_System.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MyAppContext appContext;

        public BookingRepository(MyAppContext myAppContext)
        {
            appContext = myAppContext;
        }

        public List<Booking> GetAll() => appContext.Bookings.Where(b => !b.IsDeleted).ToList();
        public async Task addAsync(Booking booking)
        {
            await appContext.Bookings.AddAsync(booking);
        }

        public async Task AddSeatReservationAsync(SeatReservation reservation)
        {
            await appContext.SeatReservations.AddAsync(reservation);
        }

        public void DeleteReservation(SeatReservation seatReservation)
        {
            appContext.SeatReservations.Remove(seatReservation);
        }

        public async Task<Booking> GetBookingWithDetailsAsync(int id)
        {
            return await appContext.Bookings
                .Include(b => b.Trip)
                .Include(b => b.SeatReservations)
                .FirstOrDefaultAsync(b => b.Id == id);
        }


        public async Task<Booking> GetByIdAsync(int id)
        {
            return await appContext.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                .Include(b => b.SeatReservations)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await appContext.Bookings
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
        public List<Booking> GetAllWithDetails()
        {
            return appContext.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                .Include(b => b.User)
                .Include(b => b.SeatReservations)
                .OrderByDescending(b => b.Id)
                .Where(b => !b.IsDeleted)
                .ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await appContext.SaveChangesAsync() > 0);
        }
        public Booking GetById(int id) => appContext.Bookings.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
        public void Add(Booking entity) => appContext.Bookings.Add(entity);
        public void Update(Booking entity) => appContext.Bookings.Update(entity);
        public void Delete(int id)
        {
            var booking = GetById(id);
            if (booking != null)
            {
                //appContext.Bookings.Update(booking);
                booking.IsDeleted = true;
                appContext.Bookings.Update(booking);
            }
        }
        public void Save() => appContext.SaveChanges();

        public IQueryable<Booking> GetAllQueryable()
        {
            return appContext.Bookings
                   .Include(t => t.SeatReservations)
                   .AsNoTracking();
        }
    }
}