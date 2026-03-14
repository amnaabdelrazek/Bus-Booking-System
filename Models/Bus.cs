using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bus_Booking_System.Models
{
    public class Bus : BaseEntity
    {
        [Display(Name = "Bus Number")]
        public string BusNum { get; set; }

        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        public string Type { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
