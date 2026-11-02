namespace CareClinicManager.Models;

public class Consultation
{
    public int Id { get; set; }
    public int MedicalRecordId { get; set; }
    public int AppointmentId { get; set; }
    public int DoctorId { get; set; }
    public string Symptoms { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string ClinicalNotes { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public MedicalRecord? MedicalRecord { get; set; }
    public Appointment? Appointment { get; set; }
    public Doctor? Doctor { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
