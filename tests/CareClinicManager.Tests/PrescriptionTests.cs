using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class PrescriptionTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly PrescriptionService _service;

    public PrescriptionTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-prescription-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _service = new PrescriptionService(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }

    private (MedicalRecord record, Consultation consultation, Medicine medicine) SeedPrescriptionContext(int stock = 200)
    {
        var patient = new Patient { Name = "Test Patient", DateOfBirth = new DateTime(1990, 1, 1), Gender = "Female", Phone = "555", Email = "p@example.com", Address = "1 St" };
        _db.Patients.Add(patient);

        var record = new MedicalRecord { Patient = patient };
        _db.MedicalRecords.Add(record);

        var department = new Department { Name = "General Medicine" };
        var specialization = new Specialization { Name = "General Practitioner" };
        _db.Departments.Add(department);
        _db.Specializations.Add(specialization);

        var doctor = new Doctor { Name = "Dr. Test", Role = "Doctor", Department = department, Specialization = specialization, Phone = "555", Email = "d@example.com" };
        _db.Doctors.Add(doctor);

        var appointment = new Appointment { Patient = patient, Doctor = doctor, Department = department, Date = DateTime.Today, Time = new TimeSpan(9, 0, 0), Status = "Scheduled" };
        _db.Appointments.Add(appointment);

        var consultation = new Consultation { MedicalRecord = record, Appointment = appointment, Doctor = doctor, Symptoms = "s", Diagnosis = "d", ClinicalNotes = "n", Date = DateTime.Today };
        _db.Consultations.Add(consultation);

        var medicine = new Medicine { Name = "Amoxicillin", StockQuantity = stock };
        _db.Medicines.Add(medicine);
        _db.PharmacyInventory.Add(new PharmacyInventory { Medicine = medicine, StockQuantity = stock });

        _db.SaveChanges();

        return (record, consultation, medicine);
    }

    private async Task<Prescription> CreateDraftAsync(MedicalRecord record, Consultation consultation, Medicine medicine, int quantity = 10)
    {
        return await _service.CreateAsync(record.Id, consultation.Id, medicine.Id, "500 mg", quantity);
    }

    [Fact]
    public async Task Create_SavesDraftWithoutChangingStock()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext(stock: 200);

        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        Assert.True(prescription.Id > 0);
        Assert.False(prescription.Issued);
        var reloaded = _db.Medicines.Single(m => m.Id == medicine.Id);
        Assert.Equal(200, reloaded.StockQuantity);
    }

    [Fact]
    public async Task Issue_DecreasesMedicineStockByQuantity()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext(stock: 200);
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        var issued = await _service.IssueAsync(prescription.Id);

        Assert.True(issued);
        var reloaded = _db.Medicines.Single(m => m.Id == medicine.Id);
        Assert.Equal(190, reloaded.StockQuantity);
    }

    [Fact]
    public async Task Issue_DecreasesPharmacyInventoryByQuantity()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext(stock: 200);
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        await _service.IssueAsync(prescription.Id);

        var inventory = _db.PharmacyInventory.Single(i => i.MedicineId == medicine.Id);
        Assert.Equal(190, inventory.StockQuantity);
    }

    [Fact]
    public async Task Issue_MarksPrescriptionIssued()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext();
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        await _service.IssueAsync(prescription.Id);

        var reloaded = _db.Prescriptions.Single(p => p.Id == prescription.Id);
        Assert.True(reloaded.Issued);
        Assert.Equal(10, reloaded.Quantity);
        Assert.Equal("500 mg", reloaded.Dosage);
        Assert.Equal(medicine.Id, reloaded.MedicineId);
    }

    [Fact]
    public async Task Issue_PrescribesTenTablets_DecreasesStockByTen()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext(stock: 100);
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        await _service.IssueAsync(prescription.Id);

        var reloaded = _db.Medicines.Single(m => m.Id == medicine.Id);
        Assert.Equal(90, reloaded.StockQuantity);
    }

    [Fact]
    public async Task Cancel_LeavesInventoryUnchanged()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext(stock: 200);
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);

        // Cancel the prescription before it is issued: stock must not change.
        var result = await _service.CancelAsync(prescription.Id);

        Assert.True(result);
        var reloaded = _db.Medicines.Single(m => m.Id == medicine.Id);
        Assert.Equal(200, reloaded.StockQuantity);
        Assert.Null(_db.Prescriptions.Find(prescription.Id));
    }

    [Fact]
    public async Task Cancel_IssuedPrescription_ReturnsFalse()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext();
        var prescription = await CreateDraftAsync(record, consultation, medicine, 10);
        await _service.IssueAsync(prescription.Id);

        var result = await _service.CancelAsync(prescription.Id);

        Assert.False(result);
        Assert.NotNull(_db.Prescriptions.Find(prescription.Id));
    }

    [Fact]
    public async Task Cancel_UnknownPrescription_ReturnsFalse()
    {
        var result = await _service.CancelAsync(9999);
        Assert.False(result);
    }

    [Fact]
    public async Task GetForMedicalRecord_ReturnsPrescriptionsForRecord()
    {
        var (record, consultation, medicine) = SeedPrescriptionContext();
        var prescription = await CreateDraftAsync(record, consultation, medicine, 5);
        await _service.IssueAsync(prescription.Id);

        var prescriptions = await _service.GetForMedicalRecordAsync(record.Id);

        Assert.Single(prescriptions);
        Assert.Equal("Amoxicillin", prescriptions[0].Medicine?.Name);
        Assert.Equal(5, prescriptions[0].Quantity);
    }
}
