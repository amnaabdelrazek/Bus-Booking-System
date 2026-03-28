namespace Bus_Booking_System.ViewModel
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Phone]
        public string Phone { get; set; } = "";

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
    }
}
