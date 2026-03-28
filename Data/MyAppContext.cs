using Bus_Booking_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_System.Data
{
    public class MyAppContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

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

            // =========================
            // Decimal Precision
            // =========================
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BusRoute>()
                .Property(r => r.Distance)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BusRoute>()
                .Property(r => r.Price)
                .HasPrecision(10, 2);

            // =========================
            // BusRoute Relationships
            // =========================
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

            // =========================
            // Seed Cities
            // =========================
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "City A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
                new City { Id = 2, Name = "City B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // =========================
            // Seed Buses
            // =========================
            modelBuilder.Entity<Bus>().HasData(
                new Bus { Id = 1, BusNum = "BUS001", TotalSeats = 40, Type = "Luxury", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // =========================
            // Seed BusRoutes
            // =========================
            modelBuilder.Entity<BusRoute>().HasData(
                new BusRoute
                {
                    Id = 1,
                    OriginCityId = 1,
                    DestinationCityId = 2,
                    Distance = 200,
                    Price = 300,
                    TimeNeeded = TimeSpan.FromHours(3),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // =========================
            // Seed Trips
            // =========================
            modelBuilder.Entity<Trip>().HasData(
                new Trip
                {
                    Id = 1,
                    BusRouteId = 1,
                    BusId = 1,
                    DepartureTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    ArrivalTime = new DateTime(2024, 1, 1, 12, 0, 0),
                    TravelDate = new DateTime(2024, 1, 1),
                    Status = TripStatus.OpenForBooking,
                    AvailableSeats = 38,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // =========================
            // Seed Roles
            // =========================
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
            );

            // =========================
            // Seed Users
            // =========================
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    FullName = "Admin",
                    UserName = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEOgFakeHashExample123456789==",
                    SecurityStamp = "1",
                    ConcurrencyStamp = "1",
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new ApplicationUser
                {
                    Id = 2,
                    FullName = "User",
                    UserName = "user@test.com",
                    NormalizedUserName = "USER@TEST.COM",
                    Email = "user@test.com",
                    NormalizedEmail = "USER@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEOgFakeHashExample123456789==",
                    SecurityStamp = "2",
                    ConcurrencyStamp = "2",
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // =========================
            // Seed UserRoles
            // =========================
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }
            );

            // =========================
            // Seed Bookings
            // =========================
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    UserId = 2,
                    TripId = 1,
                    TotalPrice = 300,
                    Status = BookingStatus.Confirmed,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<Seat>().HasData(
    new Seat { Id = 1, BusId = 1, SeatNum = "1A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 2, BusId = 1, SeatNum = "1B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 3, BusId = 1, SeatNum = "1C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 4, BusId = 1, SeatNum = "1D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 5, BusId = 1, SeatNum = "2A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 6, BusId = 1, SeatNum = "2B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 7, BusId = 1, SeatNum = "2C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 8, BusId = 1, SeatNum = "2D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 9, BusId = 1, SeatNum = "3A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 10, BusId = 1, SeatNum = "3B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 11, BusId = 1, SeatNum = "3C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 12, BusId = 1, SeatNum = "3D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 13, BusId = 1, SeatNum = "4A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 14, BusId = 1, SeatNum = "4B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 15, BusId = 1, SeatNum = "4C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 16, BusId = 1, SeatNum = "4D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 17, BusId = 1, SeatNum = "5A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 18, BusId = 1, SeatNum = "5B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 19, BusId = 1, SeatNum = "5C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 20, BusId = 1, SeatNum = "5D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 21, BusId = 1, SeatNum = "6A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 22, BusId = 1, SeatNum = "6B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 23, BusId = 1, SeatNum = "6C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 24, BusId = 1, SeatNum = "6D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 25, BusId = 1, SeatNum = "7A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 26, BusId = 1, SeatNum = "7B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 27, BusId = 1, SeatNum = "7C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 28, BusId = 1, SeatNum = "7D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 29, BusId = 1, SeatNum = "8A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 30, BusId = 1, SeatNum = "8B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 31, BusId = 1, SeatNum = "8C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 32, BusId = 1, SeatNum = "8D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 33, BusId = 1, SeatNum = "9A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 34, BusId = 1, SeatNum = "9B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 35, BusId = 1, SeatNum = "9C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 36, BusId = 1, SeatNum = "9D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },

    new Seat { Id = 37, BusId = 1, SeatNum = "10A", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 38, BusId = 1, SeatNum = "10B", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 39, BusId = 1, SeatNum = "10C", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
    new Seat { Id = 40, BusId = 1, SeatNum = "10D", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) }
);
            // =========================
            // Seed SeatReservations
            // =========================
            modelBuilder.Entity<SeatReservation>().HasData(
                new SeatReservation
                {
                    Id = 1,
                    BookingId = 1,
                    SeatId = 1,
                    TripId = 1,
                    Status = SeatReservationStatus.Confirmed,
                    ExpireAt = new DateTime(2024, 1, 1, 10, 0, 0),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new SeatReservation
                {
                    Id = 2,
                    BookingId = 1,
                    SeatId = 2,
                    TripId = 1,
                    Status = SeatReservationStatus.Released,
                    ExpireAt = new DateTime(2024, 1, 1, 10, 0, 0),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}