using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models;
using RentACar_ip.Repositories;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;

        public UserController(IRepository<User> userRepo, IRepository<Role> roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = new UserViewModel
            {
                Users = await _userRepo.GetAllAsync(),
                Roles = await _roleRepo.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewUser.FullName) ||
                string.IsNullOrWhiteSpace(model.NewUser.Username) ||
                string.IsNullOrWhiteSpace(model.NewUser.Password))
            {
                TempData["Error"] = "Tüm alanlar zorunludur.";
                return RedirectToAction("Index");
            }

            var exists = (await _userRepo.FindAsync(x => x.Username == model.NewUser.Username)).Any();
            if (exists)
            {
                TempData["Error"] = "Bu kullanıcı adı zaten kayıtlı.";
                return RedirectToAction("Index");
            }

            if (model.NewUser.RoleId == 0)
            {
                TempData["Error"] = "Bir rol seçmelisin.";
                return RedirectToAction("Index");
            }

            await _userRepo.AddAsync(model.NewUser);
            await _userRepo.SaveAsync();

            TempData["Success"] = "Personel eklendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                _userRepo.Remove(user);
                await _userRepo.SaveAsync();
            }

            TempData["Success"] = "Personel silindi.";
            return RedirectToAction("Index");
        }
    }
}
