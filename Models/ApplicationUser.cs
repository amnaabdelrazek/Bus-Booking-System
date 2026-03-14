using Microsoft.AspNetCore.Identity;

namespace Bus_Booking_System.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
