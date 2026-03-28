using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Bus_Booking_System.Models;

namespace Bus_Booking_System.ViewModel
{
    public class TripVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a route")]
        [Display(Name = "Travel Route")]
        public int BusRouteId { get; set; }

        [Required(ErrorMessage = "Please select a bus")]
        [Display(Name = "Bus")]
        public int BusId { get; set; }

        [Required]
        [Display(Name = "Travel Date")]
        public DateTime TravelDate { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Trip Status")]
        public TripStatus Status { get; set; }

        
        public IEnumerable<SelectListItem>? RoutesList { get; set; }
        public IEnumerable<SelectListItem>? BusesList { get; set; }
    }
}