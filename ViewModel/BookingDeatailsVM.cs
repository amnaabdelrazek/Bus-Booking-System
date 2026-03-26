namespace Bus_Booking_System.ViewModel
{
    public class BookingDeatailsVM
    {
        public int BookingId { get; set; }
        public string Route { get; set; } 
        public string Date { get; set; }
        public string Time { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public List<int> SeatNumbers { get; set; }
    }
}
