using System.ComponentModel.DataAnnotations;
using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Doctor;

/// <summary>
/// Carries the "New prescription" form input: a medicine, a dosage and a
/// quantity, plus the patient and consultation the prescription belongs to.
/// </summary>
public class PrescriptionFormViewModel
{
    public int MedicalRecordId { get; set; }

    public int ConsultationId { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose a medicine.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please choose a medicine.")]
    [Display(Name = "Medicine")]
    public int MedicineId { get; set; }

    [Required(ErrorMessage = "Dosage is required.")]
    [Display(Name = "Dosage")]
    public string Dosage { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    public List<Medicine> Medicines { get; set; } = new();
}

/// <summary>
/// A single prescription row shown in a patient's record.
/// </summary>
public class PrescriptionListItemViewModel
{
    public int Id { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool Issued { get; set; }
}

/// <summary>
/// The list of prescriptions for a patient's record.
/// </summary>
public class PrescriptionListViewModel
{
    public int MedicalRecordId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public List<PrescriptionListItemViewModel> Prescriptions { get; set; } = new();
}
