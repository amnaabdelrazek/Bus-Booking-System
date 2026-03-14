using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Booking_System.Models
{
    public enum SeatReservationStatus
    {
        Locked,
        Confirmed,
        Released
    }

    public class SeatReservation : BaseEntity
    {
        public int BookingId { get; set; }

        public int SeatId { get; set; }

        public SeatReservationStatus Status { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }


        [Display(Name = "Expire At")]
        public DateTime ExpireAt { get; set; }

        public Schedule? Schedule { get; set; }
        public Booking? Booking { get; set; }
        public Seat? Seat { get; set; }
    }
}
