using Microsoft.EntityFrameworkCore;

namespace RentACar_ip.Models
{
    public class Car
    {
        public int Id { get; set; }

        public int BrandId { get; set; }
        public int CarTypeId { get; set; }
        public int TransmissionTypeId { get; set; }
        public int FuelTypeId { get; set; }
        public string ModelName { get; set; }

        public int Capacity { get; set; }
        [Precision(10, 2)]
        public decimal DailyPrice { get; set; }
        public int EnginePower { get; set; }
        public string? PhotoUrl { get; set; }


        public Brand Brand { get; set; }
        public CarType CarType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public FuelType FuelType { get; set; }

        public List<FleetCar> FleetCars { get; set; }

    }
}
