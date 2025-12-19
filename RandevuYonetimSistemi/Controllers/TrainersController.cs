using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models.Entities;

[Authorize(Roles = "Admin")]
public class TrainersController : Controller
{
    private readonly ApplicationDbContext _db;
    public TrainersController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var list = await _db.Trainers
            .OrderBy(t => t.FullName)
            .ToListAsync();

        return View(list);
    }

    public IActionResult Create() => View(new Trainer());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trainer model)
    {
        // tek salon
        model.GymId = await _db.Gyms.Select(g => g.Id).FirstAsync();

        if (!ModelState.IsValid) return View(model);

        _db.Trainers.Add(model);
        await _db.SaveChangesAsync();

        TempData["Ok"] = "Antrenör eklendi ✅";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return NotFound();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trainer model)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return NotFound();

        // Gym alanını zorlamayalım (formdan gelmese de olur)
        ModelState.Remove(nameof(Trainer.Gym));
        ModelState.Remove(nameof(Trainer.GymId));

        if (!ModelState.IsValid) return View(model);

        entity.FullName = model.FullName;
        entity.Email = model.Email;
        entity.Phone = model.Phone;
        entity.Bio = model.Bio;
        entity.IsActive = model.IsActive;

        await _db.SaveChangesAsync();

        TempData["Ok"] = "Antrenör güncellendi ✅";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return NotFound();
        return View(entity);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return NotFound();

        _db.Trainers.Remove(entity);
        await _db.SaveChangesAsync();

        TempData["Ok"] = "Antrenör silindi ✅";
        return RedirectToAction(nameof(Index));
    }
}
