namespace RentACar_ip.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalFleetCars { get; set; }
        public int AvailableCars { get; set; }
        public int RentedCars { get; set; }
        public int MaintenanceCars { get; set; }

        public int PendingReservations { get; set; }
        public int EmployeeCount { get; set; }

        public double UsageRate { get; set; }
    }
}
