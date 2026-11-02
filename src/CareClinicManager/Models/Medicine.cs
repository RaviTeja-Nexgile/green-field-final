namespace CareClinicManager.Models;

public class Medicine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StockQuantity { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
