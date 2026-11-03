using CareClinicManager.Data;
using CareClinicManager.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Dashboard(string? search, string? status, string? sort, int page = 1)
    {
        var vm = await BuildViewModelAsync(search, status, sort, page);
        return View(vm);
    }

    /// <summary>
    /// Builds the Admin dashboard view model: KPI statistics for staff,
    /// departments, patients, appointments and financial reports, plus a page
    /// of recent appointments with search, status filter, sorting and
    /// pagination.
    /// </summary>
    public async Task<DashboardViewModel> BuildViewModelAsync(
        string? search, string? status, string? sort, int page)
    {
        if (page < 1)
        {
            page = 1;
        }

        var today = DateTime.Today;

        var vm = new DashboardViewModel
        {
            StaffCount = await _db.Staff.CountAsync(),
            DoctorCount = await _db.Doctors.CountAsync(),
            DepartmentCount = await _db.Departments.CountAsync(),
            PatientCount = await _db.Patients.CountAsync(),
            AppointmentCount = await _db.Appointments.CountAsync(),
            TodayAppointmentCount = await _db.Appointments.CountAsync(a => a.Date == today),
            InvoiceCount = await _db.Invoices.CountAsync(),
            PaidInvoiceCount = await _db.Invoices.CountAsync(i => i.Status == "Paid"),
            TotalRevenue = await _db.Payments.SumAsync(p => (decimal?)p.Amount) ?? 0m,
            UnpaidAmount = await _db.Invoices
                .Where(i => i.Status == "Unpaid")
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0m,
        };

        // Recent appointments data table: search, filter, sort, paginate.
        var query = _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(a => a.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(a =>
                a.Patient!.Name.Contains(term) ||
                a.Doctor!.Name.Contains(term) ||
                a.Department!.Name.Contains(term));
        }

        query = sort switch
        {
            "date" => query.OrderBy(a => a.Date).ThenBy(a => a.Id),
            "date_desc" => query.OrderByDescending(a => a.Date).ThenByDescending(a => a.Id),
            "patient" => query.OrderBy(a => a.Patient!.Name),
            "patient_desc" => query.OrderByDescending(a => a.Patient!.Name),
            "doctor" => query.OrderBy(a => a.Doctor!.Name),
            "status" => query.OrderBy(a => a.Status),
            _ => query.OrderByDescending(a => a.Date).ThenByDescending(a => a.Id),
        };

        vm.TotalCount = await query.CountAsync();
        vm.Appointments = await query
            .Skip((page - 1) * DashboardViewModel.PageSize)
            .Take(DashboardViewModel.PageSize)
            .ToListAsync();
        vm.Page = page;
        vm.Search = search;
        vm.Status = status;
        vm.Sort = sort;

        return vm;
    }
}
