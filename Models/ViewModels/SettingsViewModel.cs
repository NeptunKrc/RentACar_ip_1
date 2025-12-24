using Microsoft.AspNetCore.Identity;

namespace RentACar_ip.Models.ViewModels
{
    public class SettingsViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<CarType> CarTypes { get; set; }
        public List<FuelType> FuelTypes { get; set; }
        public List<TransmissionType> TransmissionTypes { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}
