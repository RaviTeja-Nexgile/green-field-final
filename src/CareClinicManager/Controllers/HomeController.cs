using Microsoft.AspNetCore.Mvc;

namespace CareClinicManager.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Dashboard", User.IsInRole("Admin") ? "Admin"
                : User.IsInRole("Doctor") ? "Doctor"
                : User.IsInRole("Receptionist") ? "Receptionist"
                : "Patient");
        }

        return RedirectToAction("Login", "Account");
    }

    public IActionResult Error()
    {
        return View();
    }
}
