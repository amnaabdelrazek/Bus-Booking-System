using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bus_Booking_System.ViewModel
{
    public class RouteVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select origin city")]
        [Display(Name = "Origin City")]
        public int OriginCityId { get; set; }

        [Required(ErrorMessage = "Please select destination city")]
        [Display(Name = "Destination City")]
        public int DestinationCityId { get; set; }

        [Required]
        [Range(1, 5000, ErrorMessage = "Distance must be greater than 0")]
        public decimal Distance { get; set; }

        [Required]
        [Display(Name = "Estimated Time (HH:MM)")]
        public TimeSpan TimeNeeded { get; set; }

        [Required]
        [Range(10, 10000, ErrorMessage = "Please enter a valid price")]
        public decimal Price { get; set; }

        
        public IEnumerable<SelectListItem>? CitiesList { get; set; }
    }
}