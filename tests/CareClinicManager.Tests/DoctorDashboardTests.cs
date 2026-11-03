using CareClinicManager.Controllers;
using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class DoctorDashboardTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;

    public DoctorDashboardTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-doctor-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
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

    private void SeedDashboardData()
    {
        var dept = new Department { Name = "Cardiology" };
        var spec = new Specialization { Name = "Cardiologist" };
        var doctor = new Doctor { Name = "Dr. Alice Carter", Role = "Doctor", Department = dept, Specialization = spec, Phone = "555-0101", Email = "alice@clinic.local" };
        var patient = new Patient { Name = "Eve Johnson", DateOfBirth = new DateTime(1985, 4, 12), Gender = "Female", Phone = "555-0201", Email = "eve@example.com", Address = "12 Oak Street" };
        var record = new MedicalRecord { Patient = patient };
        var medicine = new Medicine { Name = "Amoxicillin", StockQuantity = 200 };

        _db.Departments.Add(dept);
        _db.Specializations.Add(spec);
        _db.Doctors.Add(doctor);
        _db.Patients.Add(patient);
        _db.MedicalRecords.Add(record);
        _db.Medicines.Add(medicine);
        _db.SaveChanges();

        var today = DateTime.Today;
        var appointment = new Appointment
        {
            Patient = patient,
            Doctor = doctor,
            Department = dept,
            Date = today,
            Time = new TimeSpan(9, 0, 0),
            Status = "Scheduled"
        };
        _db.Appointments.Add(appointment);
        _db.SaveChanges();

        var consultation = new Consultation
        {
            MedicalRecord = record,
            Appointment = appointment,
            Doctor = doctor,
            Symptoms = "Chest pain",
            Diagnosis = "Angina",
            ClinicalNotes = "Rest and monitor",
            Date = today
        };
        _db.Consultations.Add(consultation);
        _db.SaveChanges();

        var prescription = new Prescription
        {
            MedicalRecord = record,
            Consultation = consultation,
            Medicine = medicine,
            Dosage = "500mg twice daily",
            Quantity = 10,
            Issued = true
        };
        _db.Prescriptions.Add(prescription);
        _db.SaveChanges();

        var labTest = new LaboratoryTest
        {
            MedicalRecord = record,
            Patient = patient,
            Doctor = doctor,
            TestType = "Blood count",
            Status = "Requested"
        };
        _db.LaboratoryTests.Add(labTest);
        _db.SaveChanges();
    }

    [Fact]
    public async Task Dashboard_ReturnsViewWithViewModel()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DashboardViewModel>(view.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task Dashboard_ShowsTodaysAppointments()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.TodayAppointments);
        Assert.Equal("Eve Johnson", model.TodayAppointments[0].PatientName);
        Assert.Equal("Cardiology", model.TodayAppointments[0].DepartmentName);
        Assert.Equal(1, model.TodayAppointmentsCount);
    }

    [Fact]
    public async Task Dashboard_ShowsPatientRecords()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.Patients);
        Assert.Equal("Eve Johnson", model.Patients[0].Name);
        Assert.Equal(1, model.PatientsCount);
    }

    [Fact]
    public async Task Dashboard_ShowsConsultationHistory()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.Consultations);
        Assert.Equal("Angina", model.Consultations[0].Diagnosis);
        Assert.Equal("Eve Johnson", model.Consultations[0].PatientName);
        Assert.Equal(1, model.ConsultationsCount);
    }

    [Fact]
    public async Task Dashboard_ShowsPrescriptions()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.Prescriptions);
        Assert.Equal("Amoxicillin", model.Prescriptions[0].MedicineName);
        Assert.True(model.Prescriptions[0].Issued);
        Assert.Equal(1, model.PrescriptionsCount);
    }

    [Fact]
    public async Task Dashboard_ShowsLabRequests()
    {
        SeedDashboardData();
        var controller = new DoctorController(_db);

        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.LabRequests);
        Assert.Equal("Blood count", model.LabRequests[0].TestType);
        Assert.Equal("Requested", model.LabRequests[0].Status);
        Assert.Equal(1, model.LabRequestsCount);
    }

    [Fact]
    public async Task Dashboard_ExcludesAppointmentsNotToday()
    {
        SeedDashboardData();

        // Add an appointment for tomorrow; it must not appear in today's list.
        var dept = _db.Departments.First();
        var doctor = _db.Doctors.First();
        var patient = _db.Patients.First();
        _db.Appointments.Add(new Appointment
        {
            Patient = patient,
            Doctor = doctor,
            Department = dept,
            Date = DateTime.Today.AddDays(1),
            Time = new TimeSpan(10, 0, 0),
            Status = "Scheduled"
        });
        _db.SaveChanges();

        var controller = new DoctorController(_db);
        var result = await controller.Dashboard();
        var model = Assert.IsType<DashboardViewModel>(((ViewResult)result).Model);

        Assert.Single(model.TodayAppointments);
        Assert.Equal(1, model.TodayAppointmentsCount);
    }
}
