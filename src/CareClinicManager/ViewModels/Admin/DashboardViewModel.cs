using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Admin;

/// <summary>
/// View model for the Admin dashboard: KPI statistics for staff, departments,
/// patients, appointments and financial reports, plus a page of recent
/// appointments for the data table.
/// </summary>
public class DashboardViewModel
{
    // KPI statistics (criterion 14.2)
    public int StaffCount { get; set; }
    public int DoctorCount { get; set; }
    public int DepartmentCount { get; set; }
    public int PatientCount { get; set; }
    public int AppointmentCount { get; set; }
    public int TodayAppointmentCount { get; set; }

    // Financial reports (criterion 14.2)
    public decimal TotalRevenue { get; set; }
    public decimal UnpaidAmount { get; set; }
    public int InvoiceCount { get; set; }
    public int PaidInvoiceCount { get; set; }

    // Recent appointments data table (criterion 14.6)
    public List<Appointment> Appointments { get; set; } = new();
    public int Page { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Sort { get; set; }

    public const int PageSize = 10;
}
