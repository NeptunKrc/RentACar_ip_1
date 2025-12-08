namespace RentACar_ip.Models
{
    public class ReservationRequest
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }  // pending, approved, rejected
        public DateTime CreatedAt { get; set; }
    }
}
