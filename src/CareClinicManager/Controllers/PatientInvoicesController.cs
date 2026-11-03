using CareClinicManager.Data;
using CareClinicManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Patient")]
public class PatientInvoicesController : Controller
{
    private readonly InvoiceService _invoices;
    private readonly AppDbContext _db;

    public PatientInvoicesController(InvoiceService invoices, AppDbContext db)
    {
        _invoices = invoices;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var patientId = await ResolvePatientIdAsync();
        if (patientId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var invoices = await _invoices.GetByPatientAsync(patientId.Value);
        return View(invoices);
    }

    private async Task<int?> ResolvePatientIdAsync()
    {
        var nameId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(nameId, out var userId))
        {
            var user = await _db.Users.FindAsync(userId);
            return user?.PatientId;
        }

        return null;
    }
}
