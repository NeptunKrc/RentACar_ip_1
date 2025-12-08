namespace RentACar_ip.Models
{
    public class Occupation
    {
        public int Id { get; set; }

        public int FleetCarId { get; set; }
        public FleetCar FleetCar { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
