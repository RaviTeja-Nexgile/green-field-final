namespace CareClinicManager.Models;

public class LaboratoryTest
{
    public int Id { get; set; }
    public int MedicalRecordId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string TestType { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested";

    public MedicalRecord? MedicalRecord { get; set; }
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public LaboratoryReport? Report { get; set; }
}
