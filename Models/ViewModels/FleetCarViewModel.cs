namespace RentACar_ip.Models.ViewModels
{
    public class FleetCarViewModel
    {
        public IEnumerable<FleetCar> FleetCars { get; set; }
        public FleetCar NewFleetCar { get; set; } = new FleetCar();

        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
