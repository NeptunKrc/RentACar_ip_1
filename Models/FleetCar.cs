namespace RentACar_ip.Models
{
    public class FleetCar
    {
        public int Id { get; set; }  

        public string CarSerialId { get; set; }  
        public string Status { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
