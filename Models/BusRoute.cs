using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Booking_System.Models
{
    public class BusRoute : BaseEntity
    {
        [ForeignKey(nameof(OriginCity))]
        public int OriginCityId { get; set; }

        public City OriginCity { get; set; } = null!;

        [ForeignKey(nameof(DestinationCity))]
        public int DestinationCityId { get; set; }

        public City DestinationCity { get; set; } = null!;

        public decimal Distance { get; set; }

        [Display(Name = "Time Needed")]
        public TimeSpan TimeNeeded { get; set; }

        public decimal Price { get; set; }

        public List<Trip> Trips { get; set; } = new List<Trip>();
    }
}
