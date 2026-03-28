using Bus_Booking_System.Models;

namespace Bus_Booking_System.Repository
{
    public interface IRouteRepository : IGenericRepository<BusRoute>
    {
        List<BusRoute> GetAllWithCities();
    }
}