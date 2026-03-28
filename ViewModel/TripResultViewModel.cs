namespace Bus_Booking_System.ViewModel
{
	public class TripResultViewModel
	{
		public int TripId { get; set; }

		public string From { get; set; }
		public string To { get; set; }

		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }

		public decimal Price { get; set; }

		public int AvailableSeats { get; set; }

		public string BusNumber { get; set; }
	}
}
