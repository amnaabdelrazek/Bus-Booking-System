using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bus_Booking_System.Models
{
    public class Seat : BaseEntity
    {
        public int BusId { get; set; }

        [Display(Name = "Seat Number")]
        public string SeatNum { get; set; }

        public Bus? Bus { get; set; }
        public List<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();
    }
}
