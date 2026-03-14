using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Booking_System.Models
{
    public enum BookingStatus
    {
        Pending,
        Cancelled,
        Confirmed
    }

    public class Booking : BaseEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        public int ScheduleId { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        public Schedule? Schedule { get; set; }
        public ApplicationUser? User { get; set; }
        public List<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();
    }
}
