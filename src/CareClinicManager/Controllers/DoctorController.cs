using CareClinicManager.Data;
using CareClinicManager.ViewModels.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Doctor")]
public class DoctorController : Controller
{
    private readonly AppDbContext _db;

    public DoctorController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Dashboard()
    {
        var today = DateTime.Today;

        var todayAppointments = (await _db.Appointments
            .Where(a => a.Date == today)
            .Select(a => new DashboardViewModel.AppointmentRow
            {
                Id = a.Id,
                PatientName = a.Patient != null ? a.Patient.Name : string.Empty,
                DepartmentName = a.Department != null ? a.Department.Name : string.Empty,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status
            })
            .ToListAsync())
            .OrderBy(a => a.Time)
            .ToList();

        var patients = await _db.Patients
            .OrderBy(p => p.Name)
            .Select(p => new DashboardViewModel.PatientRow
            {
                Id = p.Id,
                Name = p.Name,
                Gender = p.Gender,
                Phone = p.Phone,
                Email = p.Email
            })
            .ToListAsync();

        var consultations = await _db.Consultations
            .OrderByDescending(c => c.Date)
            .Select(c => new DashboardViewModel.ConsultationRow
            {
                Id = c.Id,
                PatientName = c.MedicalRecord != null && c.MedicalRecord.Patient != null
                    ? c.MedicalRecord.Patient.Name
                    : string.Empty,
                Diagnosis = c.Diagnosis,
                Date = c.Date
            })
            .ToListAsync();

        var prescriptions = await _db.Prescriptions
            .OrderByDescending(p => p.Id)
            .Select(p => new DashboardViewModel.PrescriptionRow
            {
                Id = p.Id,
                PatientName = p.MedicalRecord != null && p.MedicalRecord.Patient != null
                    ? p.MedicalRecord.Patient.Name
                    : string.Empty,
                MedicineName = p.Medicine != null ? p.Medicine.Name : string.Empty,
                Dosage = p.Dosage,
                Quantity = p.Quantity,
                Issued = p.Issued
            })
            .ToListAsync();

        var labRequests = await _db.LaboratoryTests
            .OrderByDescending(t => t.Id)
            .Select(t => new DashboardViewModel.LabRequestRow
            {
                Id = t.Id,
                PatientName = t.Patient != null ? t.Patient.Name : string.Empty,
                TestType = t.TestType,
                Status = t.Status
            })
            .ToListAsync();

        var model = new DashboardViewModel
        {
            TodayAppointmentsCount = todayAppointments.Count,
            PatientsCount = patients.Count,
            ConsultationsCount = consultations.Count,
            PrescriptionsCount = prescriptions.Count,
            LabRequestsCount = labRequests.Count,
            TodayAppointments = todayAppointments,
            Patients = patients,
            Consultations = consultations,
            Prescriptions = prescriptions,
            LabRequests = labRequests
        };

        return View(model);
    }
}
