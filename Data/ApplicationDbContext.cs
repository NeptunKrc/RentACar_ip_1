using RentACar_ip.Models;
using Microsoft.EntityFrameworkCore;

namespace RentACar_ip.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // USER & ROLE
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        // LOOKUP TABLES
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }

        // MAIN TABLES
        public DbSet<Car> Cars { get; set; }
        public DbSet<FleetCar> FleetCars { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }

        // CUSTOMER TABLE
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique serial no
            modelBuilder.Entity<FleetCar>()
                .HasIndex(x => x.CarSerialId)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
