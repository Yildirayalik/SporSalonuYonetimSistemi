using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models.Entities;

namespace RandevuYonetimSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminAppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /AdminAppointments
        public async Task<IActionResult> Index()
        {
            var list = await _db.Appointments
                .Include(a => a.Member)
                .OrderByDescending(a => a.StartAt)
                .ToListAsync();

            return View(list);
        }

        // GET: /AdminAppointments/Pending
        public async Task<IActionResult> Pending()
        {
            var list = await _db.Appointments
                .Include(a => a.Member)
                .Where(a => a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.StartAt)
                .ToListAsync();

            return View(list);
        }

        // POST: /AdminAppointments/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string? note)
        {
            var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();

            if (a.Status != AppointmentStatus.Pending)
            {
                TempData["Err"] = "Sadece Pending randevu onaylanabilir.";
                return RedirectToAction(nameof(Pending));
            }

            // Son bir kez çakışma kontrolü (Approved/Pending)
            var conflict = await _db.Appointments.AnyAsync(x =>
                x.Id != a.Id &&
                x.TrainerId == a.TrainerId &&
                x.Status != AppointmentStatus.Rejected &&
                x.Status != AppointmentStatus.Cancelled &&
                a.StartAt < x.EndAt && a.EndAt > x.StartAt);

            if (conflict)
            {
                TempData["Err"] = "Onaylanamadı: Bu saat dolu (çakışma var).";
                return RedirectToAction(nameof(Pending));
            }

            a.Status = AppointmentStatus.Approved;
            a.DecisionNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            await _db.SaveChangesAsync();

            TempData["Ok"] = "Randevu onaylandı ✅";
            return RedirectToAction(nameof(Pending));
        }

        // POST: /AdminAppointments/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? note)
        {
            var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();

            if (a.Status != AppointmentStatus.Pending)
            {
                TempData["Err"] = "Sadece Pending randevu reddedilebilir.";
                return RedirectToAction(nameof(Pending));
            }

            a.Status = AppointmentStatus.Rejected;
            a.DecisionNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            await _db.SaveChangesAsync();

            TempData["Ok"] = "Randevu reddedildi ❌";
            return RedirectToAction(nameof(Pending));
        }

        // Admin iptal edebilsin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? note)
        {
            var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();

            a.Status = AppointmentStatus.Cancelled;
            a.DecisionNote = string.IsNullOrWhiteSpace(note) ? "Admin iptal etti." : note.Trim();

            await _db.SaveChangesAsync();
            TempData["Ok"] = "Randevu iptal edildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
