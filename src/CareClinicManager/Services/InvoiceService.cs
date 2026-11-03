using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Services;

/// <summary>
/// Business rules for invoices and payments: list/search/sort/filter/paginate,
/// create an invoice as "Unpaid" with a computed total, and record a payment
/// that sets the invoice to "Paid".
/// </summary>
public class InvoiceService
{
    private readonly AppDbContext _db;

    public InvoiceService(AppDbContext db)
    {
        _db = db;
    }

    public const int PageSize = 10;

    /// <summary>
    /// Returns a page of invoices with the patient loaded, filtered by status
    /// and a free-text search over patient name and invoice items, sorted by
    /// the requested column.
    /// </summary>
    public async Task<(List<Invoice> Items, int TotalCount)> GetPagedAsync(
        string? search, string? status, string? sort, int page)
    {
        var query = _db.Invoices
            .Include(i => i.Patient)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(i => i.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(i =>
                i.Patient!.Name.Contains(term) ||
                i.Items.Contains(term));
        }

        query = sort switch
        {
            "patient" => query.OrderBy(i => i.Patient!.Name),
            "patient_desc" => query.OrderByDescending(i => i.Patient!.Name),
            "total" => query.OrderBy(i => i.TotalAmount),
            "total_desc" => query.OrderByDescending(i => i.TotalAmount),
            "status" => query.OrderBy(i => i.Status),
            "status_desc" => query.OrderByDescending(i => i.Status),
            _ => query.OrderByDescending(i => i.Id),
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    /// <summary>
    /// Creates an invoice with the status "Unpaid" and a total computed from
    /// the billed items. Returns the saved invoice.
    /// </summary>
    public async Task<Invoice> CreateAsync(int patientId, List<InvoiceItemInput> items)
    {
        var invoice = new Invoice
        {
            PatientId = patientId,
            Items = string.Join("; ", items.Select(i => $"{i.Description} x{i.Quantity} @ {i.UnitPrice:0.00}")),
            TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice),
            Status = "Unpaid",
        };

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();
        return invoice;
    }

    /// <summary>
    /// Records a payment for an invoice and sets the invoice status to "Paid".
    /// </summary>
    public async Task<Payment?> RecordPaymentAsync(int invoiceId, decimal amount)
    {
        var invoice = await _db.Invoices.FindAsync(invoiceId);
        if (invoice is null)
        {
            return null;
        }

        var payment = new Payment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            Date = DateTime.UtcNow,
        };

        _db.Payments.Add(payment);
        invoice.Status = "Paid";
        await _db.SaveChangesAsync();
        return payment;
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        return await _db.Invoices
            .Include(i => i.Patient)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Invoice>> GetByPatientAsync(int patientId)
    {
        return await _db.Invoices
            .Include(i => i.Payments)
            .Where(i => i.PatientId == patientId)
            .OrderByDescending(i => i.Id)
            .ToListAsync();
    }

    public async Task<List<Patient>> GetPatientsAsync()
    {
        return await _db.Patients
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}

/// <summary>A single line item on a new invoice.</summary>
public class InvoiceItemInput
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
