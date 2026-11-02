namespace CareClinicManager.Models;

public class Invoice
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Items { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Unpaid";

    public Patient? Patient { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
