namespace CareClinicManager.Models;

public class PharmacyInventory
{
    public int Id { get; set; }
    public int MedicineId { get; set; }
    public int StockQuantity { get; set; }

    public Medicine? Medicine { get; set; }
}
