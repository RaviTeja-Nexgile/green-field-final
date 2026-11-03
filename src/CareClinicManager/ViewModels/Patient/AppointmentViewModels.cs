using System.ComponentModel.DataAnnotations;
using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Patient;

/// <summary>A row in the patient's own appointment list.</summary>
public class PatientAppointmentListItemViewModel
{
    public int Id { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Status { get; set; } = string.Empty;

    public static PatientAppointmentListItemViewModel From(Appointment a) => new()
    {
        Id = a.Id,
        DoctorName = a.Doctor?.Name ?? string.Empty,
        DepartmentName = a.Department?.Name ?? string.Empty,
        Date = a.Date,
        Time = a.Time,
        Status = a.Status
    };
}

/// <summary>The patient's appointment list shown on their dashboard.</summary>
public class PatientAppointmentListViewModel
{
    public List<PatientAppointmentListItemViewModel> Appointments { get; set; } = new();
}

/// <summary>The patient self-booking form: pick a doctor, department, date and time.</summary>
public class PatientBookAppointmentViewModel
{
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

    public List<Models.Doctor> Doctors { get; set; } = new();
    public List<Models.Department> Departments { get; set; } = new();
}
