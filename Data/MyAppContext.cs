using Bus_Booking_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_System.Data
{
    public class MyAppContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
        }

        // =========================
        // DbSets
        // =========================
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
            // Precision
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
            // Cities
            // =========================
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Cairo", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) },
                new City { Id = 2, Name = "Alexandria", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // =========================
            // Buses
            // =========================
            modelBuilder.Entity<Bus>().HasData(
                new Bus { Id = 1, BusNum = "BUS-001", TotalSeats = 40, Type = "VIP", IsDeleted = false, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // =========================
            // Seats
            // =========================
            var seats = new List<Seat>();
            for (int i = 1; i <= 40; i++)
            {
                seats.Add(new Seat
                {
                    Id = i,
                    BusId = 1,
                    SeatNum = i.ToString(),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                });
            }
            modelBuilder.Entity<Seat>().HasData(seats);

            // =========================
            // BusRoutes
            // =========================
            modelBuilder.Entity<BusRoute>().HasData(
                new BusRoute
                {
                    Id = 1,
                    OriginCityId = 1,
                    DestinationCityId = 2,
                    Distance = 220,
                    TimeNeeded = new TimeSpan(3, 0, 0),
                    Price = 150,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // =========================
            // Trips
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
            // Roles
            // =========================
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
            );

            // =========================
            // Users
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
            // UserRoles
            // =========================
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }
            );

            // =========================
            // Bookings
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

            // =========================
            // SeatReservations
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