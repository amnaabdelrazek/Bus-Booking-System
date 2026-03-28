using System.ComponentModel.DataAnnotations;

namespace Bus_Booking_System.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name can only contain letters and spaces")]
        [MinLength(6, ErrorMessage = "Full Name must be at least 6 characters")]
        public string Fullname { get; set; }
        [Required]
        [Display(Name = "User Name")]
        [MaxLength(50, ErrorMessage = "User Name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "User Name can only contain letters and numbers")]
        [MinLength(4, ErrorMessage = "User Name must be at least 4 characters")]
        public string UserName { get; set; }
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$",
            ErrorMessage = "Please enter a valid Egyptian phone number")]
        public string Phone { get; set; }
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [MinLength(6)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}