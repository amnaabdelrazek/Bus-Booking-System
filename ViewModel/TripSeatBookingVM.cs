namespace Bus_Booking_System.ViewModel
{
    public class TripSeatBookingVM
    {
        public int TripId { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public List<string>? BookedSeats { get; set; }

    }
}
