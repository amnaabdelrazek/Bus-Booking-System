namespace Bus_Booking_System.ViewModel
{
    public class TripDetailsVM
    {
        public int tripId { get; set; }
        public string? FromCity {  get; set; }
        public string? ToCity { get; set; }

        public DateTime DepartureTime {  get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public int AvailableSeats {  get; set; }

        public string? BusType { get; set; }

        public decimal Price { get; set; } 
    }
}
