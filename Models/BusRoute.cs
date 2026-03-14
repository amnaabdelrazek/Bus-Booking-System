using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bus_Booking_System.Models
{
    public class BusRoute : BaseEntity
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal Distance { get; set; }

        [Display(Name = "Time Needed")]
        public TimeSpan TimeNeeded { get; set; }

        public decimal Price { get; set; }

        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
