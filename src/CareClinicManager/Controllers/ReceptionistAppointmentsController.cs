using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Services;
using CareClinicManager.ViewModels.Receptionist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Receptionist")]
public class ReceptionistAppointmentsController : Controller
{
    private readonly AppointmentService _appointmentService;
    private readonly AppDbContext _db;

    public ReceptionistAppointmentsController(AppointmentService appointmentService, AppDbContext db)
    {
        _appointmentService = appointmentService;
        _db = db;
    }

    /// <summary>The appointment list: every appointment with its status badge (6.1).</summary>
    public async Task<IActionResult> Index()
    {
        var appointments = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .ToListAsync();

        var model = new AppointmentListViewModel
        {
            Appointments = appointments
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .Select(AppointmentListItemViewModel.From)
                .ToList()
        };

        return View("~/Views/Receptionist/Appointments/Index.cshtml", model);
    }

    /// <summary>The calendar view: appointments in a month grid with status badges (5.1).</summary>
    public async Task<IActionResult> Calendar(int? year, int? month)
    {
        var now = DateTime.Today;
        var model = new AppointmentCalendarViewModel
        {
            Year = year ?? now.Year,
            Month = month ?? now.Month
        };

        var appointments = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .Where(a => a.Date.Year == model.Year && a.Date.Month == model.Month)
            .ToListAsync();

        model.Appointments = appointments
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Time)
            .Select(AppointmentListItemViewModel.From)
            .ToList();
        return View("~/Views/Receptionist/Appointments/Calendar.cshtml", model);
    }

    /// <summary>The "Book appointment" form (5.2).</summary>
    [HttpGet]
    public async Task<IActionResult> Book()
    {
        var model = new AppointmentFormViewModel
        {
            Date = DateTime.Today,
            Time = new TimeSpan(9, 0, 0),
            Patients = await _db.Patients.OrderBy(p => p.Name).ToListAsync(),
            Doctors = await _db.Doctors.OrderBy(d => d.Name).ToListAsync(),
            Departments = await _db.Departments.OrderBy(d => d.Name).ToListAsync()
        };
        return View("~/Views/Receptionist/Appointments/Book.cshtml", model);
    }

    /// <summary>Saves a valid appointment with status "Scheduled" (5.4, 5.5).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(AppointmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateFormListsAsync(model);
            return View("~/Views/Receptionist/Appointments/Book.cshtml", model);
        }

        await _appointmentService.BookAsync(
            model.PatientId!.Value,
            model.DoctorId!.Value,
            model.DepartmentId!.Value,
            model.Date!.Value,
            model.Time!.Value);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>The edit form for an existing appointment (5.6).</summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return NotFound();
        }

        var model = new AppointmentFormViewModel
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            DepartmentId = appointment.DepartmentId,
            Date = appointment.Date,
            Time = appointment.Time
        };
        await PopulateFormListsAsync(model);
        return View("~/Views/Receptionist/Appointments/Edit.cshtml", model);
    }

    /// <summary>Saves the edited appointment (5.6).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppointmentFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateFormListsAsync(model);
            return View("~/Views/Receptionist/Appointments/Edit.cshtml", model);
        }

        var updated = await _appointmentService.UpdateAsync(
            id,
            model.PatientId!.Value,
            model.DoctorId!.Value,
            model.DepartmentId!.Value,
            model.Date!.Value,
            model.Time!.Value);

        if (updated is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>Checks a patient in, setting the status to "Checked in" (6.2).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int id)
    {
        await _appointmentService.CheckInAsync(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Cancels an appointment, setting the status to "Cancelled" (5.7).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        await _appointmentService.CancelAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateFormListsAsync(AppointmentFormViewModel model)
    {
        model.Patients = await _db.Patients.OrderBy(p => p.Name).ToListAsync();
        model.Doctors = await _db.Doctors.OrderBy(d => d.Name).ToListAsync();
        model.Departments = await _db.Departments.OrderBy(d => d.Name).ToListAsync();
    }
}
