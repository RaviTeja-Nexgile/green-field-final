namespace CareClinicManager.Models;

/// <summary>A lightweight application user (not ASP.NET Core Identity).</summary>
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? PatientId { get; set; }

    public Patient? Patient { get; set; }
}
