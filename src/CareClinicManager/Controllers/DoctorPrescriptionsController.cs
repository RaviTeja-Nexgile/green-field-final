using CareClinicManager.Data;
using CareClinicManager.Services;
using CareClinicManager.ViewModels.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Doctor")]
public class DoctorPrescriptionsController : Controller
{
    private readonly AppDbContext _db;
    private readonly PrescriptionService _prescriptions;

    public DoctorPrescriptionsController(AppDbContext db, PrescriptionService prescriptions)
    {
        _db = db;
        _prescriptions = prescriptions;
    }

    /// <summary>
    /// Shows the prescriptions for a patient's medical record.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(int medicalRecordId)
    {
        var record = await _db.MedicalRecords
            .Include(m => m.Patient)
            .FirstOrDefaultAsync(m => m.Id == medicalRecordId);

        if (record is null)
        {
            return NotFound();
        }

        var prescriptions = await _prescriptions.GetForMedicalRecordAsync(medicalRecordId);

        var model = new PrescriptionListViewModel
        {
            MedicalRecordId = record.Id,
            PatientId = record.PatientId,
            PatientName = record.Patient?.Name ?? string.Empty,
            Prescriptions = prescriptions
                .Select(p => new PrescriptionListItemViewModel
                {
                    Id = p.Id,
                    MedicineName = p.Medicine?.Name ?? string.Empty,
                    Dosage = p.Dosage,
                    Quantity = p.Quantity,
                    Issued = p.Issued
                })
                .ToList()
        };

        return View("~/Views/Doctor/Prescriptions/Index.cshtml", model);
    }

    /// <summary>
    /// Shows the "New prescription" form for a patient's medical record.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create(int medicalRecordId)
    {
        var record = await _db.MedicalRecords
            .Include(m => m.Patient)
            .FirstOrDefaultAsync(m => m.Id == medicalRecordId);

        if (record is null)
        {
            return NotFound();
        }

        var model = new PrescriptionFormViewModel
        {
            MedicalRecordId = record.Id,
            PatientId = record.PatientId,
            PatientName = record.Patient?.Name ?? string.Empty,
            Medicines = await _db.Medicines.OrderBy(m => m.Name).ToListAsync()
        };

        return View("~/Views/Doctor/Prescriptions/Create.cshtml", model);
    }

    /// <summary>
    /// Saves a new prescription and decreases the pharmacy inventory.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrescriptionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Medicines = await _db.Medicines.OrderBy(m => m.Name).ToListAsync();
            return View("~/Views/Doctor/Prescriptions/Create.cshtml", model);
        }

        // A prescription is issued against a consultation when one is linked;
        // otherwise it is saved without a consultation (the seeded clinic has
        // medical records but no consultations yet).
        int? consultationId = model.ConsultationId == 0 ? null : model.ConsultationId;

        var prescription = await _prescriptions.CreateAsync(
            model.MedicalRecordId,
            consultationId,
            model.MedicineId,
            model.Dosage,
            model.Quantity);

        // Prescribing issues the prescription immediately, decreasing stock.
        await _prescriptions.IssueAsync(prescription.Id);

        return RedirectToAction(nameof(Index), new { medicalRecordId = model.MedicalRecordId });
    }

    /// <summary>
    /// Issues a draft prescription, decreasing the pharmacy inventory.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Issue(int id, int medicalRecordId)
    {
        if (!await BelongsToMedicalRecordAsync(id, medicalRecordId))
        {
            return NotFound();
        }

        await _prescriptions.IssueAsync(id);
        return RedirectToAction(nameof(Index), new { medicalRecordId });
    }

    /// <summary>
    /// Cancels a prescription before it is issued; inventory is unchanged.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, int medicalRecordId)
    {
        if (!await BelongsToMedicalRecordAsync(id, medicalRecordId))
        {
            return NotFound();
        }

        await _prescriptions.CancelAsync(id);
        return RedirectToAction(nameof(Index), new { medicalRecordId });
    }

    /// <summary>
    /// Returns true when the prescription exists and belongs to the given
    /// medical record, so a doctor cannot issue or cancel another patient's
    /// prescription by guessing its id.
    /// </summary>
    private async Task<bool> BelongsToMedicalRecordAsync(int prescriptionId, int medicalRecordId)
    {
        return await _db.Prescriptions
            .AnyAsync(p => p.Id == prescriptionId && p.MedicalRecordId == medicalRecordId);
    }
}
