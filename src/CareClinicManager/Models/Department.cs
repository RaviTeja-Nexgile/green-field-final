namespace CareClinicManager.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public ICollection<Staff> Staff { get; set; } = new List<Staff>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
