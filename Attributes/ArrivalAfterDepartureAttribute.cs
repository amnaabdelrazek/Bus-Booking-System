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
            if (value is not Trip trip)
            {
                return ValidationResult.Success;
            }

            if (trip.ArrivalTime <= trip.DepartureTime)
            {
                return new ValidationResult(
                    ErrorMessage,
                    new[] { nameof(Trip.ArrivalTime), nameof(Trip.DepartureTime) }
                );
            }

            return ValidationResult.Success;
        }
    }
}

