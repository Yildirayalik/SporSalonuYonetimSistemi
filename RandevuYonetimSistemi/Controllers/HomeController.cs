using Microsoft.AspNetCore.Mvc;

namespace RandevuYonetimSistemi.Controllers
{
    public class HomeController : Controller
    {
        // Siteye / ile girilince Portal'a yönlendir
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Portal));
        }

        // Portal: giriş seçimi ekranı
        public IActionResult Portal()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Member");
            }

            return View();
        }
    }
}
