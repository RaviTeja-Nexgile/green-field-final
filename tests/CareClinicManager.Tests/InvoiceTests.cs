using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class InvoiceTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly InvoiceService _service;

    public InvoiceTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-invoice-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _service = new InvoiceService(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }

    private Patient SeedPatient(string name)
    {
        var patient = new Patient
        {
            Name = name,
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            Phone = "555-0000",
            Email = $"{name}@example.com",
            Address = "1 Test Street",
        };
        _db.Patients.Add(patient);
        _db.SaveChanges();
        return patient;
    }

    [Fact]
    public async Task CreateAsync_SavesInvoiceAsUnpaid_WithComputedTotal()
    {
        var patient = SeedPatient("Eve Johnson");

        var invoice = await _service.CreateAsync(patient.Id, new List<InvoiceItemInput>
        {
            new() { Description = "Consultation", Quantity = 2, UnitPrice = 50m },
            new() { Description = "Blood test", Quantity = 1, UnitPrice = 25m },
        });

        Assert.Equal("Unpaid", invoice.Status);
        Assert.Equal(125m, invoice.TotalAmount);
        Assert.Equal(patient.Id, invoice.PatientId);

        var stored = await _db.Invoices.FindAsync(invoice.Id);
        Assert.NotNull(stored);
        Assert.Equal("Unpaid", stored!.Status);
        Assert.Equal(125m, stored.TotalAmount);
    }

    [Fact]
    public async Task RecordPaymentAsync_SetsInvoiceStatusToPaid()
    {
        var patient = SeedPatient("Frank Miller");
        var invoice = await _service.CreateAsync(patient.Id, new List<InvoiceItemInput>
        {
            new() { Description = "Consultation", Quantity = 1, UnitPrice = 100m },
        });

        var payment = await _service.RecordPaymentAsync(invoice.Id, 100m);

        Assert.NotNull(payment);
        Assert.Equal(invoice.Id, payment!.InvoiceId);
        Assert.Equal(100m, payment.Amount);

        var stored = await _db.Invoices.FindAsync(invoice.Id);
        Assert.Equal("Paid", stored!.Status);
    }

    [Fact]
    public async Task RecordPaymentAsync_ReturnsNull_WhenInvoiceMissing()
    {
        var payment = await _service.RecordPaymentAsync(9999, 10m);
        Assert.Null(payment);
    }

    [Fact]
    public async Task GetPagedAsync_FiltersByStatus()
    {
        var patient = SeedPatient("Eve Johnson");
        var paid = await _service.CreateAsync(patient.Id, new List<InvoiceItemInput>
        {
            new() { Description = "A", Quantity = 1, UnitPrice = 10m },
        });
        await _service.RecordPaymentAsync(paid.Id, 10m);

        await _service.CreateAsync(patient.Id, new List<InvoiceItemInput>
        {
            new() { Description = "B", Quantity = 1, UnitPrice = 20m },
        });

        var (items, total) = await _service.GetPagedAsync(null, "Paid", null, 1);

        Assert.Single(items);
        Assert.Equal(paid.Id, items[0].Id);
        Assert.Equal(1, total);
    }

    [Fact]
    public async Task GetPagedAsync_SearchesByPatientName()
    {
        var eve = SeedPatient("Eve Johnson");
        var frank = SeedPatient("Frank Miller");

        await _service.CreateAsync(eve.Id, new List<InvoiceItemInput>
        {
            new() { Description = "Consultation", Quantity = 1, UnitPrice = 10m },
        });
        await _service.CreateAsync(frank.Id, new List<InvoiceItemInput>
        {
            new() { Description = "Consultation", Quantity = 1, UnitPrice = 10m },
        });

        var (items, total) = await _service.GetPagedAsync("Eve", null, null, 1);

        Assert.Single(items);
        Assert.Equal(eve.Id, items[0].PatientId);
        Assert.Equal(1, total);
    }

    [Fact]
    public async Task GetPagedAsync_PaginatesResults()
    {
        var patient = SeedPatient("Eve Johnson");
        for (var i = 0; i < InvoiceService.PageSize + 3; i++)
        {
            await _service.CreateAsync(patient.Id, new List<InvoiceItemInput>
            {
                new() { Description = $"Item {i}", Quantity = 1, UnitPrice = 10m },
            });
        }

        var (page1, total) = await _service.GetPagedAsync(null, null, null, 1);
        var (page2, _) = await _service.GetPagedAsync(null, null, null, 2);

        Assert.Equal(InvoiceService.PageSize + 3, total);
        Assert.Equal(InvoiceService.PageSize, page1.Count);
        Assert.Equal(3, page2.Count);
    }

    [Fact]
    public async Task GetByPatientAsync_ReturnsOnlyThatPatientsInvoices()
    {
        var eve = SeedPatient("Eve Johnson");
        var frank = SeedPatient("Frank Miller");

        await _service.CreateAsync(eve.Id, new List<InvoiceItemInput>
        {
            new() { Description = "A", Quantity = 1, UnitPrice = 10m },
        });
        await _service.CreateAsync(frank.Id, new List<InvoiceItemInput>
        {
            new() { Description = "B", Quantity = 1, UnitPrice = 10m },
        });

        var invoices = await _service.GetByPatientAsync(eve.Id);

        Assert.Single(invoices);
        Assert.Equal(eve.Id, invoices[0].PatientId);
    }
}
