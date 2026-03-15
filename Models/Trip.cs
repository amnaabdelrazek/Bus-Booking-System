using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bus_Booking_System.Attributes;

namespace Bus_Booking_System.Models
{
    public enum TripStatus
    {
        OpenForBooking,
        Cancelled,
        Completed
    }

    [ArrivalAfterDeparture]
    public class Trip : BaseEntity
    {
        public int BusRouteId { get; set; }

        public int BusId { get; set; }

        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Status")]
        public TripStatus Status { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        public BusRoute? BusRoute { get; set; }
        public Bus? Bus { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

