using System;
using System.ComponentModel.DataAnnotations;
using Bus_Booking_System.Models;

namespace Bus_Booking_System.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ArrivalAfterDepartureAttribute : ValidationAttribute
    {
        public ArrivalAfterDepartureAttribute()
        {
            ErrorMessage = "Arrival time must be after departure time.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not Schedule schedule)
            {
                return ValidationResult.Success;
            }

            if (schedule.ArrivalTime <= schedule.DepartureTime)
            {
                return new ValidationResult(
                    ErrorMessage,
                    new[] { nameof(Schedule.ArrivalTime), nameof(Schedule.DepartureTime) }
                );
            }

            return ValidationResult.Success;
        }
    }
}
