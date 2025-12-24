using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar_ip.Models;

namespace RentACar_ip.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Identity kendi Users ve Roles tablolarını oluşturduğu için bunlar artık gerekmez:
        // public DbSet<User> Users { get; set; }
        // public DbSet<Role> Roles { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FleetCar> FleetCars { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity için SART

            modelBuilder.Entity<FleetCar>()
                .HasIndex(x => x.CarSerialId)
                .IsUnique();
        }
    }
}
