namespace RentACar_ip.Models
{
    public class FleetCar
    {
        public int Id { get; set; }  // Primary Key (Identity)

        public string CarSerialId { get; set; }  // Business Key (Unique)
        public string Status { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
