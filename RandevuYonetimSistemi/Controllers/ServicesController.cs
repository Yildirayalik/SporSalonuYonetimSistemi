using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models.Entities;

namespace RandevuYonetimSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _db.Services
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Service());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model)
        {

            // tek salon: GymId otomatik
            model.GymId = await _db.Gyms.Select(g => g.Id).FirstAsync();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(" | ", x.Value.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();

                TempData["Error"] = "Hizmet eklenemedi: " + string.Join(" || ", errors);
                return RedirectToAction(nameof(Index)); // Create'da kalmak yerine Index'e dönüp mesajı göster
            }

            _db.Services.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "✅ Hizmet eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Services.FindAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service model)
        {
            var entity = await _db.Services.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Güncelleme başarısız. Form hatalarını düzelt.";
                return View(model);
            }

            try
            {
                entity.Name = model.Name;
                entity.DurationMinutes = model.DurationMinutes;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;

                await _db.SaveChangesAsync();

                TempData["Success"] = "✅ Hizmet güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Services.FindAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.Services.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                _db.Services.Remove(entity);
                await _db.SaveChangesAsync();

                TempData["Success"] = "✅ Hizmet silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Silme sırasında hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
