using System.Security.Claims;
using CareClinicManager.Data;
using CareClinicManager.Services;
using CareClinicManager.ViewModels.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Patient")]
public class PatientAppointmentsController : Controller
{
    private readonly AppointmentService _appointmentService;
    private readonly AppDbContext _db;

    public PatientAppointmentsController(AppointmentService appointmentService, AppDbContext db)
    {
        _appointmentService = appointmentService;
        _db = db;
    }

    /// <summary>The patient's own appointments, shown on their dashboard (5.8).</summary>
    public async Task<IActionResult> Index()
    {
        var patientId = await GetPatientIdAsync();
        if (patientId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var appointments = await _db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .Where(a => a.PatientId == patientId.Value)
            .ToListAsync();

        var model = new PatientAppointmentListViewModel
        {
            Appointments = appointments
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .Select(PatientAppointmentListItemViewModel.From)
                .ToList()
        };

        return View("~/Views/Patient/Appointments/Index.cshtml", model);
    }

    /// <summary>The patient self-booking form (5.3).</summary>
    [HttpGet]
    public async Task<IActionResult> Book()
    {
        var patientId = await GetPatientIdAsync();
        if (patientId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new PatientBookAppointmentViewModel
        {
            Date = DateTime.Today,
            Time = new TimeSpan(9, 0, 0),
            Doctors = await _db.Doctors.OrderBy(d => d.Name).ToListAsync(),
            Departments = await _db.Departments.OrderBy(d => d.Name).ToListAsync()
        };
        return View("~/Views/Patient/Appointments/Book.cshtml", model);
    }

    /// <summary>Books the patient's own appointment with status "Scheduled" (5.3, 5.4, 5.5).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(PatientBookAppointmentViewModel model)
    {
        var patientId = await GetPatientIdAsync();
        if (patientId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            model.Doctors = await _db.Doctors.OrderBy(d => d.Name).ToListAsync();
            model.Departments = await _db.Departments.OrderBy(d => d.Name).ToListAsync();
            return View("~/Views/Patient/Appointments/Book.cshtml", model);
        }

        await _appointmentService.BookAsync(
            patientId.Value,
            model.DoctorId!.Value,
            model.DepartmentId!.Value,
            model.Date!.Value,
            model.Time!.Value);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>Resolves the signed-in patient's Patient record id from the auth cookie.</summary>
    private async Task<int?> GetPatientIdAsync()
    {
        var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(nameIdentifier) || !int.TryParse(nameIdentifier, out var userId))
        {
            return null;
        }

        var user = await _db.Users.FindAsync(userId);
        return user?.PatientId;
    }
}
