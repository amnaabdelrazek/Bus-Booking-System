namespace Bus_Booking_System.ViewModel
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
    }
}
