namespace CareClinicManager.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Status { get; set; } = "Scheduled";

    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public Department? Department { get; set; }
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}
