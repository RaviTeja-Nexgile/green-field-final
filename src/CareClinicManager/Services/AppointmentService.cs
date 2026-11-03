using CareClinicManager.Models;
using CareClinicManager.Repositories;

namespace CareClinicManager.Services;

/// <summary>
/// Holds the appointment business rules: booking saves with status "Scheduled",
/// check-in sets "Checked in", cancelling sets "Cancelled", and editing updates
/// the appointment in place.
/// </summary>
public class AppointmentService
{
    public const string Scheduled = "Scheduled";
    public const string CheckedIn = "Checked in";
    public const string InProgress = "In progress";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    private readonly AppointmentRepository _repository;

    public AppointmentService(AppointmentRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Books a new appointment with the status "Scheduled".</summary>
    public async Task<Appointment> BookAsync(
        int patientId, int doctorId, int departmentId, DateTime date, TimeSpan time)
    {
        var appointment = new Appointment
        {
            PatientId = patientId,
            DoctorId = doctorId,
            DepartmentId = departmentId,
            Date = date.Date,
            Time = time,
            Status = Scheduled
        };

        _repository.Add(appointment);
        await _repository.SaveAsync();
        return appointment;
    }

    /// <summary>Updates an existing appointment's patient, doctor, department, date and time.</summary>
    public async Task<Appointment?> UpdateAsync(
        int id, int patientId, int doctorId, int departmentId, DateTime date, TimeSpan time)
    {
        var appointment = await _repository.GetByIdAsync(id);
        if (appointment is null)
        {
            return null;
        }

        appointment.PatientId = patientId;
        appointment.DoctorId = doctorId;
        appointment.DepartmentId = departmentId;
        appointment.Date = date.Date;
        appointment.Time = time;

        await _repository.SaveAsync();
        return appointment;
    }

    /// <summary>Sets an appointment's status to "Checked in".</summary>
    public async Task<Appointment?> CheckInAsync(int id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        if (appointment is null)
        {
            return null;
        }

        appointment.Status = CheckedIn;
        await _repository.SaveAsync();
        return appointment;
    }

    /// <summary>Sets an appointment's status to "Cancelled".</summary>
    public async Task<Appointment?> CancelAsync(int id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        if (appointment is null)
        {
            return null;
        }

        appointment.Status = Cancelled;
        await _repository.SaveAsync();
        return appointment;
    }
}
