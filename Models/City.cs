using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Booking_System.Models
{
    public class City : BaseEntity
    {
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(BusRoute.OriginCity))]
        public List<BusRoute> RoutesAsOrigin { get; set; } = new List<BusRoute>();

        [InverseProperty(nameof(BusRoute.DestinationCity))]
        public List<BusRoute> RoutesAsDestination { get; set; } = new List<BusRoute>();
    }
}
