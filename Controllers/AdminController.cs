using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Models.ViewModels;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IRepository<FleetCar> _fleetRepo;
        private readonly IRepository<User> _userRepo;

        public AdminController(
            IRepository<FleetCar> fleetRepo,
            IRepository<User> userRepo)
        {
            _fleetRepo = fleetRepo;
            _userRepo = userRepo;
        }

        // --- BURASI DASHBOARD INDEX METODU ---
        public async Task<IActionResult> Index()
        {
            var fleet = await _fleetRepo.GetAllAsync();
            var users = await _userRepo.GetAllAsync();

            int total = fleet.Count();
            int available = fleet.Count(x => x.Status == "Uygun");
            int rented = fleet.Count(x => x.Status == "Kirada");
            int maintenance = fleet.Count(x => x.Status == "Bakımda");

            // Kullanım Oranı
            double usageRate = 0;
            int usable = total - maintenance;

            if (usable > 0)
                usageRate = Math.Round((double)rented / usable * 100, 1);

            var vm = new DashboardViewModel
            {
                TotalFleetCars = total,
                AvailableCars = available,
                RentedCars = rented,
                MaintenanceCars = maintenance,

                PendingReservations = 0,
                EmployeeCount = users.Count(u => u.RoleId == 1 || u.RoleId == 2),

                UsageRate = usageRate
            };

            return View(vm);
        }
    }
}
