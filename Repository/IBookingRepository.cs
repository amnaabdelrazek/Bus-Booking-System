using Bus_Booking_System.Models;

namespace Bus_Booking_System.Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        List<Booking> GetAllWithDetails();
    }
}