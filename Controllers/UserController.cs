using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar_ip.Models.ViewModels;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // PERSONEL LİSTESİ
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // YENİ PERSONEL EKLEME
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var user = new IdentityUser
            {
                UserName = vm.Username,
                Email = vm.Email
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                TempData["UserError"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return RedirectToAction("Index");
            }

            // Rol ata
            await _userManager.AddToRoleAsync(user, vm.Role);

            TempData["UserSuccess"] = "Personel başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // PERSONEL SİLME
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

        // PERSONEL ROL GÜNCELLEME
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("Index");

            var roles = await _userManager.GetRolesAsync(user);

            // Eski rolleri kaldır
            await _userManager.RemoveFromRolesAsync(user, roles);

            // Yeni rolü ekle
            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction("Index");
        }

        // PERSONEL BİLGİ DÜZENLEME
        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, string username, string email)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("Index");

            user.UserName = username;
            user.Email = email;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }
    }
}
