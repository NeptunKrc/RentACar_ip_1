using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Models.ViewModels;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FleetCarController : Controller
    {
        private readonly IRepository<FleetCar> _fleetRepo;
        private readonly IRepository<Car> _carRepo;
        private readonly IRepository<Brand> _brandRepo;

        public FleetCarController(IRepository<FleetCar> fleetRepo,
                                  IRepository<Car> carRepo,
                                  IRepository<Brand> brandRepo)
        {
            _fleetRepo = fleetRepo;
            _carRepo = carRepo;
            _brandRepo = brandRepo;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carRepo.GetAllAsync();
            var brands = await _brandRepo.GetAllAsync();

            var vm = new FleetCarViewModel
            {
                FleetCars = await _fleetRepo.GetAllAsync(),
                Cars = cars,
                Brands = brands
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FleetCarViewModel model)
        {
            if (model.NewFleetCar.CarId == 0 ||
                string.IsNullOrWhiteSpace(model.NewFleetCar.CarSerialId) ||
                string.IsNullOrWhiteSpace(model.NewFleetCar.Status))
            {
                TempData["Error"] = "Tüm alanlar zorunludur.";
                return RedirectToAction("Index");
            }

            // Büyük harfe çevir
            model.NewFleetCar.CarSerialId = model.NewFleetCar.CarSerialId.ToUpper();

            // 🔥 AYNI SERİ NUMARASI VAR MI?
            var exists = (await _fleetRepo.FindAsync(x => x.CarSerialId == model.NewFleetCar.CarSerialId)).Any();
            if (exists)
            {
                TempData["Error"] = "Bu seri numarası zaten kayıtlı!";
                return RedirectToAction("Index");
            }

            await _fleetRepo.AddAsync(model.NewFleetCar);
            await _fleetRepo.SaveAsync();

            TempData["Success"] = "Filo kaydı eklendi.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var f = await _fleetRepo.GetByIdAsync(id);
            if (f != null)
            {
                _fleetRepo.Remove(f);
                await _fleetRepo.SaveAsync();
            }

            TempData["Success"] = "Kayıt silindi.";

            return RedirectToAction("Index");
        }
    }
}
