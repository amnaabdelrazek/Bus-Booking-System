namespace Bus_Booking_System.ViewModel
{
    public class ConfirmBookingVM
    {
        public int TripId {  get; set; }

        public List<int> SeatIds { get; set; }
        public decimal PricePerSeat { get; set; }
        public decimal TotalPrice => SeatIds.Count * PricePerSeat;
    }
}
