

namespace Bus_Booking_System.ViewModel
{
    public class TripIndexVM
    {
        public List<TripDetailsVM> Trips {  get; set; } = new List<TripDetailsVM>();
        
        public DateTime? FilterDate { get; set; }
        public decimal? FilterMaxPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinPrice { get; set; }
        public string? SelectedStatus { get; set; }
    }
}
