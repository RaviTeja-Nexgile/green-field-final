using CareClinicManager.Controllers;
using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.ViewModels.Admin;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class AdminDashboardTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly AdminController _controller;

    public AdminDashboardTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-admin-dashboard-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _controller = new AdminController(_db);
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

    private (Department dept, Specialization spec, Doctor doctor) SeedDoctor(string doctorName)
    {
        var dept = new Department { Name = "Cardiology" };
        var spec = new Specialization { Name = "Cardiologist" };
        _db.Departments.Add(dept);
        _db.Specializations.Add(spec);
        _db.SaveChanges();

        var doctor = new Doctor
        {
            Name = doctorName,
            Role = "Doctor",
            DepartmentId = dept.Id,
            SpecializationId = spec.Id,
            Phone = "555-0101",
            Email = $"{doctorName}@clinic.local",
        };
        _db.Doctors.Add(doctor);
        _db.SaveChanges();
        return (dept, spec, doctor);
    }

    private Patient SeedPatient(string name)
    {
        var patient = new Patient
        {
            Name = name,
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            Phone = "555-0000",
            Email = $"{name}@example.com",
            Address = "1 Test Street",
        };
        _db.Patients.Add(patient);
        _db.SaveChanges();
        return patient;
    }

    private Appointment SeedAppointment(Patient patient, Doctor doctor, Department dept, string status, DateTime date)
    {
        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            DepartmentId = dept.Id,
            Date = date,
            Time = new TimeSpan(9, 0, 0),
            Status = status,
        };
        _db.Appointments.Add(appointment);
        _db.SaveChanges();
        return appointment;
    }

    [Fact]
    public async Task BuildViewModelAsync_ComputesKpiStatistics()
    {
        var (dept, _, doctor) = SeedDoctor("Dr. Alice Carter");
        var patient = SeedPatient("Eve Johnson");
        SeedAppointment(patient, doctor, dept, "Scheduled", DateTime.Today);

        _db.Staff.Add(new Staff { Name = "Carol Smith", Role = "Receptionist", DepartmentId = dept.Id, Phone = "555-0103", Email = "carol@clinic.local" });
        _db.SaveChanges();

        var vm = await _controller.BuildViewModelAsync(null, null, null, 1);

        Assert.Equal(1, vm.StaffCount);
        Assert.Equal(1, vm.DoctorCount);
        Assert.Equal(1, vm.DepartmentCount);
        Assert.Equal(1, vm.PatientCount);
        Assert.Equal(1, vm.AppointmentCount);
        Assert.Equal(1, vm.TodayAppointmentCount);
    }

    [Fact]
    public async Task BuildViewModelAsync_ComputesFinancialReports()
    {
        var patient = SeedPatient("Eve Johnson");

        var invoice = new Invoice { PatientId = patient.Id, Items = "Consultation", TotalAmount = 100m, Status = "Unpaid" };
        _db.Invoices.Add(invoice);
        _db.SaveChanges();

        _db.Payments.Add(new Payment { InvoiceId = invoice.Id, Amount = 40m, Date = DateTime.UtcNow });
        _db.SaveChanges();

        var vm = await _controller.BuildViewModelAsync(null, null, null, 1);

        Assert.Equal(1, vm.InvoiceCount);
        Assert.Equal(0, vm.PaidInvoiceCount);
        Assert.Equal(40m, vm.TotalRevenue);
        Assert.Equal(100m, vm.UnpaidAmount);
    }

    [Fact]
    public async Task BuildViewModelAsync_FiltersAppointmentsByStatus()
    {
        var (dept, _, doctor) = SeedDoctor("Dr. Alice Carter");
        var patient = SeedPatient("Eve Johnson");
        SeedAppointment(patient, doctor, dept, "Scheduled", DateTime.Today.AddDays(1));
        SeedAppointment(patient, doctor, dept, "Cancelled", DateTime.Today.AddDays(2));

        var vm = await _controller.BuildViewModelAsync(null, "Cancelled", null, 1);

        Assert.Single(vm.Appointments);
        Assert.Equal("Cancelled", vm.Appointments[0].Status);
        Assert.Equal(1, vm.TotalCount);
    }

    [Fact]
    public async Task BuildViewModelAsync_SearchesAppointmentsByPatientName()
    {
        var (dept, _, doctor) = SeedDoctor("Dr. Alice Carter");
        var eve = SeedPatient("Eve Johnson");
        var frank = SeedPatient("Frank Miller");
        SeedAppointment(eve, doctor, dept, "Scheduled", DateTime.Today.AddDays(1));
        SeedAppointment(frank, doctor, dept, "Scheduled", DateTime.Today.AddDays(2));

        var vm = await _controller.BuildViewModelAsync("Eve", null, null, 1);

        Assert.Single(vm.Appointments);
        Assert.Equal(eve.Id, vm.Appointments[0].PatientId);
        Assert.Equal(1, vm.TotalCount);
    }

    [Fact]
    public async Task BuildViewModelAsync_SortsAppointmentsByPatientName()
    {
        var (dept, _, doctor) = SeedDoctor("Dr. Alice Carter");
        var eve = SeedPatient("Eve Johnson");
        var frank = SeedPatient("Frank Miller");
        SeedAppointment(eve, doctor, dept, "Scheduled", DateTime.Today.AddDays(1));
        SeedAppointment(frank, doctor, dept, "Scheduled", DateTime.Today.AddDays(2));

        var vm = await _controller.BuildViewModelAsync(null, null, "patient", 1);

        Assert.Equal(2, vm.Appointments.Count);
        Assert.Equal("Eve Johnson", vm.Appointments[0].Patient!.Name);
        Assert.Equal("Frank Miller", vm.Appointments[1].Patient!.Name);
    }

    [Fact]
    public async Task BuildViewModelAsync_PaginatesAppointments()
    {
        var (dept, _, doctor) = SeedDoctor("Dr. Alice Carter");
        var patient = SeedPatient("Eve Johnson");

        for (var i = 0; i < DashboardViewModel.PageSize + 3; i++)
        {
            SeedAppointment(patient, doctor, dept, "Scheduled", DateTime.Today.AddDays(i));
        }

        var page1 = await _controller.BuildViewModelAsync(null, null, null, 1);
        var page2 = await _controller.BuildViewModelAsync(null, null, null, 2);

        Assert.Equal(DashboardViewModel.PageSize + 3, page1.TotalCount);
        Assert.Equal(DashboardViewModel.PageSize, page1.Appointments.Count);
        Assert.Equal(3, page2.Appointments.Count);
    }
}
