using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar_ip.Data;
using RentACar_ip.Models;

namespace RentACar_ip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Now;

            ViewBag.Pending = await _context.ReservationRequests
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Where(r => r.Status == "Bekliyor")
                .ToListAsync();

            ViewBag.Approved = await _context.ReservationRequests
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Where(r => r.Status == "Onaylandı" && r.EndDate >= today)
                .ToListAsync();

            ViewBag.Rejected = await _context.ReservationRequests
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Where(r => r.Status == "Reddedildi")
                .ToListAsync();

            ViewBag.Archived = await _context.ReservationRequests
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Where(r => r.EndDate < today)
                .OrderByDescending(r => r.EndDate)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _context.ReservationRequests
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return RedirectToAction("Index");

            reservation.Status = "Onaylandı";

            var fleet = await _context.FleetCars.FirstOrDefaultAsync(f => f.CarId == reservation.CarId);
            if (fleet != null)
                fleet.Status = "Kirada";

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _context.ReservationRequests.FindAsync(id);
            if (reservation == null)
                return RedirectToAction("Index");

            reservation.Status = "Reddedildi";
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
