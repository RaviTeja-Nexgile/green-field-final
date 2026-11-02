namespace CareClinicManager.Models;

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Invoice? Invoice { get; set; }
}
