namespace RentACar_ip.Models
{
    public class TransmissionType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        // Navigation
        public List<Car> Cars { get; set; }
    }
}
