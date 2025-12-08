using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Models.ViewModels;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly IRepository<Car> _carRepo;
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<CarType> _carTypeRepo;
        private readonly IRepository<TransmissionType> _transRepo;
        private readonly IRepository<FuelType> _fuelRepo;

        public CarController(
            IRepository<Car> carRepo,
            IRepository<Brand> brandRepo,
            IRepository<CarType> carTypeRepo,
            IRepository<TransmissionType> transRepo,
            IRepository<FuelType> fuelRepo
        )
        {
            _carRepo = carRepo;
            _brandRepo = brandRepo;
            _carTypeRepo = carTypeRepo;
            _transRepo = transRepo;
            _fuelRepo = fuelRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = new CarViewModel
            {
                Cars = await _carRepo.GetAllAsync(),
                Brands = await _brandRepo.GetAllAsync(),
                CarTypes = await _carTypeRepo.GetAllAsync(),
                Transmissions = await _transRepo.GetAllAsync(),
                FuelTypes = await _fuelRepo.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewCar.ModelName))
            {
                TempData["Error"] = "Model adı boş bırakılamaz.";
                return RedirectToAction("Index");
            }

            // ⭐ MODEL NAME HER ZAMAN BÜYÜK HARF OLARAK KAYDEDİLİR
            model.NewCar.ModelName = model.NewCar.ModelName.ToUpper();

            await _carRepo.AddAsync(model.NewCar);
            await _carRepo.SaveAsync();

            TempData["Success"] = "Araç başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carRepo.GetByIdAsync(id);
            if (car != null)
            {
                _carRepo.Remove(car);
                await _carRepo.SaveAsync();
            }

            TempData["Success"] = "Araç silindi.";
            return RedirectToAction("Index");
        }
    }
}
