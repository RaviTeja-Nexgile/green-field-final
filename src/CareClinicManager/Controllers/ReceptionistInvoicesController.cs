using CareClinicManager.Services;
using CareClinicManager.ViewModels.Receptionist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Receptionist")]
public class ReceptionistInvoicesController : Controller
{
    private readonly InvoiceService _invoices;

    public ReceptionistInvoicesController(InvoiceService invoices)
    {
        _invoices = invoices;
    }

    public async Task<IActionResult> Index(string? search, string? status, string? sort, int page = 1)
    {
        if (page < 1)
        {
            page = 1;
        }

        var (items, totalCount) = await _invoices.GetPagedAsync(search, status, sort, page);

        var vm = new InvoiceListViewModel
        {
            Items = items,
            Page = page,
            TotalCount = totalCount,
            Search = search,
            Status = status,
            Sort = sort,
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new InvoiceFormViewModel
        {
            Patients = await _invoices.GetPatientsAsync(),
            Items = { new InvoiceItemViewModel() },
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InvoiceFormViewModel vm)
    {
        // Drop blank rows before validating so an empty optional row does not
        // fail validation, but keep at least one row so "no items" is caught.
        vm.Items = vm.Items
            .Where(i => !string.IsNullOrWhiteSpace(i.Description))
            .ToList();

        if (!ModelState.IsValid)
        {
            vm.Patients = await _invoices.GetPatientsAsync();
            return View(vm);
        }

        var items = vm.Items
            .Select(i => new InvoiceItemInput
            {
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
            })
            .ToList();

        var invoice = await _invoices.CreateAsync(vm.PatientId, items);
        return RedirectToAction(nameof(Details), new { id = invoice.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _invoices.GetByIdAsync(id);
        if (invoice is null)
        {
            return NotFound();
        }

        var vm = new InvoiceDetailsViewModel
        {
            Invoice = invoice,
            Payment = new PaymentFormViewModel { InvoiceId = id },
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordPayment(PaymentFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var invoice = await _invoices.GetByIdAsync(vm.InvoiceId);
            if (invoice is null)
            {
                return NotFound();
            }

            var details = new InvoiceDetailsViewModel
            {
                Invoice = invoice,
                Payment = vm,
            };
            return View(nameof(Details), details);
        }

        await _invoices.RecordPaymentAsync(vm.InvoiceId, vm.Amount);
        return RedirectToAction(nameof(Details), new { id = vm.InvoiceId });
    }
}
