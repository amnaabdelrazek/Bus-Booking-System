namespace Bus_Booking_System.Repository
{
    public interface IBookingRepository
    {
        Task addAsync(Booking booking);

        Task AddSeatReservationAsync(SeatReservation reservation);
        void Update(Booking booking);

        Task<Booking> GetByIdAsync(int id);

        void DeleteReservation(SeatReservation seatReservation);

        Task<Booking> GetBookingWithDetailsAsync (int id);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);

        Task<bool> SaveChangesAsync();
    }
}
