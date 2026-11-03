using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Repositories;
using CareClinicManager.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class AppointmentTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly AppointmentRepository _repository;
    private readonly AppointmentService _service;

    public AppointmentTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-appt-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _repository = new AppointmentRepository(_db);
        _service = new AppointmentService(_repository);
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

    private (Patient patient, Doctor doctor, Department department) SeedLookups()
    {
        var department = new Department { Name = "Cardiology" };
        var specialization = new Specialization { Name = "Cardiologist" };
        var doctor = new Doctor { Name = "Dr. Alice Carter", Role = "Doctor", Department = department, Specialization = specialization, Phone = "555-0101", Email = "a@c.local" };
        var patient = new Patient { Name = "Eve Johnson", DateOfBirth = new DateTime(1985, 4, 12), Gender = "Female", Phone = "555-0201", Email = "e@example.com", Address = "12 Oak" };
        _db.Departments.Add(department);
        _db.Specializations.Add(specialization);
        _db.Doctors.Add(doctor);
        _db.Patients.Add(patient);
        _db.SaveChanges();
        return (patient, doctor, department);
    }

    [Fact]
    public async Task BookAsync_SavesAppointment_WithStatusScheduled()
    {
        var (patient, doctor, department) = SeedLookups();

        var appointment = await _service.BookAsync(
            patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(9, 0, 0));

        Assert.True(appointment.Id > 0);
        Assert.Equal(AppointmentService.Scheduled, appointment.Status);
        Assert.Equal(patient.Id, appointment.PatientId);
        Assert.Equal(doctor.Id, appointment.DoctorId);
        Assert.Equal(department.Id, appointment.DepartmentId);
    }

    [Fact]
    public async Task BookAsync_PersistsAppointment_ToDatabase()
    {
        var (patient, doctor, department) = SeedLookups();

        await _service.BookAsync(
            patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(10, 30, 0));

        var saved = await _db.Appointments.SingleAsync();
        Assert.Equal(AppointmentService.Scheduled, saved.Status);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAppointmentFields()
    {
        var (patient, doctor, department) = SeedLookups();
        var appointment = await _service.BookAsync(
            patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(9, 0, 0));

        var newDate = DateTime.Today.AddDays(3);
        var newTime = new TimeSpan(14, 15, 0);
        var updated = await _service.UpdateAsync(
            appointment.Id, patient.Id, doctor.Id, department.Id, newDate, newTime);

        Assert.NotNull(updated);
        Assert.Equal(newDate.Date, updated!.Date);
        Assert.Equal(newTime, updated.Time);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenAppointmentMissing()
    {
        var (patient, doctor, department) = SeedLookups();

        var updated = await _service.UpdateAsync(
            9999, patient.Id, doctor.Id, department.Id, DateTime.Today, new TimeSpan(9, 0, 0));

        Assert.Null(updated);
    }

    [Fact]
    public async Task CheckInAsync_SetsStatusToCheckedIn()
    {
        var (patient, doctor, department) = SeedLookups();
        var appointment = await _service.BookAsync(
            patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(9, 0, 0));

        var checkedIn = await _service.CheckInAsync(appointment.Id);

        Assert.NotNull(checkedIn);
        Assert.Equal(AppointmentService.CheckedIn, checkedIn!.Status);
    }

    [Fact]
    public async Task CancelAsync_SetsStatusToCancelled()
    {
        var (patient, doctor, department) = SeedLookups();
        var appointment = await _service.BookAsync(
            patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(9, 0, 0));

        var cancelled = await _service.CancelAsync(appointment.Id);

        Assert.NotNull(cancelled);
        Assert.Equal(AppointmentService.Cancelled, cancelled!.Status);
    }

    [Fact]
    public async Task GetByPatientAsync_ReturnsOnlyThatPatientsAppointments()
    {
        var (patient, doctor, department) = SeedLookups();
        var otherPatient = new Patient { Name = "Frank Miller", DateOfBirth = new DateTime(1990, 8, 3), Gender = "Male", Phone = "555-0202", Email = "f@example.com", Address = "34 Pine" };
        _db.Patients.Add(otherPatient);
        _db.SaveChanges();

        await _service.BookAsync(patient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(1), new TimeSpan(9, 0, 0));
        await _service.BookAsync(otherPatient.Id, doctor.Id, department.Id, DateTime.Today.AddDays(2), new TimeSpan(10, 0, 0));

        var mine = await _repository.GetByPatientAsync(patient.Id);

        Assert.Single(mine);
        Assert.Equal(patient.Id, mine[0].PatientId);
    }
}
