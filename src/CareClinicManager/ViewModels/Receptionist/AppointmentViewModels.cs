using System.ComponentModel.DataAnnotations;
using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Receptionist;

/// <summary>A row in the appointment list or calendar, shaped for display.</summary>
public class AppointmentListItemViewModel
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Status { get; set; } = string.Empty;

    public static AppointmentListItemViewModel From(Appointment a) => new()
    {
        Id = a.Id,
        PatientName = a.Patient?.Name ?? string.Empty,
        DoctorName = a.Doctor?.Name ?? string.Empty,
        DepartmentName = a.Department?.Name ?? string.Empty,
        Date = a.Date,
        Time = a.Time,
        Status = a.Status
    };
}

/// <summary>The appointment list screen: every appointment with its status badge.</summary>
public class AppointmentListViewModel
{
    public List<AppointmentListItemViewModel> Appointments { get; set; } = new();
}

/// <summary>The calendar screen: appointments grouped by day for a month grid.</summary>
public class AppointmentCalendarViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public List<AppointmentListItemViewModel> Appointments { get; set; } = new();
}

/// <summary>The book/edit form: pick a patient, doctor, department, date and time.</summary>
public class AppointmentFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please choose a patient.")]
    [Display(Name = "Patient")]
    public int? PatientId { get; set; }

    [Required(ErrorMessage = "Please choose a doctor.")]
    [Display(Name = "Doctor")]
    public int? DoctorId { get; set; }

    [Required(ErrorMessage = "Please choose a department.")]
    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Required(ErrorMessage = "Please choose a date.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Please choose a time.")]
    [DataType(DataType.Time)]
    [Display(Name = "Time")]
    public TimeSpan? Time { get; set; }

    public List<Models.Patient> Patients { get; set; } = new();
    public List<Models.Doctor> Doctors { get; set; } = new();
    public List<Models.Department> Departments { get; set; } = new();
}
