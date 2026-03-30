
namespace Bus_Booking_System.Repository
{
    public class TripRepository : ITripRepository
    {
        MyAppContext appContext;
        public TripRepository(MyAppContext _myAppContext)
        {
            appContext = _myAppContext;
        }

        public List<Trip> GetAll()
        {
            return appContext.Trips
                .Where(t => !t.IsDeleted)
                .ToList();
        }

        public Trip? GetById(int id)
        {
            return appContext.Trips
                   .Where(t => t.Id == id && !t.IsDeleted)
                   .FirstOrDefault();
        }
        public void Add(Trip entity)
        {
            entity.IsDeleted = false;
            appContext.Trips.Add(entity);
        }

        public void Delete(int id)
        {
            var trip = GetById(id);
            if (trip != null)
            {
                trip.IsDeleted = true;
                appContext.Trips.Update(trip);
            }
        }

        public void Update(Trip entity)
        {
            appContext.Trips.Update(entity);
        }

        public void Save()
        {
            appContext.SaveChanges();
        }

        public List<Trip> GetTripsWithDetails()
        {
            return appContext.Trips
                   .Include(t => t.Bus)
                   .Include(t => t.BusRoute)
                       .ThenInclude(r => r.OriginCity)
                   .Include(t => t.BusRoute)
                       .ThenInclude(r => r.DestinationCity)
                    .Include(t => t.Bookings)
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.Id)
                   .ToList();
        }


        public Trip GetTripWithBooking(int id)
        {
            return appContext.Trips
                   .Include(t => t.Bus)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                   .Include(t => t.Bookings)
                        .ThenInclude(b => b.SeatReservations)
                   .FirstOrDefault(t => t.Id == id && !t.IsDeleted);
        }

        public List<Trip> SearchTrips(int departureCityId, int arrivalCityId, DateTime TravelDate)
        {
            var trips = appContext.Trips
                   .Include(t => t.Bus)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                   .Where(t => t.BusRoute.OriginCityId == departureCityId &&
                               t.BusRoute.DestinationCityId == arrivalCityId &&
                               t.TravelDate.Date == TravelDate.Date &&
                               !t.IsDeleted)
                   .ToList();
            return trips;
        }

        public IQueryable<Trip> GetAllQueryable()
        {
            return appContext.Trips
                   .Include(t => t.Bus)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.OriginCity)
                   .Include(t => t.BusRoute)
                        .ThenInclude(r => r.DestinationCity)
                    .Where(t => !t.IsDeleted)
                   .AsNoTracking();
        }
    }
}
