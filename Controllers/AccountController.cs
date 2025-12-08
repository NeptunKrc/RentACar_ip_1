using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RentACar_ip.Repositories;
using RentACar_ip.Models;

namespace RentACar_ip.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;

        public AccountController(IRepository<User> userRepo, IRepository<Role> roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kullanıcıyı çekiyoruz
            var user = (await _userRepo.FindAsync(
                u => u.Username == username &&
                     u.Password == password
            )).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            // Kullanıcının rolünü çekiyoruz
            var role = await _roleRepo.GetByIdAsync(user.RoleId);
            if (role == null)
            {
                ViewBag.Error = "Rol bulunamadı.";
                return View();
            }

            // CLAIM OLUŞTURMA – *KRİTİK NOKTA*
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role.Name)  // *** ADMIN / EMPLOYEE ***
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
