namespace RentACar_ip.Models
{
    public class SettingsViewModel
    {
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<CarType> CarTypes { get; set; }
        public IEnumerable<FuelType> FuelTypes { get; set; }
        public IEnumerable<TransmissionType> TransmissionTypes { get; set; }
        public IEnumerable<Role> Roles { get; set; }

    }
}
