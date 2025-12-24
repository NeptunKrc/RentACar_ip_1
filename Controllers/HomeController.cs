using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar_ip.Data;
using RentACar_ip.Models;
using Microsoft.AspNetCore.SignalR;
using RentACar_ip.Hubs;

namespace RentACar_ip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ReservationHub> _hub;

        public HomeController(ApplicationDbContext context, IHubContext<ReservationHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.FuelType)
                .Include(c => c.TransmissionType)
                .Include(c => c.CarType)
                .ToListAsync();

            ViewBag.Cars = cars;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(
            int carId,
            string fullName,
            string email,
            string phone,
            string nationalId,
            DateTime startDate,
            DateTime endDate)
        {
            // Müþteri var mý?
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.NationalId == nationalId);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    NationalId = nationalId
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // Rezervasyon
            var reservation = new ReservationRequest
            {
                CustomerId = customer.Id,
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Bekliyor",
                CreatedAt = DateTime.Now
            };

            _context.ReservationRequests.Add(reservation);
            await _context.SaveChangesAsync();

            // SIGNALR — Admin’e canlý mesaj
            await _hub.Clients.All.SendAsync(
                "ReceiveReservationNotification",
                $"{customer.FullName} yeni rezervasyon oluþturdu."
            );

            TempData["Success"] = "Rezervasyon talebiniz alýnmýþtýr!";
            return RedirectToAction("Index");
        }
    }
}
