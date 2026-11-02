namespace CareClinicManager.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    public Patient? Patient { get; set; }
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<LaboratoryTest> LaboratoryTests { get; set; } = new List<LaboratoryTest>();
}
