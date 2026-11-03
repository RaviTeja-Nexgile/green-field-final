using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Repositories;

/// <summary>
/// Wraps <see cref="AppDbContext"/> access for appointments: reusable queries
/// for the calendar, the list and the patient's own appointments.
/// </summary>
public class AppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>All appointments with their patient, doctor and department, newest first.</summary>
    public async Task<List<Appointment>> GetAllAsync()
    {
        var appointments = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .ToListAsync();

        return appointments
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Time)
            .ToList();
    }

    /// <summary>Appointments for a single patient, newest first.</summary>
    public async Task<List<Appointment>> GetByPatientAsync(int patientId)
    {
        var appointments = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .Where(a => a.PatientId == patientId)
            .ToListAsync();

        return appointments
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Time)
            .ToList();
    }

    /// <summary>A single appointment with its relations, or null.</summary>
    public Task<Appointment?> GetByIdAsync(int id)
    {
        return _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public void Add(Appointment appointment) => _db.Appointments.Add(appointment);

    public void Remove(Appointment appointment) => _db.Appointments.Remove(appointment);

    public Task SaveAsync() => _db.SaveChangesAsync();
}
