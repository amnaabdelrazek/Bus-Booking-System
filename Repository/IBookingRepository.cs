using Bus_Booking_System.Models;

namespace Bus_Booking_System.Repository
{
   // public interface IBookingRepository
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task addAsync(Booking booking);

        Task AddSeatReservationAsync(SeatReservation reservation);
        void DeleteReservation(SeatReservation seatReservation);
        void Update(Booking booking);

        Task<Booking> GetByIdAsync(int id);
        Task<Booking> GetBookingWithDetailsAsync (int id);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);

        Task<bool> SaveChangesAsync();
        List<Booking> GetAllWithDetails();
    }
}