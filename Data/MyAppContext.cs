using Bus_Booking_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_System.Data
{
    public class MyAppContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>

    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
        }

        public DbSet<Bus> Buses { get; set; } = null!;
        public DbSet<BusRoute> BusRoutes { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<SeatReservation> SeatReservations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BusRoute>()
                .Property(r => r.Distance)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BusRoute>()
                .Property(r => r.Price)
                .HasPrecision(10, 2);

			
			modelBuilder.Entity<BusRoute>()
				.HasOne(br => br.OriginCity)
				.WithMany(c => c.RoutesAsOrigin)
				.HasForeignKey(br => br.OriginCityId)
				.OnDelete(DeleteBehavior.Restrict);

			
			modelBuilder.Entity<BusRoute>()
				.HasOne(br => br.DestinationCity)
				.WithMany(c => c.RoutesAsDestination)
				.HasForeignKey(br => br.DestinationCityId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<SeatReservation>()
				.HasOne(sr => sr.Seat)
				.WithMany()
				.HasForeignKey(sr => sr.SeatId)
				.OnDelete(DeleteBehavior.NoAction); 

			modelBuilder.Entity<SeatReservation>()
				.HasOne(sr => sr.Trip)
				.WithMany()
				.HasForeignKey(sr => sr.TripId)
				.OnDelete(DeleteBehavior.NoAction); 

			modelBuilder.Entity<SeatReservation>()
				.HasOne(sr => sr.Booking)
				.WithMany()
				.HasForeignKey(sr => sr.BookingId)
				.OnDelete(DeleteBehavior.Cascade);
		}

    }
}

