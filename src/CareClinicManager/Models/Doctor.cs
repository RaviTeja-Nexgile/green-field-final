namespace CareClinicManager.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int SpecializationId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Department? Department { get; set; }
    public Specialization? Specialization { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
