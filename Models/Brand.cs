namespace RentACar_ip.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }

        public List<Car> Cars { get; set; }
    }
}
