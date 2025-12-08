namespace RentACar_ip.Models.ViewModels
{
    public class CarViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public Car NewCar { get; set; } = new Car();

        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<CarType> CarTypes { get; set; }
        public IEnumerable<TransmissionType> Transmissions { get; set; }
        public IEnumerable<FuelType> FuelTypes { get; set; }
    }
}
