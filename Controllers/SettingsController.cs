using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Models.ViewModels;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<CarType> _carTypeRepo;
        private readonly IRepository<FuelType> _fuelRepo;
        private readonly IRepository<TransmissionType> _transRepo;
        private readonly IRepository<Role> _roleRepo;

        public SettingsController(
            IRepository<Brand> brandRepo,
            IRepository<CarType> carTypeRepo,
            IRepository<FuelType> fuelRepo,
            IRepository<TransmissionType> transRepo,
            IRepository<Role> roleRepo
        )
        {
            _brandRepo = brandRepo;
            _carTypeRepo = carTypeRepo;
            _fuelRepo = fuelRepo;
            _transRepo = transRepo;
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SettingsViewModel
            {
                Brands = await _brandRepo.GetAllAsync(),
                CarTypes = await _carTypeRepo.GetAllAsync(),
                FuelTypes = await _fuelRepo.GetAllAsync(),
                TransmissionTypes = await _transRepo.GetAllAsync(),
                Roles = await _roleRepo.GetAllAsync()
            };

            return View(model);
        }

        // -------------------------
        // BRAND
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> AddBrand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, message = "Marka boş olamaz." });

            name = name.Trim().ToUpper();

            var exists = (await _brandRepo.FindAsync(x => x.BrandName == name)).Any();
            if (exists)
                return Json(new { success = false, message = "Bu marka zaten mevcut." });

            await _brandRepo.AddAsync(new Brand { BrandName = name });
            await _brandRepo.SaveAsync();
            return Json(new { success = true, name });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var item = await _brandRepo.GetByIdAsync(id);
            if (item != null)
            {
                _brandRepo.Remove(item);
                await _brandRepo.SaveAsync();
            }
            return RedirectToAction("Index");
        }

        // -------------------------
        // CAR TYPE
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> AddCarType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return RedirectToAction("Index");

            name = name.Trim().ToUpper();

            var exists = (await _carTypeRepo.FindAsync(x => x.TypeName == name)).Any();
            if (exists)
                return RedirectToAction("Index");

            await _carTypeRepo.AddAsync(new CarType { TypeName = name });
            await _carTypeRepo.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarType(int id)
        {
            var item = await _carTypeRepo.GetByIdAsync(id);
            if (item != null)
            {
                _carTypeRepo.Remove(item);
                await _carTypeRepo.SaveAsync();
            }
            return RedirectToAction("Index");
        }

        // -------------------------
        // FUEL TYPE
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> AddFuel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return RedirectToAction("Index");

            name = name.Trim().ToUpper();

            var exists = (await _fuelRepo.FindAsync(x => x.FuelTypeName == name)).Any();
            if (exists)
                return RedirectToAction("Index");

            await _fuelRepo.AddAsync(new FuelType { FuelTypeName = name });
            await _fuelRepo.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFuel(int id)
        {
            var item = await _fuelRepo.GetByIdAsync(id);
            if (item != null)
            {
                _fuelRepo.Remove(item);
                await _fuelRepo.SaveAsync();
            }
            return RedirectToAction("Index");
        }

        // -------------------------
        // TRANSMISSION
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> AddTrans(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return RedirectToAction("Index");

            name = name.Trim().ToUpper();

            var exists = (await _transRepo.FindAsync(x => x.TypeName == name)).Any();
            if (exists)
                return RedirectToAction("Index");

            await _transRepo.AddAsync(new TransmissionType { TypeName = name });
            await _transRepo.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrans(int id)
        {
            var item = await _transRepo.GetByIdAsync(id);
            if (item != null)
            {
                _transRepo.Remove(item);
                await _transRepo.SaveAsync();
            }
            return RedirectToAction("Index");
        }

        // -------------------------
        // ROLE
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["RoleError"] = "Rol adı boş olamaz.";
                return RedirectToAction("Index");
            }

            roleName = roleName.ToUpper().Trim();

            var exists = (await _roleRepo.FindAsync(x => x.Name == roleName)).Any();
            if (exists)
            {
                TempData["RoleError"] = "Bu rol zaten mevcut!";
                return RedirectToAction("Index");
            }

            await _roleRepo.AddAsync(new Role { Name = roleName });
            await _roleRepo.SaveAsync();

            TempData["RoleSuccess"] = "Rol başarıyla eklendi.";
            return RedirectToAction("Index");
        }
    }
}
