namespace CareClinicManager.Models;

public class Prescription
{
    public int Id { get; set; }
    public int MedicalRecordId { get; set; }
    public int ConsultationId { get; set; }
    public int MedicineId { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool Issued { get; set; }

    public MedicalRecord? MedicalRecord { get; set; }
    public Consultation? Consultation { get; set; }
    public Medicine? Medicine { get; set; }
}
