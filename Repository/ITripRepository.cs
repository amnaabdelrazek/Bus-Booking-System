namespace Bus_Booking_System.Repository
{
    public interface ITripRepository: IGenericRepository<Trip>
    {
        List<Trip> GetTripsWithDetails();
        Trip GetTripWithBooking(int id);
    }
}
