using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Services;

/// <summary>
/// Holds the business rules for prescriptions and the pharmacy inventory:
/// issuing a prescription decreases the medicine's stock by the prescribed
/// quantity; cancelling a prescription before it is issued leaves stock
/// unchanged.
/// </summary>
public class PrescriptionService
{
    private readonly AppDbContext _db;

    public PrescriptionService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Creates a new prescription as a draft (not yet issued). The pharmacy
    /// inventory is not changed at this point.
    /// </summary>
    public async Task<Prescription> CreateAsync(
        int medicalRecordId,
        int? consultationId,
        int medicineId,
        string dosage,
        int quantity)
    {
        var prescription = new Prescription
        {
            MedicalRecordId = medicalRecordId,
            ConsultationId = consultationId,
            MedicineId = medicineId,
            Dosage = dosage,
            Quantity = quantity,
            Issued = false
        };

        _db.Prescriptions.Add(prescription);
        await _db.SaveChangesAsync();
        return prescription;
    }

    /// <summary>
    /// Issues a prescription: marks it issued and decreases the medicine's
    /// stock (and the matching pharmacy inventory row) by the prescribed
    /// quantity.
    /// </summary>
    public async Task<bool> IssueAsync(int prescriptionId)
    {
        var prescription = await _db.Prescriptions.FindAsync(prescriptionId);
        if (prescription is null || prescription.Issued)
        {
            return false;
        }

        prescription.Issued = true;

        var medicine = await _db.Medicines.FindAsync(prescription.MedicineId);
        if (medicine is not null)
        {
            medicine.StockQuantity -= prescription.Quantity;
        }

        var inventory = await _db.PharmacyInventory
            .FirstOrDefaultAsync(i => i.MedicineId == prescription.MedicineId);
        if (inventory is not null)
        {
            inventory.StockQuantity -= prescription.Quantity;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Cancels a prescription before it is issued. Stock is left unchanged.
    /// </summary>
    public async Task<bool> CancelAsync(int prescriptionId)
    {
        var prescription = await _db.Prescriptions.FindAsync(prescriptionId);
        if (prescription is null || prescription.Issued)
        {
            return false;
        }

        // Cancelling before issue never changes the pharmacy inventory.
        _db.Prescriptions.Remove(prescription);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Returns the prescriptions for a patient's medical record, with the
    /// medicine name loaded for display.
    /// </summary>
    public async Task<List<Prescription>> GetForMedicalRecordAsync(int medicalRecordId)
    {
        return await _db.Prescriptions
            .Where(p => p.MedicalRecordId == medicalRecordId)
            .Include(p => p.Medicine)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }
}
