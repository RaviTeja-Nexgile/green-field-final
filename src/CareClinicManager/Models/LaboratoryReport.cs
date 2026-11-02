namespace CareClinicManager.Models;

public class LaboratoryReport
{
    public int Id { get; set; }
    public int LaboratoryTestId { get; set; }
    public string ResultData { get; set; } = string.Empty;
    public string ResultFilePath { get; set; } = string.Empty;

    public LaboratoryTest? LaboratoryTest { get; set; }
}
