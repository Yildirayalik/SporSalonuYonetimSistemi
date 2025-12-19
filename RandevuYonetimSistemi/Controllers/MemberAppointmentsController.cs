using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Models.Entities;
using RandevuYonetimSistemi.Models.ViewModels;

namespace RandevuYonetimSistemi.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberAppointmentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // /MemberAppointments/Create
        public async Task<IActionResult> Create()
        {
            await FillDropdowns();

            // ✅ 0001 olmasın diye default bugün + şu anın saatine yakın bir değer veriyoruz
            var nowLocal = DateTime.Now;
            var vm = new AppointmentCreateVm
            {
                Date = DateOnly.FromDateTime(nowLocal),
                Time = TimeOnly.FromDateTime(nowLocal.AddMinutes(30))
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateVm vm)
        {
            await FillDropdowns();

            if (!ModelState.IsValid)
                return View(vm);

            var gymId = await _db.Gyms.Select(g => g.Id).FirstAsync();

            var trainer = await _db.Trainers.FirstAsync(t => t.Id == vm.TrainerId);
            var service = await _db.Services.FirstAsync(s => s.Id == vm.ServiceId);

            // ✅ Formdan gelen DateOnly+TimeOnly -> Local DateTime
            var localDateTime = vm.Date!.Value.ToDateTime(vm.Time!.Value);

            // ✅ Local -> UTC (timestamptz bunu istiyor)
            var utcStart = ToUtc(localDateTime);
            var utcEnd = utcStart.AddMinutes(service.DurationMinutes);

            // ✅ Çakışma kontrolü (Rejected/Cancelled hariç)
            var conflict = await _db.Appointments.AnyAsync(a =>
                a.TrainerId == trainer.Id &&
                a.Status != AppointmentStatus.Rejected &&
                a.Status != AppointmentStatus.Cancelled &&
                utcStart < a.EndAt &&
                utcEnd > a.StartAt);

            if (conflict)
            {
                ModelState.AddModelError("", "Seçtiğiniz saat dolu. Lütfen başka bir saat seçin.");
                return View(vm);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            var appt = new Appointment
            {
                GymId = gymId,

                MemberId = user.Id,
                TrainerId = trainer.Id,
                ServiceId = service.Id,

                StartAt = utcStart,      // ✅ UTC
                EndAt = utcEnd,          // ✅ UTC

                // ✅ Snapshot’lar
                TrainerNameSnapshot = trainer.FullName,
                ServiceNameSnapshot = service.Name,
                DurationMinutesSnapshot = service.DurationMinutes,
                PriceSnapshot = service.Price,

                Status = AppointmentStatus.Pending
            };

            _db.Appointments.Add(appt);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Randevu isteğiniz gönderildi. (Onay bekliyor)";
            return RedirectToAction("Index", "Member"); // istersen MemberAppointments/Index yaparız
        }

        private async Task FillDropdowns()
        {
            ViewBag.Trainers = await _db.Trainers
                .Where(t => t.IsActive)
                .OrderBy(t => t.FullName)
                .ToListAsync();

            ViewBag.Services = await _db.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        private static DateTime ToUtc(DateTime localDateTime)
        {
            // localDateTime Kind Unspecified geliyor -> local varsay
            var offset = TimeZoneInfo.Local.GetUtcOffset(localDateTime);
            var dto = new DateTimeOffset(localDateTime, offset);
            return DateTime.SpecifyKind(dto.UtcDateTime, DateTimeKind.Utc);
        }
    }
}
