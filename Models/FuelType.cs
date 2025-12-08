namespace RentACar_ip.Models
{
    public class FuelType
    {
        public int Id { get; set; }
        public string FuelTypeName { get; set; }

        public List<Car> Cars { get; set; }
    }
}
