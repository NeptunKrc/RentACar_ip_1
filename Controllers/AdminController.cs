using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Models.ViewModels;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<FleetCar> _fleetRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            IRepository<FleetCar> fleetRepo,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _fleetRepo = fleetRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // FİLO BİLGİLERİ
            var fleet = await _fleetRepo.GetAllAsync();

            int total = fleet.Count();
            int available = fleet.Count(x => x.Status == "Uygun");
            int rented = fleet.Count(x => x.Status == "Kirada");
            int maintenance = fleet.Count(x => x.Status == "Bakımda");

            int usable = total - maintenance;
            double usageRate = usable > 0
                ? Math.Round((double)rented / usable * 100, 1)
                : 0;

            // KULLANICI BİLGİSİ (Identity üzerinden)
            var allUsers = _userManager.Users.ToList();

            // Employee rolüne sahip kullanıcılar
            var employeeRole = await _roleManager.FindByNameAsync("EMPLOYEE");
            int employeeCount = 0;

            if (employeeRole != null)
            {
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("EMPLOYEE"))
                        employeeCount++;
                }
            }

            var vm = new DashboardViewModel
            {
                TotalFleetCars = total,
                AvailableCars = available,
                RentedCars = rented,
                MaintenanceCars = maintenance,

                PendingReservations = 0, // Şimdilik kullanılmadı
                EmployeeCount = employeeCount,
                UsageRate = usageRate
            };

            return View(vm);
        }
    }
}
