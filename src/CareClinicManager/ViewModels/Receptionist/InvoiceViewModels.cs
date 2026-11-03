using System.ComponentModel.DataAnnotations;
using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Receptionist;

/// <summary>View model for the invoice list screen.</summary>
public class InvoiceListViewModel
{
    public List<Invoice> Items { get; set; } = new();
    public int Page { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)Services.InvoiceService.PageSize));
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Sort { get; set; }
}

/// <summary>A single line item entered on the "New invoice" form.</summary>
public class InvoiceItemViewModel
{
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;

    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
    public decimal UnitPrice { get; set; }
}

/// <summary>View model for the "New invoice" form.</summary>
public class InvoiceFormViewModel
{
    [Required(ErrorMessage = "Please select a patient.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a patient.")]
    public int PatientId { get; set; }

    [MinLength(1, ErrorMessage = "Add at least one item to bill.")]
    public List<InvoiceItemViewModel> Items { get; set; } = new();

    public List<Patient> Patients { get; set; } = new();
}

/// <summary>View model for recording a payment on an invoice.</summary>
public class PaymentFormViewModel
{
    public int InvoiceId { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }
}

/// <summary>View model for the invoice details screen.</summary>
public class InvoiceDetailsViewModel
{
    public Invoice Invoice { get; set; } = new();
    public PaymentFormViewModel Payment { get; set; } = new();
}
