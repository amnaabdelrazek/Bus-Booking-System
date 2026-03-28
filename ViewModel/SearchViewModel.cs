namespace Bus_Booking_System.ViewModel
{
	public class SearchViewModel
	{
		public int DepartureCityId { get; set; }
		public int ArrivalCityId { get; set; }
		public DateTime TravelDate { get; set; }

		public List<City>? Cities { get; set; }
	}
}
