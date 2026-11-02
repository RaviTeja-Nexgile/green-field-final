namespace CareClinicManager.Models;

public class Notification
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }

    public Patient? Patient { get; set; }
}
